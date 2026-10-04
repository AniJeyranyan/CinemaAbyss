FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY CinemaAbyss.sln .
COPY src/Shared/CinemaAbyss.SharedKernel/CinemaAbyss.SharedKernel.csproj src/Shared/CinemaAbyss.SharedKernel/
COPY src/Monolith/CinemaAbyss.Monolith.Domain/CinemaAbyss.Monolith.Domain.csproj src/Monolith/CinemaAbyss.Monolith.Domain/
COPY src/Monolith/CinemaAbyss.Monolith.Application/CinemaAbyss.Monolith.Application.csproj src/Monolith/CinemaAbyss.Monolith.Application/
COPY src/Monolith/CinemaAbyss.Monolith.Infrastructure/CinemaAbyss.Monolith.Infrastructure.csproj src/Monolith/CinemaAbyss.Monolith.Infrastructure/
COPY src/Monolith/CinemaAbyss.Monolith.Api/CinemaAbyss.Monolith.Api.csproj src/Monolith/CinemaAbyss.Monolith.Api/
RUN dotnet restore src/Monolith/CinemaAbyss.Monolith.Api/CinemaAbyss.Monolith.Api.csproj

COPY src/Shared/CinemaAbyss.SharedKernel/ src/Shared/CinemaAbyss.SharedKernel/
COPY src/Monolith/ src/Monolith/
RUN dotnet publish src/Monolith/CinemaAbyss.Monolith.Api/CinemaAbyss.Monolith.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "CinemaAbyss.Monolith.Api.dll"]
