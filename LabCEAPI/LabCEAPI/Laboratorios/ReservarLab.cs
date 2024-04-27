using LabCEAPI.Users;
using Microsoft.AspNetCore.Mvc;

namespace LabCEAPI.Reservaciones
{
    public class ReservarLab
    {
        private Laboratorio lab {  get; set; }

        private Profesor profesor { get; set; }

        private Operador operador { get; set; }

        DateOnly dia { get; set; }

        DateTime hora_inicio { get; set; }

        DateTime hora_fin { get; set; }


        public ReservarLab(Laboratorio lab, Profesor profesor, DateOnly dia, DateTime hora_inicio, DateTime hora_fin)
        {
            this.lab = lab;
            this.profesor = profesor;
            this.dia = dia;
            this.hora_inicio = hora_inicio;
            this.hora_fin = hora_fin;
        }

        public ReservarLab(Laboratorio lab, Operador operador, DateOnly dia, DateTime hora_inicio, DateTime hora_fin)
        {
            this.lab = lab;
            this.operador = operador;
            this.dia = dia;
            this.hora_inicio = hora_inicio;
            this.hora_fin = hora_fin;
        }
    }
}
