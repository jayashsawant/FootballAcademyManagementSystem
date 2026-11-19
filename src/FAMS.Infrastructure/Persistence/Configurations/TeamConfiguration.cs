using FAMS.Domain.Academy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FAMS.Infrastructure.Persistence.Configurations;

public sealed class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.ToTable("Teams");

        // Primary Key
        builder.HasKey(t => t.Id);

        // Academy
        builder.Property(t => t.AcademyId)
            .IsRequired();

        builder.HasIndex(t => t.AcademyId);

        // Branch
        builder.Property(t => t.BranchId)
            .IsRequired();

        builder.HasIndex(t => t.BranchId);

        // Season
        builder.Property(t => t.SeasonId)
            .IsRequired();

        builder.HasIndex(t => t.SeasonId);

        // Age Group
        builder.Property(t => t.AgeGroupId)
            .IsRequired();

        builder.HasIndex(t => t.AgeGroupId);

        // Name
        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(100);

        // Team name must be unique within:
        // Academy + Branch + Season + AgeGroup
        builder.HasIndex(t => new
        {
            t.AcademyId,
            t.BranchId,
            t.SeasonId,
            t.AgeGroupId,
            t.Name
        })
        .IsUnique();

        // Academy relationship
        builder.HasOne<Domain.Academy.Academy>()
            .WithMany()
            .HasForeignKey(t => t.AcademyId)
            .OnDelete(DeleteBehavior.Restrict);

        // Branch relationship
        builder.HasOne<Branch>()
            .WithMany()
            .HasForeignKey(t => t.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        // Season relationship
        builder.HasOne<Season>()
            .WithMany()
            .HasForeignKey(t => t.SeasonId)
            .OnDelete(DeleteBehavior.Restrict);

        // Age Group relationship
        builder.HasOne<AgeGroup>()
            .WithMany()
            .HasForeignKey(t => t.AgeGroupId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}