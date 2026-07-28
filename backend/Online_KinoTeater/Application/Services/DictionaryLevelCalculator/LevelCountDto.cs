using Domain.Model.ValueObjects;

namespace Application.Services.DictionaryLevelCalculator;

public record LevelCountDto(
    Lvl Lvl,
    int Count);