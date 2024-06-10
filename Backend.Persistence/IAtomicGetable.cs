namespace Backend.Persistence;

public interface IAtomicGetable
{
    IEnumerable<(string Key, object Value)> GetAtomicValues();
}