using Core.DTO;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Controllers
{
    internal class UserRegister
    {
        public static UserRegister Instancia = null;
        public static UserRegister GetInstance()
        {
            if (Instancia == null)
            {
                Instancia = new UserRegister();
            }

            return Instancia;
        }

        public async Task Crear(Usuarios usuario)
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                UsuarioEntities usuarioEntities = new UsuarioEntities()
                {
                    UsuarioNickname = usuario.Usuario_Nickname,
                    UsuarioContraseña = usuario.Usuario_Contraseña,
                    UsuarioFechaCreacion = usuario.Usuario_FechaCreacion,
                    UsuarioVigencia = true,
                    EntidadId = 21
                };

                await SendMessageQueue(usuarioEntities);
                Logger.Info($"El usuario {usuarioEntities.UsuarioNickname} se ha enviado correctamente");

            }
            catch (Exception e)
            {
                Logger.Error(e, "Ha ocurrido un error inesperado");
                throw;
            }
        }

        #region INTEGRACION
        private async Task SendMessageQueue(UsuarioEntities UsuarioEntities)
        {

            string queueName = "core";
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["AzureServiceBus"].ConnectionString;
            var client = new QueueClient(connectionString, queueName, ReceiveMode.PeekLock);
            string messageBody = JsonConvert.SerializeObject(UsuarioEntities);
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));

            await client.SendAsync(message);
            await client.CloseAsync();
        }
        #endregion
    }
}
