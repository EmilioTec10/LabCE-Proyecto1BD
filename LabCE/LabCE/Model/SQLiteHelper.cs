using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LabCE.Model;
using System.Diagnostics;
using System.IO;

namespace LabCE.Model
{
    //AQUI SE HACE TODA LA CONECCION Y OPERACIONES CON LA BASE. 
    public class SQLiteHelper
    {


        SQLiteConnection database;

        public SQLiteHelper()
        {
            try
            {
                var dbPath = @"C:\Users\MRR79\Documents\LabCE-Proyecto1BD\Database\DBsqlite.db";
                Console.WriteLine("Ruta del archivo de base de datos: " + dbPath);


                if (!File.Exists(dbPath))
                {
                    Console.WriteLine("No se encontró la base de datos en la ubicación especificada.");
                    Console.WriteLine("Ruta del archivo de base de datos: " + dbPath);
                    return;
                }

                database = new SQLiteConnection(dbPath);
                Console.WriteLine("Base de datos abierta correctamente.");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al abrir la base de datos: " + ex.Message);
            }
        }

        public bool CheckProfesorExists(string email)
        {
            try
            {
                // Consulta SQL para verificar si existe un profesor con el email proporcionado
                var query = "SELECT COUNT(*) FROM Profesor WHERE email_prof = ?";
                var count = database.ExecuteScalar<int>(query, email);
                return count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al verificar la existencia del profesor: " + ex.Message);
                return false;
            }
        }

        public bool CheckLoginCredentials(string email, string password)
        {
            try
            {
                // Verificar primero si el profesor con el email dado existe en la base de datos
                Console.WriteLine("Verificando si el profesor existe...");
                if (!CheckProfesorExists(email))
                {
                    Console.WriteLine("Profesor no encontrado en la base de datos.");
                    // Si el profesor no existe, devolver false (credenciales incorrectas)
                    return false;
                }

                Console.WriteLine("Profesor encontrado en la base de datos. Verificando credenciales...");

                // Si el profesor existe, verificar las credenciales de inicio de sesión
                var query = "SELECT COUNT(*) FROM Profesor WHERE email_prof = ? AND password_prof = ?";
                var count = database.ExecuteScalar<int>(query, email, password);
                return count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al verificar las credenciales: " + ex.Message);
                return false;
            }
        }


    }
}