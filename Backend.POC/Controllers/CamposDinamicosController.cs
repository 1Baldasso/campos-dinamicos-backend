using Backend.Persistence;
using Backend.POC.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services;
using System.Text.Json;

namespace Backend.POC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CamposDinamicosController(IDatabaseProvider contextProvider, IDocumentCompletorService service) : ControllerBase
    {
        private readonly PocContext _context = contextProvider.Context;
        private IEnumerable<(string Key, object Value)> _values;

        [HttpGet]
        public async Task<IActionResult> GetCampos(CancellationToken cancellationToken)
        {
            return Ok(await _context.Campos.ToListAsync(cancellationToken));
        }

        [HttpPost]
        public async Task<IActionResult> CreateData(TesteDto req, CancellationToken cancellationToken)
        {
            var teste = new Teste
            {
                Fixo = req.Fixo,
                Dinamico = req.Dinamico
            };
            var entity = await _context.AddAsync(teste, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Ok(entity.Entity.Id);
        }

        [HttpGet("data/{id}")]
        public async Task<IActionResult> GetData(Guid id, CancellationToken cancellationToken)
        {
            var data = await _context.Testes.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            var campos = await _context.Campos.ToListAsync(cancellationToken);

            return Ok(new { data, campos });
        }

        [HttpGet("html")]
        public async Task<IActionResult> GetHtmlResponse(CancellationToken cancellationToken)
        {
            var teste = await _context.Testes.FirstOrDefaultAsync(x => x.Id == Guid.Parse("964BDC8E-C1D6-4741-BF73-3E400CF71852"), cancellationToken);

            var template = await _context.Templates
                .Include(x => x.Campos)
                .FirstOrDefaultAsync(cancellationToken);

            _values = teste.GetAtomicValues();

            var replaceDic = new Dictionary<string, string>();
            foreach (var item in template.Campos)
            {
                var value = GetValue(item.NomeCampo);
                var stringValue = GetValueString(value);
                replaceDic.Add(item.NomeReplace, stringValue);
            }
            var stream = await service.ReplaceAllByPathAsync(replaceDic, template.Path);
            return Ok(stream);
        }

        private object GetValue(string nomeCampo, IDictionary<string, object>? values = null)
        {
            var items = values;
            if (values == null)
                items = new Dictionary<string, object>(_values.Select(x => new KeyValuePair<string, object>(x.Key, x.Value)));

            if (!nomeCampo.Contains('.'))
            {
                return items.FirstOrDefault(x => x.Key.Equals(nomeCampo, StringComparison.OrdinalIgnoreCase)).Value;
            }

            var firstIdentifier = nomeCampo.Split('.')[0];
            var newValue = items.FirstOrDefault(x => x.Key.Equals(firstIdentifier, StringComparison.OrdinalIgnoreCase)).Value;
            var newDic = new Dictionary<string, object>();
            if (newValue is string str && str.IsValidJson())
            {
                newDic = JsonSerializer.Deserialize<Dictionary<string, object>>(str);
            }
            else if (newValue is IAtomicGetable at)
            {
                newDic = new Dictionary<string, object>(at.GetAtomicValues().Select(x => new KeyValuePair<string, object>(x.Key, x.Value)));
            }
            var newIdentifier = new string(nomeCampo.SkipWhile(x => !x.Equals('.')).Skip(1).ToArray());
            return GetValue(newIdentifier, newDic);
        }

        private string GetValueString(object obj)
        {
            if (obj is string str)
                return str;
            if (obj is DateTime dt)
                return dt.ToString("dd/MM/yy");
            if (obj is decimal dc)
                return dc.ToString("#0,00");

            return obj.ToString();
        }
    }
}