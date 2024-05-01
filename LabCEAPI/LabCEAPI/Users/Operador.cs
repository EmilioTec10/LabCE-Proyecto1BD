using LabCEAPI.Laboratorios;
using LabCEAPI.NewFolder;
using LabCEAPI.Prestamos;
using LabCEAPI.Reservaciones;
using Microsoft.Data.SqlClient;
using System;
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
        public void registrar_operador(
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
                        Console.WriteLine("Profesor registrado correctamente.");
                    }
                    else
                    {
                        Console.WriteLine("No se pudo registrar al profesor.");
                    }
                }
            }
        }

        //Metodo para ingresar como operador en la aplicacion
        public bool ingresar_operador(string email, string contraseña)
        {
           // actual_hora = new HorasLaboradas(DateTime.Now, DateOnly.FromDateTime(DateTime.Now));

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
        public void marcar_hora_entrada(string email_op)
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
                        Console.WriteLine("Turno iniciado correctamente.");
                    }
                    else
                    {
                        Console.WriteLine("No se pudo iniciar el turno.");
                    }
                }
            }
        }

        public void marcar_hora_salida(string email_op)
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
                        Console.WriteLine("Hora de salida marcada correctamente.");
                    }
                    else
                    {
                        Console.WriteLine("No se pudo marcar la hora de salida.");
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
        public PrestamoActivo solicitar_activo_estudiante(Activo activo, Profesor profesor)
        {
            DateOnly fechaActual = DateOnly.FromDateTime(DateTime.Now);
            DateTime ahora = DateTime.Now;
            PrestamoActivo prestamoActivo = new PrestamoActivo(activo, profesor, fechaActual, ahora);
            return prestamoActivo;
        }

        //Metodo que presta el activo al estudiante
        public void prestar_activo_estudiante(Activo activo, string contraseña_operador)
        {
            Activo.activos_disponibles.Remove(activo);
            
        }

        //Metodo que presta un activo a un profesor
        public void prestar_activo_profesor(Activo activo, string contraseña_profesor)
        {
            Activo.activos_disponibles.Remove(activo);
            Activo.activos_prestados.AddFirst(activo);
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
        public RetornoActivo devolucion_activo_profesor(Activo activo, string contraseña_profesor)
        {
            DateOnly fechaActual = DateOnly.FromDateTime(DateTime.Now);
            DateTime ahora = DateTime.Now;
            RetornoActivo retornoActivo = new RetornoActivo(activo, fechaActual, ahora);
            return retornoActivo;
        }

        //Metodo para reportar una averia de un activo
        public Activo reportar_averia_activo(Activo activo, string detalle)
        {
            activo.estado = "Dañado";
            activo.dellate_dañado = detalle;
            return activo;
        }

        //Metodo para ver las horas laboradas del operador
        public ReporteOperador ver_horas_laboradas(DateTime dia, Operador operador, string detalles, LinkedList<HorasLaboradas> horasLaboradas)
        {
            ReporteOperador reporteOperador = new ReporteOperador(dia, operador, detalles, horasLaboradas);
            return reporteOperador;
        }
    }
}
