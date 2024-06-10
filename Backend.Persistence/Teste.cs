using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Persistence;

public class Teste : IAtomicGetable
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string Fixo { get; set; }
    public string Dinamico { get; set; }

    public IEnumerable<(string Key, object Value)> GetAtomicValues()
    {
        yield return (nameof(Id), Id);
        yield return (nameof(Fixo), Fixo);
        yield return (nameof(Dinamico), Dinamico);
    }
}