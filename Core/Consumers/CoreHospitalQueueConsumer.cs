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
    class CoreHospitalQueueConsumer
    {
        public static CoreHospitalQueueConsumer Instancia = null;
        static hospitalEntities hospital = new hospitalEntities();
        public static CoreHospitalQueueConsumer GetInstance()
        {
            if (Instancia == null)
            {
                Instancia = new CoreHospitalQueueConsumer();
            }

            return Instancia;
        }

        static IQueueClient queueClient;
        private readonly CoreHospitalQueueConsumer _config;
        string serviceBusConnection = System.Configuration.ConfigurationManager.ConnectionStrings["AzureServiceBus"].ConnectionString;

        public CoreHospitalQueueConsumer()
        {
            queueClient = new QueueClient(serviceBusConnection, "corehospitalqueue");
        }

        public Task StartAsync()
        {
            ProcessMessageHandler();
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await queueClient.CloseAsync();
            await Task.CompletedTask;
        }

        private void ProcessMessageHandler()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        private async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            PlanDeTratamiento planTratamiento = JsonConvert.DeserializeObject<PlanDeTratamiento>((message.Body).ToString());
            hospital.PlanDeTratamiento.Add(planTratamiento);
            //LOG

            await queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }


        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            var Logger = NLog.LogManager.GetCurrentClassLogger();
            Logger.Info($"- Endpoint: {context.Endpoint}  - Entity Path: { context.EntityPath} Executing Action: {context.Action} Message handler encountered an exception {exceptionReceivedEventArgs.Exception}");
            return Task.CompletedTask;
        }
    }
}
