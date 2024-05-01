-- Usar LabCE
USE LabCE;

-- Insertar datos en la tabla Laboratorio
INSERT INTO Laboratorio (ID_lab, capacidad, computadoras, facilidades)
VALUES 
    ('F2-07', 26, 10, '2 Proyector, 2 pizarra, interactivo'),
    ('F2-08', 30, 15, '1 Pizarra, 1Proyector'),
    ('F2-09', 28, 24, '1 Pizarra, 1Proyector, 1 Pantalla ');

-- Insertar datos en la tabla Estudiante
INSERT INTO Estudiante (email_est, nombre, apellido1, apellido2, carnet)
VALUES 
    ('estudiante1@estudiantec.cr', 'Juan', 'Perez', 'Garcia', '2021552134'),
    ('estudiante2@estudiantec.cr', 'María', 'Lopez', 'Rojas', '2019356103'),
    ('estudiante3@estudiantec.cr', 'Pedro', 'González', 'Díaz', '2022080645');

-- Insertar datos en la tabla Profesor
INSERT INTO Profesor (email_prof, nombre, apellido1, apellido2, cedula, password_prof, es_admin)
VALUES 
    ('marivera@itcr.ac.cr', 'Marco', 'Rivera', 'Meneses', '1234567890', 'contraseña1',0),
    ('lachavarria@itcr.ac.cr', 'Luis Alberto', 'Chavarría', 'Zamora', '2087654321', 'contraseña2',1),
    ('labarboza@itcr.ac.cr', 'Luis Alonso', 'Barboza', 'Artavia', '1357924680', 'contraseña3',0);


-- Insertar datos en la tabla Reserva
INSERT INTO Reserva (fecha, hora_inicio, hora_fin, ID_lab, email_prof, email_est, estado, descripcion, palmada, email_op)
VALUES 
    ('2024-04-27', '08:00:00', '10:00:00', 'F2-07', NULL, 'estudiante1@estudiantec.cr', 'Ocupado', 'Reserva para Estudio', 0, NULL),
    ('2024-04-28', '10:00:00', '12:00:00', 'F2-08', 'marivera@itcr.ac.cr', NULL, 'Ocupado', 'Reserva para examen Bases', 0, NULL),
    ('2024-04-29', '14:00:00', '16:00:00', 'F2-08', NULL, 'estudiante3@estudiantec.cr', 'Ocupado', 'Palmada Datos 1', 1, 'saraym.rojas@estudiantec.cr');

-- Insertar datos en la tabla Activos
INSERT INTO Activos (ID_activo, ID_lab, tipo, estado, necesita_aprovacion, fecha_compra, marca)
VALUES 
    ('ACT001', 'F2-07', 'Equipo de laboratorio', 'Disponible', 0, '2023-01-15', 'Marca A'),
    ('ACT002', 'F2-08', 'Computadora', 'Prestado', 1, '2023-02-20', 'Marca B'),
    ('ACT003', 'F2-07', 'Multimetro', 'Dañado', 1, '2023-03-25', 'Marca C');

-- Insertar datos en la tabla Operador
INSERT INTO Operador (email_op, contrasena_op, nombre, apellido1, apellido2, cedula, carnet, fecha_nacimiento,es_admin, revisado, aprovado )
VALUES 
	('operador11@estudiantec.cr', 'contraseña2', 'Laura', 'Martínez', 'Fernández', '987654321', '2019077916', '2000-08-20',0,1,0),
    ('operador13@estudiantec.cr', 'contraseña3', 'Ana', 'Gómez', 'López', '567890123', '2022019916', '2005-12-10',0,0,0),
	('operador43@estudiantec.cr', 'contraseña2', 'Laura', 'Martínez', 'Fernández', '987654321', '2019077916', '2000-08-20',1,1,1),
    ('operador41@estudiantec.cr', 'contraseña3', 'Ana', 'Gómez', 'López', '567890123', '2022019916', '2005-12-10',0,1,1),
	('operador4@estudiantec.cr', 'contraseña5', 'María', 'González', 'Sánchez', '987654321', '2019077922', '2000-09-15', 0,1,1),
    ('saraym.rojas@estudiantec.cr', 'contraseña1', 'Mariana', 'Rojas', 'Rojas', '118480103', '2020076936', '2002-07-19',1,1,1);

-- Insertar datos en la tabla Prestamo
INSERT INTO Prestamo (ID_activo, Fecha_Hora_Solicitud, Fecha_Hora_Devolucion, estado, activo, email_prof, email_est)
VALUES 
    ('ACT001', '2024-04-25 09:00:00', '2024-04-28 09:00:00', 'Aprobado', 1, NULL, 'estudiante2@estudiantec.cr'), --sin aprov
    ('ACT002', '2024-04-26 10:00:00', '2024-04-29 10:00:00', 'Pendiente', 0, 'marivera@itcr.ac.cr', 'estudiante1@estudiantec.cr'),
    ('ACT003', '2024-04-27 11:00:00', '2024-04-30 11:00:00', 'Aprovado', 0, 'labarboza@itcr.ac.cr', NULL); --para prof

-- Insertar datos en la tabla Averia
INSERT INTO Averia (ID_prestamo, descripcion)
VALUES 
    (1, 'Equipo con fallo en la conexión'),
    (2, 'Pantalla de la computadora rota'),
    (3, 'multimetro sin cable');

-- Insertar datos en la tabla Turno
INSERT INTO Turno (email_op, fecha_hora_inicio, fecha_hora_fin)
VALUES 
    ('saraym.rojas@estudiantec.cr', '2024-04-27 08:00:00', '2024-04-27 16:00:00'),
    ('saraym.rojas@estudiantec.cr', '2024-04-28 08:00:00', '2024-04-28 16:00:00'),
    ('operador3@estudiantec.cr', '2024-04-29 08:00:00', '2024-04-29 16:00:00');
