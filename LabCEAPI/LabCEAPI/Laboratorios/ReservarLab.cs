using LabCEAPI.Users;
using Microsoft.AspNetCore.Mvc;

namespace LabCEAPI.Reservaciones
{
    public class ReservarLab
    {

        public string laboratorio { get; set; }

        public string descripcion {  get; set; }

        public DateOnly dia { get; set; }

        public DateTime hora_inicio { get; set; }

        public DateTime hora_fin { get; set; }

        public ReservarLab(string laboratorio, DateOnly dia, DateTime hora_inicio, DateTime hora_fin, string descripcion)
        {
            this.laboratorio = laboratorio;
            this.dia = dia;
            this.hora_inicio = hora_inicio;
            this.hora_fin = hora_fin;
            this.descripcion = descripcion;
        }
    }
}
