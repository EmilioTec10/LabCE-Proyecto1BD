using LabCEAPI.Laboratorios;
using LabCEAPI.NewFolder;
using LabCEAPI.Prestamos;
using LabCEAPI.Reservaciones;
using Microsoft.Data.SqlClient;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace LabCEAPI.Users
{
    public class Operador
    {
        public static LinkedList<Operador> operadores = new LinkedList<Operador>();
        public string cedula { get; set; }

        public string carne { get; set; }

        public string nombre { get; set; }

        public string apellidos {  get; set; }

        public DateTime fecha_de_nacimiento { get; set; }

        private int edad;

        public string email {  get; set; }

        public string contraseña { get; set; }

        public const string connectionString = "Data Source=LAPTOP-GB4ACP2F;Initial Catalog=LabCE;Integrated Security=True;Encrypt=False";
        HorasLaboradas actual_hora { get; set; }

        LinkedList<HorasLaboradas> HorasLaboradas { get; set; }

        public Operador (string cedula, string carne, string nombre, string apellidos, DateTime fecha_de_nacimiento, string email)
        {
            this.cedula = cedula;
            this.carne = carne;
            this.nombre = nombre;
            this.apellidos = apellidos;
            this.fecha_de_nacimiento = fecha_de_nacimiento;
            this.email = email;
        }

        public Operador()
        {

        }


        //Metodo que registra un nuevo operador y lo guarda en la base de datos
        public bool registrar_operador(
            int cedula,
            int carne,
            string nombre,
            string apellidos,
            DateTime fecha_de_nacimiento,
            int edad,
            string email,
            string contraseña)
        {
            // Consulta SQL para insertar un nuevo profesor
            string query = "INSERT INTO Operador (email_op, nombre, apellido1, apellido2, cedula, contrasena_op, fecha_nacimiento, carnet, revisado, aprovado) " +
                           "VALUES (@Email, @Nombre, @Apellido1, @Apellido2, @Cedula, @Contraseña, @FechaNacimiento, @Carnet, 0, 0)";

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

                    contraseña = EncriptacionMD5.encriptar(contraseña);

                    command.Parameters.AddWithValue("@Contraseña", contraseña);
                    command.Parameters.AddWithValue("@FechaNacimiento", new DateOnly(fecha_de_nacimiento.Year, fecha_de_nacimiento.Month, fecha_de_nacimiento.Day));
                    command.Parameters.AddWithValue("@Carnet", carne);
                    // Ejecutamos la consulta
                    int rowsAffected = command.ExecuteNonQuery();

                    // Verificamos si se insertaron correctamente los datos
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

        //Metodo para ingresar como operador en la aplicacion
        public bool ingresar_operador(string email, string contraseña)
        {

            // Consulta SQL para buscar al operador por email
            string query = "SELECT contrasena_op, email_op FROM Operador WHERE email_op = @Email";

            // Variable para almacenar la contraseña recuperada de la base de datos
            string contraseñaBaseDeDatos = null;

            // Utilizamos using para garantizar que los recursos se liberen correctamente
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abrimos la conexión
                connection.Open();

                // Creamos un comando SQL
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Establecemos el parámetro para el correo electrónico
                    command.Parameters.AddWithValue("@Email", email);

                    // Ejecutamos la consulta y obtenemos la contraseña almacenada en la base de datos
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            contraseñaBaseDeDatos = reader.GetString(0); // Suponiendo que la contraseña está en la primera columna
                            this.email = reader.GetString(1);
                        }
                    }
                }
            }

            // Si no se encontró ningún profesor con ese correo electrónico
            if (contraseñaBaseDeDatos == null)
            {
                return false;
            }

            // Comparamos la contraseña proporcionada con la contraseña almacenada en la base de datos
            contraseñaBaseDeDatos = EncriptacionMD5.desencriptar(contraseñaBaseDeDatos);
            return contraseña == contraseñaBaseDeDatos;
        }

        // Metodo que registra la hora de entrada del turno de un operador
        public bool marcar_hora_entrada(string email_op)
        {
            // Obtener la fecha y hora actual
            DateTime fechaHoraActual = DateTime.Now;

            // Consulta SQL para insertar el turno
            string query = @"INSERT INTO Turno (email_op, fecha_hora_inicio, fecha_hora_fin) 
                         VALUES (@EmailOp, @FechaHoraInicio, NULL)";

            // Crear la conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abrir la conexión
                connection.Open();

                // Crear el comando SQL
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Establecer los parámetros
                    command.Parameters.AddWithValue("@EmailOp", email_op);
                    command.Parameters.AddWithValue("@FechaHoraInicio", fechaHoraActual);

                    // Ejecutar la consulta
                    int rowsAffected = command.ExecuteNonQuery();

                    // Verificar si se realizó la inserción correctamente
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

        public bool marcar_hora_salida(string email_op)
        {
            // Obtener la fecha y hora actual
            DateTime fechaHoraActual = DateTime.Now;

            // Consulta SQL para actualizar la hora de salida
            string query = @"UPDATE Turno 
                         SET fecha_hora_fin = @FechaHoraFin
                         WHERE email_op = @EmailOp AND fecha_hora_fin IS NULL";

            // Crear la conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abrir la conexión
                connection.Open();

                // Crear el comando SQL
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Establecer los parámetros
                    command.Parameters.AddWithValue("@FechaHoraFin", fechaHoraActual);
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


            //Metodo para ver todos los laboratorios que estan disponibles en este momento
            public LinkedList<Laboratorio> ver_labs_disponibles()
        {
            LinkedList<Laboratorio> laboratorios = new LinkedList<Laboratorio>();

            // Consulta SQL con alias para que coincidan con los nombres de las propiedades en la clase Laboratorio
            string query = "SELECT ID_lab, capacidad, facilidades FROM Laboratorio";


            // Utilizamos using para garantizar que los recursos se liberen correctamente
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abrimos la conexión
                connection.Open();

                // Creamos un comando SQL
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Ejecutamos la consulta
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Iteramos sobre los resultados de la consulta
                        while (reader.Read())
                        {
                            // Creamos una instancia de Laboratorio con los datos de la fila actual
                            Laboratorio lab = new Laboratorio(reader.GetString(0), reader.GetInt32(1), reader.GetString(2));

                            // Agregamos el laboratorio a la lista de laboratorios disponibles
                            laboratorios.AddLast(lab);
                        }
                    }
                }
            }

            // Devolvemos la lista de laboratorios disponibles
            return laboratorios;
        }

        //Metodo para reservar un laboratorio en una fecha determinada
        public bool reservar_laboratorio(string nombre, DateTime dia, DateTime hora_inicio, DateTime hora_fin, string descripcion, bool palmada, string email_op)
        {
            // Definir el estado inicial de la reserva
            string estadoReserva = "Reservado";

            // Crear la conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abrir la conexión
                connection.Open();

                // Consulta SQL para insertar la reserva
                string query = @"INSERT INTO Reserva (fecha, hora_inicio, hora_fin, ID_lab, email_op, estado, descripcion, palmada) 
                        VALUES (@Fecha, @HoraInicio, @HoraFin, @IDLab, @EmailOp, @Estado, @Descripcion, @Palmada)";

                // Crear el comando SQL
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Establecer los parámetros
                    command.Parameters.AddWithValue("@Fecha", dia);
                    command.Parameters.AddWithValue("@HoraInicio", hora_inicio);
                    command.Parameters.AddWithValue("@HoraFin", hora_fin);
                    command.Parameters.AddWithValue("@IDLab", nombre);
                    command.Parameters.AddWithValue("@EmailOp", email_op);
                    command.Parameters.AddWithValue("@Estado", estadoReserva);
                    command.Parameters.AddWithValue("@Descripcion", descripcion);
                    command.Parameters.AddWithValue("@Palmada", palmada);

                    // Ejecutar la consulta
                    int rowsAffected = command.ExecuteNonQuery();

                    // Verificar si se realizó la inserción correctamente
                    if (rowsAffected > 0)
                    {
                        // La reserva se realizó con éxito
                        return true;
                    }
                    else
                    {
                        // La reserva no se pudo realizar
                        return false;
                    }
                }
            }
        }

        //Metodo para ver los activos que actualmente estan disponibles
        public LinkedList<Activo> ver_activos_disponibles()
        {
            LinkedList<Activo> activosPrestados = new LinkedList<Activo>();

            // Consulta SQL para seleccionar los activos prestados
            string query = "SELECT tipo, marca, estado, ID_activo FROM Activos WHERE estado = 'Disponible'";

            // Utilizamos using para garantizar que los recursos se liberen correctamente
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abrimos la conexión
                connection.Open();

                // Creamos un comando SQL
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Ejecutamos la consulta
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Iteramos sobre los resultados de la consulta
                        while (reader.Read())
                        {
                            // Creamos una instancia de Activo con los datos de la fila actual
                            Activo activo = new Activo(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3));


                            // Agregamos el activo a la lista de activos prestados
                            activosPrestados.AddLast(activo);
                        }
                    }
                }
            }

            // Devolvemos la lista de activos prestados
            return activosPrestados;
        }

        //Metodo que solicita a un profesor el prestamo de un activo a un estudiante
        public bool solicitar_activo_estudiante(string placa, string email_est, string email_prof)
        {
            // Obtener la placa del activo
            string placaActivo = placa;

            // Verificar si el activo existe
            if (!ExisteActivo(placaActivo))
            {
                Console.WriteLine("El activo no existe.");
                return false; // Salir del método si el activo no existe
            }

            // Obtener la fecha y hora actual
            DateTime fechaHoraSolicitud = DateTime.Now;

            // Estado del préstamo 
            string estadoPrestamo = "Pendiente";


            // Consulta SQL para insertar el préstamo
            string query = "INSERT INTO Prestamo (ID_activo, Fecha_Hora_Solicitud, estado, activo, email_prof, email_est) " +
                           "VALUES (@IDActivo, @FechaHoraSolicitud, @EstadoPrestamo, 1, @EmailProfesor, @EmailEst)";

            // Utilizamos using para garantizar que los recursos se liberen correctamente
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abrimos la conexión
                connection.Open();

                // Creamos un comando SQL
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Establecemos los parámetros
                    command.Parameters.AddWithValue("@IDActivo", placaActivo); // Suponiendo que la placa del activo es el ID_activo en la tabla
                    command.Parameters.AddWithValue("@FechaHoraSolicitud", fechaHoraSolicitud);
                    command.Parameters.AddWithValue("@EstadoPrestamo", estadoPrestamo);
                    command.Parameters.AddWithValue("@EmailProfesor", email_prof);
                    command.Parameters.AddWithValue("@EmailEst", email_est);

                    // Ejecutamos la consulta
                    int rowsAffected = command.ExecuteNonQuery();


                    // Verificamos si se insertó correctamente el préstamo
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

        private bool ExisteActivo(string placaActivo)
        {

            // Consulta SQL para verificar si el activo existe
            string query = "SELECT COUNT(*) FROM Activos WHERE ID_activo = @IDActivo";

            // Variable para almacenar el resultado de la consulta
            int count = 0;

            // Utilizamos using para garantizar que los recursos se liberen correctamente
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abrimos la conexión
                connection.Open();

                // Creamos un comando SQL
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Establecemos el parámetro
                    command.Parameters.AddWithValue("@IDActivo", placaActivo);

                    // Ejecutamos la consulta y obtenemos el resultado
                    count = (int)command.ExecuteScalar();
                }
            }

            // Si count es mayor que cero, el activo existe
            return count > 0;
        }

        // Metodo que devuelve una lista de prestamos de activos que hayan sido aprobados
        public LinkedList<PrestamoActivo> ver_prestamos_aprobados()
        {
            // Lista para almacenar los prestamos aprobados
            LinkedList<PrestamoActivo> prestamosAprobados = new LinkedList<PrestamoActivo>();

            // Query SQL para seleccionar los prestamos aprobados
            string query = @"SELECT p.email_est, p.email_prof, p.Fecha_Hora_Solicitud, p.estado, p.ID_activo
                     FROM Prestamo p
                     WHERE p.estado = 'Aprobado' AND p.activo = 1";

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
                        // Iterar sobre los resultados y mapearlos a objetos PrestamoActivo
                        while (reader.Read())
                        {
                            string email_est = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                            string email_prof = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                            DateTime fecha_hora_solicitud = reader.IsDBNull(2) ? DateTime.MinValue : reader.GetDateTime(2);
                            string estado = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                            string placa = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);

                            // Crear un objeto PrestamoActivo y agregarlo a la lista
                            PrestamoActivo prestamo = new PrestamoActivo(email_est, email_prof, fecha_hora_solicitud, estado, placa);
                            prestamosAprobados.AddLast(prestamo);
                        }

                    }
                }
            }

            // Devolver la lista de prestamos aprobados
            return prestamosAprobados;
        }

        //Metodo que presta el activo al estudiante
        public bool prestar_activo_estudiante(string email, string contraseña)
        {
            // Consulta SQL para buscar al operador por email
            string query = "SELECT contrasena_op, email_op FROM Operador WHERE email_op = @Email";

            // Variable para almacenar la contraseña recuperada de la base de datos
            string contraseñaBaseDeDatos = null;

            // Utilizamos using para garantizar que los recursos se liberen correctamente
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abrimos la conexión
                connection.Open();

                // Creamos un comando SQL
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Establecemos el parámetro para el correo electrónico
                    command.Parameters.AddWithValue("@Email", email);

                    // Ejecutamos la consulta y obtenemos la contraseña almacenada en la base de datos
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            contraseñaBaseDeDatos = reader.GetString(0); // Suponiendo que la contraseña está en la primera columna
                            this.email = reader.GetString(1);
                        }
                    }
                }
            }

            // Si no se encontró ningún profesor con ese correo electrónico
            if (contraseñaBaseDeDatos == null)
            {
                return false;
            }

            // Comparamos la contraseña proporcionada con la contraseña almacenada en la base de datos
            contraseñaBaseDeDatos = EncriptacionMD5.desencriptar(contraseñaBaseDeDatos);
            return contraseña == contraseñaBaseDeDatos;

        }

        //Metodo que presta un activo a un profesor
        public bool prestar_activo_profesor(string placa, string email_prof, string contraseña_profesor)
        {
            // Verificar si la contraseña del profesor es correcta
            if (!ValidarContraseña(email_prof, contraseña_profesor))
            {
                // La contraseña no es válida, retornar falso
                return false;
            }

            // Si la contraseña es válida, procedemos con la inserción en la tabla Prestamo
            string query = @"
                INSERT INTO Prestamo (ID_activo, Fecha_Hora_Solicitud, estado, activo, email_prof) 
                VALUES (@ID_activo, @Fecha_Hora_Solicitud, @estado, @activo, @email_prof)";

            // Crear la conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abrir la conexión
                connection.Open();

                // Crear el comando SQL
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Establecer los parámetros
                    command.Parameters.AddWithValue("@ID_activo", placa);
                    command.Parameters.AddWithValue("@Fecha_Hora_Solicitud", DateTime.Now);
                    command.Parameters.AddWithValue("@estado", "Aprobado");
                    command.Parameters.AddWithValue("@activo", true);
                    command.Parameters.AddWithValue("@email_prof", email_prof);

                    // Ejecutar la consulta
                    int rowsAffected = command.ExecuteNonQuery();

                    // Retornar true si se realizó el préstamo correctamente (al menos una fila afectada)
                    return rowsAffected > 0;
                }
            }
        }


        private bool ValidarContraseña(string email_prof, string contraseña_profesor)
        {
            // Consultar la contraseña almacenada en la base de datos para el profesor con el email dado
            string query = "SELECT password_prof FROM Profesor WHERE email_prof = @Email";

            // Variable para almacenar la contraseña recuperada de la base de datos
            string contraseñaBaseDeDatos = null;

            // Crear la conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abrir la conexión
                connection.Open();

                // Crear el comando SQL
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Establecer el parámetro para el correo electrónico
                    command.Parameters.AddWithValue("@Email", email_prof);

                    // Ejecutar la consulta y obtener la contraseña almacenada en la base de datos
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            contraseñaBaseDeDatos = reader.GetString(0); // Suponiendo que la contraseña está en la primera columna
                        }
                    }
                }
            }

            // Si no se encontró ningún profesor con ese correo electrónico, o la contraseña no coincide, retornar falso
            contraseñaBaseDeDatos = EncriptacionMD5.desencriptar(contraseñaBaseDeDatos);
            if (contraseñaBaseDeDatos == null || contraseña_profesor != contraseñaBaseDeDatos)
            {
                return false;
            }

            // La contraseña coincide
            return true;
        }

        //Metodo que deja ver los activos prestados
        public LinkedList<Activo> ver_activos_prestados()
        {
            LinkedList<Activo> activosPrestados = new LinkedList<Activo>();

            // Consulta SQL para seleccionar los activos prestados
            string query = "SELECT tipo, marca, estado, ID_activo FROM Activos WHERE estado = 'Prestado'";

            // Utilizamos using para garantizar que los recursos se liberen correctamente
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abrimos la conexión
                connection.Open();

                // Creamos un comando SQL
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Ejecutamos la consulta
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Iteramos sobre los resultados de la consulta
                        while (reader.Read())
                        {
                            // Creamos una instancia de Activo con los datos de la fila actual
                            Activo activo = new Activo(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3));


                            // Agregamos el activo a la lista de activos prestados
                            activosPrestados.AddLast(activo);
                        }
                    }
                }
            }

            // Devolvemos la lista de activos prestados
            return activosPrestados;
        }

        //Metodo para registrar la devolucion de un activo por parte de un estudiante
        public RetornoActivo devolucion_activo_estudiante(Activo activo, string contraseña_operador)
        {
            DateOnly fechaActual = DateOnly.FromDateTime(DateTime.Now);
            DateTime ahora = DateTime.Now;
            RetornoActivo retornoActivo = new RetornoActivo(activo, fechaActual, ahora);
            return retornoActivo;
        }

        //Metodo para registrar la devolucion de un activo por parte de un profesor
        public bool validar_contraseña_profesor(string email_prof, string contraseña_prof)
        {
            // Variable para almacenar el resultado de la validación
            bool contraseñaValida = false;

            // Consulta SQL para obtener la contraseña del profesor
            string query = "SELECT password_prof FROM Profesor WHERE email_prof = @EmailProf";

            // Crear la conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abrir la conexión
                connection.Open();

                // Crear el comando SQL
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Establecer el parámetro
                    command.Parameters.AddWithValue("@EmailProf", email_prof);

                    // Ejecutar la consulta y obtener el resultado
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Si se encontró el profesor
                        if (reader.Read())
                        {
                            // Obtener la contraseña del profesor
                            string contraseñaBaseDeDatos = EncriptacionMD5.desencriptar(reader.GetString(0));

                            // Validar la contraseña
                            if (contraseña_prof == contraseñaBaseDeDatos)
                            {
                                // La contraseña es válida
                                contraseñaValida = true;
                            }
                        }
                    }
                }
            }

            // Devolver el resultado de la validación de la contraseña
            return contraseñaValida;
        }

        public bool devolucion_activo_profesor(string placa, string email_prof, string contraseña_prof)
        {
            // Validar la contraseña del profesor
            bool contraseñaValida = validar_contraseña_profesor(email_prof, contraseña_prof);

            // Si la contraseña no es válida, retornar falso
            if (!contraseñaValida)
            {
                return false;
            }

            // Variable para almacenar el resultado de la devolución
            bool devolucionExitosa = false;

            // Actualizar la tabla Prestamo con la fecha y hora actual en Fecha_Hora_Devolucion y activo = 0
            string updateQuery = "UPDATE Prestamo SET Fecha_Hora_Devolucion = @FechaHoraDevolucion, activo = 0 WHERE ID_activo = @Placa AND email_prof = @EmailProf";

            // Crear la conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abrir la conexión
                connection.Open();

                // Crear el comando SQL
                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    // Establecer los parámetros
                    command.Parameters.AddWithValue("@FechaHoraDevolucion", DateTime.Now);
                    command.Parameters.AddWithValue("@Placa", placa);
                    command.Parameters.AddWithValue("@EmailProf", email_prof);

                    // Ejecutar la consulta de actualización
                    int rowsAffected = command.ExecuteNonQuery();

                    // Si al menos una fila fue afectada, la devolución fue exitosa
                    devolucionExitosa = rowsAffected > 0;
                }
            }

            return devolucionExitosa;
        }

        //Metodo para reportar una averia de un activo
        public bool reportar_averia_activo(string placa, string descripcion, string email_prof, string email_est)
        {
            // Variable para almacenar el ID del préstamo
            int idPrestamo = -1;

            // Consulta SQL para obtener el ID del préstamo
            string selectPrestamoQuery = "SELECT ID_prestamo FROM Prestamo " +
                                         "WHERE ID_activo = @Placa AND (email_prof = @EmailProf OR email_est = @EmailEst)";

            // Consulta SQL para actualizar el estado del activo
            string updateActivoQuery = "UPDATE Activos SET estado = 'Dañado' WHERE ID_activo = @Placa";

            // Consulta SQL para insertar el registro en la tabla Averia
            string insertAveriaQuery = "INSERT INTO Averia (ID_prestamo, descripcion) VALUES (@IdPrestamo, @Descripcion)";

            // Crear la conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abrir la conexión
                connection.Open();

                // Iniciar una transacción
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // Obtener el ID del préstamo
                    using (SqlCommand selectPrestamoCommand = new SqlCommand(selectPrestamoQuery, connection, transaction))
                    {
                        // Establecer los parámetros
                        selectPrestamoCommand.Parameters.AddWithValue("@Placa", placa);
                        selectPrestamoCommand.Parameters.AddWithValue("@EmailProf", email_prof);
                        selectPrestamoCommand.Parameters.AddWithValue("@EmailEst", email_est);

                        // Ejecutar la consulta y obtener el ID del préstamo
                        idPrestamo = (int)selectPrestamoCommand.ExecuteScalar();
                    }

                    // Si no se encontró el prést
                    if (idPrestamo == -1)
                    {
                        // No se encontró el préstamo, revertir la transacción y salir
                        transaction.Rollback();
                        return false;
                    }

                    // Actualizar el estado del activo a "Dañado"
                    using (SqlCommand updateActivoCommand = new SqlCommand(updateActivoQuery, connection, transaction))
                    {
                        // Establecer los parámetros
                        updateActivoCommand.Parameters.AddWithValue("@Placa", placa);

                        // Ejecutar la consulta
                        updateActivoCommand.ExecuteNonQuery();
                    }

                    // Insertar el registro en la tabla Averia
                    using (SqlCommand insertAveriaCommand = new SqlCommand(insertAveriaQuery, connection, transaction))
                    {
                        // Establecer los parámetros
                        insertAveriaCommand.Parameters.AddWithValue("@IdPrestamo", idPrestamo);
                        insertAveriaCommand.Parameters.AddWithValue("@Descripcion", descripcion);

                        // Ejecutar la consulta
                        insertAveriaCommand.ExecuteNonQuery();
                    }

                    // Confirmar la transacción
                    transaction.Commit();

                    return true;
                }
                catch (Exception)
                {
                    // Ocurrió un error, revertir la transacción y lanzar la excepción
                    transaction.Rollback();
                    throw;
                }
            }
        }



        //Metodo para ver las horas laboradas del operador
        public LinkedList<HorasLaboradas> ver_horas_laboradas(string email_op)
        {
            LinkedList<HorasLaboradas> horasLaboradas = new LinkedList<HorasLaboradas>();

            // Consulta SQL para seleccionar los turnos del operador
            string query = "SELECT fecha_hora_inicio, fecha_hora_fin FROM Turno WHERE email_op = @EmailOp";

            // Crear la conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abrir la conexión
                connection.Open();

                // Crear el comando SQL
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Establecer el parámetro
                    command.Parameters.AddWithValue("@EmailOp", email_op);

                    // Ejecutar la consulta y obtener los resultados
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Iterar sobre los resultados
                        while (reader.Read())
                        {
                            // Obtener las fechas de inicio y fin del turno
                            DateTime horaInicio = reader.GetDateTime(0);
                            DateTime horaFin = reader.GetDateTime(1);
                            // Crear una instancia de HorasLaboradas y agregarla a la lista
                            horasLaboradas.AddLast(new HorasLaboradas(horaInicio, horaFin));
                        }
                    }
                }
            }

            return horasLaboradas;
        }
    }
}
