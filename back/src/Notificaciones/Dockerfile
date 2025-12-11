# Dockerfile.service - Dockerfile parametrizable para proyectos dentro de /src
# Uso: docker build --build-arg SERVICE_NAME=Usuarios --build-arg PROJECT_PATH=src/Usuarios/Usuarios.csproj -t usuarios .

# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
ARG APP_UID=1000
USER ${APP_UID}
WORKDIR /app
EXPOSE 5000

# SDK image for building/publishing
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG SERVICE_NAME=Notificaciones
ARG PROJECT_PATH=src/Notificaciones/Notificaciones.csproj

WORKDIR /app-root

# Copiamos la solution y el código fuente
COPY AppPedidos.sln .
COPY docker-compose.dcproj .
COPY src/ ./src/

# Restauramos paquetes (por solución para caché)
RUN dotnet restore "AppPedidos.sln"

# Construimos el proyecto específico (para cacheo fino podés ajustar)
RUN dotnet build "${PROJECT_PATH}" -c ${BUILD_CONFIGURATION} -o /app-build --no-restore

# Publicar el proyecto seleccionado
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
ARG PROJECT_PATH=src/Notificaciones/Notificaciones.csproj

RUN dotnet publish "${PROJECT_PATH}" -c ${BUILD_CONFIGURATION} -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS dev
WORKDIR /app

COPY AppPedidos.sln .
COPY src/ ./src/ 

RUN dotnet restore "AppPedidos.sln"

# Imagen final runtime
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Notificaciones.dll"]
