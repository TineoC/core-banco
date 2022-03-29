using Core_Console.Models;

namespace Core_Console.Controllers
{
    public class PacientesController
    {
        HospitalContext? dbContext = null;
        public void CrearPaciente(Persona paciente)
        {
            dbContext = new HospitalContext();
            dbContext.Personas.Add(paciente);
            dbContext.SaveChanges();
        }

        public void MostrarPacientes()
        {
            dbContext = new HospitalContext();

            foreach (var paciente in dbContext.Personas)
            {
                Console.WriteLine(paciente);
            }

            dbContext.SaveChanges();
        }

        public void MostrarPaciente(int pacienteDocumento)
        {
            dbContext = new HospitalContext();

            Persona? findedPaciente = dbContext.Personas.Find(pacienteDocumento);

            Console.WriteLine(findedPaciente);

            dbContext.SaveChanges();
        }

        public void ActualizarPaciente(int pacienteDocumento, Persona newPaciente)
        {
            dbContext = new HospitalContext();
            Persona? findedPaciente = dbContext.Personas.Find(pacienteDocumento);

            if (findedPaciente != null) { findedPaciente = newPaciente; }
        }

        public void EliminarPaciente(int pacienteDocumento)
        {
            dbContext = new HospitalContext();
            Persona? findedPaciente = dbContext.Personas.Find(pacienteDocumento);

            if (findedPaciente != null) { dbContext.Personas.Remove(findedPaciente); }

        }
    }
}