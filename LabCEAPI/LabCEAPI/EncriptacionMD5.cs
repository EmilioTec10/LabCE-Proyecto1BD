using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;

//Codigo tomado del video https://www.youtube.com/watch?v=H25gXvKqOf4&ab_channel=CodingconC
namespace LabCEAPI
{
    public class EncriptacionMD5
    {

        public static string encriptar (string mensaje)
        {
            string hash = "Lab Ce elpepe";
            byte[] data = UTF8Encoding.UTF8.GetBytes (mensaje);

            MD5 md5 = MD5.Create ();

            TripleDES tripledes = TripleDES.Create ();

            tripledes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            tripledes.Mode = CipherMode.ECB;

            ICryptoTransform transform = tripledes.CreateEncryptor();
            byte[] result = transform.TransformFinalBlock(data, 0, data.Length);

            return Convert.ToBase64String(result);
        }

        public static string desencriptar (string mensajeEn)
        {
            string hash = "Lab Ce elpepe";
            byte[] data =  Convert.FromBase64String(mensajeEn);

            MD5 md5 = MD5.Create();

            TripleDES tripledes = TripleDES.Create();

            tripledes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            tripledes.Mode = CipherMode.ECB;

            ICryptoTransform transform = tripledes.CreateDecryptor();
            byte[] result = transform.TransformFinalBlock(data, 0, data.Length);

            return UTF8Encoding.UTF8.GetString(result);
        }
    }
}
