using LabCEAPI.Laboratorios;
using LabCEAPI.Prestamos;
using LabCEAPI.Reportes_de_Horas;
using Microsoft.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LabCEAPI.Users
{
    public class Admin
    {
        private string email { get; set; }
        private string contraseña { get; set; }

        public const string connectionString = "Data Source=LAPTOP-GB4ACP2F;Initial Catalog=LabCE;Integrated Security=True;Encrypt=False";

        //Metodo que permite ingresar como administrador a la aplicacion
        public bool ingresar_admin(string email, string contraseña)
        {
            // Consulta SQL para buscar al profesor por email
            string queryProfesor = "SELECT password_prof FROM Profesor WHERE email_prof = @Email AND es_admin = 1";

            // Consulta SQL para buscar al operador por email
            string queryOperador = "SELECT contrasena_op FROM Operador WHERE email_op = @Email AND es_admin = 1";

            // Variable para almacenar la contraseña recuperada de la base de datos
            string contraseñaBaseDeDatos = null;

            // Utilizamos using para garantizar que los recursos se liberen correctamente
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abrimos la conexión
                connection.Open();

                // Creamos un comando SQL para buscar en la tabla Profesor
                using (SqlCommand commandProfesor = new SqlCommand(queryProfesor, connection))
                {
                    // Establecemos el parámetro para el correo electrónico
                    commandProfesor.Parameters.AddWithValue("@Email", email);

                    // Ejecutamos la consulta en la tabla Profesor
                    using (SqlDataReader reader = commandProfesor.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            contraseñaBaseDeDatos = reader.GetString(0); // Suponiendo que la contraseña está en la primera columna
                        }
                    }
                }

                // Si no se encontró ninguna contraseña en la tabla Profesor, buscamos en la tabla Operador
                if (contraseñaBaseDeDatos == null)
                {
                    using (SqlCommand commandOperador = new SqlCommand(queryOperador, connection))
                    {
                        // Establecemos el parámetro para el correo electrónico
                        commandOperador.Parameters.AddWithValue("@Email", email);

                        // Ejecutamos la consulta en la tabla Operador
                        using (SqlDataReader reader = commandOperador.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                contraseñaBaseDeDatos = reader.GetString(0); // Suponiendo que la contraseña está en la primera columna
                            }
                        }
                    }
                }
            }

            // Si no se encontró ningún profesor ni operador con ese correo electrónico
            if (contraseñaBaseDeDatos == null)
            {
                return false;
            }

            // Comparamos la contraseña proporcionada con la contraseña almacenada en la base de datos
            contraseñaBaseDeDatos = EncriptacionMD5.desencriptar(contraseñaBaseDeDatos);
            return contraseña == contraseñaBaseDeDatos;
        }


        //Metodo que registra a un profesor por parte del administrador y lo guarda en la base de datos
        public void registrar_profesor(
            string cedula,
            string nombre,
            string apellidos,
            DateOnly fecha_de_nacimiento,
            int edad,
            string email
            )
        {
            //Registra los datos en la base

            // Consulta SQL para insertar un nuevo profesor
            string query = "INSERT INTO Profesor (email_prof, nombre, apellido1, apellido2, cedula, fecha_de_nacimiento) " +
                           "VALUES (@Email, @Nombre, @Apellido1, @Apellido2, @Cedula, @FechaNacimiento)";

            // Utilizamos using para garantizar que los recursos se liberen correctamente
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abrimos la conexión
                connection.Open();

                // Creamos un comando SQL
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Establecemos los parámetros
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Nombre", nombre);
                    // Aquí puedes separar los apellidos si están en una sola cadena
                    string[] apellidosArray = apellidos.Split(' ');
                    command.Parameters.AddWithValue("@Apellido1", apellidosArray[0]);
                    command.Parameters.AddWithValue("@Apellido2", apellidosArray.Length > 1 ? apellidosArray[1] : "");
                    command.Parameters.AddWithValue("@Cedula", cedula);
                    command.Parameters.AddWithValue("@FechaNacimiento", fecha_de_nacimiento);

                    // Ejecutamos la consulta
                    int rowsAffected = command.ExecuteNonQuery();

                    Profesor profesor = new Profesor();
                    profesor.generar_nueva_contraseña(email);

                    // Verificamos si se insertaron correctamente los datos
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Profesor registrado correctamente.");
                    }
                    else
                    {
                        Console.WriteLine("No se pudo registrar al profesor.");
                    }
                }
            }
        }

        //Metodo que devulve una lista con todos los activos
        public LinkedList<Activo> ver_activos()
        {
            LinkedList<Activo> activos = new LinkedList<Activo>();

            // Consulta SQL para seleccionar los operadores con revisado = 0
            string query = "SELECT ID_activo, ID_lab, tipo, estado, necesita_aprovacion, fecha_compra, marca FROM Activos";

            // Crear la conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abrir la conexión
                connection.Open();

                // Crear el comando SQL
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Ejecutar la consulta y obtener los resultados
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Iterar sobre los resultados
                        while (reader.Read())
                        {
                            // Crear una instancia de Operador con los datos recuperados
                            Activo activo = new Activo(reader.GetString(2), reader.GetString(6), reader.GetString(3), reader.GetString(0), reader.GetString(1), reader.GetDateTime(5), reader.GetBoolean(4));

                            // Agregar el operador a la lista
                            activos.AddLast(activo);
                        }
                    }
                }
            }

            return activos;
        }


        //Permite crear un activo y guardarlo en la base de datos
        public bool crear_activo(Activo activo)
        {
            // Variable para almacenar si la inserción fue exitosa
            bool insercionExitosa = false;

            // Query SQL para insertar un nuevo activo
            string query = @"INSERT INTO Activos (ID_activo, ID_lab, tipo, estado, necesita_aprovacion, fecha_compra, marca) 
                     VALUES (@ID_activo, @ID_lab, @tipo, @estado, @necesita_aprovacion, @fecha_compra, @marca)";

            // Crear la conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abrir la conexión
                connection.Open();

                // Crear el comando SQL
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Establecer los parámetros del comando SQL con los valores del objeto Activo
                    command.Parameters.AddWithValue("@ID_activo", activo.placa);
                    command.Parameters.AddWithValue("@ID_lab", activo.lab);
                    command.Parameters.AddWithValue("@tipo", activo.tipo);
                    command.Parameters.AddWithValue("@estado", activo.estado);
                    command.Parameters.AddWithValue("@necesita_aprovacion", activo.necesita_aprobacion ? 1 : 0);
                    command.Parameters.AddWithValue("@fecha_compra", new DateOnly(activo.purchase_date.Year, activo.purchase_date.Month, activo.purchase_date.Day));
                    command.Parameters.AddWithValue("@marca", activo.marca);

                    // Ejecutar el comando SQL para insertar el nuevo activo y obtener el número de filas afectadas
                    int rowsAffected = command.ExecuteNonQuery();

                    // Verificar si la inserción fue exitosa (si se afectó al menos una fila)
                    insercionExitosa = rowsAffected > 0;
                }
            }

            // Devolver si la inserción fue exitosa
            return insercionExitosa;
        }

        // Metodo que modifica los atributos del activo ingresado
        public bool modificar_activo(Activo activo)
        {
            // Query SQL para actualizar un activo existente
            string query = @"UPDATE Activos 
                     SET ID_lab = @ID_lab, 
                         tipo = @tipo, 
                         estado = @estado, 
                         necesita_aprovacion = @necesita_aprovacion, 
                         fecha_compra = @fecha_compra, 
                         marca = @marca
                     WHERE ID_activo = @ID_activo";

            // Variable para almacenar si la modificación fue exitosa
            bool modificacionExitosa = false;

            // Crear la conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abrir la conexión
                connection.Open();

                // Crear el comando SQL
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Establecer los parámetros del comando SQL con los valores del objeto Activo
                    command.Parameters.AddWithValue("@ID_activo", activo.placa);
                    command.Parameters.AddWithValue("@ID_lab", activo.lab);
                    command.Parameters.AddWithValue("@tipo", activo.tipo);
                    command.Parameters.AddWithValue("@estado", activo.estado);
                    command.Parameters.AddWithValue("@necesita_aprovacion", activo.necesita_aprobacion ? 1 : 0);
                    command.Parameters.AddWithValue("@fecha_compra", activo.purchase_date);
                    command.Parameters.AddWithValue("@marca", activo.marca);

                    // Ejecutar el comando SQL para actualizar el activo y obtener el número de filas afectadas
                    int rowsAffected = command.ExecuteNonQuery();

                    // Verificar si la modificación fue exitosa (si se afectó al menos una fila)
                    modificacionExitosa = rowsAffected > 0;
                }
            }

            // Devolver si la modificación fue exitosa
            return modificacionExitosa;
        }

        // Metodo que devulve una lista con los profesores registrados
        public LinkedList<Profesor> ver_profesores_registrados()
        {
            LinkedList<Profesor> profesores = new LinkedList<Profesor>();

            // Consulta SQL para seleccionar los operadores con revisado = 0
            string query = "SELECT cedula, nombre, apellido1, apellido2, fecha_de_nacimiento, email_prof FROM Profesor";

            // Crear la conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abrir la conexión
                connection.Open();

                // Crear el comando SQL
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Ejecutar la consulta y obtener los resultados
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Iterar sobre los resultados
                        while (reader.Read())
                        {
                            // Crear una instancia de Operador con los datos recuperados
                            Profesor profesor = new Profesor(reader.GetString(0), reader.GetString(1), reader.GetString(2) + " " + reader.GetString(3), new DateOnly(reader.GetDateTime(4).Year, reader.GetDateTime(4).Month, reader.GetDateTime(4).Day), reader.GetString(5));


                            // Agregar el operador a la lista
                            profesores.AddLast(profesor);
                        }
                    }
                }
            }

            return profesores;
        }

        //Metodo que permite modificar datos de un profesor
        public bool modificar_profesor(Profesor profesor)
        {
            // Consulta SQL para actualizar los datos del profesor
            string query = @"UPDATE Profesor 
                             SET nombre = @Nombre, 
                                 apellido1 = @Apellido1, 
                                 apellido2 = @Apellido2, 
                                 cedula = @Cedula,
                                 fecha_de_nacimiento = @FechaNacimiento
                             WHERE email_prof = @EmailProf";

            // Crear la conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abrir la conexión
                connection.Open();

                // Crear el comando SQL
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Establecer los parámetros
                    command.Parameters.AddWithValue("@Nombre", profesor.nombre);
                    command.Parameters.AddWithValue("@Apellido1", profesor.apellidos.Split(' ')[0]); // Primer apellido
                    command.Parameters.AddWithValue("@Apellido2", profesor.apellidos.Split(' ')[1]); // Segundo apellido
                    command.Parameters.AddWithValue("@Cedula", profesor.cedula);
                    command.Parameters.AddWithValue("@EmailProf", profesor.email);
                    command.Parameters.AddWithValue("@FechaNacimiento", profesor.fecha_de_nacimiento);

                    // Ejecutar la consulta
                    int rowsAffected = command.ExecuteNonQuery();

                    // Verificar si se actualizó correctamente el profesor
                    if (rowsAffected > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }


        //Metodo que elimina el usuario de un profesor de la base de datos y de la aplicacion
        public bool eliminar_profesor(string email)
        {
            // Consulta SQL para eliminar todas las filas relacionadas con el profesor
            string deleteQuery = @"
                DELETE Averia
                FROM Averia
                INNER JOIN Prestamo ON Averia.ID_prestamo = Prestamo.ID_prestamo
                WHERE Prestamo.email_prof = @Email;

                DELETE FROM Prestamo WHERE email_prof = @Email;

                DELETE FROM Reserva WHERE email_prof = @Email;

                DELETE FROM Profesor WHERE email_prof = @Email;
            ";

            // Variable para almacenar si se realizó la eliminación
            bool eliminado = false;

            // Crear la conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abrir la conexión
                connection.Open();

                // Iniciar una transacción
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // Crear el comando SQL para eliminar todas las filas relacionadas con el profesor
                    using (SqlCommand command = new SqlCommand(deleteQuery, connection, transaction))
                    {
                        // Establecer el parámetro para el correo electrónico
                        command.Parameters.AddWithValue("@Email", email);

                        // Ejecutar la consulta de eliminación
                        int rowsAffected = command.ExecuteNonQuery();

                        // Verificar si se eliminó correctamente al profesor
                        if (rowsAffected > 0)
                        {
                            // Confirmar la transacción
                            transaction.Commit();
                            eliminado = true;
                        }
                        else
                        {
                            // Revertir la transacción si no se realizó la eliminación
                            transaction.Rollback();
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Revertir la transacción si ocurre algún error
                    transaction.Rollback();
                    Console.WriteLine("Error al eliminar al profesor: " + ex.Message);
                }
            }

            // Devolver si se realizó la eliminación correctamente
            return eliminado;
        }





        //Metodo que ve todos los operadores registrados en la aplicacion
        public LinkedList<Operador> ver_todos_los_operadores_registrados()
        {
            LinkedList<Operador> operadores = new LinkedList<Operador>();

            // Consulta SQL para seleccionar los operadores con revisado = 0
            string query = "SELECT cedula, carnet, nombre, apellido1, apellido2, fecha_nacimiento, email_op FROM Operador WHERE revisado = 0";

            // Crear la conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abrir la conexión
                connection.Open();

                // Crear el comando SQL
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Ejecutar la consulta y obtener los resultados
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Iterar sobre los resultados
                        while (reader.Read())
                        {
                            // Crear una instancia de Operador con los datos recuperados
                            Operador operador = new Operador(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3) + " " + reader.GetString(4), reader.GetDateTime(5), reader.GetString(6));
                               

                            // Agregar el operador a la lista
                            operadores.AddLast(operador);
                        }
                    }
                }
            }

            return operadores;
        }


        //Metodo que acepta un operador ingresado en la aplicacion
        public bool aceptar_operador(string email_op)
        {
            // Consulta SQL para actualizar el operador con el correo electrónico proporcionado
            string query = "UPDATE Operador SET aprovado = 1, revisado = 1 WHERE email_op = @EmailOp";

            // Crear la conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abrir la conexión
                connection.Open();

                // Crear el comando SQL
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Establecer el parámetro para el correo electrónico
                    command.Parameters.AddWithValue("@EmailOp", email_op);

                    // Ejecutar la consulta
                    int rowsAffected = command.ExecuteNonQuery();

                    // Verificar si se realizó la actualización correctamente
                    if (rowsAffected > 0)
                    {
                        GeneradorContraseña.mandar_correo_sistema(email_op, "Ya puedes utilizar el sistema de LabCE!!!");
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }


        //Metodo que rechaza un operador ingresado en la aplicacion
        public bool rechazar_operador(string email_op)
        {
            // Consulta SQL para actualizar el operador con el correo electrónico proporcionado
            string query = "UPDATE Operador SET aprovado = 0, revisado = 1 WHERE email_op = @EmailOp";

            // Crear la conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abrir la conexión
                connection.Open();

                // Crear el comando SQL
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Establecer el parámetro para el correo electrónico
                    command.Parameters.AddWithValue("@EmailOp", email_op);

                    // Ejecutar la consulta
                    int rowsAffected = command.ExecuteNonQuery();

                    // Verificar si se realizó la actualización correctamente
                    if (rowsAffected > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        // Metodo que devuelve una lista con todos los usuarios registrados
        public LinkedList<Usuario> ver_todos_usuario()
        {
            LinkedList<Usuario> usuarios = new LinkedList<Usuario>();

            string query = @"
        SELECT 
            O.cedula,
            CONCAT(O.nombre, ' ', O.apellido1, ' ', O.apellido2) AS nombre_apellidos,
            O.fecha_nacimiento,
            O.email_op AS email
        FROM 
            Operador O
        UNION ALL
        SELECT 
            P.cedula,
            CONCAT(P.nombre, ' ', P.apellido1, ' ', P.apellido2) AS nombre_apellidos,
            P.fecha_de_nacimiento,
            P.email_prof AS email
        FROM 
            Profesor P";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string cedula = reader["cedula"].ToString();
                            string nombreApellidos = reader["nombre_apellidos"].ToString();
                            DateTime fechaDeNacimiento = (DateTime)reader["fecha_nacimiento"];
                            string email = reader["email"].ToString();

                            // Separar el nombre y los apellidos
                            string[] partesNombre = nombreApellidos.Split(' ');
                            string nombre = partesNombre[0];
                            string apellidos = partesNombre.Length > 1 ? nombreApellidos.Substring(partesNombre[0].Length + 1) : "";

                            // Crear un objeto Usuario y agregarlo a la lista
                            Usuario usuario = new Usuario(cedula, nombre, apellidos, new DateOnly(fechaDeNacimiento.Year, fechaDeNacimiento.Month, fechaDeNacimiento.Day), email);
                            usuarios.AddLast(usuario);
                        }
                    }
                }
            }

            return usuarios;
        }


        //Metodo que genera una nueva contraseña aleatoria para el administrador
        public bool generar_nueva_contraseña(string email)
        {
            this.contraseña = GeneradorContraseña.NuevaContraseña();

            GeneradorContraseña.mandar_correo(email, this.contraseña);

            // Consulta SQL para actualizar la contraseña del profesor
            string queryProfesor = "UPDATE Profesor SET password_prof = @NuevaContraseña WHERE email_prof = @EmailProfesor";

            // Consulta SQL para actualizar la contraseña del operador
            string queryOperador = "UPDATE Operador SET contrasena_op = @NuevaContraseña WHERE email_op = @EmailOperador";

            // Crear la conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abrir la conexión
                connection.Open();

                // Crear el comando SQL para actualizar la contraseña del profesor
                using (SqlCommand commandProfesor = new SqlCommand(queryProfesor, connection))
                {
                    // Establecer los parámetros para el profesor
                    this.contraseña = EncriptacionMD5.encriptar(this.contraseña);
                    commandProfesor.Parameters.AddWithValue("@NuevaContraseña", this.contraseña);
                    commandProfesor.Parameters.AddWithValue("@EmailProfesor", email);

                    // Ejecutar la consulta para el profesor
                    int rowsAffectedProfesor = commandProfesor.ExecuteNonQuery();

                    // Crear el comando SQL para actualizar la contraseña del operador
                    using (SqlCommand commandOperador = new SqlCommand(queryOperador, connection))
                    {
                        // Establecer los parámetros para el operador
                        commandOperador.Parameters.AddWithValue("@NuevaContraseña", this.contraseña);
                        commandOperador.Parameters.AddWithValue("@EmailOperador", email);

                        // Ejecutar la consulta para el operador
                        int rowsAffectedOperador = commandOperador.ExecuteNonQuery();

                        // Verificar si se realizaron las actualizaciones correctamente en ambas tablas
                        if (rowsAffectedProfesor > 0 || rowsAffectedOperador > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
        }


        //Metodo que genera el reporte de horas laboradas por los operadores
        public LinkedList<ReporteOperador> generar_reporte()
        {
            LinkedList<ReporteOperador> reporte = new LinkedList<ReporteOperador>();

            // Consulta SQL para seleccionar los turnos de los operadores, incluyendo nombre, apellido1 y apellido2
            string query = "SELECT o.nombre, o.apellido1, o.apellido2, t.fecha_hora_inicio, t.fecha_hora_fin " +
                           "FROM Turno t " +
                           "INNER JOIN Operador o ON t.email_op = o.email_op " +
                           "ORDER BY o.nombre, o.apellido1, o.apellido2";

            // Crear la conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abrir la conexión
                connection.Open();

                // Crear el comando SQL
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Ejecutar la consulta y obtener los resultados
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Variables para mantener el estado del operador anterior
                        string operadorAnterior = ""; // Inicializamos con una cadena vacía
                        ReporteOperador reporteOperadorActual = null;

                        // Iterar sobre los resultados
                        while (reader.Read())
                        {
                            // Obtener los datos del turno y del operador
                            string nombre = reader.GetString(0);
                            string apellido1 = reader.GetString(1);
                            string apellido2 = reader.GetString(2);
                            DateTime horaInicio = reader.GetDateTime(3);
                            DateTime horaFin = reader.GetDateTime(4);

                            // Construir una cadena única que represente al operador actual
                            string operadorActual = $"{nombre} {apellido1} {apellido2}";

                            // Verificar si es un nuevo operador
                            if (!operadorAnterior.Equals(operadorActual))
                            {
                                // Si no es el primer operador, agregar el reporte anterior a la lista
                                if (reporteOperadorActual != null)
                                {
                                    reporte.AddLast(reporteOperadorActual);
                                }

                                // Crear una nueva instancia de ReporteOperador para el operador actual
                                reporteOperadorActual = new ReporteOperador(DateTime.Now, new LinkedList<HorasLaboradas>(), nombre, apellido1, apellido2);

                                // Actualizar el operador actual
                                operadorAnterior = operadorActual;
                            }

                            // Agregar las horas laboradas del turno actual a la lista del operador actual
                            reporteOperadorActual.HorasLaboradas.AddLast(new HorasLaboradas(horaInicio, horaFin));
                        }

                        // Agregar el último reporte a la lista
                        if (reporteOperadorActual != null)
                        {
                            reporte.AddLast(reporteOperadorActual);
                        }
                    }
                }
            }

            return reporte;
        }




    }
}
