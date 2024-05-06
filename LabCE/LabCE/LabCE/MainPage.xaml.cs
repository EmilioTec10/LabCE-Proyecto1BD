using LabCE.Model;
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
            CrearYGuardarDatos();
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

        private async void CrearYGuardarDatos()
        {
            // Laboratorios
            var laboratorio1 = new LaboratorioModel
            {
                ID_lab = "LAB1",
                capacidad = 30,
                computadoras = 20,
                facilidades = "Proyector, pizarra, conexiones de red"
            };

            await App.MyDataBase.CreateLaboratorio(laboratorio1);

            var laboratorio2 = new LaboratorioModel
            {
                ID_lab = "LAB2",
                capacidad = 25,
                computadoras = 15,
                facilidades = "Pizarra, conexiones de red"
            };

            await App.MyDataBase.CreateLaboratorio(laboratorio2);

            var laboratorio3 = new LaboratorioModel
            {
                ID_lab = "LAB3",
                capacidad = 40,
                computadoras = 30,
                facilidades = "Proyector, pizarra"
            };

            await App.MyDataBase.CreateLaboratorio(laboratorio3);

            // Estudiantes
            var estudiante1 = new EstudianteModel
            {
                email_est = "alina@estudiantec.cr",
                nombre = "Alina",
                apellido1 = "Rojas",
                apellido2 = "Villegas",
                carnet = "123456781"
            };

            await App.MyDataBase.CreateEstudiante(estudiante1);

            var estudiante2 = new EstudianteModel
            {
                email_est = "daniela@estudiantec.cr",
                nombre = "Daniela",
                apellido1 = "Granados",
                apellido2 = "Sibaja",
                carnet = "987654321"
            };

            await App.MyDataBase.CreateEstudiante(estudiante2);

            var estudiante3 = new EstudianteModel
            {
                email_est = "norma@estudiantec.cr",
                nombre = "Norma",
                apellido1 = "Gonzalez",
                apellido2 = "Ramirez",
                carnet = "456123789"
            };

            await App.MyDataBase.CreateEstudiante(estudiante3);

            // Activos
            var activo1 = new ActivosModel
            {
                ID_activo = "ACT1x",
                ID_lab = "LAB1",
                tipo = "Computadora",
                estado = "Disponible",
                necesita_aprovacion = true,
                fecha_compra = DateTime.Now.ToString(),
                marca = "HP"
            };

            await App.MyDataBase.CreateActivos(activo1);

            var activo2 = new ActivosModel
            {
                ID_activo = "ACT2x",
                ID_lab = "LAB2",
                tipo = "Proyector",
                estado = "Disponible",
                necesita_aprovacion = true,
                fecha_compra = DateTime.Now.ToString(),
                marca = "Epson"
            };

            await App.MyDataBase.CreateActivos(activo2);

            var activo3 = new ActivosModel
            {
                ID_activo = "ACT3xy",
                ID_lab = "LAB3",
                tipo = "Impresora",
                estado = "Disponible",
                necesita_aprovacion = true,
                fecha_compra = DateTime.Now.ToString(),
                marca = "Canon"
            };

            await App.MyDataBase.CreateActivos(activo3);

            var activo4 = new ActivosModel
            {
                ID_activo = "ACT4abc",
                ID_lab = "LAB3",
                tipo = "Multimetro",
                estado = "Disponible",
                necesita_aprovacion = true,
                fecha_compra = DateTime.Now.ToString(),
                marca = "Canon"
            };


            await App.MyDataBase.CreateActivos(activo4);
        }
    }
}