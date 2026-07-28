using Application.Services.DictionaryLevelCalculator;
using Domain.Model.Enum;

namespace Application.Interfaces.DictionaryLevelCalculator;

public interface IDictionaryLevelCalculator
{
    EnglishLevel Calculate(IReadOnlyList<LevelCountDto> levelsCount);
}