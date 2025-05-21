# Base runtime image for production
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Expose HTTP port
EXPOSE 8080

# Force ASP.NET Core to use HTTP (no certs)
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files and restore dependencies
COPY WebjetPriceComparer.sln ./
COPY WebjetPriceComparer.Api/WebjetPriceComparer.Api.csproj WebjetPriceComparer.Api/
COPY WebjetPriceComparer.Application/WebjetPriceComparer.Application.csproj WebjetPriceComparer.Application/
COPY WebjetPriceComparer.Infrastructure/WebjetPriceComparer.Infrastructure.csproj WebjetPriceComparer.Infrastructure/
COPY WebjetPriceComparer.Domain/WebjetPriceComparer.Domain.csproj WebjetPriceComparer.Domain/
RUN dotnet restore WebjetPriceComparer.Api/WebjetPriceComparer.Api.csproj

# Copy all source files
COPY . .

# Build the app
WORKDIR /src/WebjetPriceComparer.Api
RUN dotnet build WebjetPriceComparer.Api.csproj -c $BUILD_CONFIGURATION -o /app/build

# Publish the app
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish WebjetPriceComparer.Api.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Start the app
ENTRYPOINT ["dotnet", "WebjetPriceComparer.Api.dll"]
