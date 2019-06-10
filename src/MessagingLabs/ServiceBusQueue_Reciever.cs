using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace MessagingLabs
{
    public static class ServiceBusQueue_Reciever
    {


        [FunctionName("ServiceBusQueue_Reciever")]
        public static void Run([ServiceBusTrigger("%QUEUENAME%", Connection = "SERVICEBUSCONNSTR")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        }
    }
}
