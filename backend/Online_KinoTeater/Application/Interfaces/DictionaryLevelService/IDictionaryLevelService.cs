using Domain.Model.Enum;

namespace Application.Interfaces.DictionaryLevelService;

public interface IDictionaryLevelService
{
    Task<EnglishLevel> GetLevelAsync(Guid userId, CancellationToken cancellationToken);
}