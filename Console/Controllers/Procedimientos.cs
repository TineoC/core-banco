using Core_Console.Models;

namespace Core_Console.Controllers
{
    public class ProcedimientosController
    {
        HospitalContext? dbContext = null;
        public void CrearProcedimiento(ProcesoMedico procedimiento)
        {
            dbContext = new HospitalContext();
            dbContext.ProcesoMedicos.Add(procedimiento);
            dbContext.SaveChanges();
        }

        public void MostrarProcedimientos()
        {
            dbContext = new HospitalContext();

            foreach (var procedimiento in dbContext.ProcesoMedicos)
            {
                Console.WriteLine(procedimiento);
            }

            dbContext.SaveChanges();
        }

        public void MostrarProcedimiento(int procesoID)
        {
            dbContext = new HospitalContext();

            ProcesoMedico? findedProcedimiento = dbContext.ProcesoMedicos.Find(procesoID);

            Console.WriteLine(findedProcedimiento);

            dbContext.SaveChanges();
        }

        public void ActualizarProcedimiento(int procesoID, ProcesoMedico newProcedimiento)
        {
            dbContext = new HospitalContext();
            ProcesoMedico? findedProcedimiento = dbContext.ProcesoMedicos.Find(procesoID);

            if (findedProcedimiento != null) { findedProcedimiento = newProcedimiento; }
        }

        public void EliminarProcedimiento(int procesoID)
        {
            dbContext = new HospitalContext();
            ProcesoMedico? findedProcedimiento = dbContext.ProcesoMedicos.Find(procesoID);

            if (findedProcedimiento != null) { dbContext.ProcesoMedicos.Remove(findedProcedimiento); }

        }
    }
}