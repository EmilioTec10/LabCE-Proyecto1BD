using LabCEAPI.Users;

namespace LabCEAPI.Laboratorios
{
    public class ReporteOperador
    {
        private DateTime dia { get; set; }

        private Operador operador { get; set; }

        private string detalles { get; set; }

        private LinkedList<HorasLaboradas> HorasLaboradas { get; set; }

        public ReporteOperador (DateTime dia, Operador operador, string detalles, LinkedList<HorasLaboradas> horasLaboradas)
        {
            this.dia = dia;
            this.operador = operador;
            this.detalles = detalles;
            HorasLaboradas = horasLaboradas;
        }
    }
}
