using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LabCE
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void RegistrarButton_Clicked(object sender, EventArgs e)
        {
            // Aquí puedes abrir la página de registro
            await Navigation.PushAsync(new RegistroPage());
        }

        private async void IniciarSesionButton_Clicked(object sender, EventArgs e)
        {
            // Aquí puedes abrir la página de inicio de sesión
            await Navigation.PushAsync(new LoginPage());
        }
    }
}