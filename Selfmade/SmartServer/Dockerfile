FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim-arm32v7 AS base
EXPOSE 80 1883

#
# Service
#
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS publish-service
WORKDIR /src
COPY SmartServer SmartServer/
COPY SmartServer.Common SmartServer.Common/
COPY SmartServer.Ef SmartServer.Ef/
WORKDIR /src/SmartServer
RUN dotnet publish "SmartServer.csproj" -c Release -o /app/publish

#
# Angular
#
FROM node:12.17.0-alpine AS build-angular
WORKDIR /src
COPY webui ./
RUN ["npm", "install"]
RUN ["npm", "run", "build"]

#
# Finalize
#
FROM base AS final
WORKDIR /app
COPY --from=publish-service /app/publish .
COPY --from=build-angular /src/dist/webui /app/wwwroot/
ENTRYPOINT ["dotnet", "SmartServer.dll"]