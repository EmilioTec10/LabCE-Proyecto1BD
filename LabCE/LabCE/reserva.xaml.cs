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

        Button buscarButton;
        Label reservaInfoLabel;

        Grid tablaReserva;

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

            horaFin = new TimePicker();
            horaFin.Time = new TimeSpan(12, 0, 0);
            horaFin.Format = "HH:mm"; // Formato de 24 horas
            horaFin.PropertyChanged += horaFin_PropertyChanged;


            buscarButton = new Button
            {
                Text = "Mostrar Información de Reserva",
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 20, 0, 0)
            };
            buscarButton.Clicked += buscarButton_Clicked;

            reservaInfoLabel = new Label
            {
                Text = "",
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                Margin = new Thickness(0, 20, 0, 0)
            };

            //  TABLA DE RESERVAS BUSCADAS: 
            tablaReserva = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Star }
                },
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Star },
                    new RowDefinition { Height = GridLength.Star },
                    new RowDefinition { Height = GridLength.Star }
                },
                IsVisible = false // La tabla es invisible inicialmente
            };


            // Agregar DatePicker a la página
            Content = new StackLayout
            {
                Margin = new Thickness(20),
                Children = {
                    new Label { Text = "Selecciona la fecha de la reserva" },
                    datePicker,
                    new Label { Text = "Selecciona la hora de inicio de la reserva" },
                    horaInicio,
                    new Label { Text = "Selecciona la hora de fin de la reserva" },
                    horaFin,
                    buscarButton, // Agregar el botón al diseño
                    reservaInfoLabel,
                    tablaReserva // Agregar la tabla a la página
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

        private void horaFin_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
            {
                var selectedTime = horaInicio.Time;

                // Verificar si la hora seleccionada no es exacta
                if (selectedTime.Minutes != 0 && selectedTime.Minutes != 30)
                {
                    DisplayAlert("Error", "Por favor selecciona una hora exacta (por ejemplo, 7:00 o 11:30).", "OK");
                    // Establecer la hora al minuto más cercano
                    horaFin.Time = new TimeSpan(selectedTime.Hours, selectedTime.Minutes < 30 ? 0 : 30, 0);
                }

                // Verificar si la hora seleccionada está dentro del rango permitido
                if (selectedTime < new TimeSpan(7, 30, 0) || selectedTime > new TimeSpan(21, 0, 0))
                {
                    DisplayAlert("Error", "Por favor selecciona una hora entre las 7:30 AM y las 9:00 PM.", "OK");
                    // Establecer la hora predeterminada dentro del rango permitido
                    horaFin.Time = selectedTime < new TimeSpan(7, 30, 0) ? new TimeSpan(7, 30, 0) : new TimeSpan(21, 0, 0);
                }
            }

        }

        private void buscarButton_Clicked(object sender, EventArgs e)
        {

            //Asi para mandar al  servidor a que busque
            // Obtener la fecha y horas seleccionadas
            DateTime selectedDate = datePicker.Date;
            TimeSpan selectedStartTime = horaInicio.Time;
            TimeSpan selectedEndTime = horaFin.Time;

            // Construir el mensaje de información de la reserva
            string reservaInfo = $"Fecha: {selectedDate:d}\nHora de inicio: {selectedStartTime:hh\\:mm}\nHora de fin: {selectedEndTime:hh\\:mm}";

            // Mostrar la información en el Label
            reservaInfoLabel.Text = reservaInfo;

            // Mostrar la tabla después de mostrar la información de la reserva
            tablaReserva.IsVisible = true;


            // Llenar la información de la tabla
            tablaReserva.Children.Add(new Label { Text = "Lab", FontAttributes = FontAttributes.Bold }, 0, 0);
            tablaReserva.Children.Add(new Label { Text = "Capacidad", FontAttributes = FontAttributes.Bold }, 1, 0);
            tablaReserva.Children.Add(new Label { Text = "Computadoras", FontAttributes = FontAttributes.Bold }, 2, 0);
            tablaReserva.Children.Add(new Label { Text = "Facilidades", FontAttributes = FontAttributes.Bold }, 3, 0);
            tablaReserva.Children.Add(new Label { Text = "Reservar", FontAttributes = FontAttributes.Bold }, 4, 0);

            // Agregar datos de la primera fila
            tablaReserva.Children.Add(new Label { Text = "F207" }, 0, 1);
            tablaReserva.Children.Add(new Label { Text = "28" }, 1, 1);
            tablaReserva.Children.Add(new Label { Text = "26" }, 2, 1);
            tablaReserva.Children.Add(new Label { Text = "pizarra" }, 3, 1);
            tablaReserva.Children.Add(new Label { Text = "x" }, 4, 1);



            // Agregar datos de la segunda fila
            tablaReserva.Children.Add(new Label { Text = "F2078" }, 0, 2);
            tablaReserva.Children.Add(new Label { Text = "23" }, 1, 2);
            tablaReserva.Children.Add(new Label { Text = "23" }, 2, 2);
            tablaReserva.Children.Add(new Label { Text = "pizarreea" }, 3, 2);
            tablaReserva.Children.Add(new Label { Text = "x" }, 4, 2);
        }

    }
}