using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SensorCourier.Models;

public class TargetDbContext : DbContext
{
    public TargetDbContext(DbContextOptions<TargetDbContext> options) : base(options)
    {
    }
    public DbSet<SensorMeasurement> SensorMeasurements { get; set; }
    public DbSet<SensorMetadata> SensorMetadata { get; set; }
    public DbSet<Parameter> Parameters { get; set; }

    public static async Task InitializeAsync(TargetDbContext db, ILogger logger)
    {
        logger.LogInformation("Initializing database.");

        var appliedMigrations = await db.Database.GetAppliedMigrationsAsync();
        logger.LogInformation("Applied migrations: {Migrations}.", string.Join(", ", appliedMigrations));

        var pendingMigrations = await db.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {
            logger.LogInformation($"Applying migrations: {string.Join(", ", pendingMigrations)}");
            await db.Database.MigrateAsync();
        }
        else
        {
            logger.LogInformation("No pending migrations found.");
        }

        // Already seeded
        if (db.Parameters.Any())
            return;

        logger.LogInformation("Seeding database.");

        db.Parameters.AddRange(
            new Parameter { Name = "BatchDelaySeconds", Value = "60" },
            new Parameter { Name = "BatchSize", Value = "10" }
        );

        await db.SaveChangesAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // specify datetime kind as UTC
        modelBuilder.Entity<SensorMeasurement>()
            .Property(e => e.Timestamp)
            .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
        modelBuilder.Entity<SensorMetadata>()
            .Property(e => e.Timestamp)
            .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

    }
}
