using Application.Interfaces.Context;
using Domain.Repositories.Subtitle;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Subtitle;

public class SubtitleRepository(IDataContext context) : ISubtitleRepository
{
    public async Task<Domain.Model.Entyties.Subtitle?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        await context.Subtitles
            .AsSplitQuery()
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

    public async Task AddAsync(Domain.Model.Entyties.Subtitle subtitle, CancellationToken cancellationToken) => 
        await context.Subtitles.AddAsync(subtitle, cancellationToken);
    
    public Task DeleteAsync(Domain.Model.Entyties.Subtitle subtitle, CancellationToken cancellationToken)
    {
        context.Subtitles.Remove(subtitle);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(
        Guid contentId,
        string language,
        CancellationToken cancellationToken) =>
            await context.Subtitles
                .AnyAsync(s => s.VideoId == contentId &&
                    s.LanguageCode == language,
                    cancellationToken);
}
