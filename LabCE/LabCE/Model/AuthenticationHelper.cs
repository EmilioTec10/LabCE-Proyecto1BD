using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

using SQLite;
namespace LabCE.Model
{
    public static class AuthenticationHelper
    {
        public static async Task<bool> LoginAsync(string email, string password)
        {
            // Obtener el profesor con el correo electrónico dado
            Profesor profesor = await App.MyDatabase.GetProfesorByEmail(email);


            // Verificar si se encontró un profesor con el correo electrónico dado y si la contraseña coincide
            Debug.WriteLine("Entra a verifica");
            return profesor != null && profesor.PasswordProf == password;
        }
    }
}

