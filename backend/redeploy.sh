#!/bin/bash

# Define variables
CONTAINER_NAME="diuna-api"
IMAGE_NAME="diuna-api"
PORT_HTTP=8080
PORT_HTTPS=8443

echo "Stopping and removing the old container (if exists)..."
# Stop and remove the old container
sudo docker stop $CONTAINER_NAME 2>/dev/null || true
sudo docker rm $CONTAINER_NAME 2>/dev/null || true

echo "Building the Docker image..."
# Build the Docker image
sudo docker build -t $IMAGE_NAME .

echo "Running the new container..."
# Run the new container
sudo docker run -d --privileged -p $PORT_HTTP:80 -p $PORT_HTTPS:443 --name $CONTAINER_NAME $IMAGE_NAME

echo "Deployment complete. The API should be accessible at http://localhost:$PORT_HTTP and https://localhost:$PORT_HTTPS"
