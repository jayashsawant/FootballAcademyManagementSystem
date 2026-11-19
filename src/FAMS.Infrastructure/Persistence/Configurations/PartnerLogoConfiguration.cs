using FAMS.Domain.Content;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FAMS.Infrastructure.Persistence.Configurations;

public class PartnerLogoConfiguration : IEntityTypeConfiguration<PartnerLogo>
{
    public void Configure(EntityTypeBuilder<PartnerLogo> builder)
    {
        builder.ToTable("PartnerLogos");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.LogoUrl).IsRequired().HasMaxLength(500);
        builder.Property(p => p.LinkUrl).HasMaxLength(500);
    }
}