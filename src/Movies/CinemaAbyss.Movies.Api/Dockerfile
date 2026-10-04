FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY CinemaAbyss.sln .
COPY src/Shared/CinemaAbyss.SharedKernel/CinemaAbyss.SharedKernel.csproj src/Shared/CinemaAbyss.SharedKernel/
COPY src/Movies/CinemaAbyss.Movies.Domain/CinemaAbyss.Movies.Domain.csproj src/Movies/CinemaAbyss.Movies.Domain/
COPY src/Movies/CinemaAbyss.Movies.Application/CinemaAbyss.Movies.Application.csproj src/Movies/CinemaAbyss.Movies.Application/
COPY src/Movies/CinemaAbyss.Movies.Infrastructure/CinemaAbyss.Movies.Infrastructure.csproj src/Movies/CinemaAbyss.Movies.Infrastructure/
COPY src/Movies/CinemaAbyss.Movies.Api/CinemaAbyss.Movies.Api.csproj src/Movies/CinemaAbyss.Movies.Api/
RUN dotnet restore src/Movies/CinemaAbyss.Movies.Api/CinemaAbyss.Movies.Api.csproj

COPY src/Shared/CinemaAbyss.SharedKernel/ src/Shared/CinemaAbyss.SharedKernel/
COPY src/Movies/ src/Movies/
RUN dotnet publish src/Movies/CinemaAbyss.Movies.Api/CinemaAbyss.Movies.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8081
ENTRYPOINT ["dotnet", "CinemaAbyss.Movies.Api.dll"]
