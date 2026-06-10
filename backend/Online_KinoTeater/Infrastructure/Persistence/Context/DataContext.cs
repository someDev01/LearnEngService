using Application.Interfaces.Context;
using Domain.Model.Entyties;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Infrastructure.Persistence.Context;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options), IDataContext
{
    public DbSet<YoutubeVideo> YoutubeVideos { get; set; }

    public DbSet<Subtitle> Subtitles { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<Note> Notes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
    }
}
