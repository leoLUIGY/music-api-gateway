FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY ["music-api-gateway.csproj", "./"]

RUN dotnet restore "music-api-gateway.csproj"

COPY . .

RUN dotnet publish "music-api-gateway.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_HTTP_PORTS=8080

ENV Services__RecommendationApi=http://host.docker.internal:5001
ENV Services__CatalogAPI=http://host.docker.internal:5000

COPY --from=build /app/publish .

COPY .env .env

EXPOSE 8080

ENTRYPOINT ["dotnet", "music-api-gateway.dll"]