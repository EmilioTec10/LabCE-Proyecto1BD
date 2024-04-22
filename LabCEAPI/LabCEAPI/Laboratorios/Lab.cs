using LabCEAPI.Prestamos;

namespace LabCEAPI.Reservaciones
{
    public class Lab
    {
        private string nombre {  get; set; }

        private int cantidad_personas {  get; set; }

        private LinkedList<Activo> Activos { get; set; }
    }
}
