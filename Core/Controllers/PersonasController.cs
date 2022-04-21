using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Controllers
{
    internal class PersonasController
    {
        static hospitalEntities hospital = new hospitalEntities();

        public static void MostrarInformacion(Persona persona)
        {
            Console.WriteLine($"Tipo de Documento: {persona.Persona_TipoDocumento}");
            Console.WriteLine($"Documento: {persona.Persona_Documento}");
            Console.WriteLine($"Nombres: {persona.Persona_Nombre}");
            Console.WriteLine($"Apellidos: {persona.Persona_Apellido}");
            Console.WriteLine($"Tipo de Persona: {persona.Persona_TipoPersona}");
            Console.WriteLine($"Escribe tu correo electrónico: {persona.Persona_CorreoElectronico}");
            Console.WriteLine($"Teléfono: {persona.Persona_Telefono}");
            Console.WriteLine($"Dirección: {persona.Persona_Direccion}");
            Console.WriteLine($"ID Aseguradora: {persona.Persona_IdAseguradora}");
            Console.WriteLine($"Fecha Creación: {persona.Persona_FechaCreacion}");
            Console.WriteLine($"Id Usuario Creador: {persona.Persona_IdUsuarioCreador}");
            Console.WriteLine($"Vigencia: {persona.Persona_Vigencia}");
        }

        public static void Crear()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            Console.Clear();

            try
            {
                bool exists;
                int TipoDocumento;

                do
                {
                    Console.WriteLine("Ingresa el ID del Tipo de Documento: ");
                    TipoDocumento = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.TipoDocumento.Any(tipo => Equals(tipo.TipoDocumento_Id,TipoDocumento));

                    if (!exists)
                    {
                        Logger.Error("No existen ID's para ese Tipo de Documento");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                    }
                    
                } while (!exists);

                Console.Write("Escribe tu documento: ");
                string Documento = Console.ReadLine();

                Console.Write("Escribe tus nombres: ");
                string nombre = Console.ReadLine();

                Console.Write("Escribe tus apellidos: ");
                string apellidos = Console.ReadLine();

                int TipoPersona;

                do
                {
                    Console.WriteLine("Ingresa el ID del Tipo de Persona: ");
                    TipoPersona = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.TipoPersona.Any(tipo => Equals(tipo.TipoPersona_Id, TipoPersona));

                    if (!exists)
                    {
                        Logger.Error("No existen ID's para ese Tipo de Persona");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                    }

                } while (!exists);

                Console.Write("Escribe tu correo electrónico: ");
                string correo = Console.ReadLine();

                Console.Write("Escribe tu teléfono: ");
                string telefono = Console.ReadLine();

                Console.Write("Escribe tu dirección: ");
                string direccion = Console.ReadLine();

                int AseguradoraID;

                do
                {
                    Console.WriteLine("Ingresa el ID de la Aseguradora: ");
                    AseguradoraID = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Aseguradora.Any(aseg => Equals(aseg.Aseguraodra_Id, AseguradoraID));

                    if (!exists)
                    {
                        Logger.Error("No existen ID's para esa Aseguradora");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                    }

                } while (!exists);

                hospital.Persona.Add(new Persona()
                {
                    Persona_TipoDocumento = TipoDocumento,
                    Persona_Documento = Documento,
                    Persona_Nombre = nombre,
                    Persona_Apellido = apellidos,
                    Persona_TipoPersona = TipoPersona,
                    Persona_CorreoElectronico = correo,
                    Persona_Telefono = telefono,
                    Persona_Direccion = direccion,
                    Persona_IdUsuarioCreador = Program.loggerUserID,
                    Persona_IdAseguradora = AseguradoraID,
                    Persona_FechaCreacion = DateTime.Now,
                    Persona_Vigencia = true
                });

                Logger.Info($"Se ha creado la persona correctamente con el documento {Documento}");

                hospital.SaveChanges();
            }
            catch (Exception e)
            {
                Logger.Error(e, "Ha ocurrido un error inesperado");
                throw;
            }  
        }
        public static void Mostrar()
        {
            bool exists = false;
            string Documento;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            do
            {
                Console.Write("Escribe el documento de la persona a mostrar: ");
                Documento = Console.ReadLine();

                Console.Clear();

                exists = hospital.Persona.Any(pers => pers.Persona_Documento == Documento);

                if (!exists)
                {
                    Console.WriteLine("No existen personas con ese documento");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);

            Console.Clear();

            Persona persona = hospital.Persona
                            .Where(
                                per => per.Persona_Documento == Documento
                            )
                            .FirstOrDefault();

            MostrarInformacion(persona);
        }
        public static void MostrarTodos()
        {
            int index = 1;
            foreach (Persona persona in hospital.Persona.ToList())
            {
                Console.Clear();
                Console.WriteLine($"Persona: {index}");

                MostrarInformacion(persona);

                index++;
            }
        }
        public static void Actualizar()
        {
            bool exists = false;
            string Documento;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            do
            {
                Console.Write("Escribe el documento de la persona a actualizar: ");
                Documento = Console.ReadLine();

                Console.Clear();

                exists = hospital.Persona.Any(pers => pers.Persona_Documento == Documento);

                if (!exists)
                {
                    Console.WriteLine("No existen personas con ese documento");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);

            Console.Write("Escribe tus nombres (actualizado): ");
            string nombre = Console.ReadLine();

            Console.Write("Escribe tus apellidos (actualizado): ");
            string apellidos = Console.ReadLine();

            Console.Write("Escribe tu nuevo correo electrónico: ");
            string correo = Console.ReadLine();

            Console.Write("Escribe tu nuevo numero de teléfono: ");
            string telefono = Console.ReadLine();

            Console.Write("Escribe tu nueva dirección: ");
            string direccion = Console.ReadLine();

            int nuevaAseguradoraID;

            do
            {
                Console.WriteLine("Ingresa el ID de tu nueva Aseguradora: ");
                nuevaAseguradoraID = Int32.Parse(Console.ReadLine());

                Console.Clear();

                exists = hospital.Aseguradora.Any(aseg => Equals(aseg.Aseguraodra_Id, nuevaAseguradoraID));

                if (!exists)
                {
                    Logger.Error("No existen ID's para esa Aseguradora");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }

            } while (!exists);

            Persona nuevaPersona = hospital.Persona.Where(
                    pers => pers.Persona_Documento == Documento
                ).First();

            nuevaPersona.Persona_Nombre = nombre;
            nuevaPersona.Persona_Apellido = apellidos;
            nuevaPersona.Persona_CorreoElectronico = correo;
            nuevaPersona.Persona_Telefono = telefono;
            nuevaPersona.Persona_Direccion = direccion;
            nuevaPersona.Persona_IdAseguradora = nuevaAseguradoraID;
            nuevaPersona.Persona_Vigencia = true;

            Logger.Info($"La persona con el documento {Documento} ha sido actualizado.");

            hospital.SaveChanges();
        }
        public static void Eliminar()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                bool exists = false;
                string Documento;

                do
                {
                    Console.Write("Escribe el documento de la persona a eliminar: ");
                    Documento = Console.ReadLine();

                    Console.Clear();

                    exists = hospital.Persona.Any(pers => pers.Persona_Documento == Documento);

                    if (!exists)
                    {
                        Console.WriteLine("No existen personas con ese documento");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                hospital.Persona.Where(
                    pers => pers.Persona_Documento == Documento
                    ).First().Persona_Vigencia = false;

                hospital.SaveChanges();

                Logger.Info($"La persona con el documento {Documento} ha sido eliminado.");
            }
            catch (Exception e)
            {
                Logger.Error(e.Message, "Ha ocurrido un error inesperado");
                throw;
            }           
        }
    }
}
