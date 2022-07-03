$ImageName = "webhooks-server-environment"

docker rmi $ImageName -f
docker build -t $ImageName .