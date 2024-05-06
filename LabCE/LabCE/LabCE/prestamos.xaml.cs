using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Internals;

namespace LabCE
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class prestamos : ContentPage
    {
        Grid tablaPrestamos;
        List<Prestamo> listaPrestamos;

        public prestamos()
        {
            listaPrestamos = new List<Prestamo>
            {
                new Prestamo { Id = 1, FechaSolicitud = DateTime.Now, Email = "mmrr@gmail.com", Aprobado = true },
                new Prestamo { Id = 2, FechaSolicitud = DateTime.Now, Email = "sss@gmail.com", Aprobado = true },
                new Prestamo { Id = 3, FechaSolicitud = DateTime.Now, Email = "aaaaa@gmail.com", Aprobado = true }
                // Puedes agregar más préstamos si lo deseas
            };

            tablaPrestamos = new Grid
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
                    new RowDefinition { Height = GridLength.Auto }
                },
                IsVisible = true
            };

            // Encabezados de las columnas
            tablaPrestamos.Children.Add(new Label { Text = "ID Prestamos" }, 0, 0);
            tablaPrestamos.Children.Add(new Label { Text = "Fecha solicitud" }, 1, 0);
            tablaPrestamos.Children.Add(new Label { Text = "Email" }, 2, 0);
            tablaPrestamos.Children.Add(new Label { Text = "Aprobado" }, 3, 0);
            tablaPrestamos.Children.Add(new Label { Text = "Enviar" }, 4, 0);

            // Filas de datos
            for (int i = 0; i < listaPrestamos.Count; i++)
            {
                int index = i; // Variable local para guardar el valor actual de 'i'

                tablaPrestamos.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                tablaPrestamos.Children.Add(new Label { Text = listaPrestamos[index].Id.ToString() }, 0, index + 1);
                tablaPrestamos.Children.Add(new Label { Text = listaPrestamos[index].FechaSolicitud.ToString() }, 1, index + 1);
                tablaPrestamos.Children.Add(new Label { Text = listaPrestamos[index].Email }, 2, index + 1);

                // Switch para "Aprobado"
                Switch aprobadoSwitch = new Switch();
                aprobadoSwitch.IsToggled = listaPrestamos[index].Aprobado;
                aprobadoSwitch.Toggled += (sender, e) =>
                {
                    // Actualizar el estado del préstamo
                    listaPrestamos[index].Aprobado = aprobadoSwitch.IsToggled;
                    listaPrestamos[index].Estado = aprobadoSwitch.IsToggled ? "Aprobado" : "Negado";
                };
                tablaPrestamos.Children.Add(aprobadoSwitch, 3, index + 1);

                // Botón de enviar
                // Botón de enviar
                Button enviarButton = new Button { Text = "Enviar" };
                enviarButton.Clicked += async (sender, e) =>
                {
                    StringBuilder mensaje = new StringBuilder();
                    mensaje.AppendLine("Información de los préstamos:");

                    foreach (var prestamo in listaPrestamos)
                    {
                        mensaje.AppendLine($"ID: {prestamo.Id}, Fecha solicitud: {prestamo.FechaSolicitud}, Email: {prestamo.Email}, Aprobado: {(prestamo.Aprobado ? "Aprobado" : "Negado")}");
                    }

                    await DisplayAlert("Información de préstamos", mensaje.ToString(), "Aceptar");

                    // Ocultar la fila después de mostrar el mensaje
                    int rowIndex = tablaPrestamos.Children.IndexOf(enviarButton.Parent);
                    if (rowIndex != -1)
                    {
                        for (int colIndex = 0; colIndex < tablaPrestamos.ColumnDefinitions.Count; colIndex++)
                        {
                            tablaPrestamos.Children.RemoveAt(rowIndex * tablaPrestamos.ColumnDefinitions.Count);
                        }
                        tablaPrestamos.RowDefinitions.RemoveAt(rowIndex);
                    }
                };
                tablaPrestamos.Children.Add(enviarButton, 4, index + 1);




            }


            Content = new StackLayout
            {
                Margin = new Thickness(20),
                Children = {
                    new Label { Text = "Prestamos que requieren de su permiso." },
                    tablaPrestamos
                }
            };
        }
    }

    public class Prestamo
    {
        public int Id { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string Email { get; set; }
        public bool Aprobado { get; set; }
        public string Estado { get; set; } // Nuevo campo para el estado como cadena
    }
}