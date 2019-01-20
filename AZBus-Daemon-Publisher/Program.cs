using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Specialized;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AZBus_Daemon_Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }
        static async Task MainAsync()
        {
            await SendMessagesAsync();
            Console.ReadKey();
        }

        private async static Task SendMessagesAsync()
        {
            try
            {
                Console.WriteLine("============START SENDING MESSAGES ==========");

                IJobDetail job = JobBuilder.Create<PublishMessageJob>()
                   .WithIdentity("job1", "group1")
                   .Build();


                var trigger = TriggerBuilder.Create()
                  .WithIdentity("trigger7", "group1")
                  .WithSimpleSchedule(x => x
                      .WithIntervalInSeconds(3)
                      .RepeatForever())
                  //.EndAt(DateBuilder.DateOf(22, 0, 0))
                  .Build();

                NameValueCollection props = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" }
                };

                StdSchedulerFactory factory = new StdSchedulerFactory(props);
                IScheduler scheduler = await factory.GetScheduler();

                await scheduler.ScheduleJob(job, trigger);
                await scheduler.Start();
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }
    }
}
