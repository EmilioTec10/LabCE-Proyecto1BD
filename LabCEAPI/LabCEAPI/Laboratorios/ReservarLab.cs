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

        public ReservarLab(Laboratorio lab, Profesor profesor, DateOnly dia, DateTime hora)
        {
            this.lab = lab;
            this.profesor = profesor;
            this.dia = dia;
            this.hora = hora;
        }

        public ReservarLab(Laboratorio lab, Operador operador, DateOnly dia, DateTime hora)
        {
            this.lab = lab;
            this.operador = operador;
            this.dia = dia;
            this.hora = hora;
        }
    }
}
