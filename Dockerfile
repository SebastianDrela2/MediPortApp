# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
COPY . ./
# Restore NuGet packages
RUN dotnet restore MediPort.Console/MediPort.Console.csproj --disable-parallel
RUN dotnet restore MediPort.Api/MediPort.API.csproj --disable-parallel
RUN dotnet publish MediPort.Console/*.csproj -c Release -o out --no-restore

FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /app
COPY --from=build /app/MediPort.Console/out ./

ENTRYPOINT ["dotnet", "MediPort.Console.dll"]