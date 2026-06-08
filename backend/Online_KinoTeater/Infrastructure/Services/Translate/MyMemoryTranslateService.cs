using Application.Common.Cache;
using Application.Interfaces.CacheService;
using Application.Interfaces.Translate;
using Application.Settings.Cache;
using Domain.Model.Common;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Infrastructure.Services.Translate;

public class MyMemoryTranslateService(
    HttpClient client,
    ICacheService cacheService,
    IOptions<CacheTtlSettings> optionsCacheKeys) : ITranslateService
{
    private readonly CacheTtlSettings _settingsCacheKeys = optionsCacheKeys.Value;

    public async Task<Result<string?>> TranslateAsync(string word, CancellationToken cancellationToken)
    {
        var keyTranslation = CacheKeyBuilder.BuildTranslateWordKey(word);
        TimeSpan ttlKeyTranslation = TimeSpan.FromHours(_settingsCacheKeys.TranslationTtlHours);

        #region CACHING 
        var cachedTranslations = await cacheService.GetByKeyAsync<string>(keyTranslation);
        if(cachedTranslations is not null)
            return Result<string?>.Success(cachedTranslations);
        #endregion

        #region RESPONSE
        var url = $"get?q={word}&langpair=en|ru";

        var response = await client.GetAsync(url, cancellationToken);
        if (!response.IsSuccessStatusCode)
            return Result<string?>.Failure($"Translation error: {response.StatusCode}");

        var json = await response.Content.ReadAsStringAsync(cancellationToken);

        using var doc = JsonDocument.Parse(json);

        string? translation = doc.RootElement
            .GetProperty("responseData")
            .GetProperty("translatedText")
            .GetString();
        if (string.IsNullOrEmpty(translation))
            return Result<string?>.Success(null);
        #endregion

        #region SET KEY
        await cacheService.SetAsync(
            keyTranslation,
            translation,
            ttlKeyTranslation);
        #endregion

        return Result<string?>.Success(translation);        
    }
}
