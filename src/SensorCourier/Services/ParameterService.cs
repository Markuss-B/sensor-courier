using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SensorCourier.App.Models;
using SensorCourier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorCourier.App.Services;

public class ParameterService
{
    private readonly ILogger<ParameterService> _logger;
    private readonly IDbContextFactory<TargetDbContext> _dbContextFactory;

    public ParameterService(ILogger<ParameterService> logger, IDbContextFactory<TargetDbContext> dbContextFactory)
    {
        _logger = logger;
        _dbContextFactory = dbContextFactory;
    }

    /// <summary>
    /// Get the app settings from the database parameters table.
    /// </summary>
    /// <returns><see cref="AppSettings"/> which define app functionality.</returns>
    public AppSettings GetAppSettings()
    {
        try
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            // Fetch parameters from the database
            var parameters = dbContext.Parameters
                .Where(p => AppSettings.PropertyNames.Contains(p.Name))
                .ToDictionary(p => p.Name, p => p.Value);

            _logger.LogDebug("Got parameters: {parameters}", parameters);

            AppSettings appSettings = new();

            foreach (var prop in AppSettings.PropertyInfos)
            {
                if (parameters.TryGetValue(prop.Name, out var value))
                {
                    try
                    {
                        // Convert and set the property value
                        var convertedValue = Convert.ChangeType(value, prop.PropertyType);
                        prop.SetValue(appSettings, convertedValue);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to convert value '{value}' for property '{PropertyName}'.", value, prop.Name);
                        throw new InvalidOperationException($"Invalid value for property '{prop.Name}'.", ex);
                    }
                }
            }

            if(!appSettings.IsSet())
            {
                _logger.LogError("App settings are not set.");
                throw new InvalidOperationException("App settings are not set.");
            }

            _logger.LogDebug("Loaded app settings: {appSettings}", appSettings);
            return appSettings;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Failed to load application settings.");
            throw new Exception("An error occurred while retrieving app settings from database.", ex);
        }
    }

    /// <summary>
    /// Get the last measurement timestamp from the database table <see cref="SensorMeasurement"/>.
    /// </summary>
    /// <returns>DateTime of the last saved measurement, or <see cref="DateTime.MinValue"/> if the table is empty.</returns>
    public DateTime GetLastMeasurementTimeStamp()
    {
        try
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            // Check if the table is empty
            if (!dbContext.SensorMeasurements.Any())
            {
                _logger.LogWarning("The SensorMeasurements table is empty. Returning DateTime.MinValue.");
                return DateTime.MinValue;
            }

            // Get the maximum timestamp
            DateTime dateTime = dbContext.SensorMeasurements.Max(m => m.Timestamp);

            // Return the last timestamp
            return dateTime;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "An error occurred while retrieving the last measurement timestamp.");
            throw new Exception("Failed to retrieve the last measurement timestamp.", ex);
        }
    }

    /// <summary>
    /// Get the last metadata timestamp from the database table <see cref="SensorMetadata"/>.
    /// </summary>
    /// <returns>DateTime of the last saved metadata, or <see cref="DateTime.MinValue"/> if the table is empty.</returns>
    public DateTime GetLastMetadataTimeStamp()
    {
        try
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            // Check if the table is empty
            if (!dbContext.SensorMetadata.Any())
            {
                _logger.LogWarning("The SensorMetadatas table is empty. Returning DateTime.MinValue.");
                return DateTime.MinValue;
            }
            // Get the maximum timestamp
            DateTime dateTime = dbContext.SensorMetadata.Max(m => m.Timestamp);
            // Return the last timestamp
            return dateTime;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "An error occurred while retrieving the last metadata timestamp.");
            throw new Exception("Failed to retrieve the last metadata timestamp.", ex);
        }
    }

}
