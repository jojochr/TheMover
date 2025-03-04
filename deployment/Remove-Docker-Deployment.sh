#!/bin/bash

# Set -e to exit immediately if a command exits with a non-zero status.
set -e

echo "Removing TheMover Stack if exists..."
if [[ "$(docker stack ls)" == *"TheMover"* ]]
then
  docker stack rm TheMover
  echo "TheMover stack removed"
else
  echo "TheMover stack is currently not deployed"
fi

echo ""
echo "Removing containers related to TheMover..."
echo "(Should be gone already by removing the docker stack)"
CONTAINERS_TO_REMOVE=( $(docker container ls --filter name="TheMover*" -q) )
if [[ ${#CONTAINERS_TO_REMOVE[@]} -gt 0 ]]
then
  docker container rm "${CONTAINERS_TO_REMOVE[@]}"
  echo "Removed \"${#CONTAINERS_TO_REMOVE[@]}\" containers"
else
  echo "No containers left to remove"
fi

echo ""
echo "Removing images related to TheMover..."
IMAGES_TO_REMOVE=( $(docker images --filter reference="*/*/the-mover*" -q) )
if [[ ${#IMAGES_TO_REMOVE[@]} -gt 0 ]]
then
  docker image rm "${IMAGES_TO_REMOVE[@]}"
  echo "Removed \"${#IMAGES_TO_REMOVE[@]}\" images"
else
  echo "No images left to remove"
fi

echo ""
echo "Removing volumes related to TheMover..."
VOLUMES_TO_REMOVE=( $(docker volume ls --filter name="TheMover*" -q) )
if [[ ${#VOLUMES_TO_REMOVE[@]} -gt 0 ]]
then
  docker volume rm -f "${VOLUMES_TO_REMOVE[@]}"
  echo "Removed \"${#VOLUMES_TO_REMOVE[@]}\" volumes"
else
  echo "No volumes left to remove"
fi

echo ""
echo "Removing networks related to TheMover..."
NETWORKS_TO_REMOVE=( $(docker network ls --filter name="TheMover*" -q) )
if [[ ${#NETWORKS_TO_REMOVE[@]} -gt 0 ]]
then
  docker network rm -f "${NETWORKS_TO_REMOVE[@]}"
  echo "Removed \"${#NETWORKS_TO_REMOVE[@]}\" networks"
else
  echo "No networks left to remove"
fi

echo ""
echo "Finished cleanup"

exit 0
