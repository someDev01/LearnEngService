using Application.Common.Llm;
using Infrastructure.Services.Llm.OpenAiCompatible;

namespace Infrastructure.Services.Llm.Groq;

public class GroqClient(HttpClient client) : OpenAiCompatibleLlmClient(client)
{
    public override LlmProvider Provider => LlmProvider.Groq;
}
