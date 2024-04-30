-- Crear laDB 


-- Usar  LabCE
USE LabCE;

-- Crear tabla Laboratorio
CREATE TABLE Laboratorio (
    ID_lab  VARCHAR(7) PRIMARY KEY,
    capacidad INT,
    computadoras INT,
    facilidades TEXT
);

-- Crear tabla Estudiante
CREATE TABLE Estudiante (
    email_est VARCHAR(50) PRIMARY KEY,
    nombre VARCHAR(50),
    apellido1 VARCHAR(50),
    apellido2 VARCHAR(50),
    carnet VARCHAR(10) UNIQUE
);

-- Crear tabla Profesor
CREATE TABLE Profesor (
    email_prof VARCHAR(50) PRIMARY KEY,
    nombre VARCHAR(50),
    apellido1 VARCHAR(50),
    apellido2 VARCHAR(50),
    cedula VARCHAR(10) UNIQUE,
    password_prof VARCHAR(100)
);
