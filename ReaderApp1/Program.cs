using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.ServiceBus.Messaging;

namespace ReaderApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting");

            string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");

            QueueClient Client = QueueClient.CreateFromConnectionString(connectionString, "serversncodedemoqueue");

            OnMessageOptions options = new OnMessageOptions();
            options.AutoComplete = false;
            options.AutoRenewTimeout = TimeSpan.FromMinutes(1);

            Client.OnMessage((message) =>
            {
                try
                {
                    string sMessage = " UserCode: " + message.Properties["UserCode"];

                    Console.WriteLine("Found new User - " + sMessage);
                    

                    // Remove message from queue.
                    message.Complete();

                 
                }
                catch (Exception ex)
                {
                    // Indicates a problem, unlock message in queue.
                    message.Abandon();
                }
            }, options);

            Console.ReadKey();
        }
    }
}
