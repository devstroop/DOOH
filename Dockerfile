FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY Server/DOOH.Server.csproj Server/
COPY Client/DOOH.Client.csproj Client/
RUN dotnet restore "Server/DOOH.Server.csproj"
COPY . .
WORKDIR "/src/Server"
RUN dotnet build "DOOH.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "DOOH.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=publish /app/publish .

USER $APP_UID
ENTRYPOINT ["sh", "entrypoint.sh"]
