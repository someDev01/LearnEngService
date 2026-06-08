using Application.Interfaces.Code;

namespace Infrastructure.Services.Code;

public class CodeGenerationService : ICodeGenerationService
{
    private readonly string _availibleChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public string GenerationAsync()
    {
        string code;
        var random = new Random();
        do
        {
            code = new string(Enumerable.Range(0, 5)
                .Select(_ => _availibleChars[random.Next(0, _availibleChars.Length)])
                .ToArray());
        }
        while (!IsValidCode(code));

        return code;
    }

    private static bool IsValidCode(string code)
    {
        if (IsDigit(code) && !IsLetter(code)) return false;
        if (!IsDigit(code) && IsLetter(code)) return false;

        return true;
    }
    private static bool IsDigit(string code) => code.Any(char.IsDigit);
    private static bool IsLetter(string code) => code.Any(char.IsLetter);
}
