using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Config;

namespace Publisher
{
    class Publisher
    {
        private Socket socket;

        public bool Connected { get { return socket.Connected; } }

        public Publisher()
        {

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
           
        }

        public void Close()
        {
            socket.Close();
        }

        public bool Connect(string ip, int port)
        {
            socket.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
            return socket.Connected;
        }

        public bool Send(string str_data)
        {
            var data = Encoding.UTF8.GetBytes(str_data);

            try
            {
                if (socket.Connected)
                {
                    socket.Send(data);
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");

            }
            
            return false;
            
        }

    }
}
