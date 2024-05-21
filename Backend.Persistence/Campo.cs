using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Backend.Persistence;

public class Campo
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public required string Name { get; set; }
    private string? _placeholder = null;

    public string? Placeholder
    {
        get => string.IsNullOrEmpty(_placeholder) ? Label : _placeholder;
        set
        {
            _placeholder = value;
        }
    }

    public required string Label { get; set; }
    public required CampoTipo Type { get; set; }
    public string? ExtraData { get; set; }
    public bool Required { get; set; } = false;
}