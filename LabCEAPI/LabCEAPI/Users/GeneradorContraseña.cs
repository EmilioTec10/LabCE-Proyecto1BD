using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace LabCEAPI.Users
{
    //Clase que contiene metodos para generar una nueva contraseña de manera aleatorio
    public class GeneradorContraseña
    {
        private static readonly Random random = new Random();

        private static string email_envio = "mrodigez6@gmail.com"; //Email que realiza el envio de la contra al correo

        private static string contra_envio = "wwlydgzgfsuearmq";

        private static string alias_envio = "LaboratoriosCE";

        private static MailMessage mensaje_email;

        public static string NuevaContraseña(int length = 8)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@*$^";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static void mandar_correo(string email, string nueva_contraseña)
        {
            mensaje_email = new MailMessage();
            mensaje_email.From = new MailAddress(email_envio, alias_envio, System.Text.Encoding.UTF8);
            mensaje_email.To.Add(email);
            mensaje_email.Subject = "Nueva contraseña LabCE";
            mensaje_email.Body = "Su nueva contraseña es: " + nueva_contraseña;
            mensaje_email.Priority = MailPriority.High;

            try
            {
                SmtpClient smtp = new SmtpClient();
                smtp.UseDefaultCredentials = false;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com";
                smtp.Credentials = new System.Net.NetworkCredential(email_envio, contra_envio);
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors) { return true; };
                smtp.EnableSsl = true;
                smtp.Send(mensaje_email);
                Console.WriteLine("Enviado");

            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            
        }

    }
}
