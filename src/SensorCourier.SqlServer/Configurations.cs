using Microsoft.EntityFrameworkCore;

namespace SensorCourier.SqlServer.Configurations
{
    public static class Configuruations
    {
        /// <summary>
        /// Configure the model builder for the SqlServer database.
        /// Enables automatic migration creation for SqlServer to use SqlServer specific models.
        /// Currently SqlServer server uses NVARCHAR(MAX) for JSON columns.
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