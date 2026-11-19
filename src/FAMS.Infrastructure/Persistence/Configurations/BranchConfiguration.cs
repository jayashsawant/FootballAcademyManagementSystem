using FAMS.Domain.Academy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FAMS.Infrastructure.Persistence.Configurations;

public class BranchConfiguration : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.ToTable("Branches");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.Address)
            .HasMaxLength(500);

        builder.Property(b => b.City)
            .HasMaxLength(100);

        builder.Property(b => b.ContactPhone)
            .HasMaxLength(20);

        builder.Property(b => b.ContactEmail)
            .HasMaxLength(256);

        builder.HasIndex(b => new { b.AcademyId, b.Name });

        builder.Navigation(b => b.Grounds)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(b => b.Grounds)
            .WithOne()
            .HasForeignKey(g => g.BranchId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}