using LabCEAPI.Users;

namespace LabCEAPI.Prestamos
{
    public class Activo
    {
        private string tipo {  get; set; }

        private DateOnly purchase_date {  get; set; }

        private string marca { get; set; }

        private string placa { get; set; }

        private bool prestado { get; set; }
    }
}
