FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY CinemaAbyss.sln .
COPY src/Shared/CinemaAbyss.SharedKernel/CinemaAbyss.SharedKernel.csproj src/Shared/CinemaAbyss.SharedKernel/
COPY src/Proxy/CinemaAbyss.Proxy.Domain/CinemaAbyss.Proxy.Domain.csproj src/Proxy/CinemaAbyss.Proxy.Domain/
COPY src/Proxy/CinemaAbyss.Proxy.Application/CinemaAbyss.Proxy.Application.csproj src/Proxy/CinemaAbyss.Proxy.Application/
COPY src/Proxy/CinemaAbyss.Proxy.Infrastructure/CinemaAbyss.Proxy.Infrastructure.csproj src/Proxy/CinemaAbyss.Proxy.Infrastructure/
COPY src/Proxy/CinemaAbyss.Proxy.Api/CinemaAbyss.Proxy.Api.csproj src/Proxy/CinemaAbyss.Proxy.Api/
RUN dotnet restore src/Proxy/CinemaAbyss.Proxy.Api/CinemaAbyss.Proxy.Api.csproj

COPY src/Shared/CinemaAbyss.SharedKernel/ src/Shared/CinemaAbyss.SharedKernel/
COPY src/Proxy/ src/Proxy/
RUN dotnet publish src/Proxy/CinemaAbyss.Proxy.Api/CinemaAbyss.Proxy.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8000
ENTRYPOINT ["dotnet", "CinemaAbyss.Proxy.Api.dll"]
