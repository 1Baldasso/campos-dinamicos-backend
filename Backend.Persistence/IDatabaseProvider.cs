namespace Backend.Persistence;

public interface IDatabaseProvider
{
    PocContext Context { get; }
}