using Application.Common.Llm;
using Domain.Model.Common;

namespace Application.Interfaces.Clients.Llm;

public interface ILlmClient
{
    LlmProvider Provider { get; }
    Task<Result<string?>> SendAsync(string prompt, string model, CancellationToken cancellationToken);
}
