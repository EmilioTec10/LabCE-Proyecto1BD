using LabCE.Model;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;

namespace LabCE
{
    public partial class App : Application
    {
        private static SQLiteHelper db;
        public static SQLiteHelper MyDataBase
        {
            get
            {
                if (db== null)
                {
                    db = new SQLiteHelper(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"SQLite.db3"));
                }
                return db;
            }
        }

        public static ProfesorModel ProfesorActual { get; set; } // Propiedad para almacenar al profesor actual

        public App()
        {
            InitializeComponent();


            MainPage = new NavigationPage(new MainPage()); // Envuelve MainPage en una NavigationPage
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