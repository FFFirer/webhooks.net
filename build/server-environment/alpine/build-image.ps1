$ImageName = "webhooks-env"
$Version = "6.0-alpine"
$Tag = "$($ImageName):$($Version)"

docker rmi $Tag -f
docker build -t $Tag .