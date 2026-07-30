using Application.Interfaces.Context;
using Application.Interfaces.DictionaryLevelService;
using Application.Interfaces.ProfileQuery;
using Application.Interfaces.Storage;
using Application.Profile.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.ProfileQuery;

public class ProfileQueryService(
    IDictionaryLevelService dictionaryLevelService,
    IFileStorageService fileStorageService,
    IDataContext context): IProfileQueryService
{
    public async Task<ProfileDto> GetAsync(Guid userId, CancellationToken cancellationToken)
    {
        var notesCount = await context.Notes
            .Where(n => n.UserId == userId)
            .CountAsync(cancellationToken);

        var videosCount = await context.YoutubeVideos.CountAsync(cancellationToken);

        var englishLevel = await dictionaryLevelService.GetLevelAsync(userId, cancellationToken);
        
        var lastAddedWord = await context.Notes
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .Select(n => new LastActivityDto(
                "Added",
                n.Word,
                n.CreatedAt))
            .FirstOrDefaultAsync(cancellationToken);

        var lastTrainedWord = await context.Notes   
            .Where(n => n.UserId == userId && n.LastTrainedAt != null)
            .OrderByDescending(n => n.LastTrainedAt)
            .Select(n => new LastActivityDto(
                "Trained",
                n.Word,
                n.LastTrainedAt!.Value))
            .FirstOrDefaultAsync(cancellationToken);
        
        var user = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId,cancellationToken);
        var avatarUrl = string.IsNullOrWhiteSpace(user?.AvatarPath) ? 
            null : fileStorageService.GetPublicUrl(user.AvatarPath);

        var activities = new[]
        {
            lastAddedWord,
            lastTrainedWord,
        }.Where(a => a != null)
        .Select(a => a!)
        .ToList();

        var profileDto = new ProfileDto(
            notesCount,
            videosCount,
            englishLevel,
            activities,
            avatarUrl!.Value);

        return profileDto;
    }
}