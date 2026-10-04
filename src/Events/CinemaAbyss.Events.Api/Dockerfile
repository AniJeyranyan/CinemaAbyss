FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY CinemaAbyss.sln .
COPY src/Shared/CinemaAbyss.SharedKernel/CinemaAbyss.SharedKernel.csproj src/Shared/CinemaAbyss.SharedKernel/
COPY src/Events/CinemaAbyss.Events.Domain/CinemaAbyss.Events.Domain.csproj src/Events/CinemaAbyss.Events.Domain/
COPY src/Events/CinemaAbyss.Events.Application/CinemaAbyss.Events.Application.csproj src/Events/CinemaAbyss.Events.Application/
COPY src/Events/CinemaAbyss.Events.Infrastructure/CinemaAbyss.Events.Infrastructure.csproj src/Events/CinemaAbyss.Events.Infrastructure/
COPY src/Events/CinemaAbyss.Events.Api/CinemaAbyss.Events.Api.csproj src/Events/CinemaAbyss.Events.Api/
RUN dotnet restore src/Events/CinemaAbyss.Events.Api/CinemaAbyss.Events.Api.csproj

COPY src/Shared/CinemaAbyss.SharedKernel/ src/Shared/CinemaAbyss.SharedKernel/
COPY src/Events/ src/Events/
RUN dotnet publish src/Events/CinemaAbyss.Events.Api/CinemaAbyss.Events.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8082
ENTRYPOINT ["dotnet", "CinemaAbyss.Events.Api.dll"]
