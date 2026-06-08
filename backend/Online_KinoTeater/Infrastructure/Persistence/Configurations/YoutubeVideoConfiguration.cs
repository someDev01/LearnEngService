using Domain.Model.Entyties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class YoutubeVideoConfiguration : IEntityTypeConfiguration<YoutubeVideo>
{
    public void Configure(EntityTypeBuilder<YoutubeVideo> builder)
    {
        builder.ToTable("YoutubeVideos");
        builder.HasKey(x => x.Id);

        #region PROPERTIES
        builder.Property(p => p.YoutubeId)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(p => p.Title)
            .HasMaxLength(100)
            .IsRequired();
        #endregion

        #region VALUEOBJECTS/ONE
        builder.OwnsOne(yv => yv.LexicalComplexity, l =>
        {
            l.Property(p => p.Value)
                .HasColumnName("Complexity")
                .HasMaxLength(20)
                .IsRequired();
        });

        builder.OwnsOne(yv => yv.Duration, d =>
        {
            d.Property(p => p.Hour)
                .HasColumnName("DurationHour");

            d.Property(p => p.Minutes)
                .HasColumnName("DurationMinutes");

            d.Property(p => p.Seconds)
                .HasColumnName("DurationSeconds");
        });
        #endregion

        #region ENTITY MANY 
        builder
            .HasMany<Subtitle>()
            .WithOne()
            .HasForeignKey(s => s.VideoId)
            .OnDelete(DeleteBehavior.Cascade);
        #endregion

        builder
            .HasIndex(yv => yv.YoutubeId)
            .IsUnique();
    }
}
