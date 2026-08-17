using Application.Common.Llm;
using Application.Interfaces.Clients.Llm;
using Application.Interfaces.Llm;
using Application.Settings.Llm;
using Domain.Model.Common;
using Microsoft.Extensions.Options;

namespace Application.Services.Llm;

public class LlmService(
    IOptions<LlmSettings> options,
    IEnumerable<ILlmClient> clients): ILlmService
{
    private readonly LlmSettings _settings = options.Value;
    
    public async Task<Result<string?>> ExecuteAsync(
        LlmProvider provider, 
        LlmPurpose purpose, 
        string prompt, 
        CancellationToken cancellationToken)
    {
        var providerSettings = _settings.Providers
            .FirstOrDefault(s => s.Provider == provider.ToString());
        if (providerSettings is null)
            return Result<string?>.Failure("LLM provider is not configured");
        
        var moderSettings = providerSettings.Models
            .FirstOrDefault(m => m.Purpose == purpose.ToString());
        if (moderSettings is null)
            return Result<string?>.Failure("LLM model for this purpose is not configured");
        
        var client = clients.FirstOrDefault(cl => cl.Provider == provider);
        if (client is null)
            return Result<string?>.Failure("LLM client for this provider is not registered");

        return await client.SendAsync(prompt, moderSettings.Name, cancellationToken);
    }
}