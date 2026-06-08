using Domain.Model.Common;
using Domain.Model.ValueObjects;

namespace Domain.Model.Entyties;

public class Subtitle : Entity
{
    public Guid VideoId { get; private set; }

    public string LanguageCode { get; private set; }

    public Language Language => new(LanguageCode);

    public string FileKey { get; private set; }

    public SubtitleFormat Format { get; private set; }

    private Subtitle() { }

    private Subtitle(
        Guid videoId,
        Language language,
        string fileUrl,
        SubtitleFormat format)
    {
        VideoId = videoId;
        LanguageCode = language.Code;
        FileKey = fileUrl;
        Format = format;
    }

    public static Result<Subtitle> Create(
        Guid videoId,
        Language language,
        string fileUrl,
        SubtitleFormat format)
    {
        if (language is null)
            return Result<Subtitle>.Failure("Язык субтитров не указан!");

        if (string.IsNullOrEmpty(fileUrl))
            return Result<Subtitle>.Failure("Файл субититров не указан");

        return Result<Subtitle>.Success(new Subtitle(videoId, language, fileUrl, format));
    }

    #region UPDATE PROPERTIES
    public Result UpdateFile(string file)
    {
        if (string.IsNullOrWhiteSpace(file))
            return Result.Failure("Файл не указан для изменения");

        FileKey = file;
        return Result.Success();
    }

    public Result UpdateFormat(SubtitleFormat format)
    {
        Format = format;
        return Result.Success();
    }
    #endregion
}
