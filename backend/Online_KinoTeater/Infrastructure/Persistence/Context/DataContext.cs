using Application.Interfaces.Context;
using Domain.Model.Entyties;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Infrastructure.Persistence.Context;

public class DataContext(IConfiguration configuration) : DbContext, IDataContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("DataBase"));
        builder.EnableDynamicJson();

        optionsBuilder
            //.UseNpgsql(configuration.GetConnectionString("DataBase"))
            .UseNpgsql(builder.Build())
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors()
            .LogTo(Console.WriteLine, LogLevel.Information);
    }

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
