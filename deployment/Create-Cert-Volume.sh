#!/bin/bash

# Set -e to exit immediately if a command exits with a non-zero status.
set -e

echo "Checking if certificate volume exists..."
MAYBE_EXISTING_VOLUME=( $(docker volume ls --filter name="TheMover_traefik_certs" -q) )
if [[ ${#MAYBE_EXISTING_VOLUME[@]} -gt 0 ]]
then
  echo "The docker volume \"TheMover_traefik_certs\" already exists."
else
  echo "Creating docker volume \"TheMover_traefik_certs\"..."
  docker volume create \
    -d local \
    --scope single \
    --sharing all \
    TheMover_traefik_certs
fi

exit 0
