using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
//using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace LabCE
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class reserva : ContentPage
    {
        DatePicker datePicker;

        //TimePicker myTimePicker;

        public reserva()
        {
            InitializeComponent();

            datePicker = new DatePicker();

            datePicker.MinimumDate = DateTime.Today;

            // Establecer la fecha máxima como hoy + 3 semanas
            datePicker.MaximumDate = DateTime.Today.AddDays(21);

            datePicker.DateSelected += DatePicker_DateSelected;

            datePicker.DateSelected += DatePicker_DateSelected;

            horaInicio = new TimePicker();
            horaInicio.Time = new TimeSpan(7, 0, 0); // Establecer la hora predeterminada a las 7:00
            horaInicio.Format = "HH:mm"; // Formato de 24 horas
            horaInicio.PropertyChanged += horaInicio_PropertyChanged;

            // Agregar DatePicker a la página
            Content = new StackLayout
            {
                Margin = new Thickness(20),
                Children = {
                    new Label { Text = "Selecciona la fecha de la reserva" },
                    datePicker,
                    new Label { Text = "Selecciona la hora de inicio de la reserva" },
                    horaInicio
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
        private void horaInicio_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
            {
                var selectedTime = horaInicio.Time;
                // Verificar si la hora seleccionada no es exacta
                if (selectedTime.Minutes != 0 && selectedTime.Minutes != 30)
                {
                    DisplayAlert("Error", "Por favor selecciona una hora exacta (por ejemplo, 7:00 o 11:30).", "OK");
                    horaInicio.Time = new TimeSpan(selectedTime.Hours, 0, 0); // Establecer la hora a la más cercana en punto
                }

                if (selectedTime < new TimeSpan(7, 0, 0) || selectedTime > new TimeSpan(20, 30, 0))
                {
                    DisplayAlert("Error", "Por favor selecciona una hora entre las 7:00 AM y las 8:30 PM.", "OK");
                    horaInicio.Time = new TimeSpan(7, 0, 0); // Establecer la hora predeterminada a las 7:00 AM
                }
            }
        }

    }
}