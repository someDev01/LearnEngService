namespace Application.Common.Email;

public static class EmailExtensions
{
    public static bool IsValidEmailAndGmail(this string value)
    {
        var countDogs = value.Count(l => l == '@');
        if (countDogs != 1) return false;

        var emailParts = value.Split('@');
        string local = emailParts[0];
        string domain = emailParts[1];

        if (string.IsNullOrWhiteSpace(local) || string.IsNullOrWhiteSpace(domain))
            return false;

        if (!domain.EndsWith(".ru") && !domain.EndsWith(".com"))
            return false;

        return true;
    }

    public static bool IsValidName(this string value)
    {
        if (!value.Any(char.IsLetter) || !char.IsLetter(value[0])) return false;

        return true;
    }
}
