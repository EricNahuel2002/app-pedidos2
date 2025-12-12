CREATE DATABASE IF NOT EXISTS db_menus_test;
CREATE DATABASE IF NOT EXISTS db_notificaciones_test;
CREATE DATABASE IF NOT EXISTS db_ordenes_test;
CREATE DATABASE IF NOT EXISTS db_usuarios_test;

GRANT ALL PRIVILEGES ON db_menus_test.* TO 'admin'@'%';
GRANT ALL PRIVILEGES ON db_notificaciones_test.* TO 'admin'@'%';
GRANT ALL PRIVILEGES ON db_ordenes_test.* TO 'admin'@'%';
GRANT ALL PRIVILEGES ON db_usuarios_test.* TO 'admin'@'%';
FLUSH PRIVILEGES;
