using System.Text;

namespace Services;

public class DocumentCompletorService : IDocumentCompletorService
{
    public Task<Stream> ReplaceAllAsync(Dictionary<string, string> variables, string htmlString)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(htmlString);

        foreach (var variable in variables)
        {
            stringBuilder.Replace(variable.Key, variable.Value);
        }

        return Task.FromResult((Stream)new MemoryStream(Encoding.UTF8.GetBytes(stringBuilder.ToString())));
    }

    public async Task<Stream> ReplaceAllByPathAsync(Dictionary<string, string> variables, string path)
    {
        return await ReplaceAllAsync(variables, await File.ReadAllTextAsync(path));
    }
}