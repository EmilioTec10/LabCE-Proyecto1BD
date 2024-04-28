using LabCEAPI.Prestamos;
using LabCEAPI.Users;

namespace LabCEAPI.NewFolder
{
    public class PrestamoActivo
    {
        private Activo activo {  get; set; }

        private Profesor aprobador { get; set; }

        public bool aprobado { get; set; }

        private DateOnly dia { get; set; }

        private DateTime hora { get; set; }

        public PrestamoActivo(Activo activo, Profesor aprobador, DateOnly dia, DateTime hora)
        {
            this.activo = activo;
            this.aprobador = aprobador;
            this.dia = dia;
            this.hora = hora;
        }
    }
}
