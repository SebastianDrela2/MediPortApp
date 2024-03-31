# Use the .NET SDK image to build the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the solution file and restore dependencies
COPY MediPort.sln .
COPY MediPort.Console ./MediPort.Console/
COPY MediPort.Api ./MediPort.Api/
COPY MediPort.RestApi ./MediPort.RestApi/
COPY MediPort.Api.Tests ./MediPort.Api.Tests/

RUN dotnet restore

# Publish the application
RUN dotnet publish MediPort.Console/MediPort.Console.csproj -c Release -o /app/MediPort.RestApi/out

# Use a smaller runtime image for the final stage
FROM mcr.microsoft.com/dotnet/runtime:6.0
WORKDIR /app

# Copy the published output from the build stage to the final stage
COPY --from=build /app/MediPort.RestApi/out ./

# Set the entry point for the container
ENTRYPOINT ["dotnet", "MediPort.RestApi.dll"]