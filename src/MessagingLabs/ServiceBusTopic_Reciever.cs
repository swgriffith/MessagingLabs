using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace MessagingLabs
{
    public static class ServiceBusTopic_Reciever
    {
        [FunctionName("ServiceBusTopic_Reciever")]
        public static void Run([ServiceBusTrigger("%TOPIC%", "Subscriber1", Connection = "SERVICEBUSCONNSTR")]string mySbMsg, ILogger log)
        {
            log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
        }
    }
}
