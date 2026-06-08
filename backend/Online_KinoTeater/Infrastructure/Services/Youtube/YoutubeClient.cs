using Application.Interfaces.Clients.Youtube;
using Application.InternalDtos.Youtube;
using Infrastructure.Dtos;
using Infrastructure.Settings.Youtube;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Xml;

namespace Infrastructure.Services.Youtube;

public class YoutubeClient(
    HttpClient client,
    IOptions<YoutubeApiSettings> options) : IYoutubeClient
{
    private readonly YoutubeApiSettings _settings = options.Value;

    public async Task<YoutubeVideoApiResponse?> GetVideoAsync(string youtubeVideoId)
    {
        var url = $"?id={youtubeVideoId}&" +
            $"part=snippet,contentDetails&" +
            $"key={_settings.ApiKey}";

        var response = await client.GetAsync(url);
        if (!response.IsSuccessStatusCode)
            return null!;

        var data = await response.Content.ReadFromJsonAsync<YoutubeApiResponse>();

        var item = data?.Items.FirstOrDefault();
        var (Hours, Minutes, Seconds) = ParseDuration(item!.ContentDetails.Duration);

        var result = new YoutubeVideoApiResponse(
            item.Snippet.Title,
            Hours,
            Minutes,
            Seconds);

        return result;
    }

    private (int Hours, int Minutes, int Seconds) ParseDuration(string isoDuration)
    {
        var timeSpan = XmlConvert.ToTimeSpan(isoDuration);
        return (timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
    }
}
