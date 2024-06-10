using System.Text.Json;

namespace Backend.POC.Extensions;

public static class JsonExtensions
{
    public static bool IsValidJson(this string json)
    {
        try
        {
            JsonDocument.Parse(json);
            return true;
        }
        catch (JsonException)
        {
            return false;
        }
    }
}