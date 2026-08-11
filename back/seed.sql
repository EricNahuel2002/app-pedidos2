-- ============================================================================
-- SEED de desarrollo para App Pedidos
-- Bases: db_usuarios, db_menus, db_ordenes, db_notificaciones
--
-- Contraseña de todos los usuarios: Password123!
-- El rol ya se crea al arrancar el servicio Usuarios (cliente/repartidor/administrador).
--
-- Ejecutar con el backend levantado:
--   docker compose exec mysql mysql --default-character-set=utf8mb4 -uadmin -proot < seed.sql
--
-- Es idempotente: solo inserta datos que no existen.
-- ============================================================================

USE db_usuarios;

-- ------------------------------ USUARIOS ------------------------------
INSERT INTO Usuarios (Nombre, Email, Contrasenia)
SELECT 'Eric Aquino', 'cliente@example.com', '$2a$11$8mb9aYX5oCP51.J7rThyMO7dAR462v9IWN60vOTJFMEg0TjFr5j4y'
WHERE NOT EXISTS (SELECT 1 FROM Usuarios WHERE Email = 'cliente@example.com');

INSERT INTO Usuarios (Nombre, Email, Contrasenia)
SELECT 'Maria Garcia', 'maria@example.com', '$2a$11$8mb9aYX5oCP51.J7rThyMO7dAR462v9IWN60vOTJFMEg0TjFr5j4y'
WHERE NOT EXISTS (SELECT 1 FROM Usuarios WHERE Email = 'maria@example.com');

INSERT INTO Usuarios (Nombre, Email, Contrasenia)
SELECT 'Juan Perez', 'repartidor@example.com', '$2a$11$8mb9aYX5oCP51.J7rThyMO7dAR462v9IWN60vOTJFMEg0TjFr5j4y'
WHERE NOT EXISTS (SELECT 1 FROM Usuarios WHERE Email = 'repartidor@example.com');

INSERT INTO Usuarios (Nombre, Email, Contrasenia)
SELECT 'Administrador', 'admin@example.com', '$2a$11$8mb9aYX5oCP51.J7rThyMO7dAR462v9IWN60vOTJFMEg0TjFr5j4y'
WHERE NOT EXISTS (SELECT 1 FROM Usuarios WHERE Email = 'admin@example.com');

INSERT INTO Usuarios (Nombre, Email, Contrasenia)
SELECT 'Carlos Gomez', 'carlos@example.com', '$2a$11$8mb9aYX5oCP51.J7rThyMO7dAR462v9IWN60vOTJFMEg0TjFr5j4y'
WHERE NOT EXISTS (SELECT 1 FROM Usuarios WHERE Email = 'carlos@example.com');

SET @idEric = (SELECT Id FROM Usuarios WHERE Email = 'cliente@example.com');
SET @idMaria = (SELECT Id FROM Usuarios WHERE Email = 'maria@example.com');
SET @idJuan = (SELECT Id FROM Usuarios WHERE Email = 'repartidor@example.com');
SET @idAdmin = (SELECT Id FROM Usuarios WHERE Email = 'admin@example.com');
SET @idCarlos = (SELECT Id FROM Usuarios WHERE Email = 'carlos@example.com');

-- ------------------------------ CLIENTES ------------------------------
INSERT INTO Clientes (IdUsuario, Direccion, NumeroTelefonico, Saldo)
SELECT @idEric, 'Lamadrid 123', '1122334455', 150.50
WHERE NOT EXISTS (SELECT 1 FROM Clientes WHERE IdUsuario = @idEric);

INSERT INTO Clientes (IdUsuario, Direccion, NumeroTelefonico, Saldo)
SELECT @idMaria, 'Av. Siempre Viva 742', '4455667788', 75.00
WHERE NOT EXISTS (SELECT 1 FROM Clientes WHERE IdUsuario = @idMaria);

-- ------------------------------ REPARTIDORES ------------------------------
INSERT INTO Repartidores (IdUsuario, Dni, FotoDniUrl, Verificado)
SELECT @idJuan, '30123456', '', 1
WHERE NOT EXISTS (SELECT 1 FROM Repartidores WHERE IdUsuario = @idJuan);

INSERT INTO Repartidores (IdUsuario, Dni, FotoDniUrl, Verificado)
SELECT @idCarlos, '40223344', '', 0
WHERE NOT EXISTS (SELECT 1 FROM Repartidores WHERE IdUsuario = @idCarlos);

-- ------------------------------ USUARIOS-ROLES ------------------------------
INSERT INTO UsuariosRoles (IdUsuario, IdRol)
SELECT @idEric, Id FROM Roles
WHERE Nombre = 'cliente' AND NOT EXISTS (SELECT 1 FROM UsuariosRoles WHERE IdUsuario = @idEric AND IdRol = (SELECT Id FROM Roles WHERE Nombre = 'cliente'));

INSERT INTO UsuariosRoles (IdUsuario, IdRol)
SELECT @idMaria, Id FROM Roles
WHERE Nombre = 'cliente' AND NOT EXISTS (SELECT 1 FROM UsuariosRoles WHERE IdUsuario = @idMaria AND IdRol = (SELECT Id FROM Roles WHERE Nombre = 'cliente'));

INSERT INTO UsuariosRoles (IdUsuario, IdRol)
SELECT @idJuan, Id FROM Roles
WHERE Nombre = 'repartidor' AND NOT EXISTS (SELECT 1 FROM UsuariosRoles WHERE IdUsuario = @idJuan AND IdRol = (SELECT Id FROM Roles WHERE Nombre = 'repartidor'));

INSERT INTO UsuariosRoles (IdUsuario, IdRol)
SELECT @idAdmin, Id FROM Roles
WHERE Nombre = 'administrador' AND NOT EXISTS (SELECT 1 FROM UsuariosRoles WHERE IdUsuario = @idAdmin AND IdRol = (SELECT Id FROM Roles WHERE Nombre = 'administrador'));

INSERT INTO UsuariosRoles (IdUsuario, IdRol)
SELECT @idCarlos, Id FROM Roles
WHERE Nombre = 'repartidor' AND NOT EXISTS (SELECT 1 FROM UsuariosRoles WHERE IdUsuario = @idCarlos AND IdRol = (SELECT Id FROM Roles WHERE Nombre = 'repartidor'));

-- ============================================================================
USE db_menus;

INSERT IGNORE INTO Menus (Id, Nombre, Descripcion, Precio, Imagen) VALUES
(1, 'Empanadas de carne', 'Empanadas caseras al horno, rellenas de carne cortada a cuchillo.', 800, 'https://picsum.photos/seed/empanadas/400/300'),
(2, 'Pizza de muzzarella', 'Pizza con salsa, muzzarella y orégano.', 1200, 'https://picsum.photos/seed/pizza/400/300'),
(3, 'Hamburguesa con papas', 'Hamburguesa completa con papas fritas.', 1500, 'https://picsum.photos/seed/hamburguesa/400/300'),
(4, 'Lomo completo', 'Lomo con jamón, queso, huevo y papas fritas.', 2000, 'https://picsum.photos/seed/lomo/400/300'),
(5, 'Milanesa napolitana', 'Milanesa con salsa, jamón, queso y papas.', 1800, 'https://picsum.photos/seed/milanesa/400/300'),
(6, 'Tarta de verdura', 'Tarta de calabaza, espinaca y queso.', 900, 'https://picsum.photos/seed/tarta/400/300'),
(7, 'Sandwich de milanesa', 'Sándwich de milanesa con lechuga, tomate y mayonesa.', 1100, 'https://picsum.photos/seed/sandwich/400/300'),
(8, 'Ensalada César', 'Ensalada con pollo, croutons, parmesano y salsa César.', 1300, 'https://picsum.photos/seed/cesar/400/300');

-- ============================================================================
USE db_ordenes;

INSERT IGNORE INTO Ordenes (IdOrden, IdCliente, IdMenu, NombreMenu, NombreCliente, EmailCliente, PrecioAPagar, Estado, Direccion, IdRepartidor, NombreRepartidor, DniRepartidor, FechaOrden) VALUES
(1, @idEric, 1, 'Empanadas de carne', 'Eric Aquino', 'cliente@example.com', 800, 'PENDIENTE', 'Lamadrid 123', NULL, NULL, NULL, NOW()),
(2, @idEric, 3, 'Hamburguesa con papas', 'Eric Aquino', 'cliente@example.com', 1500, 'EN CURSO', 'Lamadrid 123', @idJuan, 'Juan Perez', '30123456', NOW()),
(3, @idMaria, 4, 'Lomo completo', 'Maria Garcia', 'maria@example.com', 2000, 'FINALIZADA', 'Av. Siempre Viva 742', @idJuan, 'Juan Perez', '30123456', NOW()),
(4, @idMaria, 6, 'Tarta de verdura', 'Maria Garcia', 'maria@example.com', 900, 'CANCELADA', 'Av. Siempre Viva 742', NULL, NULL, NULL, NOW()),
(5, @idEric, 8, 'Ensalada César', 'Eric Aquino', 'cliente@example.com', 1300, 'PENDIENTE', 'Lamadrid 123', NULL, NULL, NULL, NOW()),
(6, @idEric, 2, 'Pizza de muzzarella', 'Eric Aquino', 'cliente@example.com', 1200, 'FINALIZADA', 'Lamadrid 123', @idJuan, 'Juan Perez', '30123456', NOW()),
(7, @idEric, 5, 'Milanesa napolitana', 'Eric Aquino', 'cliente@example.com', 1800, 'FINALIZADA', 'Lamadrid 123', @idJuan, 'Juan Perez', '30123456', NOW());

INSERT IGNORE INTO Resenas (IdOrden, IdCliente, IdRepartidor, NombreCliente, NombreRepartidor, Puntaje, Comentario, FechaCreacion) VALUES
(3, @idMaria, @idJuan, 'Maria Garcia', 'Juan Perez', 5, 'Excelente repartidor, muy puntual y amable.', NOW()),
(6, @idEric, @idJuan, 'Eric Aquino', 'Juan Perez', 4, 'Muy buen servicio, todo llegó perfecto.', NOW()),
(7, @idEric, @idJuan, 'Eric Aquino', 'Juan Perez', 3, 'Tardó un poco más de lo esperado, pero llegó todo bien.', NOW());

-- ============================================================================
USE db_notificaciones;

INSERT INTO Notificaciones (IdUsuario, Mensaje, Leida, FechaCreacion) VALUES
(@idEric, 'Bienvenido a App Pedidos', 1, NOW()),
(@idEric, 'Tu pedido de Empanadas de carne fue confirmado', 1, NOW()),
(@idEric, 'Tu pedido de Hamburguesa con papas fue tomado por Juan Perez', 0, NOW()),
(@idMaria, 'Tu pedido de Lomo completo fue finalizado', 1, NOW()),
(@idMaria, 'Tu pedido de Tarta de verdura fue cancelado', 0, NOW()),
(@idMaria, 'Bienvenida a App Pedidos', 0, NOW());
