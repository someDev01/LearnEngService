using Domain.Model.Common;

namespace Domain.Model.ValueObjects;

public record Transcription(string Value): ValueObject
{
    public static Result<Transcription> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) 
            return Result<Transcription>.Failure("Транскрипция не указана");

        return Result<Transcription>.Success(new Transcription(value));
    }
}
