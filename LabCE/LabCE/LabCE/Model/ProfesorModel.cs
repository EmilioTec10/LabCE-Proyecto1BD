using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace LabCE.Model
{
    public class ProfesorModel
    {
        [PrimaryKey]
        public string email_prof { get; set; }

        public string password_prof { get; set; }
        public string nombre { get; set; }
        public string apellido1 { get; set; }
        public string apellido2 { get; set; }
        public string cedula { get; set; }
        
    }
}
