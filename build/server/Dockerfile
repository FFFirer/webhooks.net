FROM webhooks-env:6.0-alpine AS BASE

WORKDIR /app
COPY . .

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "WebHooks.API.dll"]