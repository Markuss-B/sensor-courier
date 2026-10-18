# Sensor Courier

A .NET 8 background service that extracts environmental sensor measurements and metadata from MongoDB, transforms them into relational records, and loads them into SQL databases.

The service is part of the **Sensor Monitoring System**, developed for Aranet environmental sensors at the University of Latvia.

**Technologies:** C#, .NET 8, MongoDB, Entity Framework Core, SQL Server, MySQL, Docker

## Features

- **ETL processing:** Extracts, transforms, and loads sensor measurements and metadata from MongoDB into relational databases.
- **Multiple database providers:** Supports SQL Server and MySQL through configurable Entity Framework Core providers.
- **Incremental data transfer:** Uses the latest exported timestamps to retrieve new records.
- **Configurable batching:** Allows adjustment of batch size and delay between processing cycles.
- **Concurrent processing:** Processes measurement and metadata exports asynchronously.
- **Database initialization:** Applies Entity Framework Core migrations and initializes default parameters.
- **Background execution:** Runs as a .NET hosted service with logging and error handling.
- **Containerization:** Supports Docker-based deployment and environment-specific configuration.

## Architecture

The application is organized into four .NET projects, separating ETL processing, database models, and provider-specific migrations.

### Project Structure

| Project | Responsibility |
|---|---|
| `SensorCourier` | Main application, background worker, ETL services, and MongoDB integration |
| `SensorCourier.Models` | Relational data models and shared Entity Framework Core `DbContext` |
| `SensorCourier.MySql` | MySQL-specific database configuration and migrations |
| `SensorCourier.SqlServer` | SQL Server-specific database configuration and migrations |

The main application references the database provider projects, which share the common data model.

This separation allows the application to select its target database at runtime while maintaining a consistent data model.

### Main Components

| Component | Responsibility |
|---|---|
| `Worker` | Controls the ETL execution loop and processing intervals |
| `ETLService` | Coordinates data extraction, transformation, and loading |
| `ExtractionRepository` | Retrieves source measurements and metadata from MongoDB |
| `LoadRepository` | Persists transformed records in the destination SQL database |
| `ParameterService` | Retrieves processing parameters and the latest exported timestamps |
| `MongoDb` | Provides access to the source MongoDB database |
| `TargetDbContext` | Defines the target database model and handles database initialization |

<img width="1222" height="450" alt="image" src="https://github.com/user-attachments/assets/980645fa-5994-4cef-9a29-c3d344c4286c" />


## ETL Pipeline

The service periodically transfers new sensor measurements and metadata from MongoDB to a configured relational database.

Each processing cycle follows these steps:

1. Retrieve the current batch configuration from the SQL database.
2. Determine the latest exported measurement and metadata timestamps.
3. Extract newer records from the corresponding MongoDB collections.
4. Transform the source documents into relational records.
5. Load the transformed records into the destination database.
6. Wait for the configured delay before starting the next cycle.

Measurement and metadata exports run as separate asynchronous tasks, synchronized using `Task.WhenAll()`.

### Data Transformation

MongoDB stores sensor measurements as documents containing dynamic key-value pairs.

For example, a measurement document may contain:

```json
{
  "sensorId": "SNS12",
  "timestamp": "2025-01-15T12:00:00Z",
  "measurements": {
    "co2": 950,
    "temperature": 22.5,
    "humidity": 42
  }
}
```

The ETL process transforms each measurement field into a separate relational record.

| Timestamp | SensorId | MetricKey | MetricValue |
|---|---|---|---|
| 2025-01-15 12:00:00 | SNS12 | co2 | 950 |
| 2025-01-15 12:00:00 | SNS12 | temperature | 22.5 |
| 2025-01-15 12:00:00 | SNS12 | humidity | 42 |

This converts one MongoDB measurement document into multiple SQL rows.

The same approach is used for sensor metadata, with individual metadata attributes stored as separate records.

The transformation supports varying measurement and metadata fields without requiring a dedicated SQL column for every possible sensor attribute.

*The examples above use illustrative sensor data.*

### Incremental Extraction

Instead of re-exporting the entire MongoDB collection during every processing cycle, the application uses timestamps to identify new records.

The `ParameterService` retrieves the maximum stored timestamp from the destination measurement and metadata tables.

These timestamps are then used by the extraction repository to retrieve newer MongoDB documents.

If the destination tables are empty, extraction begins from the earliest available records.

Measurement and metadata exports maintain separate timestamps, allowing each dataset to progress independently.

## Database Support

The application supports two relational database providers:

- **SQL Server**
- **MySQL**

The target provider is selected through application configuration.

### Runtime Provider Selection

The application configures its Entity Framework Core `DbContext` based on the `Provider` setting.

Each provider has its own configuration and migration assembly, while both share the same relational models.

This design makes it possible to extend the application with additional providers, such as PostgreSQL, by adding provider-specific configuration and migrations.

### Database Initialization

During startup, the application:

1. Selects the configured database provider.
2. Configures the appropriate Entity Framework Core provider and migration assembly.
3. Initializes the destination database.
4. Applies pending migrations.
5. Seeds default processing parameters if necessary.

The default parameters are:

| Parameter | Default | Description |
|---|---|---|
| `BatchSize` | 10 | Maximum number of source documents processed per batch |
| `BatchDelaySeconds` | 60 | Delay between processing cycles, in seconds |

## Relational Data Model

The destination database contains three primary tables.

### SensorMeasurements

Stores individual sensor measurement values.

| Field | Description |
|---|---|
| `Id` | Auto-generated primary key |
| `Timestamp` | Measurement timestamp |
| `SensorId` | Sensor identifier |
| `MetricKey` | Measurement name, such as `co2` |
| `MetricValue` | Measurement value stored as text |

### SensorMetadata

Stores changes to sensor metadata and attributes.

| Field | Description |
|---|---|
| `Id` | Auto-generated primary key |
| `Timestamp` | Metadata timestamp |
| `SensorId` | Sensor identifier |
| `MetaKey` | Metadata attribute name |
| `MetaValue` | Metadata attribute value stored as text |

### Parameters

Stores configuration values controlling the ETL process.

| Field | Description |
|---|---|
| `Id` | Auto-generated primary key |
| `Name` | Parameter name |
| `Value` | Parameter value |

## Configurable Batch Processing

The background worker retrieves processing settings from the destination database before each batch.

Two parameters control the ETL schedule:

- **BatchSize:** Maximum number of MongoDB source documents retrieved per extraction batch.
- **BatchDelaySeconds:** Time to wait between processing cycles.

Because these parameters are stored in the database and read during execution, their values can be modified without rebuilding the application.

The worker processes measurements and metadata concurrently and waits for both tasks to finish before starting the next cycle.

### Logging and Error Handling

The service logs important events, including:

- Database initialization and migrations.
- Batch execution and extracted record counts.
- Latest processed timestamps.
- Successful data loading.
- Processing errors and exceptions.
- Time until the next processing cycle.

## Configuration

The application supports configuration through `appsettings.json`, environment variables, and command-line arguments.

### Database Provider

Set the `Provider` configuration value to either:

- `MySql`
- `SqlServer`

### Connection Settings

| Setting | Description |
|---|---|
| `Provider` | Target relational database provider |
| `ConnectionStrings__MySql` | MySQL connection string |
| `ConnectionStrings__SqlServer` | SQL Server connection string |
| `MongoDbSettings__ConnectionString` | Source MongoDB connection string |
| `MongoDbSettings__DatabaseName` | Source MongoDB database name |

### Example Configuration

The following example selects MySQL as the destination database:

```json
{
  "Provider": "MySql",
  "ConnectionStrings": {
    "MySql": "Server=localhost;Database=SensorCourier;User=<USER>;Password=<PASSWORD>;"
  },
  "MongoDbSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "sensor-dev"
  }
}
```

Connection strings should be adjusted for the target environment. Avoid committing actual database credentials to the repository.

## Running Locally with Docker

The repository includes a Docker Compose configuration in `.build/compose.yaml`.

### Prerequisites

- Docker with Docker Compose
- A running MongoDB instance containing sensor data
- A supported SQL database (MySQL or SQL Server)
- Docker networks connecting the application to its databases

The provided Docker configuration uses MySQL-compatible MariaDB as its default destination and expects MongoDB to be available through the Sensor Consumer's Docker network.

### Configure the Application

Update `.build/compose.yaml` with the appropriate source and destination database settings:

- `MongoDbSettings__ConnectionString`
- `MongoDbSettings__DatabaseName`
- `Provider`
- `ConnectionStrings__MySql` or `ConnectionStrings__SqlServer`

The example Compose configuration uses the external Docker networks:

- `sensor-network`
- `maria_sql-network`

Ensure these networks exist and that both databases are accessible before starting the service.

### Start the Application

From the `.build` directory:

```bash
docker compose up -d --build
```

### View Logs

```bash
docker logs -f sensor-courier-dev
```

On successful initialization, the application creates or updates the destination database schema and begins exporting available sensor data.

### Adjust Processing Parameters

After initialization, the `Parameters` table contains:

- `BatchSize`
- `BatchDelaySeconds`

Modify these values in the destination database to change the processing behavior.

The updated settings are read during subsequent processing cycles.

### Stop the Application

```bash
docker compose down
```

## Related Components

| Component | Responsibility |
|---|---|
| [Sensor Consumer](https://github.com/Markuss-B/sensor-consumer) | Receives and processes MQTT sensor measurements and metadata, storing them in MongoDB |
| [Sensor Web API](https://github.com/Markuss-B/sensor-webapi) | Provides REST endpoints for sensor and notification management and real-time updates through SignalR |
| [Sensor Dashboard](https://github.com/Markuss-B/sensor-dashboard) | Angular web application for viewing measurements, managing sensors, and configuring notifications |
| [Sensor Notifications](https://github.com/Markuss-B/sensor-notifications) | Evaluates measurement-based notification rules and tracks active and resolved violations |
| [Sensor Courier](https://github.com/Markuss-B/sensor-courier) | Exports measurements from MongoDB to SQL Server or MySQL |
