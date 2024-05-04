
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


namespace LabCE
{
    public partial class App : Application

    {
        private static SQLiteHelper db;
        public static SQLiteHelper MyDatabase

        {
            get
            {
                if (db == null)
                {
                    Debug.WriteLine("Entro a path de base");
                    string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DBsqlite.db");
                    db = new SQLiteHelper(dbPath);
                }
                return db;

            }
           
        }
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
