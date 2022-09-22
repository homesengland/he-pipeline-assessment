#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS="http://*:8080"
EXPOSE 4430

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Elsa.Server/Elsa.Server.csproj", "Elsa.Server/"]
COPY ["Elsa.CustomActivities/Elsa.CustomActivities.csproj", "Elsa.CustomActivities/"]
COPY ["Elsa.Models/Elsa.CustomModels.csproj", "Elsa.Models/"]
COPY ["Elsa.CustomInfrastructure/Elsa.CustomInfrastructure.csproj", "Elsa.CustomInfrastructure/"]
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