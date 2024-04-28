namespace LabCEAPI
{
    public class HorasLaboradas
    {
        public DateOnly fecha { get; set; }
        public DateTime hora_ingreso { get; set; }
        public DateTime hora_salida { get; set; }
        public float horas_trabajadas { get; set; }

        public HorasLaboradas(DateTime Hora_ingreso, DateOnly fecha)
        {
            this.hora_ingreso = Hora_ingreso;
            this.fecha = fecha;
        }
    }
}
