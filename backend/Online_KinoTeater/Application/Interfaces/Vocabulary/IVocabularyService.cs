using Application.InternalDtos.Translated;
using Application.Requests.Vocabulary;
using Domain.Model.Common;

namespace Application.Interfaces.Vocabulary;

public interface IVocabularyService
{
    Task<Result<NoteDataDto?>> GenerationTranslateAsync(
        VocabularyRequestDto request,
        CancellationToken cancellationToken);
}
