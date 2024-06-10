using System.Text.Json;

namespace Backend.Persistence;

public class DatabasePopulator(PocContext context) : IDatabaseProvider
{
    private readonly PocContext _context = context;

    public PocContext Context => _context.Campos.Any() ? _context : PopulateContext();

    private PocContext PopulateContext()
    {
        ICollection<dynamic> estadosTeste = [
            new{ label = "Paraná", value = "PR" },
            new{ label = "São Paulo", value = "SP" },
            new{ label = "Rio Grande do Sul", value = "RS" },
        ];

        var dataTeste = "{ \"idade\": 15, \"estado\": \"PR\", \"telefone\": \"42998029837\", \"maiorDeIdade\": true }";
        _context.Campos.AddRange(
            new() { Name = "idade", Type = CampoTipo.Number, Label = "Idade" },
            new() { Name = "estado", Type = CampoTipo.Select, Label = "Estado", ExtraData = JsonSerializer.Serialize(estadosTeste) },
            new() { Name = "maiorDeIdade", Type = CampoTipo.Checkbox, Computed = true, Label = "Maior de Idade?", ComputeFunction = "parseInt(dados.dinamico.idade) >= 18" },
            new() { Name = "telefone", Type = CampoTipo.Mask, Label = "Telefone", ExtraData = "(00) 0000-0000" },
            new() { Name = "idadeMae", Type = CampoTipo.Number, Label = "Idade da mãe", Computed = true, ComputeFunction = "parseInt(dados.dinamico.idade) + 30" }

        );

        _context.Testes.Add(new Teste()
        {
            Id = Guid.Parse("964BDC8E-C1D6-4741-BF73-3E400CF71852"),
            Fixo = "Teste",
            Dinamico = dataTeste
        });

        _context.Templates.Add(new TemplateInfo
        {
            Nome = "Teste",
            Path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "teste.html"),
            Campos = [
                new TemplateCampo { NomeCampo = "Fixo", NomeReplace="%FIXO%" },
                new TemplateCampo { NomeCampo = "Dinamico.estado", NomeReplace = "%ESTADO%" },
                new TemplateCampo { NomeCampo = "Dinamico.telefone", NomeReplace = "%TELEFONE%" }
            ]
        });

        _context.SaveChanges();
        return _context;
    }
}