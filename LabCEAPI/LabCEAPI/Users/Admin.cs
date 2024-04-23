using LabCEAPI.Laboratorios;

namespace LabCEAPI.Users
{
    public class Admin
    {
        private string email { get; set; }
        private string contraseña { get; set; }

        //Metodo que permite ingresar como administrador a la aplicacion
        public void ingresar_admin(string email, string contraseña)
        {

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

        }

        //Metodo que permite modificar datos de un profesor
        public void modificar_profesor(string email, string new_email, string contraseña, string new_contraseña)
        {

        }

        //Metodo que elimina el usuario de un profesor de la base de datos y de la aplicacion
        public void eliminar_profesor(Profesor profesor)
        {
            
        }

        //Metodo que ve todos los operadores registrados en la aplicacion
        public LinkedList<Operador> ver_todos_los_operadores_registrados()
        {
            return Operador.operadores;
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

        }

    }
}
