using Backend.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.POC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CamposDinamicosController(IDatabaseProvider contextProvider) : ControllerBase
    {
        private readonly PocContext _context = contextProvider.Context;

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
    }
}