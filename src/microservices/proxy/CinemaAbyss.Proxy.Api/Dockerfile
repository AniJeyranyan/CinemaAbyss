# Build context: src/microservices/proxy
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY CinemaAbyss.Proxy.Domain/*.csproj CinemaAbyss.Proxy.Domain/
COPY CinemaAbyss.Proxy.Application/*.csproj CinemaAbyss.Proxy.Application/
COPY CinemaAbyss.Proxy.Infrastructure/*.csproj CinemaAbyss.Proxy.Infrastructure/
COPY CinemaAbyss.Proxy.Api/*.csproj CinemaAbyss.Proxy.Api/
RUN dotnet restore CinemaAbyss.Proxy.Api/CinemaAbyss.Proxy.Api.csproj

COPY . .
RUN dotnet publish CinemaAbyss.Proxy.Api/CinemaAbyss.Proxy.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8000
ENTRYPOINT ["dotnet", "CinemaAbyss.Proxy.Api.dll"]
