using Domain.Model.Common;

namespace Application.Common.YoutubeId;

public static class YoutubeIdExtension
{
    public static Result<string> GetYoutubeId(string url)
    {
        if (string.IsNullOrWhiteSpace(url)) return Result<string>.Failure("URL youtube video не указано");

        try
        {
            var uri = new Uri(url);
            var segments = uri.Segments;

            if (segments.Length >= 3 && segments[1].Trim('/') == "embed")
                return Result<string>.Success(segments[2].Trim('/'));
        }
        catch(UriFormatException)
        {   
            return Result<string>.Failure("Неверный формат url youtube video");
        }

        return Result<string>.Failure("Ссылка не является embed ссылкой Youtube");
    }
}
