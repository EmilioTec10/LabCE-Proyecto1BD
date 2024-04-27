
USE LabCE;

-- Agregar la columna email_op a la tabla Reserva y establecerla como una clave externa (para palmadas)
ALTER TABLE Reserva
ADD email_op VARCHAR(50) FOREIGN KEY REFERENCES Operador(email_op);
