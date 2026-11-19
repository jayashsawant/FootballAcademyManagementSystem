using FAMS.Domain.People;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FAMS.Infrastructure.Persistence.Configurations;

public sealed class PlayerEnrollmentConfiguration
    : IEntityTypeConfiguration<PlayerEnrollment>
{
    public void Configure(
        EntityTypeBuilder<PlayerEnrollment> builder)
    {
        builder.ToTable("PlayerEnrollments");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(e => e.EnrollmentDate)
            .IsRequired();

        builder.Property(e => e.IsCurrent)
            .IsRequired();

        builder.HasIndex(e => e.PlayerId);

        builder.HasIndex(e => e.SeasonId);

        builder.HasIndex(e => e.BranchId);

        builder.HasIndex(e => e.AgeGroupId);

        builder.HasIndex(e => e.BatchId);

        builder.HasIndex(e => e.TeamId);

        builder.HasIndex(x => x.PlayerId)
            .HasDatabaseName(
                "IX_PlayerEnrollments_PlayerId_CurrentOnly")
            .IsUnique()
            .HasFilter("\"IsCurrent\" = TRUE");

        builder.HasOne<Player>()
            .WithMany(p => p.Enrollments)
            .HasForeignKey(e => e.PlayerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}