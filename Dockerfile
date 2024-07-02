FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Create /https directory with appropriate permissions
RUN mkdir -p /https && chown -R $APP_UID:$APP_UID /https

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

# Copy published app and entrypoint script
COPY --from=publish /app/publish .

COPY entrypoint.sh .
RUN chmod +x entrypoint.sh

# Ensure /https is writable and has the correct permissions
RUN mkdir -p /https && chown -R $APP_UID:$APP_UID /https

USER $APP_UID

ENTRYPOINT ["bash", "./entrypoint.sh"]
