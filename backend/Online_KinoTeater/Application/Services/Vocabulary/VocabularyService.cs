using Application.Common.Prompt;
using Application.Interfaces.Vocabulary;
using Application.InternalDtos.Translated;
using Application.Requests.Vocabulary;
using Domain.Model.Common;
using System.Text.Json;
using Application.Interfaces.Llm;

namespace Application.Services.Vocabulary;

public class VocabularyService(ILlmService llmService) : IVocabularyService
{
    public async Task<Result<NoteDataDto?>> GenerateAsync(
        VocabularyRequestDto request,
        CancellationToken cancellationToken)
    {
        var prompt = PromptBuilder.Build(
            request.Text,
            request.Context,
            request.Translations!,
            request.Example);

        var response = await llmService.ExecuteAsync(
            Common.Llm.LlmProvider.Groq,
            Common.Llm.LlmPurpose.Note,
            prompt,
            cancellationToken);
        if (!response.IsSuccess)
            return Result<NoteDataDto?>.Failure(response.Error!);
        
        try
        {
            var result = JsonSerializer.Deserialize<NoteDataDto>(response.Value!);
            return Result<NoteDataDto?>.Success(result);
        }
        catch(Exception ex)
        {
            return Result<NoteDataDto?>.Failure($"{ex.Message}");
        }
    }
}
