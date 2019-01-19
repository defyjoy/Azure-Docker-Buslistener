using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using AZBus_Daemon_Listener.Config;

namespace AZBus_Daemon_Listener
{
    class Program
    {
        private readonly static string ServiceBusConnectionString = string.Empty;
        private readonly static string Topic = string.Empty;
        private readonly static string Subscription = string.Empty;

        private readonly static ISubscriptionClient subscriptionClient;

        static Program()
        {
            IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();
            //ServiceBusConnectionString = config.GetSection("Azure:BusEndpoint").Value;
            //Topic = config.GetSection("Azure:Topic").Value;
            //Subscription = config.GetSection("Azure:Subscription").Value;

            //subscriptionClient = new SubscriptionClient(ServiceBusConnectionString, Topic, Subscription);
        }

        public static async Task Main(string[] args)
        {

            var builder = new HostBuilder()
                 .ConfigureAppConfiguration((hostingContext, config) =>
                 {
                     config.AddEnvironmentVariables();

                     if (args != null)
                     {
                         config.AddCommandLine(args);
                     }
                 })
                 .ConfigureServices((hostContext, services) =>
                 {
                     services.AddOptions();
                     services.Configure<MessageReceiverConfig>(hostContext.Configuration.GetSection("Daemon"));
                     services.AddSingleton<IHostedService, MessageReceiverService>();
                     services.Configure<AppConfig>(hostContext.Configuration);
                 })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                });

            await builder.RunConsoleAsync();
        }

        static async Task MainAsync()
        {
            Console.WriteLine("============START LISTENING TO INCOMING MESSAGES ==========");
            await SubscribeAsync();

        }

        private static async Task SubscribeAsync()
        {
            // Configure the message handler options in terms of exception handling, number of concurrent messages to deliver, etc.
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                // Maximum number of concurrent calls to the callback ProcessMessagesAsync(), set to 1 for simplicity.
                // Set it according to how many messages the application wants to process in parallel.
                MaxConcurrentCalls = 1,

                // Indicates whether MessagePump should automatically complete the messages after returning from User Callback.
                // False below indicates the Complete will be handled by the User Callback as in `ProcessMessagesAsync` below.
                AutoComplete = false
            };

            // Register the function that processes messages.
            subscriptionClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }
        static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            // Process the message.
            Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");

            // Complete the message so that it is not received again.
            // This can be done only if the subscriptionClient is created in ReceiveMode.PeekLock mode (which is the default).
            await subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);

            // Note: Use the cancellationToken passed as necessary to determine if the subscriptionClient has already been closed.
            // If subscriptionClient has already been closed, you can choose to not call CompleteAsync() or AbandonAsync() etc.
            // to avoid unnecessary exceptions.
        }

        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }
    }
}
