using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Config.ConnectionInfo;
using Config;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace Subscriber
{
  
        class Subscriber
        {
            private Socket socket = null;
            private Form1 main_form;
            public string subreddit;
           

            public Subscriber(string subreddit, Form1 main_form)
            {
                this.subreddit = subreddit;
                this.main_form = main_form;
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
            public void Connect(string ipAddress, int port)
            {
               if(socket == null)
                   socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
               
                socket.BeginConnect(new IPEndPoint(IPAddress.Parse(ipAddress), port), ConnectedCallback, null);

            }
            private void ConnectedCallback(IAsyncResult asyncResult)
            {
                if (socket.Connected)
                {
                    Subscribe();
                    StartReceive();
                }
                else
                {
                socket.Close();
                socket = null;
                    Thread.Sleep(Sub_Conn_Delay);
                    Connect(BrokerIP, BrokerPort);
                }
            }

            public void Subscribe(string subreddit = null)
            {
                if(subreddit != null)
                    this.subreddit = subreddit;
                Send(Encoding.UTF8.GetBytes(this.subreddit));

            }
            private void StartReceive()
            {
                Connection connection = new Connection();
                connection.socket = socket;
               
        
                socket.BeginReceive(connection.Data, 0, connection.Data.Length, SocketFlags.None, ReceiveCallback, connection); 
            }
            private void ReceiveCallback(IAsyncResult asyncResult)
            {
                Connection connection = asyncResult.AsyncState as Connection;
                try
                {
                    int buffSize = socket.EndReceive(asyncResult, out SocketError response);

                    if (response == SocketError.Success)
                    {
                        string str_data = Encoding.UTF8.GetString(connection.Data,0, buffSize);

                   
                        JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
                        {
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };

                    
                        main_form.UpdateMeme(JsonConvert.DeserializeObject<Meme>(str_data, jsonSerializerSettings));

                    }
                }
                catch (Exception)
                {}
                finally
                {
                    try
                    {
                        connection.socket.BeginReceive(connection.Data, 0, connection.Data.Length, SocketFlags.None, ReceiveCallback, connection);

                    }
                    catch (Exception )
                    {
                        connection.socket.Close();
                        socket.Close();
                        socket = null;
                        Thread.Sleep(Sub_Conn_Delay);
                        Connect(BrokerIP, BrokerPort);
                    }
                }
            }

            private void Send(byte[] data)
            {
                try
                {
                    socket.Send(data);
                }
                catch (Exception )
                {
                   
                }
             }
        }
}
