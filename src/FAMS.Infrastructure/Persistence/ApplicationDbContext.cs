using FAMS.Domain.Academy;
using FAMS.Domain.Billing;
using FAMS.Domain.Coaches;
using FAMS.Domain.Common;
using FAMS.Domain.Content;
using FAMS.Domain.Identity;
using FAMS.Domain.People;
using FAMS.Domain.Training;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Reflection;

namespace FAMS.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    private readonly IPublisher _publisher;

    private static readonly ValueConverter<DateTime, DateTime> UtcConverter = new(
        v => v.Kind == DateTimeKind.Utc ? v : DateTime.SpecifyKind(v, DateTimeKind.Utc),
        v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

    private static readonly ValueConverter<DateTime?, DateTime?> UtcConverterNullable = new(
        v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v,
        v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IPublisher publisher)
        : base(options)
    {
        _publisher = publisher;
    }

    // =========================
    // Identity
    // =========================

    public DbSet<Role> Roles => Set<Role>();

    public DbSet<User> Users => Set<User>();

    public DbSet<RefreshToken> RefreshTokens =>
        Set<RefreshToken>();


    // =========================
    // Academy
    // =========================

    public DbSet<Domain.Academy.Academy> Academies =>
        Set<Domain.Academy.Academy>();

    public DbSet<Branch> Branches =>
        Set<Branch>();

    public DbSet<Ground> Grounds =>
        Set<Ground>();

    public DbSet<Season> Seasons =>
        Set<Season>();

    public DbSet<NumberSequence> NumberSequences =>
        Set<NumberSequence>();

    public DbSet<AgeGroup> AgeGroups =>
        Set<AgeGroup>();

    public DbSet<Batch> Batches =>
        Set<Batch>();

    public DbSet<Team> Teams =>
        Set<Team>();


    // =========================
    // People
    // =========================

    public DbSet<Coach> Coaches =>
        Set<Coach>();

    public DbSet<Parent> Parents =>
        Set<Parent>();

    public DbSet<Player> Players =>
        Set<Player>();

    public DbSet<PlayerEnrollment> PlayerEnrollments =>
        Set<PlayerEnrollment>();


    // =========================
    // Billing
    // =========================

    public DbSet<Invoice> Invoices =>
        Set<Invoice>();

    public DbSet<Payment> Payments =>
        Set<Payment>();


    // =========================
    // Training
    // =========================

    public DbSet<TrainingSession> TrainingSessions =>
        Set<TrainingSession>();

    public DbSet<AttendanceRecord> AttendanceRecords =>
        Set<AttendanceRecord>();


    // =========================
    // Content
    // =========================

    public DbSet<SiteSettings> SiteSettings =>
        Set<SiteSettings>();

    public DbSet<GalleryImage> GalleryImages =>
        Set<GalleryImage>();

    public DbSet<NewsPost> NewsPosts =>
        Set<NewsPost>();

    public DbSet<TrialRequest> TrialRequests =>
        Set<TrialRequest>();

    public DbSet<PartnerLogo> PartnerLogos =>
        Set<PartnerLogo>();


    // =========================
    // Model Configuration
    // =========================

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            Assembly.GetExecutingAssembly());

        ApplySoftDeleteQueryFilters(modelBuilder);

        ApplyUtcDateTimeConverters(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }


    // =========================
    // Soft Delete Query Filters
    // =========================

    private static void ApplySoftDeleteQueryFilters(
        ModelBuilder modelBuilder)
    {
        foreach (var entityType
                 in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(SoftDeletableEntity)
                .IsAssignableFrom(entityType.ClrType))
            {
                continue;
            }

            var parameter =
                System.Linq.Expressions.Expression.Parameter(
                    entityType.ClrType,
                    "e");

            var property =
                System.Linq.Expressions.Expression.Property(
                    parameter,
                    nameof(SoftDeletableEntity.IsDeleted));

            var condition =
                System.Linq.Expressions.Expression.Equal(
                    property,
                    System.Linq.Expressions.Expression.Constant(false));

            var lambda =
                System.Linq.Expressions.Expression.Lambda(
                    condition,
                    parameter);

            modelBuilder
                .Entity(entityType.ClrType)
                .HasQueryFilter(lambda);
        }
    }


    // =========================
    // UTC DateTime Converters
    // =========================
    // Npgsql requires every DateTime written to a "timestamp with time zone"
    // column to have Kind = Utc. Rather than fixing every DateTime literal
    // across the codebase individually, this applies a single converter to
    // every DateTime / DateTime? property on every entity, forcing the Kind
    // tag to Utc without shifting the actual date/time value.

    private static void ApplyUtcDateTimeConverters(
        ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime))
                {
                    property.SetValueConverter(UtcConverter);
                }
                else if (property.ClrType == typeof(DateTime?))
                {
                    property.SetValueConverter(UtcConverterNullable);
                }
            }
        }
    }


    // =========================
    // Save Changes
    // =========================

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        ApplyAuditInformation();

        var domainEvents =
            CollectDomainEvents();

        var result =
            await base.SaveChangesAsync(
                cancellationToken);

        await DispatchDomainEventsAsync(
            domainEvents,
            cancellationToken);

        return result;
    }


    // =========================
    // Audit Information
    // =========================

    private void ApplyAuditInformation()
    {
        var entries =
            ChangeTracker.Entries<AuditableEntity>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    // CreatedAt is handled by
                    // AuditableEntity.
                    break;

                case EntityState.Modified:

                    entry.Entity.MarkAsUpdated(
                        userId: null);

                    break;
            }
        }
    }


    // =========================
    // Domain Events
    // =========================

    private List<IDomainEvent> CollectDomainEvents()
    {
        var entitiesWithEvents =
            ChangeTracker
                .Entries<BaseEntity>()
                .Select(entry => entry.Entity)
                .Where(entity =>
                    entity.DomainEvents.Count > 0)
                .ToList();

        var domainEvents =
            entitiesWithEvents
                .SelectMany(entity =>
                    entity.DomainEvents)
                .ToList();

        foreach (var entity in entitiesWithEvents)
        {
            entity.ClearDomainEvents();
        }

        return domainEvents;
    }


    private async Task DispatchDomainEventsAsync(
        List<IDomainEvent> domainEvents,
        CancellationToken cancellationToken)
    {
        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(
                new DomainEventNotification(
                    domainEvent),
                cancellationToken);
        }
    }
}


public sealed class DomainEventNotification
    : INotification
{
    public IDomainEvent DomainEvent { get; }

    public DomainEventNotification(
        IDomainEvent domainEvent)
    {
        DomainEvent = domainEvent;
    }
}