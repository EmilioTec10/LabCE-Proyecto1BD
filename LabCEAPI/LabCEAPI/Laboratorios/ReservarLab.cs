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

        DateTime hora { get; set; }

        int duracion { get; set; }

        public ReservarLab(Laboratorio lab, Profesor profesor, DateOnly dia, DateTime hora, int duracion)
        {
            this.lab = lab;
            this.profesor = profesor;
            this.dia = dia;
            this.hora = hora;
            this.duracion = duracion;
        }

        public ReservarLab(Laboratorio lab, Operador operador, DateOnly dia, DateTime hora, int duracion)
        {
            this.lab = lab;
            this.operador = operador;
            this.dia = dia;
            this.hora = hora;
            this.duracion = duracion;
        }
    }
}
