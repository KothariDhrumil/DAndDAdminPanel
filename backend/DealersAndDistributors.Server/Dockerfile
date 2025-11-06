FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
# Create non-root user
RUN addgroup -S dotnet && adduser -S dotnet -G dotnet
USER dotnet
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src
# Copy csproj and restore as distinct layers
COPY DealersAndDistributors.sln ./
COPY DealersAndDistributors.Server/DealersAndDistributors.Server.csproj DealersAndDistributors.Server/
COPY Application/Application.csproj Application/
COPY Domain/Domain.csproj Domain/
COPY Infrastructure/Infrastructure.csproj Infrastructure/
COPY Shared/Shared.csproj Shared/
COPY SharedKernel/SharedKernel.csproj SharedKernel/
COPY AuthPermissions.SqlServer/AuthPermissions.SqlServer.csproj AuthPermissions.SqlServer/
# Copy local DLL folder so project references like ..\Library\*.dll resolve during build
COPY Library ./Library
RUN dotnet restore DealersAndDistributors.Server/DealersAndDistributors.Server.csproj --nologo

# Copy the rest of the source and build
COPY . .
WORKDIR /src/DealersAndDistributors.Server
RUN dotnet publish -c Release -o /app/publish \
    -p:PublishTrimmed=false \
    -p:PublishReadyToRun=false \
    --nologo

FROM base AS final
# Healthcheck (hits /health endpoint)
HEALTHCHECK --interval=30s --timeout=5s --start-period=10s CMD wget -qO- http://localhost:8080/health || exit 1
WORKDIR /app
# Set URLs to 0.0.0.0:8080
ENV ASPNETCORE_URLS=http://+:8080
ENV DOTNET_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "DealersAndDistributors.Server.dll"]
