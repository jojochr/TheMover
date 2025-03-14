#!/bin/bash

# Set -e to exit immediately if a command exits with a non-zero status.
set -e


echo "Checking if certificate volume exists..."
VOLUME_NAME="traefik_certs"
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

echo ""
echo "Checking if network for reverse proxy exists..."
NETWORK_NAME="reverse_proxy"
MAYBE_EXISTING_NETWORK=( $(docker network ls --filter name=$NETWORK_NAME -q) )
if [[ ${#MAYBE_EXISTING_NETWORK[@]} -gt 0 ]]
then
  echo "The docker volume \"$NETWORK_NAME\" already exists."
else
  echo "Creating docker network \"$NETWORK_NAME\"..."
  docker network create \
    --driver overlay \
    --scope swarm \
    --attachable \
    $NETWORK_NAME
fi

exit 0
