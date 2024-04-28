-- Usar LabCE
USE LabCE;

ALTER TABLE Profesor
ADD es_admin BIT DEFAULT 0;

CREATE TABLE Reserva (
    ID_reserva INT IDENTITY(1,1) PRIMARY KEY, -- COnsecutivo 1 a 1
    fecha DATE,
    hora_inicio TIME,
    hora_fin TIME,
    ID_lab VARCHAR(7),
    email_prof VARCHAR(50),
    email_est VARCHAR(50),
    estado VARCHAR(20), --  VARCHAR para los estados
    descripcion VARCHAR(100),
    palmada BIT DEFAULT 0,
    FOREIGN KEY (ID_lab) REFERENCES Laboratorio(ID_lab),
    FOREIGN KEY (email_prof) REFERENCES Profesor(email_prof),
    FOREIGN KEY (email_est) REFERENCES Estudiante(email_est),
);

CREATE TABLE Activos (
    ID_activo VARCHAR(50) PRIMARY KEY,
    ID_lab VARCHAR(7),
    tipo VARCHAR(100),
    estado VARCHAR(20), -- VARCHAR para los estados
    necesita_aprovacion BIT,
    fecha_compra DATE,
    marca VARCHAR(100),
    FOREIGN KEY (ID_lab) REFERENCES Laboratorio(ID_lab)
);

CREATE TABLE Operador (
    email_op VARCHAR(50) PRIMARY KEY,
    contrasena_op VARCHAR(100),
    nombre VARCHAR(50),
    apellido1 VARCHAR(50),
    apellido2 VARCHAR(50),
    cedula VARCHAR(9),
    carnet VARCHAR(20),
    fecha_nacimiento DATE,
    es_admin BIT DEFAULT 0,
    revisado BIT DEFAULT 1,
    aprovado BIT DEFAULT 1
);

CREATE TABLE Prestamo (
    ID_prestamo INT IDENTITY(1,1) PRIMARY KEY, -- COnsecutivo 1 a 1
    ID_activo VARCHAR(50),
    Fecha_Hora_Solicitud DATETIME,
    Fecha_Hora_Devolucion DATETIME,
    estado VARCHAR(20), --  VARCHAR para los estados
    activo BIT,
    email_prof VARCHAR(50),
    email_est VARCHAR(50),
    FOREIGN KEY (ID_activo) REFERENCES Activos(ID_activo),
    FOREIGN KEY (email_prof) REFERENCES Profesor(email_prof),
    FOREIGN KEY (email_est) REFERENCES Estudiante(email_est)
);

CREATE TABLE Averia (
    ID_averia INT IDENTITY(1,1) PRIMARY KEY, -- COnsecutivo 1 a 1
    ID_prestamo INT,
    descripcion VARCHAR(100),
    FOREIGN KEY (ID_prestamo) REFERENCES Prestamo(ID_prestamo)
);

CREATE TABLE Turno (
    ID_turno INT IDENTITY(1,1) PRIMARY KEY, -- COnsecutivo 1 a 1
    email_op VARCHAR(50),
    fecha_hora_inicio DATETIME,
    fecha_hora_fin DATETIME,
    FOREIGN KEY (email_op) REFERENCES Operador(email_op)
);


