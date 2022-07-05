FROM mcr.microsoft.com/dotnet/runtime:6.0-alpine AS BASE

WORKDIR /app
COPY . .

ENTRYPOINT ["dotnet", "WebHooks.EFCore.Migrator.dll"]