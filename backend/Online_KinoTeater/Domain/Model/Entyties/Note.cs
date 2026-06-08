using Domain.Model.Common;
using Domain.Model.ValueObjects;

namespace Domain.Model.Entyties;

public class Note : Entity
{
    public Guid UserId { get; set; }

    public string Word { get; set; } = string.Empty;

    public List<string> Translations { get; set; } = [];

    public Transcription Transcription { get; set; }

    public Lvl? Lvl { get; set; }

    public List<Example> Examples { get; set; } = [];

    public int RepetitionScore { get; set; }

    public Source? Source { get; set; }

    public DateTime? LastTrainedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    protected Note() { }

    private Note(
        Guid userId,
        string word,
        List<string>? translations,
        Transcription transcription,
        List<Example>? examples,
        int repetitionScore,
        Lvl? lvl = null,
        Source? source = null)
    {
        UserId = userId;
        Word = word;
        Translations = translations ?? [];
        Transcription = transcription;
        Examples = examples ?? [];
        RepetitionScore = repetitionScore;
        Lvl = lvl;
        Source = source;
        CreatedAt = DateTime.UtcNow;
        LastTrainedAt = null;
    }

    public static Result<Note> Create(
        Guid userId,
        string word,
        List<string>? translations,
        Transcription transcription,
        List<Example>? examples,
        int repetitionScore,
        Lvl? lvl = null,
        Source? source = null)
    {
        if (string.IsNullOrWhiteSpace(word))
            return Result<Note>.Failure("Слово на английском не указано");

        if (transcription is null)
            return Result<Note>.Failure("Транскрипция слова не может быть пустая");

        if (translations?.Count > 3)
            return Result<Note>.Failure("Переводов слова не может быть  больше 3");

        if (examples?.Count > 3)
            return Result<Note>.Failure("Примеров не может быть больше 3");

        return Result<Note>.Success(new Note(
            userId,
            word,
            translations ?? [],
            transcription,
            examples ?? [],
            repetitionScore,
            lvl,
            source));
    }

    #region UPDATE PROPETIES
    public Result UpdateWord(string word)
    {
        if (string.IsNullOrWhiteSpace(word))
            return Result.Failure("Слово для изменения не указано");

        Word = word;
        return Result.Success();
    }

    public Result UpdateTranslations(List<string> translations)
    {
        if (translations is null || !translations.Any())
            return Result.Failure("Перевод для изменения не указан");

        Translations = translations;
        return Result.Success();
    }

    public Result UpdateRepetitionScore(bool isCorrect)
    {
        if (isCorrect)
            RepetitionScore++;
        else
            RepetitionScore--;

        RepetitionScore = Math.Max(0, RepetitionScore);
        LastTrainedAt = DateTime.UtcNow;

        return Result.Success();
    }
    #endregion

    #region UPDATE VALUEOBJECTS
    public Result UpdateExamples(List<Example> examples)
    {
        if (examples is null || examples.Count == 0)
            return Result.Failure("Пример для изменения не указан");

        Examples = examples ?? [];
        return Result.Success();
    }
    #endregion
}
