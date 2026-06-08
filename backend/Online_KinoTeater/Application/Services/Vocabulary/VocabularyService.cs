using Application.Common.Prompt;
using Application.Interfaces.Clients.Llm;
using Application.Interfaces.Vocabulary;
using Application.InternalDtos.Translated;
using Application.Requests.Vocabulary;
using Domain.Model.Common;
using System.Text.Json;

namespace Application.Services.Vocabulary;

public class VocabularyService(ILlmClient llmClient) : IVocabularyService
{
    public async Task<Result<NoteDataDto?>> GenerationTranslateAsync(
        VocabularyRequestDto request,
        CancellationToken cancellationToken)
    {
        var prompt = PromptBuilder.Build(
            request.Text,
            request.Context,
            request.IsIncludedTranslations,
            request.IsIncludedExamples);

        var result = await Execute(prompt, cancellationToken);
        return result;
    }

    private async Task<Result<NoteDataDto?>> Execute(string prompt,  CancellationToken cancellationToken)
    {
        var response = await llmClient.SendAsync(prompt, cancellationToken);
        if (!response.IsSuccess) return Result<NoteDataDto?>.Failure(response.Error!);

        if (string.IsNullOrWhiteSpace(response.Value))
            return Result<NoteDataDto?>.Success(null);

        try
        {
            var result = JsonSerializer.Deserialize<NoteDataDto>(
                response!.Value,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return Result<NoteDataDto?>.Success(result);
        }
        catch (JsonException)
        {
            return Result<NoteDataDto?>.Success(null);
        }
    }
}
