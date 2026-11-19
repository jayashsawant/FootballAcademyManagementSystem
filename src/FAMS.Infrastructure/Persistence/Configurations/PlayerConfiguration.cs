using FAMS.Domain.People;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FAMS.Infrastructure.Persistence.Configurations;

public sealed class PlayerConfiguration
    : IEntityTypeConfiguration<Player>
{
    public void Configure(
        EntityTypeBuilder<Player> builder)
    {
        builder.ToTable("Players");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.PlayerNumber)
            .IsRequired()
            .HasMaxLength(30);

        builder.HasIndex(p => p.PlayerNumber)
            .IsUnique();

        builder.Property(p => p.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Position)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(p => p.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(p => p.MonthlyFeeOverride)
            .HasColumnType("decimal(18,2)");

        builder.HasIndex(p => p.ParentId);

        builder.HasMany(p => p.Enrollments)
            .WithOne()
            .HasForeignKey(e => e.PlayerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}