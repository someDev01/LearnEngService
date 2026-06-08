using Domain.Model.Entyties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class SubtitleConfiguration : IEntityTypeConfiguration<Subtitle>
{
    public void Configure(EntityTypeBuilder<Subtitle> builder)
    {
        builder.ToTable("Subtitles");
        builder.HasKey(x => x.Id);

        #region PROPERTIES
        builder.Property(p => p.FileKey)
            .HasColumnName("File")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(p => p.Format)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(p => p.LanguageCode)
            .HasColumnName("LanguageCode")
            .HasMaxLength(20)
            .IsRequired();
        #endregion

        builder.HasIndex(s => new
        {
            s.VideoId,
            s.LanguageCode
        }).IsUnique();
    }
}
