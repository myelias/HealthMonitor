Identity server lets users login, register, logout, and register with an external authentication provider (Fitbit, Google)

Currently, this server runs at https://localhost:5000. Need to implement SSL to have https work. The Gateway Service does not route to the Identity Server.

The DockerFiles are used to create images for each service. The docker-compose.yaml file is used to run those images.
