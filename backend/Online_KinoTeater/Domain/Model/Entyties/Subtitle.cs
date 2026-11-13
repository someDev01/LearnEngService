using Domain.Model.Common;
using Domain.Model.Enum;
using Domain.Model.ValueObjects;

namespace Domain.Model.Entyties;

public class Subtitle : Entity
{
    public Guid VideoId { get; private set; }

    public LanguageCode LanguageCode { get; private set; }

    public string FileKey { get; private set; }

    public SubtitleFormat Format { get; private set; }

    private Subtitle() { }

    private Subtitle(
        Guid videoId,
        LanguageCode languageCode,
        string fileUrl,
        SubtitleFormat format)
    {
        VideoId = videoId;
        LanguageCode = languageCode;
        FileKey = fileUrl;
        Format = format;
    }

    public static Result<Subtitle> Create(
        Guid videoId,
        LanguageCode languageCode,
        string fileUrl,
        SubtitleFormat format)
    {
        if (!System.Enum.IsDefined(languageCode))
            return Result<Subtitle>.Failure("Языковые дорожки для субтитров не определены");

        if (string.IsNullOrEmpty(fileUrl))
            return Result<Subtitle>.Failure("Файл субтитров не указан");

        return Result<Subtitle>.Success(new Subtitle(videoId, languageCode, fileUrl, format));
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
