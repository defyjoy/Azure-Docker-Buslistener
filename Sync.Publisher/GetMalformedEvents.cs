using System;
using System.Text;
using Microsoft.Azure.EventHubs;
//using Microsoft.Azure.ServiceBus.
namespace Sync.Publisher
{
    class GetMalformedEvents
    {
        // IoT connection string looks like "HostName=xxxxx.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=xxxxxx";
        // EH connection string looks like "Endpoint=sb://[namespace].servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[key]";
        static string connectionString;
        private static readonly string EventHubConnectionString = "Endpoint=sb://hubsynctest.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=cq79u0oL47lFWFs2azBXwaeCg7g4hFa/8iztzruTQeg=";

        private static string EventHubName = "hubsync";
        private static string partitionId;
        static string iotHubD2cEndpoint = "messages/events";
        static EventHubClient eventHubClient;

        //static void Main(string[] args)

        //{
        //    Console.Write("Enter the connection string:");
        //    connectionString = Console.ReadLine();

        //    Console.Write("Enter the partition Id:");
        //    partitionId = Console.ReadLine();

        //    Console.Write("Enter the offset number:");
        //    long offset;
        //    if (long.TryParse(Console.ReadLine(), out offset) == false)
        //    {
        //        Console.Write("Enter a valid offset value.");
        //        Console.ReadLine();
        //        return;
        //    }

        //    // Uncomment the following line if you are using event hub 
        //    // eventHubClient = EventHubClient.CreateFromConnectionString("your_connection_string");

        //    // Use the following for IoT hub 
        //    var connectionStringBuilder = new EventHubsConnectionStringBuilder(EventHubConnectionString)
        //    {
        //        EntityPath = EventHubName
        //    };

        //    // Number of events is set to 1000 in this example, you can choose to use a different value. 
        //    PrintMessages(partitionId, offset, 1000);

        //    Console.WriteLine("Done");
        //    Console.ReadLine();

        //}

        static void PrintMessages(string partitionId, long offset, int numberOfEvents)
        {
            //EventHubReceiver receiver;

            //try
            //{
            //    receiver = eventHubClient.GetDefaultConsumerGroup().CreateReceiver(partitionId, offset.ToString(), true);
            //}

            //catch (ArgumentException)
            //{
            //    Console.WriteLine("No data for the specified offset in this partition.");
            //    return;
            //}

            //try
            //{
            //    foreach (var e in receiver.Receive(numberOfEvents, TimeSpan.FromMinutes(1)))
            //    {
            //        Console.WriteLine(Encoding.UTF8.GetString(e.GetBytes()));
            //        Console.WriteLine("----");
            //    }
            //}

            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.ToString());
            //    return;
            //}

            //receiver.Close();
        }
    }
}