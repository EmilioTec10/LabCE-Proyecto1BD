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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistroPage : ContentPage
    {
        public RegistroPage()
        {
            InitializeComponent();
        }

        private async void RegistrarseButton_Clicked(object sender, EventArgs e)
        {
            // Validar el formato del correo electrónico
            if (!IsValidEmail(EmailEntry.Text))
            {
                await DisplayAlert("Error", "El correo electrónico no es válido.", "Aceptar");
                return;
            }

            // Guardar datos en la base de datos SQLite usando SQLiteHelper
            var profesor = new ProfesorModel 
            {
                email_prof = EmailEntry.Text,
                password_prof = PasswordEntry.Text,
                nombre = NombreEntry.Text,
                apellido1 = Apellido1Entry.Text,
                apellido2 = Apellido2Entry.Text,
                cedula = CedulaEntry.Text
            };

            App.MyDataBase.CreateProfesor(profesor); // Inserta el profesor en la base de datos

            await DisplayAlert("Registro", "¡Registrado exitosamente!", "Aceptar");
        }


        // Método para validar el formato del correo electrónico
        bool IsValidEmail(string email)
        {
            // Aquí puedes usar una expresión regular u otra lógica para validar el correo electrónico
            return email.Contains("@itcr.ac.cr");
        }
    }
}
