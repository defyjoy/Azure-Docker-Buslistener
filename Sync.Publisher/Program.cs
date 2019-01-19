using Microsoft.Azure.EventHubs;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sync.Publisher.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sync.Publisher
{
    class Program
    {
        private static readonly string EventHubConnectionString = "Endpoint=sb://hubsynctest.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=cq79u0oL47lFWFs2azBXwaeCg7g4hFa/8iztzruTQeg=";
        private static string EventHubName = "hubsync";
        private static EventHubClient eventHubClient;

        static async Task Main(string[] args)
        {
            // Creates an EventHubsConnectionStringBuilder object from the connection string, and sets the EntityPath.
            // Typically, the connection string should have the entity path in it, but this simple scenario
            // uses the connection string from the namespace.
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(EventHubConnectionString)
            {
                EntityPath = EventHubName
            };

            eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());

            await SendMessagesToEventHub();

            await eventHubClient.CloseAsync();

            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
        }

        private static async Task SendMessagesToEventHub()
        {
            string connectionString = "Server=tcp:testfrontsteps.database.windows.net,1433;Initial Catalog=hubsynctest;Persist Security Info=False;User ID=qa;Password=frontsteps@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            SyncContext context = new SyncContext(connectionString);


            List<Member> members = await context.Set<Member>().Include(x => x.Contacts).ToListAsync();
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            string message = JsonConvert.SerializeObject(members, settings);

            try
            {
                await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(message)));
            }
            catch (Exception)
            {
                throw;
            }
            Console.WriteLine($"{members.Count} messages sent.");
        }
    }
}
