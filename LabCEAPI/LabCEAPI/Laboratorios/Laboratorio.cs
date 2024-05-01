using LabCEAPI.Prestamos;

namespace LabCEAPI.Reservaciones
{
    public class Laboratorio
    {
        public static LinkedList<Laboratorio> labs = new LinkedList<Laboratorio>();
        public  string nombre {  get; set; }

        public int cantidad_personas {  get; set; }

        public string facilidades { get; set; }

        public LinkedList<Activo> Activos { get; set; }

        public Laboratorio(string nombre)
        {
            this.nombre = nombre;   
        }

        public Laboratorio(string nombre, int cantidad_personas, string facilidades)
        {
            this.nombre = nombre;
            this.cantidad_personas = cantidad_personas;
            this.facilidades = facilidades;
        }
    }
}
