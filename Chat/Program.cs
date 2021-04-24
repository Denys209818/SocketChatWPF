using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chat
{
    class Program
    {
        private static List<Socket> adresses = new List<Socket>();
        static void Main()
        {
            Console.OutputEncoding = Encoding.Default;
            Console.InputEncoding = Encoding.Default;

            string ip = "127.0.0.1";
            int port = 2314;
            try
            {

                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                socket.Bind(endPoint);
                Console.WriteLine("Сервер запущений...");

                socket.Listen(10);

                while (true)
                {
                    Socket handler = socket.Accept();


                    Task.Run(() => ClientHandlerAsync(handler));
                }
            } catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void ClientHandlerAsync(Socket handler) 
        {
            if (!adresses.Contains(handler)) 
            {
                adresses.Add(handler);
            }
            while (handler.Connected) 
            {
                StringBuilder builder = new StringBuilder();
                byte[] bytes = new byte[256];
                int countByte = 0;

                do
                {
                    countByte = handler.Receive(bytes);
                    builder.Append(Encoding.UTF8.GetString(bytes));
                } while (handler.Available > 0);

                Console.WriteLine("[userIp]: " + handler.RemoteEndPoint.ToString() + "\nMessage: " + builder.ToString());

                foreach (var item in adresses.Where(x => x.Connected))
                {
                    item.Send(Encoding.UTF8.GetBytes(builder.ToString()));
                }
                
            }

        }
    }
}
