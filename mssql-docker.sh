#!/bin/bash

IMAGE_NAME=mcr.microsoft.com/mssql/server:2017-latest
CONTAINER_NAME=mssql
CONTAINER_ID=$(docker container ls -qa --filter name=${CONTAINER_NAME})

if [ -n "${CONTAINER_ID}" ]; then
    docker container start ${CONTAINER_ID}
else
    docker run -d \
        -e 'ACCEPT_EULA=Y' \
        -e 'SA_PASSWORD=Pa$$word' \
        -p 1433:1433 \
        --name ${CONTAINER_NAME} \
        ${IMAGE_NAME}
fi
