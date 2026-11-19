using FAMS.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FAMS.Infrastructure.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    private static readonly DateTime SeedTimestamp = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(r => r.Name)
            .IsUnique();

        builder.Property(r => r.Description)
            .HasMaxLength(250);

        builder.HasData(
            new
            {
                Id = WellKnownRoles.AdminId,
                Name = WellKnownRoles.Admin,
                Description = "Full access to all academy operations.",
                CreatedAt = SeedTimestamp
            },
            new
            {
                Id = WellKnownRoles.BranchManagerId,
                Name = WellKnownRoles.BranchManager,
                Description = "Manages operations for a specific branch.",
                CreatedAt = SeedTimestamp
            },
            new
            {
                Id = WellKnownRoles.CoachId,
                Name = WellKnownRoles.Coach,
                Description = "Manages training, attendance, and player development.",
                CreatedAt = SeedTimestamp
            },
            new
            {
                Id = WellKnownRoles.StaffId,
                Name = WellKnownRoles.Staff,
                Description = "General academy staff member.",
                CreatedAt = SeedTimestamp
            },
            new
            {
                Id = WellKnownRoles.FinanceOfficerId,
                Name = WellKnownRoles.FinanceOfficer,
                Description = "Manages fees, invoices, and payments.",
                CreatedAt = SeedTimestamp
            },
            new
            {
                Id = WellKnownRoles.ReceptionistId,
                Name = WellKnownRoles.Receptionist,
                Description = "Handles admissions and front-desk operations.",
                CreatedAt = SeedTimestamp
            },
            new
            {
                Id = WellKnownRoles.ParentId,
                Name = WellKnownRoles.Parent,
                Description = "Parent portal access, limited to own children's data.",
                CreatedAt = SeedTimestamp
            },
            new
            {
                Id = WellKnownRoles.ContentEditorId,
                Name = WellKnownRoles.ContentEditor,
                Description = "Manages public website content: branding, gallery, news, trial requests.",
                CreatedAt = SeedTimestamp
            });
    }
}