#!/bin/bash

# Set -e to exit immediately if a command exits with a non-zero status.
set -e

NETWORK_NAME="Reverse_Proxy"

echo "Checking if network for reverse proxy exists..."
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
