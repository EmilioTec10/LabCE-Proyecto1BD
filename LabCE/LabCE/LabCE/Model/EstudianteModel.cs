using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabCE.Model
{
    public class EstudianteModel
    {
        [PrimaryKey]
        public string email_est { get; set; }
        public string nombre { get; set; }
        public string apellido1 { get; set; }
        public string apellido2 { get; set; }
        public string carnet { get; set; }
    }
}
