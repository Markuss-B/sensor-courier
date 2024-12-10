// https://stackoverflow.com/a/69603035/28000980

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SensorCourier.MySql.Configurations;

namespace SensorCourier.MySql.Extensions;
public static class MySqlDbContextOptionsExtensions
{
    public static DbContextOptionsBuilder UseMySqlExtensions(
        this DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.ReplaceService<IModelCustomizer, AwesomeModelCustomizer>();
}

public class AwesomeModelCustomizer : RelationalModelCustomizer
{
    public AwesomeModelCustomizer(ModelCustomizerDependencies dependencies)
        : base(dependencies) { }
    public override void Customize(ModelBuilder modelBuilder, DbContext context)
    {
        // Do something before context.OnModelCreating(modelBuilder)...
        base.Customize(modelBuilder, context);
        // Do something after context.OnModelCreating(modelBuilder)...

        Configuruations.ConfigureModelBuilder(modelBuilder);
    }
}