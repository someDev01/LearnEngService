namespace Application.Common.Json;

public static class JsonExtensions
{
    public static string CleanJson(string content)
    {
        return content
            .Replace("```json", "")
            .Replace("```", "")
            .Trim();
    }
}
