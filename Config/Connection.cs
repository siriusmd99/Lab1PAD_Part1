using System.Net.Sockets;

namespace Config
{
    public enum ClientType
    {
        Publisher,
        Subscriber
    }
    public class Connection
    {
        public Socket socket;
        public ClientType ClientType;
        public byte[] Data = new byte[Config.ConnectionInfo.DataSizeLimit];
        public string SubRedit;

    }
}
