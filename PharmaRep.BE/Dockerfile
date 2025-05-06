# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

EXPOSE 5000
EXPOSE 5001

WORKDIR /app

# Copy all source code
COPY ./PharmaRep.BE .

# Restore dependencies
WORKDIR /app/src/Bootstrapper/Bootstrapper
RUN dotnet restore

# Build the project
RUN dotnet build -c Release --no-restore

# Publish the project
RUN dotnet publish -c Release -o /app/publish --no-restore

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

WORKDIR /app

# Copy published files
COPY --from=build /app/publish .

# Set the entry point
ENTRYPOINT ["dotnet", "Bootstrapper.dll"]
