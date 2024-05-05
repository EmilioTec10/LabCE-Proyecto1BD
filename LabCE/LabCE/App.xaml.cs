
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using LabCE.Model;
using System.Runtime.InteropServices;
using SQLite;
using System.Diagnostics;

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
using System.IO;

namespace LabCE
{
    public partial class App : Application

    {
        public static SQLiteHelper MyDatabase { get; set; }

        public App()
        {
            InitializeComponent();
            MyDatabase = new SQLiteHelper();

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
