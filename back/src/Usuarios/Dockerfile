# Dockerfile.service - Dockerfile parametrizable para proyectos dentro de /src
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
ARG APP_UID=1000
USER ${APP_UID}
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG SERVICE_NAME=Usuarios
ARG PROJECT_PATH=src/Usuarios/Usuarios.csproj

WORKDIR /app-root

# Copiamos archivos de solución y metadatos necesarios (importante: incluir .dcproj si está referenciado)
COPY AppPedidos.sln .
COPY docker-compose.dcproj .
# Si tienes otros archivos de solución en la raíz (global.json, Directory.Build.props) agrégalos:
# COPY global.json .
# COPY Directory.Build.props .

# Copiamos el código fuente
COPY src/ ./src/

# Restauramos paquetes (por solución para caché)
RUN dotnet restore "AppPedidos.sln"

# Construimos el proyecto específico
RUN dotnet build "${PROJECT_PATH}" -c ${BUILD_CONFIGURATION} -o /app-build --no-restore

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
ARG PROJECT_PATH=src/Usuarios/Usuarios.csproj

RUN dotnet publish "${PROJECT_PATH}" -c ${BUILD_CONFIGURATION} -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS dev
WORKDIR /app

COPY AppPedidos.sln .
COPY src/ ./src/ 
COPY docker-compose.dcproj .

RUN dotnet restore "AppPedidos.sln"

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Usuarios.dll"]
