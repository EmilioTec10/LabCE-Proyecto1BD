
using System;

using Xamarin.Forms;

namespace LabCE
{
    public partial class AppShell : TabbedPage
    {
        public AppShell()
        {
            InitializeComponent();
        }

        // Evento para el botón en la segunda pestaña
        private async void Boton_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Mensaje", "¡Has hecho clic en el botón!", "OK");
        }
    }
}
