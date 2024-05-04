using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LabCE.Model;
using System.Diagnostics;

namespace LabCE.Model
{
    //AQUI SE HACE TODA LA CONECCION Y OPERACIONES CON LA BASE. 
    public class SQLiteHelper
    {
        private readonly SQLiteAsyncConnection db;

        public SQLiteHelper(string dbPath)
        {
            db = new SQLiteAsyncConnection(dbPath);
        }
        public Task<List<Profesor>> ReadProfesores()
        {
            return db.Table<Profesor>().ToListAsync();
        }
        public Task<Profesor> Login(string email, string password)
        {
            return db.Table<Profesor>().Where(x => x.EmailProf == email && x.PasswordProf == password).FirstOrDefaultAsync();
        }

        public async Task<Profesor> GetProfesorByEmail(string email)
        {
            Debug.WriteLine("entro a profesor by email");
            return await db.Table<Profesor>().FirstOrDefaultAsync(p => p.EmailProf == email);
        }
    }
}