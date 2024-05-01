using LabCEAPI.Users;
using System.Diagnostics;

namespace LabCEAPI.Prestamos
{
    public class Activo
    {
        public static LinkedList<Activo> activos_disponibles = new LinkedList<Activo>();

        public static LinkedList<Activo> activos_prestados = new LinkedList<Activo>();
        public string tipo {  get; set; }

        public int ID { get; set; }

        private DateOnly purchase_date {  get; set; }

        public string marca { get; set; }

        public string placa { get; set; }

        public string estado { get; set; }

        public string dellate_dañado { get; set; }

        public Activo (string tipo, string marca, string estado)
        {
            this.tipo = tipo;
            this.marca = marca;
            this.estado = estado;
        }

        public Activo(string tipo, string marca, string estado, string placa)
        {
            this.tipo = tipo;
            this.marca = marca;
            this.placa = placa;
            this.estado = estado;
        }
    }
}
