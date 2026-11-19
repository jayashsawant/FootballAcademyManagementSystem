using FAMS.Domain.Academy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FAMS.Infrastructure.Persistence.Configurations;

public sealed class BatchConfiguration : IEntityTypeConfiguration<Batch>
{
    public void Configure(EntityTypeBuilder<Batch> builder)
    {
        builder.ToTable("Batches");

        // Primary Key
        builder.HasKey(b => b.Id);

        // Academy
        builder.Property(b => b.AcademyId)
            .IsRequired();

        builder.HasIndex(b => b.AcademyId);

        // Branch
        builder.Property(b => b.BranchId)
            .IsRequired();

        builder.HasIndex(b => b.BranchId);

        // Season
        builder.Property(b => b.SeasonId)
            .IsRequired();

        builder.HasIndex(b => b.SeasonId);

        // Age Group
        builder.Property(b => b.AgeGroupId)
            .IsRequired();

        builder.HasIndex(b => b.AgeGroupId);

        // Name
        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(100);

        // A batch name should be unique within:
        // Academy + Branch + Season + AgeGroup
        builder.HasIndex(b => new
        {
            b.AcademyId,
            b.BranchId,
            b.SeasonId,
            b.AgeGroupId,
            b.Name
        })
        .IsUnique();

        // Academy relationship
        builder.HasOne<Domain.Academy.Academy>()
            .WithMany()
            .HasForeignKey(b => b.AcademyId)
            .OnDelete(DeleteBehavior.Restrict);

        // Branch relationship
        builder.HasOne<Branch>()
            .WithMany()
            .HasForeignKey(b => b.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        // Season relationship
        builder.HasOne<Season>()
            .WithMany()
            .HasForeignKey(b => b.SeasonId)
            .OnDelete(DeleteBehavior.Restrict);

        // Age Group relationship
        builder.HasOne<AgeGroup>()
            .WithMany()
            .HasForeignKey(b => b.AgeGroupId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}