#!/bin/bash
set -e          # fail on command errors
set -o pipefail # fail if there is a failure within a pipe
echo "CONTAINER_NAME  is $CONTAINER_NAME"
sleep 10  # Waits 5 seconds.
CID=$(docker ps -q -f status=running -f name=^/${CONTAINER_NAME}$)

docker logs $CONTAINER_NAME

if [ ! "${CID}" ]; then
  echo "Container is not running and/or doesn't exist"
  exit 1
else
  echo "Container is running and exists"
  exit 0
fi