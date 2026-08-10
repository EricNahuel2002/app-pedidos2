# App Pedidos

Plataforma de pedidos de comida con arquitectura de microservicios: backend en .NET 9 y frontend en Angular.

## Estructura

- `back/` — Microservicios .NET 9:
  - `ApiGateway` — gateway con Ocelot (único punto de entrada, puerto 5100)
  - `Auth` — generación y validación de tokens JWT, login y logout
  - `Usuarios` — registro de clientes y repartidores, credenciales (contraseñas con BCrypt)
  - `Menus` — catálogo de menús
  - `Ordenes` — ciclo de vida de las órdenes
  - `Notificaciones` — notificaciones a usuarios
  - `docker-compose.yml`, `docker-compose-dev.yml` (hot reload) y `docker-compose-test.yml`
- `front/` — Aplicación Angular 20 con PrimeNG (puerto 4200)

## Configuración de JWT

La clave de firma de los tokens **no se almacena en los archivos de configuración base** (`appsettings.json`). Se lee de la variable de entorno `Jwt__Key` y debe coincidir en todos los microservicios.

Al ejecutar con Docker Compose la variable se inyecta automáticamente desde `docker-compose*.yml`. En desarrollo local, definirla en el entorno o en `appsettings.Development.json` de cada servicio.

| Variable | Valor de desarrollo (no usar en producción) |
| --- | --- |
| `Jwt__Key` | Clave simétrica de 32+ caracteres |
| `Jwt__Issuer` | `auth-api` |
| `Jwt__Audience` | `api-gateway` |

En producción, reemplazar `Jwt__Key` por un secreto generado y protegerla con un gestor de secretos.
