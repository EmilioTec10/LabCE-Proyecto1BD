
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LabCE
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage()); // Envuelve LoginPage en una NavigationPage
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
