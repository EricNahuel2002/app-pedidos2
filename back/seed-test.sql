USE db_usuarios_test;

INSERT INTO Usuarios (Nombre, Email, Contrasenia)
VALUES ('Eric', 'ericaquino2002@gmail.com', '$2a$11$2AkPdj2y7dD18yHQaiqT9egFiQmrqUDetBXdkdI.gTyROvMnCUAoK');

SET @idUsuario = LAST_INSERT_ID();

INSERT INTO Clientes (IdUsuario, Direccion, NumeroTelefonico, Saldo)
VALUES (@idUsuario, 'Lamadrid', '123456789', 0);

INSERT INTO UsuariosRoles (IdUsuario, IdRol)
VALUES (@idUsuario, (SELECT Id FROM Roles WHERE Nombre = 'cliente'));

USE db_menus_test;

INSERT INTO Menus (Id, Nombre, Descripcion, Precio, Imagen)
VALUES (1, 'Empanadas', 'Ricas empanadas de jamon y queso', 10, 'nada');

USE db_ordenes_test;

INSERT INTO Ordenes (IdOrden, IdCliente, IdMenu, NombreMenu, NombreCliente, EmailCliente, PrecioAPagar, Estado, Direccion, FechaOrden)
VALUES (1, 1, 1, 'Empanadas', 'Eric', 'ericaquino2002@gmail.com', 10, 'PENDIENTE', 'Lamadrid', NOW());
