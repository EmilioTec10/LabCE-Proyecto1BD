using System.Runtime.CompilerServices;

namespace LabCEAPI.Users
{
    public class Operador
    {
        private int cedula { get; set; }

        private int carne { get; set; }

        private string nombre { get; set; }

        private string apellidos {  get; set; }

        private DateOnly fecha_de_nacimiento { get; set; }

        private int edad;

        private string email {  get; set; }

        private string contraseña {  get; set; }

        private DateTime hora_de_ingreso {  get; set; }

        private DateTime hora_de_salida { get; set; }

        public void ver_labs_disponibles()
        {

        }

        public void seleccionar_lab_disponible()
        {

        }

        public void ver_activos_disponibles()
        {

        }

        public void prestar_activo_estudiante()
        {

        }

        public void prestar_activo_profesor()
        {

        }

        public void ver_activos_prestados()
        {

        }

        public void registrar_devolucion_activo()
        {

        }

        public void reportar_averia_activo()
        {

        }

        public void ver_horas_laboradas()
        {

        }
    }
}
