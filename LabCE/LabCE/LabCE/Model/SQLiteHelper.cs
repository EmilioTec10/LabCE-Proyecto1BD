using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.Threading.Tasks;
using System.Runtime.InteropServices.ComTypes;

namespace LabCE.Model
{
    public class SQLiteHelper
    {
        private readonly SQLiteAsyncConnection db;

        public SQLiteHelper(string dbPath)
        {
            db = new SQLiteAsyncConnection(dbPath);
            db.CreateTableAsync<ProfesorModel>();
        }

        public Task<int> CreateProfesor (ProfesorModel profesor)
        {
            return db.InsertAsync(profesor);
        }

        public Task<List<ProfesorModel>> ReadProfesor()
        {
            return db.Table<ProfesorModel>().ToListAsync();
        }

        public Task<int> UpdateProfesor(ProfesorModel profesor)
        {
            return db.UpdateAsync(profesor);
        }

        public Task<ProfesorModel> GetProfesorByEmail(string email)
        {
            return db.Table<ProfesorModel>().Where(p => p.email_prof == email).FirstOrDefaultAsync();
        }

    }
}
