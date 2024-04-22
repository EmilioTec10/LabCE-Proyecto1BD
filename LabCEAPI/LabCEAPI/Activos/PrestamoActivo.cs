using LabCEAPI.Prestamos;
using LabCEAPI.Users;

namespace LabCEAPI.NewFolder
{
    public class PrestamoActivo
    {
        private Activo activo {  get; set; }

        private Profesor aprobador { get; set; }

        private bool aprobado { get; set; }
    }
}
