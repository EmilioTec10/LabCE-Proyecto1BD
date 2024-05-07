using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Internals;
using LabCE.Model;
using System.Windows.Input;
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
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class prestamos : ContentPage
    {
        public event EventHandler PrestamoActualizado;

        public ICommand AprobarPrestamoCommand { get; set; }
        public ICommand NegarPrestamoCommand { get; set; }

        public prestamos()
        {
            InitializeComponent();

            // Asignar los comandos
            AprobarPrestamoCommand = new Command<PrestamoModel>(async (prestamo) => await AprobarPrestamoAsync(prestamo));
            NegarPrestamoCommand = new Command<PrestamoModel>(async (prestamo) => await NegarPrestamoAsync(prestamo));

            // Suscribirte al evento de actualización
            PrestamoActualizado += (sender, args) => CargarPrestamos();

            CargarPrestamos();
        }

        private async Task AprobarPrestamoAsync(PrestamoModel prestamo)
        {
            // Cambiar el estado del préstamo a "Aprobado"
            prestamo.estado = "Aprobado";

            // Actualizar el préstamo en la base de datos
            await App.MyDataBase.UpdatePrestamoAsync(prestamo);

            // Disparar el evento de actualización
            PrestamoActualizado?.Invoke(this, EventArgs.Empty);

            Console.WriteLine($"El préstamo ha sido aprobado.");
        }

        private async Task NegarPrestamoAsync(PrestamoModel prestamo)
        {
            // Cambiar el estado del préstamo a "Negado"
            prestamo.estado = "Negado";

            // Actualizar el préstamo en la base de datos
            await App.MyDataBase.UpdatePrestamoAsync(prestamo);

            // Disparar el evento de actualización
            PrestamoActualizado?.Invoke(this, EventArgs.Empty);

            Console.WriteLine($"El préstamo ha sido negado.");
        }
        private async void CargarPrestamos()
        {
            // Obtener el email del profesor actual (supongamos que está almacenado en una variable global)
            string emailProfesorActual = App.ProfesorActual.email_prof; // Suponiendo que tienes una propiedad en la clase App para almacenar el email del profesor actual

            // Obtener los préstamos pendientes asociados al email del profesor actual
            List<PrestamoModel> prestamosPendientes = await App.MyDataBase.GetPrestamosPendientesPorProfesor(emailProfesorActual);

            // Mostrar los préstamos en el ListView
            listViewPrestamos.ItemsSource = prestamosPendientes;
        }
    }
}