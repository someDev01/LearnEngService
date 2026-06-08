using Domain.Model.Common;

namespace Application.Interfaces.Clients.Llm;

public interface ILlmClient
{
    Task<Result<string?>> SendAsync(string prompt, CancellationToken cancellationToken);
}
