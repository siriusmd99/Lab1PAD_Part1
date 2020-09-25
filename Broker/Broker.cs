using Config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static Config.ConnectionInfo;

namespace Broker
{
    class Broker
    {

        private Socket socket;
        List<Connection> connections = new List<Connection>();

        public Broker()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
        }
   
        public void StartListen(string ip, int port)
        {
            socket.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
            socket.Listen(BrokerConnectionsLimit);
            Accept();
        }

        private void Accept()
        {
            socket.BeginAccept(AcceptedCallBack, null);
            Console.WriteLine("Began to accept");
        }


        private void HandleData(byte[] data, int buffSize, Connection connection)
        {
            string str_data = Encoding.UTF8.GetString(data, 0, buffSize);
            
            Meme meme;
            try
            {
                JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                meme = JsonConvert.DeserializeObject<Meme>(str_data, jsonSerializerSettings);
                connection.ClientType = ClientType.Publisher;

                Console.WriteLine($"Incoming Meme (SubReddit: {meme.subreddit}) Received from Publisher: {str_data}\n");
                foreach (Connection con in connections)
                {
                   
                    if(con.ClientType == ClientType.Subscriber && con.SubRedit == meme.subreddit)
                    {
                        con.socket.Send(data, buffSize, SocketFlags.None);
                    }
                }
                Console.WriteLine("Meme sent to subscribers\n");


            }
            catch(Exception)
            {
                Console.WriteLine($"Incoming Topic from Subscriber: {str_data}\n");
                connection.ClientType = ClientType.Subscriber;
                connection.SubRedit = str_data;
        
            }
          


        }
        private void ReceiveCallBack(IAsyncResult asyncResult)
        {
            Connection connection = asyncResult.AsyncState as Connection;
            try
            {
                Socket senderSocket = connection.socket;
                int buffsize = senderSocket.EndReceive(asyncResult, out SocketError response);

                if (response == SocketError.Success)
                {
                    
                    HandleData(connection.Data, buffsize ,connection);

                }
            }
            catch (Exception)
            {
                Console.WriteLine("Could not receive the data from client !");
            }
            finally
            {
                try
                {
                    connection.socket.BeginReceive(connection.Data, 0, connection.Data.Length, SocketFlags.None, ReceiveCallBack, connection);
                }
                catch (Exception)
                {
                    connection.socket.Close();
                    connections.Remove(connection);
                }

            }

        }

        private void AcceptedCallBack(IAsyncResult asyncResult)
        {
            Connection connection = new Connection();
            connections.Add(connection);
            try
            {
                connection.socket = socket.EndAccept(asyncResult);
                connection.socket.BeginReceive(connection.Data, 0, connection.Data.Length, SocketFlags.None, ReceiveCallBack, connection);
            }
            catch (Exception)
            {
                
                Console.WriteLine("Couldn't accept connection.");

            }
            finally
            {
                Accept();
            }
        }

    }
}
