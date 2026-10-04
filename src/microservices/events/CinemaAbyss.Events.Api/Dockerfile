# Build context: src/microservices/events
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY CinemaAbyss.Events.Domain/*.csproj CinemaAbyss.Events.Domain/
COPY CinemaAbyss.Events.Application/*.csproj CinemaAbyss.Events.Application/
COPY CinemaAbyss.Events.Infrastructure/*.csproj CinemaAbyss.Events.Infrastructure/
COPY CinemaAbyss.Events.Api/*.csproj CinemaAbyss.Events.Api/
RUN dotnet restore CinemaAbyss.Events.Api/CinemaAbyss.Events.Api.csproj

COPY . .
RUN dotnet publish CinemaAbyss.Events.Api/CinemaAbyss.Events.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8082
ENTRYPOINT ["dotnet", "CinemaAbyss.Events.Api.dll"]
