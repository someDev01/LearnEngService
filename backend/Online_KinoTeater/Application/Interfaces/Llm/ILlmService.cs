using Application.Common.Llm;
using Domain.Model.Common;

namespace Application.Interfaces.Llm;

public interface ILlmService
{
    Task<Result<string?>> ExecuteAsync(
        LlmProvider provider,
        LlmPurpose purpose,
        string prompt,
        CancellationToken cancellationToken);
}