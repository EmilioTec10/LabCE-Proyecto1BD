namespace LabCEAPI.Users
{
    public class Usuario
    {
        public string cedula { get; set; }

        public string nombre { get; set; }

        public string apellidos { get; set; }

        public DateOnly fecha_de_nacimiento { get; set; }

        public string email { get; set; }

        public Usuario (string cedula, string nombre, string apellidos, DateOnly fecha_de_nacimiento, string email)
        {
            this.cedula = cedula;
            this.nombre = nombre;
            this.apellidos = apellidos;
            this.fecha_de_nacimiento = fecha_de_nacimiento;
            this.email = email;
        }
    }
}
