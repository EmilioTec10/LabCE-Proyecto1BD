using LabCEAPI.Users;

namespace LabCEAPI.Prestamos
{
    public class Activo
    {
        public static LinkedList<Activo> activos_disponibles = new LinkedList<Activo>();

        public static LinkedList<Activo> activos_prestados = new LinkedList<Activo>();
        private string tipo {  get; set; }

        private DateOnly purchase_date {  get; set; }

        private string marca { get; set; }

        private string placa { get; set; }

        private bool prestado { get; set; }

        public bool dañado { get; set; }

        public string dellate_dañado {  get; set; } 
    }
}
