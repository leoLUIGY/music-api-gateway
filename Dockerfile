# Etapa 1 - Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY ["music-api-gateway.csproj", "./"]

RUN dotnet restore "music-api-gateway.csproj"

COPY . .

RUN dotnet publish "music-api-gateway.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore


# Etapa 2 - Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Development

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "music-api-gateway.dll"]