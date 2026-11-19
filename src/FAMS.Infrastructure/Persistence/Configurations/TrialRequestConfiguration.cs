using FAMS.Domain.Content;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FAMS.Infrastructure.Persistence.Configurations;

public class TrialRequestConfiguration : IEntityTypeConfiguration<TrialRequest>
{
    public void Configure(EntityTypeBuilder<TrialRequest> builder)
    {
        builder.ToTable("TrialRequests");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name).IsRequired().HasMaxLength(100);
        builder.Property(t => t.Email).IsRequired().HasMaxLength(256);
        builder.Property(t => t.Mobile).IsRequired().HasMaxLength(20);
        builder.Property(t => t.Notes).HasMaxLength(1000);
        builder.Property(t => t.Status).HasConversion<string>().HasMaxLength(20);

        builder.HasIndex(t => t.Status);
    }
}