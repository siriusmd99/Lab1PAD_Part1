using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Config.ConnectionInfo;
using static Config.Service;
namespace Publisher
{
    class Program
    {
        static int TryConnectionDelay = 2000;

        static void Main()
        {
            Thread thread = new Thread(StartConnection);
            thread.Start();
            Console.ReadKey();
        }

        static void StartConnection()
        {
        
            Publisher publisher = new Publisher();


            Connect(publisher);



            using (WebClient client = new WebClient())
            {
                while(true)
                {
                    try
                    {
                        string json = client.DownloadString(API_URL);
                        Console.WriteLine($"Sent {json}\n\n");
                        publisher.Send(json);
                    }catch
                    {
                        Console.WriteLine("Connection to Socket Disrupted, retrying connection...");
                        publisher.Close();
                        Connect(publisher);
                    }
                    Thread.Sleep(Sleep_Delay);
                }
               
            }
           
        }

        static void Connect(Publisher pub)
        {
            while (!pub.Connect(BrokerIP, BrokerPort))
            {
                Console.WriteLine($"Could not connect to Broker, retrying in {TryConnectionDelay / 1000} seconds.");
                Thread.Sleep(TryConnectionDelay);
            }
            Console.WriteLine("Successfully Connected to Broker");
        }




    }
}
