using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Config.ConnectionInfo;
namespace Broker
{
    class Program
    {
        static void Main()
        {
            Thread thread = new Thread(StartBrokerListen);
            thread.Start();
            Console.ReadKey();
        }
        static void StartBrokerListen()
        {
            Broker broker = new Broker();
            broker.StartListen(BrokerIP, BrokerPort);
        }
    }
}
