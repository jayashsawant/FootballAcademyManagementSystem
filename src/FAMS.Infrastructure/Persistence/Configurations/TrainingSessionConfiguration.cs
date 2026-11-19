using FAMS.Domain.Training;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FAMS.Infrastructure.Persistence.Configurations;

public class TrainingSessionConfiguration : IEntityTypeConfiguration<TrainingSession>
{
    public void Configure(EntityTypeBuilder<TrainingSession> builder)
    {
        builder.ToTable("TrainingSessions");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(t => t.Notes).HasMaxLength(1000);

        builder.HasIndex(t => new { t.BranchId, t.SessionDate });
        builder.HasIndex(t => t.CoachId);

        builder.HasMany(t => t.AttendanceRecords)
            .WithOne()
            .HasForeignKey(a => a.TrainingSessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(t => t.AttendanceRecords)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}