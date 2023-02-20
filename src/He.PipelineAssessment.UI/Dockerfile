#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS="http://*:5555"
EXPOSE 5555

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["He.PipelineAssessment.UI/He.PipelineAssessment.UI.csproj", "He.PipelineAssessment.UI/"]
COPY ["Elsa.CustomWorkflow.Sdk/Elsa.CustomWorkflow.Sdk.csproj", "Elsa.CustomWorkflow.Sdk/"]
COPY ["Elsa.Models/Elsa.CustomModels.csproj", "Elsa.Models/"]
COPY "nuget.config" .

RUN curl -L "https://raw.githubusercontent.com/Microsoft/artifacts-credprovider/master/helpers/installcredprovider.sh" | sh
ARG INTERNAL_FEED_ACCESSTOKEN
ENV VSS_NUGET_EXTERNAL_FEED_ENDPOINTS \
    "{\"endpointCredentials\": [{\"endpoint\":\"https://pkgs.dev.azure.com/homesengland/_packaging/homesengland/nuget/v3/index.json\", \"username\":\"docker\", \"password\":\"${INTERNAL_FEED_ACCESSTOKEN}\"}]}"

RUN dotnet restore "He.PipelineAssessment.UI/He.PipelineAssessment.UI.csproj"
COPY . .
WORKDIR "/src/He.PipelineAssessment.UI"
RUN dotnet build "He.PipelineAssessment.UI.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "He.PipelineAssessment.UI.csproj" -c Debug -o /app/publish /p:UseAppHost=false 

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "He.PipelineAssessment.UI.dll"]