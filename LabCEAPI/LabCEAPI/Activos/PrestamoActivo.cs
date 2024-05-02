using LabCEAPI.Prestamos;
using LabCEAPI.Users;

namespace LabCEAPI.NewFolder
{
    public class PrestamoActivo
    {
        public string email_est {  get; set; }

        public string email_prof { get; set; }

        public string estado { get; set; }

        public string placa { get; set; }

        public DateTime fecha_hora_solicitud { get; set; }

        public PrestamoActivo(string email_est, string email_prof, DateTime fecha_hora_solicitud, string estado, string placa)
        {
            this.email_est = email_est;
            this.email_prof = email_prof;
            this.fecha_hora_solicitud = fecha_hora_solicitud;
            this.estado = estado;
            this.placa = placa;
        }

        public PrestamoActivo(string email_est, string email_prof, DateTime fecha_hora_solicitud, string estado)
        {
            this.email_est = email_est;
            this.email_prof = email_prof;
            this.fecha_hora_solicitud = fecha_hora_solicitud;
            this.estado = estado;
        }
    }
}
