using Microsoft.EntityFrameworkCore;
using SensorCourier.App.Models;
using SensorCourier.Models;

namespace SensorCourier.App.Services;

public class ParameterService
{
    private readonly ILogger<ParameterService> _logger;
    private readonly TargetDbContext _dbContext;

    public ParameterService(ILogger<ParameterService> logger, TargetDbContext targetDbContext)
    {
        _logger = logger;
        _dbContext = targetDbContext;
    }

    /// <summary>
    /// Get the app settings from the database parameters table.
    /// </summary>
    /// <returns><see cref="AppSettings"/> which define app functionality.</returns>
    public async Task<AppSettings> GetAppSettings(CancellationToken cancellationToken)
    {
        try
        {
            // Fetch parameters from the database
            var parameters = await _dbContext.Parameters
                .Where(p => AppSettings.PropertyNames.Contains(p.Name))
                .ToDictionaryAsync(p => p.Name, p => p.Value, cancellationToken);

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

            if (!appSettings.IsSet())
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
    public async Task<DateTime> GetLastMeasurementTimeStamp(CancellationToken cancellationToken)
    {
        try
        {
            // Check if the table is empty
            if (!await _dbContext.SensorMeasurements.AnyAsync(cancellationToken))
            {
                _logger.LogWarning("The SensorMeasurements table is empty. Returning DateTime.MinValue.");
                return DateTime.MinValue;
            }

            // Get the maximum timestamp
            DateTime dateTime = await _dbContext.SensorMeasurements.MaxAsync(m => m.Timestamp, cancellationToken);

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
    public async Task<DateTime> GetLastMetadataTimeStamp(CancellationToken cancellationToken)
    {
        try
        {
            // Check if the table is empty
            if (!await _dbContext.SensorMetadata.AnyAsync(cancellationToken))
            {
                _logger.LogWarning("The SensorMetadatas table is empty. Returning DateTime.MinValue.");
                return DateTime.MinValue;
            }
            // Get the maximum timestamp
            DateTime dateTime = await _dbContext.SensorMetadata.MaxAsync(m => m.Timestamp, cancellationToken);

            return dateTime;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "An error occurred while retrieving the last metadata timestamp.");
            throw new Exception("Failed to retrieve the last metadata timestamp.", ex);
        }
    }
}
