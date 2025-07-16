#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

USER root
RUN apt-get update && apt-get -y upgrade

RUN groupadd -g 999 appuser && \
useradd -r -u 999 -g appuser appuser
USER appuser

WORKDIR /app
ENV ASPNETCORE_URLS="http://*:5555"
EXPOSE 5555

FROM node:23.3.0 AS node_base
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
COPY --from=node_base . .
WORKDIR /src
COPY ["Elsa.Dashboard/Elsa.Dashboard.csproj", "Elsa.Dashboard/"]
COPY ["Elsa.Server/Elsa.Server.csproj", "Elsa.Server/"]
COPY ["Elsa.CustomActivities/Elsa.CustomActivities.csproj", "Elsa.CustomActivities/"]
COPY ["Elsa.Models/Elsa.CustomModels.csproj", "Elsa.Models/"]
COPY ["Elsa.CustomInfrastructure/Elsa.CustomInfrastructure.csproj", "Elsa.CustomInfrastructure/"]
COPY "nuget.config" .
COPY "Directory.Packages.props" .

RUN curl -L "https://raw.githubusercontent.com/Microsoft/artifacts-credprovider/master/helpers/installcredprovider.sh" | sh
ARG INTERNAL_FEED_ACCESSTOKEN
ENV VSS_NUGET_EXTERNAL_FEED_ENDPOINTS \
    "{\"endpointCredentials\": [{\"endpoint\":\"https://pkgs.dev.azure.com/homesengland/_packaging/homesengland/nuget/v3/index.json\", \"username\":\"docker\", \"password\":\"${INTERNAL_FEED_ACCESSTOKEN}\"}]}"

RUN dotnet restore "Elsa.Dashboard/Elsa.Dashboard.csproj"
COPY . .
WORKDIR "/src/Elsa.Dashboard"
RUN npm install
RUN npm run workflow:build:dev
RUN npm run workflow:pack:debug
RUN npm run custom:build
RUN dotnet build "Elsa.Dashboard.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "Elsa.Dashboard.csproj" -c Debug -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Elsa.Dashboard.dll"]
