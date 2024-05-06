using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.Threading.Tasks;
using System.Runtime.InteropServices.ComTypes;
using LabCE.Model;

namespace LabCE.Model
{
    public class SQLiteHelper
    {
        private readonly SQLiteAsyncConnection db;

        public SQLiteHelper(string dbPath)
        {
            db = new SQLiteAsyncConnection(dbPath);
            db.CreateTableAsync<ProfesorModel>();
            db.CreateTableAsync<LaboratorioModel>();
            db.CreateTableAsync<EstudianteModel>();
            db.CreateTableAsync<ActivosModel>();
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


        public async Task<int> CreateLaboratorio(LaboratorioModel laboratorio)
        {
            var existingLab = await db.Table<LaboratorioModel>().Where(l => l.ID_lab == laboratorio.ID_lab).FirstOrDefaultAsync();
            if (existingLab == null)
                return await db.InsertAsync(laboratorio);
            else
                return 0; // Indica que el laboratorio ya existe
        }

        public async Task<int> CreateEstudiante(EstudianteModel estudiante)
        {
            var existingEstudiante = await db.Table<EstudianteModel>().Where(e => e.email_est == estudiante.email_est).FirstOrDefaultAsync();
            if (existingEstudiante == null)
                return await db.InsertAsync(estudiante);
            else
                return 0; // Indica que el estudiante ya existe
        }


        public async Task<int> CreateActivos(ActivosModel activo)
        {
            var existingActivo = await db.Table<ActivosModel>().Where(a => a.ID_activo == activo.ID_activo).FirstOrDefaultAsync();
            if (existingActivo == null)
                return await db.InsertAsync(activo);
            else
                return 0; // Indica que el activo ya existe
        }

    }
}
