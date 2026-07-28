using Application.Interfaces.DictionaryLevelCalculator;
using Domain.Model.Enum;

namespace Application.Services.DictionaryLevelCalculator;

public class DictionaryLevelCalculator: IDictionaryLevelCalculator
{
    public EnglishLevel Calculate(IReadOnlyList<LevelCountDto> levelsCount)
    {
        if (levelsCount.Count == 0)
            return EnglishLevel.A1;
        
        var totalWordsCount = levelsCount.Sum(l => l.Count);

        var totalLevelWeight = levelsCount.Sum(l =>
        {
            var level = Enum.Parse<EnglishLevel>(l.Lvl.Value!);

            return (int)level * l.Count;
        });

        var averageLevel = (double)totalLevelWeight / totalWordsCount;

        return (EnglishLevel)Math.Round(averageLevel);
    }
}