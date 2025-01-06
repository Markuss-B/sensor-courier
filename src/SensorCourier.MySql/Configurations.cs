using Microsoft.EntityFrameworkCore;

namespace SensorCourier.MySql.Configurations
{
    public static class Configuruations
    {
        /// <summary>
        /// Configure the model builder for the MySql database.
        /// Enables automatic migration creation for MySql to use MySql specific models.
        /// Currently not used.
        /// Could be used to set the JSON column type for the SensorMeasurement and SensorMetadata models.
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void ConfigureModelBuilder(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<SensorMeasurement>()
            //    .Property(e => e.MeasurementsJson)
            //    .HasColumnType("JSON");
            //modelBuilder.Entity<SensorMetadata>()
            //    .Property(e => e.MetadataJson)
            //    .HasColumnType("JSON");

            // set datetimes to be timestamp
            //modelBuilder.Entity<SensorMeasurement>()
            //    .Property(e => e.Timestamp)
            //    .HasColumnType("TIMESTAMP");

            //modelBuilder.Entity<SensorMetadata>()
            //    .Property(e => e.Timestamp)
            //    .HasColumnType("TIMESTAMP");
        }
    }
}