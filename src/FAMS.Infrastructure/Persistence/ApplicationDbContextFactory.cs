using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FAMS.Infrastructure.Persistence;

public sealed class ApplicationDbContextFactory
    : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(
        string[] args)
    {
        var environment =
            Environment.GetEnvironmentVariable(
                "ASPNETCORE_ENVIRONMENT")
            ?? "Development";

        var basePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "..",
            "FAMS.API");

        if (!Directory.Exists(basePath))
        {
            basePath =
                Directory.GetCurrentDirectory();
        }

        var configuration =
            new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile(
                    "appsettings.json",
                    optional: false,
                    reloadOnChange: false)
                .AddJsonFile(
                    $"appsettings.{environment}.json",
                    optional: true,
                    reloadOnChange: false)
                .AddEnvironmentVariables()
                .Build();

        var connectionString =
            configuration[
                "Database:ConnectionString"];

        if (string.IsNullOrWhiteSpace(
                connectionString))
        {
            throw new InvalidOperationException(
                "Database connection string was not found.");
        }

        var optionsBuilder =
            new DbContextOptionsBuilder<
                ApplicationDbContext>();

        optionsBuilder.UseNpgsql(
            connectionString);

        IPublisher publisher =
            new DesignTimePublisher();

        return new ApplicationDbContext(
            optionsBuilder.Options,
            publisher);
    }


    private sealed class DesignTimePublisher
        : IPublisher
    {
        public Task Publish(
            object notification,
            CancellationToken cancellationToken =
                default)
        {
            return Task.CompletedTask;
        }


        public Task Publish<TNotification>(
            TNotification notification,
            CancellationToken cancellationToken =
                default)
            where TNotification : INotification
        {
            return Task.CompletedTask;
        }
    }
}