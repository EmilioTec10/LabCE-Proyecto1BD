using LabCEAPI.Prestamos;
using LabCEAPI.Users;

namespace LabCEAPI.NewFolder
{
    public class RetornoActivo
    {
        private Activo activo_retornado { get; set; }

        private DateOnly dia { get; set; }

        private DateTime hora { get; set; }

        public RetornoActivo (Activo activo_retornado, DateOnly dia, DateTime hora)
        {
            this.activo_retornado = activo_retornado;
            this.dia = dia;
            this.hora = hora;
        }
    }
}
