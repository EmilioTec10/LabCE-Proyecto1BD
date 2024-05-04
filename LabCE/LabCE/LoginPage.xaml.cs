using LabCE.Model;
using System;
using System.Diagnostics;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using System.Threading.Tasks;
using LabCE.Model;
using System.Diagnostics;


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

            bool loggedIn = await AuthenticationHelper.LoginAsync(correo, contrasena);


            // Realizar la lógica de inicio de sesión
            if (loggedIn)
            {
                Debug.WriteLine("if de logged in");
                // Si las credenciales son correctas, navegar a la página de menú
                // await DisplayAlert("siiii", "Credenciales correctas", "OK");
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
