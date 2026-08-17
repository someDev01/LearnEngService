using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Application.Common.Llm;
using Application.Interfaces.Clients.Llm;
using Domain.Model.Common;
using Infrastructure.Dtos;

namespace Infrastructure.Services.Llm.OpenAiCompatible;

public  abstract class OpenAiCompatibleLlmClient(HttpClient client): ILlmClient
{
    public  abstract LlmProvider Provider { get; }
    public async Task<Result<string?>> SendAsync(string prompt, string model, CancellationToken cancellationToken)
    {
        try
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
            
            if (response.StatusCode == HttpStatusCode.TooManyRequests)
                return Result<string?>.Failure("LLM_RATE_LIMITED");
        
            if (!response.IsSuccessStatusCode)
                return Result<string?>.Failure($"LLM error: {response.StatusCode}");
        
            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var parsed = JsonSerializer.Deserialize<LlmResponse>(json);
            var content = parsed?.choices[0].message?.content.Trim();

            return Result<string?>.Success(content);
        }
        catch (HttpRequestException)
        {
            return Result<string?>.Failure($"Ошибка сети при обращении к {Provider}");
        }
        catch (TaskCanceledException)
        {
            return Result<string?>.Failure("Сервис отвечает слишком долго. Попробуйте еще раз");
        }
    }
}