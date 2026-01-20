# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY WaterTrail.Api/WaterTrail.Api.csproj WaterTrail.Api/
RUN dotnet restore WaterTrail.Api/WaterTrail.Api.csproj

# Copy all source code and build
COPY . .
WORKDIR /src/WaterTrail.Api
RUN dotnet build -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Create data directory for SQLite
RUN mkdir -p /app/data

COPY --from=publish /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "WaterTrail.Api.dll"]
