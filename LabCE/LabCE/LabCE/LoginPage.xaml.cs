using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using LabCE.Model;

namespace LabCE
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            // Obtener el correo electrónico y la contraseña ingresados por el usuario
            string correo = txtCorreo.Text;
            string contrasena = txtContrasena.Text;

            // Buscar en la base de datos si existe un profesor con las credenciales proporcionadas
            var profesor = await App.MyDataBase.GetProfesorByEmail(correo);

            // Realizar la lógica de inicio de sesión
            if (profesor != null && profesor.password_prof == contrasena)
            {
                // Si las credenciales son correctas, navegar a la página de menú
                //await DisplayAlert("Éxito", "Inicio de sesión exitoso", "OK");
                await Navigation.PushAsync(new AppShell());
            }
            else
            {
                // Si las credenciales son incorrectas, mostrar un mensaje de error
                await DisplayAlert("Error", "Credenciales incorrectas", "OK");
            }
        }
    }
}