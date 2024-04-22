using LabCEAPI.Users;
using Microsoft.AspNetCore.Mvc;

namespace LabCEAPI.Reservaciones
{
    public class ReservarLab
    {
        private Lab lab {  get; set; }

        private Profesor profesor { get; set; }

        private Operador operador { get; set; }

        DateOnly dia { get; set; }

        DateTime hora { get; set; }
    }
}
