using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AZBus_Daemon_Publisher
{
    public class PublishMessageJob : IJob
    {
        private readonly string ServiceBusConnectionString = string.Empty;
        private readonly string Topic = string.Empty;
        private readonly ITopicClient topicClient;

        public PublishMessageJob()
        {
            IConfiguration config = new ConfigurationBuilder()
                   .AddJsonFile("appsettings.json", true, true)
                   .Build();

            ServiceBusConnectionString = config.GetSection("Azure:Bus-Endpoint").Value;
            Topic = config.GetSection("Azure:Topic").Value;

            topicClient = new TopicClient(ServiceBusConnectionString, Topic);
        }

        public async Task Execute(IJobExecutionContext context)
        {
            // Create a new message to send to the topic.
            string messageBody = $"Job Executed At :  {DateTime.Now.ToString("MMM dd yyyy h:mm:ss tt") }";
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));

            await topicClient.SendAsync(message);
            
            // Write the body of the message to the console.
            await Console.Out.WriteLineAsync($"{messageBody}");

            // Send the message to the topic.
        }
    }
}
