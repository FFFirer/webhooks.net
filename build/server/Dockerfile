FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS BASE

WORKDIR /app
COPY . .

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "WebHooks.API.dll"]