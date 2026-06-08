
using Domain.Model.Common;

namespace Domain.Model.ValueObjects;

public record Language(string Code): ValueObject
{
    private static readonly string[] _languages = ["en", "ru"];

    public static Result<Language> Create(string code)
    {
        var codeLowerCase = code.ToLower();

        if (string.IsNullOrEmpty(codeLowerCase))
            return Result<Language>.Failure("Не добавлен код языка для субтитров!");

        if(!_languages.Contains(codeLowerCase))
            return Result<Language>.Failure("Неккоректно заданы языковые дорожки. Допустимы: En и Ru!");

        return Result<Language>.Success(new Language(codeLowerCase));    
    }
}
