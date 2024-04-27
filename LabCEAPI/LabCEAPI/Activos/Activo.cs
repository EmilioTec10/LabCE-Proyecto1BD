using LabCEAPI.Users;

namespace LabCEAPI.Prestamos
{
    public class Activo
    {
        public static LinkedList<Activo> activos_disponibles = new LinkedList<Activo>();

        public static LinkedList<Activo> activos_prestados = new LinkedList<Activo>();
        public string tipo {  get; set; }

        private DateOnly purchase_date {  get; set; }

        public string marca { get; set; }

        public string placa { get; set; }

        public bool prestado { get; set; }

        public bool dañado { get; set; }

        public string dellate_dañado {  get; set; } 

        public Activo(string tipo, string marca, string placa, bool dañado, string dellate_dañado)
        {
            this.tipo = tipo;
            this.marca = marca;
            this.placa = placa;
            this.dañado = dañado;
            this.dellate_dañado = dellate_dañado;
        }
    }
}
