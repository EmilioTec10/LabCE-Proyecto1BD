using LabCEAPI.NewFolder;
using LabCEAPI.Prestamos;
using LabCEAPI.Reservaciones;
using Microsoft.Data.SqlClient;

namespace LabCEAPI.Users
{
    
    public class Profesor
    {
        public int cedula {  get; set; }

        public string nombre { get; set; }

        public string apellidos { get; set; }

        public DateOnly fecha_de_nacimiento { get; set; }

        public int edad {  get; set; }

        public string email {  get; set; }

        public string contraseña { get; set; }

        public const string connectionString = "Data Source=LAPTOP-GB4ACP2F;Initial Catalog=LabCE;Integrated Security=True;Encrypt=False";

        //Metodo que registra a un profesor en la base de datos
        public bool registrar_profesor(
            int cedula, 
            string nombre, 
            string apellidos,  
            int edad, 
            string email, 
            string contraseña,
            DateTime fecha_de_nacimiento)
        {
            //Registra los datos en la base

            // Consulta SQL para insertar un nuevo profesor
            string query = "INSERT INTO Profesor (email_prof, nombre, apellido1, apellido2, cedula, password_prof, fecha_de_nacimiento) " +
                           "VALUES (@Email, @Nombre, @Apellido1, @Apellido2, @Cedula, @Contraseña, @FechaNacimiento)";

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

        //Metodo que ingresa a un profesor en la aplicacion
        public bool ingresar_profesor(string email, string contraseña)
        {

            // Consulta SQL para buscar al profesor por email
            string query = "SELECT password_prof, email_prof FROM Profesor WHERE email_prof = @Email";

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

        //Metodo que genera una contraseña nueva aleatoriamente y la almacena en la base de datos
        public bool generar_nueva_contraseña(string email)
        {
            this.contraseña = GeneradorContraseña.NuevaContraseña();
            
            GeneradorContraseña.mandar_correo(email, this.contraseña);

            string contraseña_encriptada = EncriptacionMD5.encriptar(this.contraseña);

            // Consulta SQL para actualizar la contraseña
            string query = "UPDATE Profesor SET password_prof = @NuevaContraseña WHERE email_prof = @Email";

            // Cadena de conexión a la base de datos

            // Utilizamos using para garantizar que los recursos se liberen correctamente
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abrimos la conexión
                connection.Open();

                // Creamos un comando SQL
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Establecemos los parámetros
                    command.Parameters.AddWithValue("@NuevaContraseña", contraseña_encriptada);
                    command.Parameters.AddWithValue("@Email", email);

                    // Ejecutamos la consulta
                    int rowsAffected = command.ExecuteNonQuery();

                    // Verificamos si se actualizó correctamente la contraseña
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

        //Metodo que muestra todos los activos que actualemente estan prestados
        public LinkedList<Activo> ver_activos_prestados()
        {
            LinkedList<Activo> activosPrestados = new LinkedList<Activo>();

            // Consulta SQL para seleccionar los activos prestados
            string query = "SELECT tipo, marca, estado, ID_activo, ID_lab FROM Activos WHERE estado = 'Prestado'";

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
                            Activo activo = new Activo(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4));
         

                            // Agregamos el activo a la lista de activos prestados
                            activosPrestados.AddLast(activo);
                        }
                    }
                }
            }

            // Devolvemos la lista de activos prestados
            return activosPrestados;
        }

        //Metodo que solicita por parte del profesor un prestamo de un activo
        public void solicitar_prestamo_activo(string placa, string email_prof)
        {
            // Obtener la placa del activo
            string placaActivo = placa;

            // Verificar si el activo existe
            if (!ExisteActivo(placaActivo))
            {
                Console.WriteLine("El activo no existe.");
                return; // Salir del método si el activo no existe
            }
            ModificarEstadoActivo(placaActivo, "Prestado");

            // Obtener la fecha y hora actual
            DateTime fechaHoraSolicitud = DateTime.Now;

            // Estado del préstamo (por ejemplo, "En espera")
            string estadoPrestamo = "Aprobado";

            // Consulta SQL para insertar el préstamo
            string query = "INSERT INTO Prestamo (ID_activo, Fecha_Hora_Solicitud, estado, activo, email_prof) " +
                           "VALUES (@IDActivo, @FechaHoraSolicitud, @EstadoPrestamo, 1, @EmailProfesor)";

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

                    // Ejecutamos la consulta
                    int rowsAffected = command.ExecuteNonQuery();

                    // Verificamos si se insertó correctamente el préstamo
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Préstamo solicitado correctamente.");
                    }
                    else
                    {
                        Console.WriteLine("No se pudo solicitar el préstamo.");
                    }
                }
            }
        }

        // Método para verificar si un activo existe en la base de datos
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

        private void ModificarEstadoActivo(string placaActivo, string nuevoEstado)
        {

            // Consulta SQL para actualizar el estado del activo
            string query = "UPDATE Activos SET estado = @NuevoEstado WHERE ID_activo = @IDActivo";

            // Utilizamos using para garantizar que los recursos se liberen correctamente
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abrimos la conexión
                connection.Open();

                // Creamos un comando SQL
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Establecemos los parámetros
                    command.Parameters.AddWithValue("@NuevoEstado", nuevoEstado);
                    command.Parameters.AddWithValue("@IDActivo", placaActivo);

                    // Ejecutamos la consulta
                    int rowsAffected = command.ExecuteNonQuery();

                    // Verificamos si se actualizó correctamente el estado del activo
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Estado del activo actualizado correctamente.");
                    }
                    else
                    {
                        Console.WriteLine("No se pudo actualizar el estado del activo.");
                    }
                }
            }
        }

        // Metodo para devolver los prestamos pendientes de un profesor específico
        public LinkedList<PrestamoActivo> ver_prestamos_pendientes(string email_prof)
        {
            LinkedList<PrestamoActivo> prestamosPendientes = new LinkedList<PrestamoActivo>();

            string query = "SELECT p.email_est, p.email_prof, p.fecha_hora_solicitud, p.estado, p.ID_activo, " +
                           "e.nombre, e.apellido1, e.apellido2 " +
                           "FROM Prestamo p " +
                           "INNER JOIN Estudiante e ON p.email_est = e.email_est " +
                           "WHERE p.estado = 'Pendiente' AND p.email_prof = @EmailProf";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EmailProf", email_prof); // Parámetro para el email del profesor
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string emailEst = reader["email_est"].ToString();
                    string emailProf = reader["email_prof"].ToString();
                    DateTime fechaHoraSolicitud = (DateTime)reader["fecha_hora_solicitud"];
                    string estado = reader["estado"].ToString();
                    string placa = reader["ID_activo"].ToString();
                    string nombreEstudiante = reader["nombre"].ToString();
                    string apellido1Estudiante = reader["apellido1"].ToString();
                    string apellido2Estudiante = reader["apellido2"].ToString();

                    // Concatenar los apellidos si es necesario
                    string apellidosEstudiante = apellido1Estudiante + " " + apellido2Estudiante;

                    PrestamoActivo prestamo = new PrestamoActivo(emailEst, emailProf, fechaHoraSolicitud, estado, placa, nombreEstudiante, apellidosEstudiante);
                    prestamosPendientes.AddLast(prestamo);
                }
                reader.Close();
            }

            return prestamosPendientes;
        }



        //Metodo que acepta la solicitud de un prestamos a un estudiante
        public void aceptar_solicitud_prestamo(string ID_activo, string email_estudiante, string email_prof)
        {
            // Estado a asignar al préstamo
            string nuevoEstado = "Aprobado";

            // Consulta SQL para actualizar el estado del préstamo
            string query = "UPDATE Prestamo SET estado = @NuevoEstado " +
                           "WHERE ID_activo = @IDActivo AND email_est = @EmailEstudiante AND email_prof = @EmailProfe";

            // Utilizamos using para garantizar que los recursos se liberen correctamente
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abrimos la conexión
                connection.Open();

                // Creamos un comando SQL
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Establecemos los parámetros
                    command.Parameters.AddWithValue("@NuevoEstado", nuevoEstado);
                    command.Parameters.AddWithValue("@IDActivo", ID_activo);
                    command.Parameters.AddWithValue("@EmailEstudiante", email_estudiante);
                    command.Parameters.AddWithValue("@EmailProfe", email_prof);

                    // Ejecutamos la consulta
                    int rowsAffected = command.ExecuteNonQuery();

                    // Verificamos si se actualizó correctamente el estado del préstamo
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Solicitud de préstamo aprobada correctamente.");
                    }
                    else
                    {
                        Console.WriteLine("No se pudo aprobar la solicitud de préstamo.");
                    }
                }
            }
        }

        //Metodo que rechaza la solicitud de un prestamos a un estudiante
        public void rechazar_solicitud_prestamo(string ID_activo, string email_estudiante, string email_prof)
        {
            // Estado a asignar al préstamo
            string nuevoEstado = "Rechazado";

            // Consulta SQL para actualizar el estado del préstamo
            string query = "UPDATE Prestamo SET estado = @NuevoEstado " +
                           "WHERE ID_activo = @IDActivo AND email_est = @EmailEstudiante AND email_prof = @EmailProfe";

            // Utilizamos using para garantizar que los recursos se liberen correctamente
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abrimos la conexión
                connection.Open();

                // Creamos un comando SQL
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Establecemos los parámetros
                    command.Parameters.AddWithValue("@NuevoEstado", nuevoEstado);
                    command.Parameters.AddWithValue("@IDActivo", ID_activo);
                    command.Parameters.AddWithValue("@EmailEstudiante", email_estudiante);
                    command.Parameters.AddWithValue("@EmailProfe", email_prof);

                    // Ejecutamos la consulta
                    int rowsAffected = command.ExecuteNonQuery();

                    // Verificamos si se actualizó correctamente el estado del préstamo
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Solicitud de préstamo aprobada correctamente.");
                    }
                    else
                    {
                        Console.WriteLine("No se pudo aprobar la solicitud de préstamo.");
                    }
                }
            }
        }


        //Metodo que devuelve los laboratorios que estan disponibles
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

        //Metodo que reserva un laboratorio por parte del profesor
        public bool reservar_laboratorio(Laboratorio lab, DateTime dia, DateTime hora_inicio, DateTime hora_fin, string descripcion, bool palmada, string email_prof)
        {
            // Definir el estado inicial de la reserva
            string estadoReserva = "Reservado";

            // Crear la conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abrir la conexión
                connection.Open();

                // Consulta SQL para insertar la reserva
                string query = @"INSERT INTO Reserva (fecha, hora_inicio, hora_fin, ID_lab, email_prof, estado, descripcion, palmada) 
                        VALUES (@Fecha, @HoraInicio, @HoraFin, @IDLab, @EmailProf, @Estado, @Descripcion, @Palmada)";

                // Crear el comando SQL
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Establecer los parámetros
                    command.Parameters.AddWithValue("@Fecha", dia);
                    command.Parameters.AddWithValue("@HoraInicio", hora_inicio);
                    command.Parameters.AddWithValue("@HoraFin", hora_fin);
                    command.Parameters.AddWithValue("@IDLab", lab.nombre);
                    command.Parameters.AddWithValue("@EmailProf", email_prof);
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
    }
}
