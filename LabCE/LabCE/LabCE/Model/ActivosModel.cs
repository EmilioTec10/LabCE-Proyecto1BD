using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabCE.Model
{
    public class ActivosModel
    {
        [PrimaryKey]
        public string ID_activo { get; set; }
        public string ID_lab { get; set; }
        public string tipo { get; set; }
        public string estado { get; set; }
        public bool necesita_aprovacion { get; set; }
        public string fecha_compra { get; set; }
        public string marca { get; set; }
    }
}
