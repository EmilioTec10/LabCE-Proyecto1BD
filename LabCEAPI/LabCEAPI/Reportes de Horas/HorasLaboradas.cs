namespace LabCEAPI
{
    public class HorasLaboradas
    {
        public DateOnly fecha { get; set; }
        public DateTime hora_ingreso { get; set; }
        public DateTime hora_salida { get; set; }
        public float horas_trabajadas { get; set; }

        public HorasLaboradas(DateTime hora_ingreso, DateTime hora_salida)
        {
            this.hora_ingreso = hora_ingreso;
            this.hora_salida = hora_salida;
            fecha = new DateOnly(hora_ingreso.Year, hora_ingreso.Month, hora_ingreso.Day);
            horas_trabajadas = (hora_salida.Hour - hora_ingreso.Hour) + ((hora_salida.Minute - hora_ingreso.Minute)/ 60.0f);
        }
    }
}
