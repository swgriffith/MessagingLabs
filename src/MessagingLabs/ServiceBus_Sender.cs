using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;


namespace MessagingLabs
{
    public static class ServiceBus_Sender
    {
        static string ServiceBusConnectionString;
        static string QueueName;
        static string TopicName;
        static IQueueClient queueClient;
        static ITopicClient topicClient;


        [FunctionName("ServiceBus_Sender")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            ServiceBusConnectionString = config["SERVICEBUSCONNSTR"];
            QueueName = config["QUEUENAME"];
            TopicName = config["TOPIC"];

            try
            {

            log.LogInformation("New Service Bus Message Recieved....");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string msg = data.msg;

            await SendMessagesAsync(msg, log);

                return (ActionResult)new OkObjectResult($"Msg written to svc bus: {msg} \n");

            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error: {ex.InnerException}");
            }

        }

        static async Task SendMessagesAsync(string msgBody, ILogger log)
        {
            try
            {
                queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
                topicClient = new TopicClient(ServiceBusConnectionString, TopicName);

                // Create a new message to send to the queue.
                string messageBody = $"Message: {msgBody}";
                var message = new Message(Encoding.UTF8.GetBytes(messageBody));

                // Send the message to the queue.
                await queueClient.SendAsync(message);
                // Send the message to the topic
                await topicClient.SendAsync(message);
            }
            catch (Exception ex)
            {
                log.LogInformation($"Message Send Error: {ex.InnerException}");
                throw ex;
            }
        }




        
    }
}
