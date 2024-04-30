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
    public partial class reserva : ContentPage
    {
        DatePicker datePicker;

        public reserva()
        {
            InitializeComponent();

            datePicker = new DatePicker();

            datePicker.MinimumDate = DateTime.Today;

            // Establecer la fecha máxima como hoy + 3 semanas
            datePicker.MaximumDate = DateTime.Today.AddDays(21);

            datePicker.DateSelected += DatePicker_DateSelected;

            datePicker.DateSelected += DatePicker_DateSelected;

            // Agregar DatePicker a la página
            Content = new StackLayout
            {
                Margin = new Thickness(20),
                Children = {
                    new Label { Text = "Selecciona la fecha de la reserva" },
                    datePicker
                }
            };
        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            // Verificar si la fecha seleccionada es de lunes a sábado
            if (e.NewDate.DayOfWeek == DayOfWeek.Sunday)
            {
                DisplayAlert("Error", "Solo se pueden reservar días de lunes a viernes.", "OK");
                datePicker.Date = DateTime.Today; // Restablecer la fecha seleccionada
            }
        }
    }
}