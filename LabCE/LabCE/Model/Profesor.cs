using System;
using System.Collections.Generic;
using System.Text;
using SQLite; 

namespace LabCE.Model
{

    public class Profesor
    {
        [PrimaryKey]
        public string EmailProf { get; set; }
        public string Nombre { get; set; }
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }
        [Unique]
        public string Cedula { get; set; }
        public string PasswordProf { get; set; }
        public int EsAdmin { get; set; }

        public Profesor()
        {
            // Constructor vacío necesario para SQLite
        }
    }
    }
