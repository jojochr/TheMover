# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY . .

# Get all nuget packages
RUN dotnet restore

# Build application to the containers App directory
# UseAppHost=false -> Makes it so no executables get generated. Just dlls
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app /p:UseAppHost=false

#############
# Serve Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS final
WORKDIR /app

# Copy build artifacts from earlier build Stage
COPY --from=build /app ./

EXPOSE 8080/tcp
ENTRYPOINT ["dotnet", "TheMover.API.dll"]
