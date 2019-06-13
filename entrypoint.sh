#!/bin/bash

set -e
>&2 echo "Migrate database..."
run_cmd="dotnet /app/bin/Release/netcoreapp2.2/publish/AllNewsServer.dll"

until dotnet ef database update; do
>&2 echo "SQL Server is starting up"
sleep 1
done

>&2 echo "SQL Server is up - executing command"
exec $run_cmd