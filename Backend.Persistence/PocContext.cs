using Microsoft.EntityFrameworkCore;

namespace Backend.Persistence;

public class PocContext(DbContextOptions<PocContext> options) : DbContext(options)
{
    public DbSet<Teste> Testes { get; set; }
    public DbSet<Campo> Campos { get; set; }

    public DbSet<TemplateInfo> Templates { get; set; }
    public DbSet<TemplateCampo> TemplateCampos { get; set; }
}