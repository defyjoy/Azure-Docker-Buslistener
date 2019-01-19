using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AZBus_Daemon_Publisher
{
    class Program
    {
        private readonly static string ServiceBusConnectionString = string.Empty;
        private readonly static string Topic = string.Empty;
        private readonly static ITopicClient topicClient;

        static Program()
        {
            IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();

            ServiceBusConnectionString = config.GetSection("Azure:Bus-Endpoint").Value;
            Topic = config.GetSection("Azure:Topic").Value;

            topicClient = new TopicClient(ServiceBusConnectionString, Topic);

        }
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }
        static async Task MainAsync()
        {
            await SendMessagesAsync();
            Console.ReadKey();
            await topicClient.CloseAsync();
        }

        private async static Task SendMessagesAsync()
        {
            try
            {
                Console.WriteLine("============START SENDING MESSAGES ==========");
                int i = 0;
                do
                {
                    while (!Console.KeyAvailable)
                    {
                         // Create a new message to send to the topic.
                            string messageBody = $"Message {++i}";
                            var message = new Message(Encoding.UTF8.GetBytes(messageBody));

                            // Write the body of the message to the console.
                            Console.WriteLine($"Sending message: {messageBody}");

                            // Send the message to the topic.
                            await topicClient.SendAsync(message);
                            Thread.Sleep(3000);
                        
                    }
                } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }
    }
}
