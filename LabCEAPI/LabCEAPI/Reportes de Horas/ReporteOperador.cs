using LabCEAPI.Users;

namespace LabCEAPI.Laboratorios
{
    public class ReporteOperador
    {
        public DateTime dia { get; set; }

        public string nombre { get; set; }

        public string apellido1 { get; set; }
        public string apellido2 {  get; set; }

        public LinkedList<HorasLaboradas> HorasLaboradas { get; set; }

        public ReporteOperador (DateTime dia, LinkedList<HorasLaboradas> horasLaboradas, string nombre, string apellido1, string apellido2)
        {
            this.dia = dia;
            HorasLaboradas = horasLaboradas;
            this.nombre = nombre;
            this.apellido1 = apellido1;
            this.apellido2 = apellido2;
        }
    }
}
