#!/bin/bash

docker run -d \
    -e 'ACCEPT_EULA=Y' \
    -e 'SA_PASSWORD=Pa$$word' \
    -p 1433:1433 \
    --name mssql \
     mcr.microsoft.com/mssql/server:2017-latest
