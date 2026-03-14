# Stage 1: Build and Publish
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS publish
WORKDIR /src

# Reduce SDK memory usage
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1
ENV DOTNET_NOLOGO=1
ENV DOTNET_GCConserveMemory=9
ENV DOTNET_GCHeapHardLimit=0x1E000000

# Copy solution and project files
COPY ["NNews.API/NNews.API.csproj", "NNews.API/"]
COPY ["NNews.Application/NNews.Application.csproj", "NNews.Application/"]
COPY ["NNews.Domain/NNews.Domain.csproj", "NNews.Domain/"]
COPY ["NNews.Infra/NNews.Infra.csproj", "NNews.Infra/"]
COPY ["NNews.Infra.Interfaces/NNews.Infra.Interfaces.csproj", "NNews.Infra.Interfaces/"]

# Restore dependencies
RUN dotnet restore "NNews.API/NNews.API.csproj"

# Copy all source files
COPY . .

# Publish (already includes build) with minimal memory usage
WORKDIR "/src/NNews.API"
RUN dotnet publish "NNews.API.csproj" -c Release -o /app/publish /p:UseAppHost=false /m:1 /p:TieredCompilation=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Install curl for health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Create necessary directories with correct permissions
RUN mkdir -p /app/logs /app/certs && \
    chmod 755 /app/logs && \
    chmod 755 /app/certs

# Expose port
EXPOSE 80

# Copy published application
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Docker

# Run the application
ENTRYPOINT ["dotnet", "NNews.API.dll"]
