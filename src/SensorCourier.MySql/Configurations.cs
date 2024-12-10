using Microsoft.EntityFrameworkCore;
using SensorCourier.Models;

namespace SensorCourier.MySql.Configurations
{
    public static class Configuruations
    {
        /// <summary>
        /// Configure the model builder for the MySql database.
        /// Enables automatic migration creation for MySql to use MySql specific models.
        /// Currently used to set the JSON column type for the SensorMeasurement and SensorMetadata models.
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void ConfigureModelBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SensorMeasurement>()
                .Property(e => e.MeasurementsJson)
                .HasColumnType("JSON");
            modelBuilder.Entity<SensorMetadata>()
                .Property(e => e.MetadataJson)
                .HasColumnType("JSON");
        }
    }
}