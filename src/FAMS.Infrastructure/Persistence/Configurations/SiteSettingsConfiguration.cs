using FAMS.Domain.Content;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FAMS.Infrastructure.Persistence.Configurations;

public class SiteSettingsConfiguration : IEntityTypeConfiguration<SiteSettings>
{
    public void Configure(EntityTypeBuilder<SiteSettings> builder)
    {
        builder.ToTable("SiteSettings");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.LogoUrl).HasMaxLength(500);
        builder.Property(s => s.HeroImageUrl).HasMaxLength(500);
        builder.Property(s => s.HeroHeading).HasMaxLength(200);
        builder.Property(s => s.HeroSubtext).HasMaxLength(500);
        builder.Property(s => s.AboutText).HasMaxLength(4000);
        builder.Property(s => s.FacebookUrl).HasMaxLength(500);
        builder.Property(s => s.InstagramUrl).HasMaxLength(500);
        builder.Property(s => s.YoutubeUrl).HasMaxLength(500);
        builder.Property(s => s.WhatsappUrl).HasMaxLength(500);
        builder.Property(s => s.LinkedinUrl).HasMaxLength(500);
    }
}