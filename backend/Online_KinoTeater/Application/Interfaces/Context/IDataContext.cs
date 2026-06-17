using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces.Context;

public interface IDataContext
{
    DbSet<Domain.Model.Entyties.YoutubeVideo> YoutubeVideos { get; set; }

    DbSet<Domain.Model.Entyties.Subtitle> Subtitles { get; set; }

    DbSet<Domain.Model.Entyties.User> Users { get; set; }

    DbSet<Domain.Model.Entyties.Note> Notes { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
