This project is a console application that reads sensor data from MongoDb and writes it to a target sql database.

# Other features
- Configurable to run batch jobs at a specified interval.
- Can run as a console application or docker container.
- Can be configured to monitor for changes in the source database and write to the target database instead of running batch jobs. Can be configured to do both.

# Supported target databases
- MySql
- SqlServer

# Running migrations: 
1. Open Package Manager Console
2. Set the default project to the project to:
   - MySql project: `SensorCourier.MySql`
   - SqlServer project: `SensorCourier.SqlServer`

## MySql project
Add-Migration Init -Args "--Provider MySql"
Remove-Migration -Args "--Provider MySql"

## SqlServer project
Add-Migration Init -Args "--Provider SqlServer"
Remove-Migration -Args "--Provider SqlServer"

# Project structure
## Dependency structuer
SensorCourier -> MySql/SqlServer -> Models

## Migrations projects
- SensorCourier.MySql
- SensorCourier.SqlServer
These projects are used to generate database specific migrations.

## Models project
- SensorCourier.Models
Used by the migrations project and the main project for target database models.
- Target database is to where data will be loaded.