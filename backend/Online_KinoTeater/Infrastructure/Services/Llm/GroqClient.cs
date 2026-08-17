using Application.Interfaces.Clients.Llm;
using Domain.Model.Common;
using Infrastructure.Dtos;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Application.Common.Llm;

namespace Infrastructure.Services.Llm;

public class GroqClient(
    HttpClient client) : ILlmClient
{
    public LlmProvider Provider => LlmProvider.Groq;

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
            var parsed = JsonSerializer.Deserialize<GroqResponse>(json);
            var content = parsed?.choices[0].message?.content.Trim();
            Console.WriteLine(content);

            return Result<string?>.Success(content);
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
}
