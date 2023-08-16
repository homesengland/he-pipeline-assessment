#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS="http://*:5555"
EXPOSE 5555

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Elsa.Server/Elsa.Server.csproj", "Elsa.Server/"]
COPY ["Elsa.CustomActivities/Elsa.CustomActivities.csproj", "Elsa.CustomActivities/"]
COPY ["Elsa.Models/Elsa.CustomModels.csproj", "Elsa.Models/"]
COPY ["Elsa.CustomInfrastructure/Elsa.CustomInfrastructure.csproj", "Elsa.CustomInfrastructure/"]
COPY "nuget.config" .

RUN curl -L "https://raw.githubusercontent.com/Microsoft/artifacts-credprovider/master/helpers/installcredprovider.sh" | sh
ARG INTERNAL_FEED_ACCESSTOKEN
ENV VSS_NUGET_EXTERNAL_FEED_ENDPOINTS \
    "{\"endpointCredentials\": [{\"endpoint\":\"https://pkgs.dev.azure.com/homesengland/_packaging/homesengland/nuget/v3/index.json\", \"username\":\"docker\", \"password\":\"${INTERNAL_FEED_ACCESSTOKEN}\"}]}"

RUN dotnet restore "Elsa.Server/Elsa.Server.csproj"
COPY . .
WORKDIR "/src/Elsa.Server"
RUN dotnet build "Elsa.Server.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "Elsa.Server.csproj" -c Debug -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Elsa.Server.dll"]