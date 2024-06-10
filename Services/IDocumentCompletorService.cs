namespace Services;

public interface IDocumentCompletorService
{
    public Task<Stream> ReplaceAllAsync(Dictionary<string, string> variables, string htmlString);

    public Task<Stream> ReplaceAllByPathAsync(Dictionary<string, string> variables, string path);
}