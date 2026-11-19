using FAMS.Domain.Content;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FAMS.Infrastructure.Persistence.Configurations;

public class GalleryImageConfiguration : IEntityTypeConfiguration<GalleryImage>
{
    public void Configure(EntityTypeBuilder<GalleryImage> builder)
    {
        builder.ToTable("GalleryImages");
        builder.HasKey(g => g.Id);

        builder.Property(g => g.ImageUrl).IsRequired().HasMaxLength(500);
        builder.Property(g => g.Caption).HasMaxLength(200);
    }
}