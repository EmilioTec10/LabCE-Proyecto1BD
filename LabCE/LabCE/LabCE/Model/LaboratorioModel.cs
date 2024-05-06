using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabCE.Model
{
    public class LaboratorioModel
    {
        [PrimaryKey]
        public string ID_lab { get; set; }
        public int capacidad { get; set; }
        public int computadoras { get; set; }
        public string facilidades { get; set; }
    }
}
