using LabCEAPI.Users;

namespace LabCEAPI.Laboratorios
{
    public class ReporteOperador
    {
        public DateTime dia { get; set; }

        public string email_op { get; set; }

        public string detalles { get; set; }

        public LinkedList<HorasLaboradas> HorasLaboradas { get; set; }

        public ReporteOperador (DateTime dia, LinkedList<HorasLaboradas> horasLaboradas, string email_op)
        {
            this.dia = dia;
            HorasLaboradas = horasLaboradas;
            this.email_op = email_op;
        }
    }
}
