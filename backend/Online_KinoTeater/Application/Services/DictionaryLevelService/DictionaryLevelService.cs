using Application.Interfaces.Context;
using Application.Interfaces.DictionaryLevelCalculator;
using Application.Interfaces.DictionaryLevelService;
using Application.Services.DictionaryLevelCalculator;
using Domain.Model.Enum;
using Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.DictionaryLevelService;

public class DictionaryLevelService(
    IDictionaryLevelCalculator dictionaryLevelCalculator,
    IDataContext context): IDictionaryLevelService
{
    public async Task<EnglishLevel> GetLevelAsync(Guid userId, CancellationToken cancellationToken)
    {
        var levelsCount = await context.Notes
            .Where(n => n.UserId == userId)
            .GroupBy(g => g.Lvl.Value)
            .Select(g => new
            {
                Level = g.Key,
                Count = g.Count()
            })
            .ToListAsync(cancellationToken);
        
        var dto = levelsCount
            .Select(l => new LevelCountDto(
                Lvl.Create(l.Level).Value,
                l.Count))
            .ToList();

        return dictionaryLevelCalculator.Calculate(dto);
    }
}