#!/bin/bash

# Set -e to exit immediately if a command exits with a non-zero status.
set -e

VOLUME_NAME="TheMover_traefik_certs"

echo "Checking if certificate volume exists..."
MAYBE_EXISTING_VOLUME=( $(docker volume ls --filter name=$VOLUME_NAME -q) )
if [[ ${#MAYBE_EXISTING_VOLUME[@]} -gt 0 ]]
then
  echo "The docker volume \"$VOLUME_NAME\" already exists."
else
  echo "Creating docker volume \"$VOLUME_NAME\"..."
  docker volume create \
    -d local \
    --scope single \
    --sharing all \
    $VOLUME_NAME
fi

exit 0
