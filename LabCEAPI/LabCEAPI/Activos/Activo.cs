using LabCEAPI.Users;
using System.Diagnostics;

namespace LabCEAPI.Prestamos
{
    public class Activo
    {
        public static LinkedList<Activo> activos_disponibles = new LinkedList<Activo>();

        public static LinkedList<Activo> activos_prestados = new LinkedList<Activo>();
        public string tipo {  get; set; }

        public string lab {  get; set; }

        public DateTime purchase_date {  get; set; }

        public string marca { get; set; }

        public string placa { get; set; }

        public string estado { get; set; }
        public bool necesita_aprobacion {  get; set; }

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

        public Activo(string tipo, string marca, string estado, string placa, string lab)
        {
            this.tipo = tipo;
            this.marca = marca;
            this.placa = placa;
            this.estado = estado;
            this.lab = lab;
        }

        public Activo(string tipo, string marca, string estado, string placa, string lab, DateTime purchase_date, bool necesita_aprobacion)
        {
            this.tipo = tipo;
            this.marca = marca;
            this.placa = placa;
            this.estado = estado;
            this.lab = lab;
            this.purchase_date = purchase_date;
            this.necesita_aprobacion = necesita_aprobacion;
        }
    }
}
