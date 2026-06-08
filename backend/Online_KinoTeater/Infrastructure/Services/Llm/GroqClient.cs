using Application.Common.Cache;
using Application.Common.Ttl;
using Application.Interfaces.CacheService;
using Application.Interfaces.Clients.Llm;
using Domain.Model.Common;
using Infrastructure.Dtos;
using Infrastructure.Settings.Llm;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Infrastructure.Services.Llm;

public class GroqClient(
    HttpClient client,
    ICacheService cacheService,
    IOptions<LlmSettings> optionsLlm) : ILlmClient
{
    private readonly LlmSettings _settingsLlm = optionsLlm.Value;

    public async Task<Result<string?>> SendAsync(string prompt, CancellationToken cancellationToken)
    {
        try
        {
            if (await IsGlobalBlockedAsync())
                return Result<string?>.Failure("Лимит сервиса превышен. Попробуйте позже");

            var model = await GetAvailableModelAsync();

            if(model is null)
            {
                await SetGlobalBlockedAsync();
                return Result<string?>.Failure("Лимит сервиса превышеню Попробуйте позже");
            }

            var result =  await SendToModelAsync(
                prompt,
                model,
                cancellationToken);

            return result;
        }
        catch (HttpRequestException)
        {
            return Result<string?>.Failure("Ошибка сети при обращении к groq");
        }
        catch (TaskCanceledException)
        {
            return Result<string?>.Failure("Сервис отвечает слишком долго. Попробуйте еще раз");
        }
    }

    private async Task<Result<string?>> SendToModelAsync(string prompt, string model, CancellationToken cancellationToken)
    {
        var uri = "chat/completions";
        var body = new
        {
            model,
            messages = new[]
                {
                    new
                    {
                        role = "user",
                        content = prompt
                    }
                },
        };

        var response = await client.PostAsJsonAsync(uri, body, cancellationToken);

        if (IsRateLimitResponse(response))
        {
            await MarkModelAsBlockedAsync(model);

            return await RetryWithNextModelAsync(prompt, cancellationToken);
        }

        if (!response.IsSuccessStatusCode)
        {
            return Result<string?>.Failure($"LLM error: {response.StatusCode}");
        }
        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        var parsed = JsonSerializer.Deserialize<GroqResponse>(json);
        var content = parsed?.choices[0].message?.content.Trim();

        return Result<string?>.Success(content);
    }

    private async Task<Result<string?>> RetryWithNextModelAsync(string prompt, CancellationToken cancellationToken)
    {
        var model = await GetAvailableModelAsync();

        if(model is null)
        {
            await SetGlobalBlockedAsync();

            return Result<string?>.Failure("Лимит сервиса превышен. Попробуйте позже");
        }

        var result = await SendToModelAsync(prompt, model, cancellationToken);
        return result;
    }

    private async Task<string> GetAvailableModelAsync()
    {
        foreach(var model in _settingsLlm.Models)
        {
            Console.WriteLine($"model: {model}");
            bool isBlocked = await IsModelBlockedAsync(model);

            if (!isBlocked)
                return model;
        }

        return null!;
    }

    private async Task<bool> IsModelBlockedAsync(string model)
    {
        var modelLimitKey = CacheKeyBuilder.BuildLlmModelLimitKey(model);
        return await cacheService.KeyExistsAsync(modelLimitKey);
    }
    private async Task MarkModelAsBlockedAsync(string model)
    {
        var modelLimitKey = CacheKeyBuilder.BuildLlmModelLimitKey(model);
        await cacheService.SetAsync(
            modelLimitKey,
            "modelBlocked",
            TtlHelper.UntilNextUtcTime());
    }

    private async Task<bool> IsGlobalBlockedAsync()
    {
        var globalLimitKey = CacheKeyBuilder.BuildLlmGlobalLimitsKey();
        return await cacheService.KeyExistsAsync(globalLimitKey);
    }
    private async Task SetGlobalBlockedAsync()
    {
        var globalLimitKey = CacheKeyBuilder.BuildLlmGlobalLimitsKey();
        await cacheService.SetAsync(
            globalLimitKey,
            "globalBlocked",
            TtlHelper.UntilNextUtcTime());
    }

    private bool IsRateLimitResponse(HttpResponseMessage response) =>
        response.StatusCode == HttpStatusCode.TooManyRequests;
}
