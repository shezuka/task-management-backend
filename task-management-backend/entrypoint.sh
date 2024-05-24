#!/bin/bash

set -e
export CONN_STR="Host=$DB_HOST;Database=$DB_NAME;Username=$DB_USER;Password=$DB_PASSWORD"
dotnet ef database update --connection "$CONN_STR"
exec "$@"
