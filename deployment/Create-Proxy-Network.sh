#!/bin/bash

# Set -e to exit immediately if a command exits with a non-zero status.
set -e

echo "Checking if network for reverse proxy exists..."
MAYBE_EXISTING_NETWORK=( $(docker network ls --filter name="Reverse_Proxy" -q) )
if [[ ${#MAYBE_EXISTING_NETWORK[@]} -gt 0 ]]
then
  echo "The docker volume \"Reverse_Proxy\" already exists."
else
  echo "Creating docker network \"Reverse_Proxy\"..."
  docker network create \
    --driver overlay \
    --scope swarm \
    --attachable \
    Reverse_Proxy
fi

exit 0
