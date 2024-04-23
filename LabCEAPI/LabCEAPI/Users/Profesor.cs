using LabCEAPI.NewFolder;
using LabCEAPI.Prestamos;
using LabCEAPI.Reservaciones;

namespace LabCEAPI.Users
{
    public class Profesor
    {
        private int cedula {  get; set; }

        private string nombre { get; set; }

        private string apellidos { get; set; }

        private DateOnly fecha_de_nacimiento { get; set; }

        private int edad {  get; set; }

        private string email {  get; set; }

        private string contraseña { get; set; }
        
        //Metodo que registra a un profesor en la base de datos
        public void registrar_profesor(
            int cedula, 
            string nombre, 
            string apellidos, 
            DateOnly fecha_de_nacimiento, 
            int edad, 
            string email, 
            string contraseña)
        {
            //Registra los datos en la base
        }

        //Metodo que ingresa a un profesor en la aplicacion
        public void ingresar_profesor(string email, string contraseña)
        {

        }

        //Metodo que genera una contraseña nueva aleatoriamente y la almacena en la base de datos
        public void generar_nueva_contraseña()
        {
            this.contraseña = GeneradorContraseña.NuevaContraseña();
            
            GeneradorContraseña.mandar_correo(this.email, this.contraseña);

            string contraseña_encriptada = EncriptacionMD5.encriptar(this.contraseña);
        }

        //Metodo que muestra todos los activos que actualemente estan prestados
        public void ver_activos_prestados()
        {

        }

        //Metodo que solicita por parte del profesor un prestamo de un activo
        public void solicitar_prestamo_activo()
        {

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
        public ReservarLab reservar_laboratorio(Laboratorio lab, DateOnly dia, DateTime hora)
        {
            ReservarLab reservarLab = new ReservarLab(lab, this, dia, hora);
            return reservarLab;
        }
    }
}
