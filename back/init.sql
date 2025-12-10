CREATE DATABASE IF NOT EXISTS db_menus;
CREATE DATABASE IF NOT EXISTS db_notificaciones;
CREATE DATABASE IF NOT EXISTS db_ordenes;
CREATE DATABASE IF NOT EXISTS db_usuarios;

GRANT ALL PRIVILEGES ON db_menus.* TO 'admin'@'%';
GRANT ALL PRIVILEGES ON db_notificaciones.* TO 'admin'@'%';
GRANT ALL PRIVILEGES ON db_ordenes.* TO 'admin'@'%';
GRANT ALL PRIVILEGES ON db_usuarios.* TO 'admin'@'%';
FLUSH PRIVILEGES;
