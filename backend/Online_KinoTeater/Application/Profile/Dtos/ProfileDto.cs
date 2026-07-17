namespace Application.Profile.Dtos;

public record ProfileDto(
    int AddedCount,
    int TrainedCount,
    int NotesCount,
    int VideosCount);