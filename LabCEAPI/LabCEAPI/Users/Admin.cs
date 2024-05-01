using LabCEAPI.Laboratorios;
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
            string query = "SELECT password_prof, email_prof FROM Profesor WHERE email_prof = @Email and es_admin = 1";

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
           // contraseñaBaseDeDatos = EncriptacionMD5.desencriptar(contraseñaBaseDeDatos);
            return contraseña == contraseñaBaseDeDatos;
        }

        //Metodo que registra a un profesor por parte del administrador y lo guarda en la base de datos
        public void registrar_profesor(
            int cedula,
            string nombre,
            string apellidos,
            DateOnly fecha_de_nacimiento,
            int edad,
            string email
            )
        {
            //Registra los datos en la base

            // Consulta SQL para insertar un nuevo profesor
            string query = "INSERT INTO Profesor (email_prof, nombre, apellido1, apellido2, cedula) " +
                           "VALUES (@Email, @Nombre, @Apellido1, @Apellido2, @Cedula)";

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

        //Metodo que permite modificar datos de un profesor
        public void modificar_profesor(Profesor profesor)
        {
            // Consulta SQL para actualizar los datos del profesor
            string query = @"UPDATE Profesor 
                             SET nombre = @Nombre, 
                                 apellido1 = @Apellido1, 
                                 apellido2 = @Apellido2, 
                                 cedula = @Cedula
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

                    // Ejecutar la consulta
                    int rowsAffected = command.ExecuteNonQuery();

                    // Verificar si se actualizó correctamente el profesor
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Datos del profesor actualizados correctamente.");
                    }
                    else
                    {
                        Console.WriteLine("No se pudo actualizar los datos del profesor.");
                    }
                }
            }
        }


        //Metodo que elimina el usuario de un profesor de la base de datos y de la aplicacion
        public void eliminar_profesor(string email)
        {
            // Consulta SQL para eliminar al profesor por su correo electrónico
            string query = "DELETE FROM Profesor WHERE email_prof = @Email";

            // Crear la conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abrir la conexión
                connection.Open();

                // Crear el comando SQL
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Establecer el parámetro para el correo electrónico
                    command.Parameters.AddWithValue("@Email", email);

                    // Ejecutar la consulta de eliminación
                    int rowsAffected = command.ExecuteNonQuery();

                    // Verificar si se eliminó correctamente el profesor
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("El profesor con el correo electrónico " + email + " ha sido eliminado.");
                    }
                    else
                    {
                        Console.WriteLine("No se encontró ningún profesor con el correo electrónico " + email + ".");
                    }
                }
            }
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
        public void aceptar_operador(Operador operador)
        {
            
        }

        //Metodo que rechaza un operador ingresado en la aplicacion
        public void rechazar_operador(Operador operador)
        {

        }

        //Metodo que genera una nueva contraseña aleatoria para el administrador
        public void generar_nueva_contraseña()
        {
            this.contraseña =  GeneradorContraseña.NuevaContraseña();

            GeneradorContraseña.mandar_correo(this.email, this.contraseña);
        }

        //Metodo que genera el reporte de horas laboradas por los operadores
        public void generar_reporte()
        {
            ReporteGeneral reporteGeneral = new ReporteGeneral();   

        }

    }
}
