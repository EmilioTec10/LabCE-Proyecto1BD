using LabCEAPI.Prestamos;

namespace LabCEAPI.Reservaciones
{
    public class Laboratorio
    {
        public static LinkedList<Laboratorio> labs = new LinkedList<Laboratorio>();
        public  string nombre {  get; set; }

        public int capacidad {  get; set; }

        public string facilidades { get; set; }

        public int computadores { get; set; }

        public string activos { get; set; }

       // public LinkedList<Activo> Activos { get; set; }

        public Laboratorio(string nombre)
        {
            this.nombre = nombre;   
        }

        public Laboratorio(string nombre, int cantidad_personas, string facilidades, int computadores, string activos)
        {
            this.nombre = nombre;
            this.capacidad = cantidad_personas;
            this.facilidades = facilidades;
            this.computadores = computadores;
            this.activos = activos;
        }
    }
}
