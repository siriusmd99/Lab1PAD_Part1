using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config
{
    public static class ConnectionInfo
    {
        public static string BrokerIP = "127.0.0.1";
        public static int BrokerPort = 1337;
        public static int BrokerConnectionsLimit = 10;
        public static int DataSizeLimit = 1024;
        public static int Sub_Conn_Delay = 1000;
    }
}
