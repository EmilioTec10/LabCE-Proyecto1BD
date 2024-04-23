using LabCEAPI.Prestamos;

namespace LabCEAPI.Reservaciones
{
    public class Laboratorio
    {
        public static LinkedList<Laboratorio> labs = new LinkedList<Laboratorio>();
        private string nombre {  get; set; }

        private int cantidad_personas {  get; set; }

        private string facilidades { get; set; }

        private LinkedList<Activo> Activos { get; set; }
    }
}
