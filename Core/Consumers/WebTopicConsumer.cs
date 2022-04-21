using Core.DTO;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Consumers
{
    class WebTopicConsumer
    {
        public static WebTopicConsumer Instancia = null;
        static hospitalEntities hospital = new hospitalEntities();
        public static WebTopicConsumer GetInstance()
        {
            if (Instancia == null)
            {
                Instancia = new WebTopicConsumer();
            }

            return Instancia;
        }

        static SubscriptionClient subscriptionClient;
        private readonly CajaTopicConsumer _config;
        string serviceBusConnection = System.Configuration.ConfigurationManager.ConnectionStrings["AzureServiceBus"].ConnectionString;

        public WebTopicConsumer()
        {
            subscriptionClient = new SubscriptionClient(serviceBusConnection, "webtopic", "WebTopic-Core");
        }

        public async Task StartAsync()
        {
            Console.WriteLine("############## Starting Consumer - webtopic  ####################");
            ProcessMessageHandler();
            await Task.CompletedTask;
        }

        public async Task StopAsync()
        {
            Console.WriteLine("############## Stopping Consumer - webtopic ####################");
            await subscriptionClient.CloseAsync();
            await Task.CompletedTask;
        }

        public void ProcessMessageHandler()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            subscriptionClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        public async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {

            Console.WriteLine("### Processing Message - webtopic ###");
            Console.WriteLine($"{DateTime.Now}");
            Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");
            //  Mensajes _product = JsonConvert.DeserializeObject<Product>((message.Body).ToString());
            Values Values = JsonConvert.DeserializeObject<Values>((message.Body).ToString());

            switch (Values.EntidadId)
            {
                case 1:
                    AperturaYCierreDeCaja aperturaYCierre = JsonConvert.DeserializeObject<AperturaYCierreDeCaja>((message.Body).ToString());
                    hospital.AperturaYCierreDeCaja.Add(aperturaYCierre);
                    //LOG
                    break;
                case 2:
                    Aseguradora Aseguradora = JsonConvert.DeserializeObject<Aseguradora>((message.Body).ToString());
                    hospital.Aseguradora.Add(Aseguradora);
                    //LOG
                    break;
                case 3:

                    Autorizacion Autorizacion = JsonConvert.DeserializeObject<Autorizacion>((message.Body).ToString());
                    hospital.Autorizacion.Add(Autorizacion);
                    //LOG
                    break;
                case 4:
                    Caja caja = JsonConvert.DeserializeObject<Caja>((message.Body).ToString());
                    hospital.Caja.Add(caja);
                    //LOG
                    break;
                case 5:
                    Caja_Usuario cajaUsuario = JsonConvert.DeserializeObject<Caja_Usuario>((message.Body).ToString());
                    hospital.Caja_Usuario.Add(cajaUsuario);
                    //LOG
                    break;
                case 6:
                    Cuentas cuenta = JsonConvert.DeserializeObject<Cuentas>((message.Body).ToString());
                    hospital.Cuentas.Add(cuenta);
                    //LOG
                    break;
                case 7:
                    Cuenta_Factura cuentaFactura = JsonConvert.DeserializeObject<Cuenta_Factura>((message.Body).ToString());
                    hospital.Cuenta_Factura.Add(cuentaFactura);
                    //LOG
                    break;
                case 8:
                    Egreso egreso = JsonConvert.DeserializeObject<Egreso>((message.Body).ToString());
                    hospital.Egreso.Add(egreso);
                    //LOG
                    break;
                case 9:
                    FacturaDetalle factura = JsonConvert.DeserializeObject<FacturaDetalle>((message.Body).ToString());
                    hospital.FacturaDetalle.Add(factura);
                    //LOG
                    break;
                case 10:
                    FacturaEncabezado facturaEncabezado = JsonConvert.DeserializeObject<FacturaEncabezado>((message.Body).ToString());
                    hospital.FacturaEncabezado.Add(facturaEncabezado);
                    //LOG
                    break;
                case 11:
                    Pago pago = JsonConvert.DeserializeObject<Pago>((message.Body).ToString());
                    hospital.Pago.Add(pago);
                    //LOG
                    break;
                case 12:
                    Perfil perfil = JsonConvert.DeserializeObject<Perfil>((message.Body).ToString());
                    hospital.Perfil.Add(perfil);
                    //LOG
                    break;
                case 13:
                    Persona persona = JsonConvert.DeserializeObject<Persona>((message.Body).ToString());
                    hospital.Persona.Add(persona);
                    //LOG
                    break;
                case 14:
                    PlanDeTratamiento planTratamiento = JsonConvert.DeserializeObject<PlanDeTratamiento>((message.Body).ToString());
                    hospital.PlanDeTratamiento.Add(planTratamiento);
                    //LOG
                    break;
                case 15:
                    ProcesoMedico procesoMedico = JsonConvert.DeserializeObject<ProcesoMedico>((message.Body).ToString());
                    hospital.ProcesoMedico.Add(procesoMedico);
                    //LOG
                    break;
                case 16:
                    ReciboIngreso reciboIngreso = JsonConvert.DeserializeObject<ReciboIngreso>((message.Body).ToString());
                    hospital.ReciboIngreso.Add(reciboIngreso);
                    //LOG
                    break;
                case 17:
                    TipoDocumento tipoDocumento = JsonConvert.DeserializeObject<TipoDocumento>((message.Body).ToString());
                    hospital.TipoDocumento.Add(tipoDocumento);
                    //LOG
                    break;
                case 18:
                    TipoPago tipoPago = JsonConvert.DeserializeObject<TipoPago>((message.Body).ToString());
                    hospital.TipoPago.Add(tipoPago);
                    //LOG
                    break;
                case 19:
                    TipoPersona tipoPersona = JsonConvert.DeserializeObject<TipoPersona>((message.Body).ToString());
                    hospital.TipoPersona.Add(tipoPersona);
                    //LOG
                    break;
                case 20:
                    TipoProceso tipoProceso = JsonConvert.DeserializeObject<TipoProceso>((message.Body).ToString());
                    hospital.TipoProceso.Add(tipoProceso);
                    //LOG
                    break;
                case 21:
                    Usuarios Usuarios = JsonConvert.DeserializeObject<Usuarios>((message.Body).ToString());
                    hospital.Usuarios.Add(Usuarios);
                    //LOG
                    break;
            }
            await subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        public Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            //LOG
            return Task.CompletedTask;
        }

    }
}
