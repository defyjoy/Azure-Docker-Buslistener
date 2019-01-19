using AZBus_Daemon_Listener.Config;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AZBus_Daemon_Listener
{
    public class MessageReceiverService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IOptions<MessageReceiverConfig> _config;
        private readonly IOptions<AppConfig> _appConfig;

        private readonly string ServiceBusConnectionString = "Endpoint=sb://sb-integration-dev-poc.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=+7SWoldUMoJBxnsJdjL+KllGMm6vN9uyWZiJnlUP0mM=";
        private readonly string Topic = "sb-publisher-topic";
        private readonly string Subscription = "sb-integration-subscription";

        private readonly ISubscriptionClient subscriptionClient;

        public MessageReceiverService(ILogger<MessageReceiverService> logger
            , IOptions<MessageReceiverConfig> config
            , IOptions<AppConfig> appConfig)
        {
            _logger = logger;
            _config = config;
            _appConfig = appConfig;
            subscriptionClient = new SubscriptionClient(ServiceBusConnectionString, Topic, Subscription);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Starting daemon: {_appConfig.Value.AzureConfig}");
            await SubscribeAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping daemon.");
            return Task.CompletedTask;
        }


        private async Task SubscribeAsync()
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

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }

        private async Task ProcessMessagesAsync(Message message, CancellationToken token)
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


        public void Dispose()
        {
            _logger.LogInformation("Disposing....");
        }
    }
}
