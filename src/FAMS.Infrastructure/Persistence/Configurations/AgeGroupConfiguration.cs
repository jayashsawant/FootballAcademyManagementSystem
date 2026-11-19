using FAMS.Domain.Academy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FAMS.Infrastructure.Persistence.Configurations;

public sealed class AgeGroupConfiguration : IEntityTypeConfiguration<AgeGroup>
{
    public void Configure(EntityTypeBuilder<AgeGroup> builder)
    {
        builder.ToTable("AgeGroups");

        // Primary Key
        builder.HasKey(a => a.Id);

        // Academy
        builder.Property(a => a.AcademyId)
            .IsRequired();

        builder.HasIndex(a => a.AcademyId);

        // Name
        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(50);

        // One academy should not have duplicate age-group names.
        builder.HasIndex(a => new
        {
            a.AcademyId,
            a.Name
        })
        .IsUnique();

        // Age range
        builder.Property(a => a.MinimumAge)
            .IsRequired(false);

        builder.Property(a => a.MaximumAge)
            .IsRequired(false);

        // Display order
        builder.Property(a => a.SortOrder)
            .IsRequired();

        // Active state
        builder.Property(a => a.IsActive)
            .IsRequired();

        // Academy relationship
        builder.HasOne<Domain.Academy.Academy>()
            .WithMany()
            .HasForeignKey(a => a.AcademyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}