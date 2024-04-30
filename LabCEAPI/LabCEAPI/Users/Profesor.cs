using LabCEAPI.NewFolder;
using LabCEAPI.Prestamos;
using LabCEAPI.Reservaciones;
using Microsoft.Data.SqlClient;

namespace LabCEAPI.Users
{
    
    public class Profesor
    {
        private int cedula {  get; set; }

        private string nombre { get; set; }

        private string apellidos { get; set; }

        private DateOnly fecha_de_nacimiento { get; set; }

        private int edad {  get; set; }

        public string email {  get; set; }

        public string contraseña { get; set; }

        public const string connectionString = "Data Source=LAPTOP-GB4ACP2F;Initial Catalog=LabCE;Integrated Security=True;Encrypt=False";

        //Metodo que registra a un profesor en la base de datos
        public void registrar_profesor(
            int cedula, 
            string nombre, 
            string apellidos,  
            int edad, 
            string email, 
            string contraseña)
        {
            //Registra los datos en la base
            // Cadena de conexión a tu base de datos SQL Server
            string connectionString = "Data Source=LAPTOP-GB4ACP2F;Initial Catalog=LabCE;Integrated Security=True;Encrypt=False";

            // Consulta SQL para insertar un nuevo profesor
            string query = "INSERT INTO Profesor (email_prof, nombre, apellido1, apellido2, cedula, password_prof) " +
                           "VALUES (@Email, @Nombre, @Apellido1, @Apellido2, @Cedula, @Contraseña)";

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

        //Metodo que ingresa a un profesor en la aplicacion
        public bool ingresar_profesor(string email, string contraseña)
        {

            // Consulta SQL para buscar al profesor por email
            string query = "SELECT password_prof FROM Profesor WHERE email_prof = @Email";

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
        public void generar_nueva_contraseña(string email)
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
                        Console.WriteLine("Contraseña actualizada correctamente.");
                    }
                    else
                    {
                        Console.WriteLine("No se pudo actualizar la contraseña.");
                    }
                }
            }
        }

        //Metodo que muestra todos los activos que actualemente estan prestados
        public LinkedList<Activo> ver_activos_prestados()
        {
            return Activo.activos_prestados;
        }

        //Metodo que solicita por parte del profesor un prestamo de un activo
        public void solicitar_prestamo_activo(Activo activo)
        {
            PrestamoActivo prestamo = new PrestamoActivo(activo, this, DateOnly.FromDateTime(DateTime.Now), DateTime.Now);
            Activo.activos_disponibles.Remove(activo);
            Activo.activos_prestados.AddFirst(activo);
        }

        //Metodo que acepta la solicitud de un prestamos a un estudiante
        public void aceptar_solicitud_prestamo(PrestamoActivo prestamo)
        {
            prestamo.aprobado = true;
        }

        //Metodo que rechaza la solicitud de un prestamos a un estudiante
        public void rechazar_solicitud_prestamo(PrestamoActivo prestamo)
        {
            prestamo.aprobado = false;
        }

        //Metodo que devuelve los laboratorios que estan disponibles
        public LinkedList<Laboratorio> ver_labs_disponibles()
        {
            return Laboratorio.labs;
        }

        //Metodo que reserva un laboratorio por parte del profesor
        public ReservarLab reservar_laboratorio(Laboratorio lab, DateOnly dia, DateTime hora_inicio, DateTime hora_fin)
        {
            ReservarLab reservarLab = new ReservarLab(lab, this, dia, hora_inicio, hora_fin);
            return reservarLab;
        }

 
    }
}
