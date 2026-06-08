namespace Application.Common.Seacrh;

public static class SearchNormaliser
{
    public static string Normalize(string value)
    {
        var result = value
            .Trim()
            .ToLowerInvariant()
            .Replace(" ", "");
        return result;
    }
}
