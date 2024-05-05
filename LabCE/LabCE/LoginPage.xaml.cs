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

            // Verificar las credenciales utilizando DatabaseService
            SQLiteHelper databaseService = new SQLiteHelper();
            bool isLoggedIn = databaseService.CheckLoginCredentials(correo, contrasena);



            if (isLoggedIn)
            {
                // Si las credenciales son correctas, navegar a la página de menú
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
