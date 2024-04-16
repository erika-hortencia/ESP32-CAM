FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /App

## Project API Builder. [.NET 5.0 (SDK)]
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS builder
WORKDIR /App
COPY Core/ ./Core/
RUN dotnet publish ./Core/Core.csproj -c "Debug" -o out

## Project API runtime (.NET 5.0 (RUNTIME)).
FROM base AS core
ENV DEBIAN_FRONTEND=noninteractive
WORKDIR /App
COPY --from=builder /App/out .
ENTRYPOINT ["dotnet", "Core.dll"]
