using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LabCE
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : TabbedPage
    {
        public AppShell()
        {
            InitializeComponent();
    
        }

        private async void Boton_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Mensaje", "¡Has hecho clic en el botón!", "OK");
        }
    }

}