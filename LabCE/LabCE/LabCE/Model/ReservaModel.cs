using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabCE.Model
{
    public class ReservaModel
    {
        [PrimaryKey, AutoIncrement]
        public int ID_reserva { get; set; }
        public string fecha { get; set; }
        public string hora_inicio { get; set; }
        public string hora_fin { get; set; }
        public string ID_lab { get; set; }
        public string email_prof { get; set; }
        public string email_est { get; set; }
        public string estado { get; set; }
        public string descripcion { get; set; }
        public bool palmada { get; set; }
        public string email_op { get; set; }
    }
}
