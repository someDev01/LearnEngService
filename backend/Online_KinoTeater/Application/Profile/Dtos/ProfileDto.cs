using Domain.Model.Enum;

namespace Application.Profile.Dtos;

public record ProfileDto(
    int TrainedCount,
    int NotesCount,
    int VideosCount,
    EnglishLevel EnglishLevel,
    IReadOnlyList<LastActivityDto?> Activities);