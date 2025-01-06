using Microsoft.EntityFrameworkCore;

namespace SensorCourier.SqlServer.Configurations
{
    public static class Configuruations
    {
        /// <summary>
        /// Configure the model builder for the SqlServer database.
        /// Enables automatic migration creation for SqlServer to use SqlServer specific models.
        /// Currently not used.
        /// Could be used to set specific column types for SQL Server so that the migration generator creates the correct column types.
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void ConfigureModelBuilder(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<SensorMeasurement>()
            //    .Property(e => e.MeasurementsJson)
            //    .HasColumnType("NVARCHAR(MAX)");
            //modelBuilder.Entity<SensorMetadata>()
            //    .Property(e => e.MetadataJson)
            //    .HasColumnType("NVARCHAR(MAX)");
        }
    }
}