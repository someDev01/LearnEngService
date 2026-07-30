using Domain.Model.Enum;

namespace Application.Profile.Dtos;

public record ProfileDto(
    int NotesCount,
    int VideosCount,
    EnglishLevel EnglishLevel,
    IReadOnlyList<LastActivityDto?> Activities,
    string AvatarUrl);