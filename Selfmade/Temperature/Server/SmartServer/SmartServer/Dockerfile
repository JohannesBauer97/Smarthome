#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["SmartServer/SmartServer.csproj", "SmartServer/"]
RUN dotnet restore "SmartServer/SmartServer.csproj"
COPY . .
WORKDIR "/src/SmartServer"
RUN dotnet build "SmartServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmartServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartServer.dll"]