using Application.Interfaces.Context;
using Domain.Model.Entyties;
using Domain.Repositories.Video;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Video;

public class VideoRepository(IDataContext context) : IVideoRepository
{
    public async Task<YoutubeVideo?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        await context.YoutubeVideos
            .FirstOrDefaultAsync(yv => yv.Id == id, cancellationToken);

    public async Task AddAsync(YoutubeVideo video, CancellationToken cancellationToken) => 
        await context.YoutubeVideos .AddAsync(video, cancellationToken);    

    public Task DeleteAsync(YoutubeVideo video, CancellationToken cancellationToken)
    {
        context.YoutubeVideos.Remove(video);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(string youtubeId, CancellationToken cancellationToken) =>
        await context.YoutubeVideos
            .AnyAsync(yv => yv.YoutubeId == youtubeId, cancellationToken);
}
