using Domain.Model.Entyties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class NoteConfiguration : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.ToTable("Notes");
        builder.HasKey(n => n.Id);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        #region PROPERTIES
        builder.Property(n => n.Word)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(n => n.Translations)
            .HasColumnType("text[]")
            .IsRequired();

        #endregion

        #region VALUEOBJECTS/ONE 
        builder.OwnsOne(n => n.Transcription, t =>
        {
            t.Property(p => p.Value)
                .HasColumnName("Transcription")
                .HasMaxLength(50)
                .IsRequired();
        });

        builder.OwnsOne(n => n.Lvl, l =>
        {
            l.Property(p => p.Value)
                .HasColumnName("Lvl")
                .HasMaxLength(2)
                .IsRequired();
        });

        builder.OwnsOne(n => n.Source, s =>
        {
            s.Property(p => p.YoutubeVideoTitle)
                .HasColumnName("YoutubeVideoTitle")
                .HasMaxLength(100)
                .IsRequired();

            s.Property(p => p.Context)
                .HasMaxLength(100)
                .IsRequired();

            s.Property(p => p.Hours)
                .HasColumnName("DurationContextHours");
            s.Property(p => p.Minutes)
                .HasColumnName("DurationContextMinutes");
            s.Property(p => p.Seconds)
                .HasColumnName("DurationContextSeconds");
        });


        #endregion

        #region VALUEOBJECTS/MANY
        builder.OwnsMany(n => n.Examples, ex =>
        {
            ex.ToTable("NoteExamples");

            ex.Property<Guid>("Id");
            ex.HasKey("Id");

            ex.WithOwner().HasForeignKey("NoteId");

            ex.Property(p => p.Text)
                .HasMaxLength(100);
            ex.Property(p => p.Translate)
                .HasMaxLength(100);
        });
        #endregion
    }
}
