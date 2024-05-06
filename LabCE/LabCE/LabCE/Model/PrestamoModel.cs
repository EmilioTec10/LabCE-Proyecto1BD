using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabCE.Model
{
    public class PrestamoModel
    {
        [PrimaryKey]
        public int ID_prestamo { get; set; }
        public string ID_activo { get; set; }
        public string Fecha_Hora_Solicitud { get; set; }
        public string Fecha_Hora_Devolucion { get; set; }
        public string estado { get; set; }
        public bool activo { get; set; }
        public string email_prof { get; set; }
        public string email_est { get; set; }
    }
}
