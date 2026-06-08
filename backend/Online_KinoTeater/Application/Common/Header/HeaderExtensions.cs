namespace Application.Common.HeaderKey;

public static class HeaderExtensions
{
    public static string ToKebabCaseHeaderName(string key)
    {
        if (string.IsNullOrWhiteSpace(key)) return key;

        var result = new System.Text.StringBuilder();
        for(int i = 0;  i < key.Length; i++)
        {
            char c = key[i];
            if(char.IsUpper(c) && i > 0)
            {
                result.Append('-');
            }
            result.Append(c);
        }

        return result.ToString();
    }
}
