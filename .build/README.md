# To run with docker compose

Modify the `compose.yaml` file to set mongo connectionstring and target db connectionstring and provider.
Provider can be MySql, SqlServer.

By default uses the MariaDb database

Run the docker compose file in the main directory.
```
docker compose up -d
```
To see logs use docker desktop or run the following command.
```
docker logs sensor-courier-dev
```
If everything is working correctly you should see in your sql db server a new database with new tables. After which data will be extracted from mongo and inserted into the target database.
In parameters table set:
BatchDelaySeconds (default 60)
BatchSize (default 10)

## MariaDB
Check the MariaDb folder for MariaDb container.

## If you want an image file 
Save image in tar file
```
docker save sensor-courier-dev > sensor-courier-dev.tar
```
