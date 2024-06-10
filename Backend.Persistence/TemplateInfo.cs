using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Persistence;

public record TemplateInfo
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Guid { get; set; }
    public string Nome { get; set; }
    public string Path { get; set; }
    public ICollection<TemplateCampo> Campos { get; set; }
}