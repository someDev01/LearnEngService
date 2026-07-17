using Application.Interfaces.Context;
using Application.Interfaces.ProfileQuery;
using Application.Profile.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.ProfileQuery;

public class ProfileQueryService(IDataContext context): IProfileQueryService
{
    public async Task<ProfileDto> GetAsync(Guid userId, CancellationToken cancellationToken)
    {
        var today = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);

        var addedNotesCount = await context.Notes
            .Where(n => n.UserId == userId &&  n.CreatedAt >= today && n.CreatedAt < tomorrow)
            .CountAsync(cancellationToken);

        var trainedNotesCount = await context.Notes
            .Where(n => n.UserId == userId && n.LastTrainedAt >= today && n.LastTrainedAt < tomorrow)
            .CountAsync(cancellationToken);

        var notesCount = await context.Notes
            .Where(n => n.UserId == userId)
            .CountAsync(cancellationToken);

        var videosCount = await context.YoutubeVideos.CountAsync(cancellationToken);

        var profileDto = new ProfileDto(
            addedNotesCount,
            trainedNotesCount,
            notesCount,
            videosCount);

        return profileDto;
    }
}