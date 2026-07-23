FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS base
WORKDIR /app
EXPOSE 8080
USER root
RUN apk add --no-cache icu-libs tzdata
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
USER app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release

ARG GITHUB_USERNAME
ARG GITHUB_PASSWORD
ENV GITHUB_USERNAME=$GITHUB_USERNAME
ENV GITHUB_PASSWORD=$GITHUB_PASSWORD

WORKDIR /src
COPY ["DocumentsAPI.Presentation/DocumentsAPI.Presentation.csproj", "DocumentsAPI.Presentation/"]
COPY ["DocumentsAPI.Application/DocumentsAPI.Application.csproj", "DocumentsAPI.Application/"]
COPY ["DocumentsAPI.Infrastructure/DocumentsAPI.Infrastructure.csproj", "DocumentsAPI.Infrastructure/"]
COPY ["DocumentsAPI.Domain/DocumentsAPI.Domain.csproj", "DocumentsAPI.Domain/"]
COPY ["nuget.config", "./"]

RUN dotnet nuget update source "github" \
    --username "$GITHUB_USERNAME" \
    --password "$GITHUB_PASSWORD" \
    --store-password-in-clear-text \
    --configfile nuget.config

RUN dotnet restore "./DocumentsAPI.Presentation/DocumentsAPI.Presentation.csproj"
COPY . .
WORKDIR "/src/DocumentsAPI.Presentation"
RUN dotnet build "./DocumentsAPI.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DocumentsAPI.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DocumentsAPI.Presentation.dll"]