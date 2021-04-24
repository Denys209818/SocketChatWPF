using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient
{
    class Program
    {
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
                socket.Connect(endPoint);

                Console.WriteLine("Ведіть повідомлення: ");

                string message;

                message = Console.ReadLine();

                socket.Send(Encoding.UTF8.GetBytes(message));
                Console.WriteLine("Повідомлення відправлено!");
            } 
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }

            Console.Read();
        }
    }
}
