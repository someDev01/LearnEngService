using Application.Common.Llm;
using Infrastructure.Services.Llm.OpenAiCompatible;

namespace Infrastructure.Services.Llm.OpenRouter;

public class OpenRouterClient(HttpClient client): OpenAiCompatibleLlmClient(client)
{
    public override LlmProvider Provider => LlmProvider.OpenRouter;
}