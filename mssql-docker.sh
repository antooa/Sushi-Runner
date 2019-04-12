#!/bin/bash

IMAGE_NAME=mcr.microsoft.com/mssql/server:2017-latest
CONTAINER_NAME=mssql
CONTAINER_ID=$(docker container ls -qa --filter name=${CONTAINER_NAME})

run_mssql_docker() {
    docker run -d \
        -e 'ACCEPT_EULA=Y' \
        -e 'SA_PASSWORD=Pa$$word' \
        -p 1433:1433 \
        --name ${CONTAINER_NAME} \
        ${IMAGE_NAME}
}

if [ ! -z "${1}" ]; then
    if [ "${1}" == "-r" ]; then
        docker container kill ${CONTAINER_NAME}
        docker container rm ${CONTAINER_NAME}
        run_mssql_docker
    else
        echo "Command ${1} unknown"
    fi
    exit 0
fi

if [ -n "${CONTAINER_ID}" ]; then
    docker container start ${CONTAINER_ID}
else
    run_mssql_docker
fi
