using ClientWPFInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientWPFInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string ipServ { get; set; } = "127.0.0.1";
        public int portServ { get; set; } = 2945;
        private Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private StringBuilder builder { get; set; } = new StringBuilder();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new WindowModel();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            portServ = new Random().Next(1000, 9999);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ipServ), portServ);
            socket.Bind(endPoint);
            socket.Connect(IPAddress.Parse("127.0.0.1"), 2314);
            Task.Run(() => SocketHandler());
        }

        private void SocketHandler() 
        {
                while (true)
                {
                    byte[] bytes = new byte[256];
                    int countBytes = 0;
                    do
                    {
                        countBytes = socket.Receive(bytes);
                        builder.Append(Encoding.UTF8.GetString(bytes));
                    } while (socket.Available > 0);

                    Dispatcher.Invoke(() => { 
                        txtChat.Text = builder.ToString();
                    });
                    builder.AppendLine();
                }
           
        }

        private void SocketSend() 
        {
            bool isValidName = false;
            string nikName = null;
            Dispatcher.Invoke(() => {
                isValidName = !string.IsNullOrEmpty(this.txtName.Text);
                nikName = this.txtName.Text;
            });   
            if (isValidName)
            {

                bool isEmpty = true;
                string message = String.Empty;
                Dispatcher.Invoke(() =>
                {
                    isEmpty = string.IsNullOrEmpty(this.txtBoxChat.Text);
                    message = this.txtBoxChat.Text.Trim();
                });
                if (!isEmpty)
                {
                    lock (socket) 
                    {
                        socket.Send(Encoding.UTF8.GetBytes(nikName + ": " + message));
                        Dispatcher.Invoke(() =>
                        {
                            this.txtBoxChat.Text = "";
                        });
                    }


                }
            }
            else 
            {
                MessageBox.Show("Ведіть нік!");
            }
        }

        private async void btnSend_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => SocketSend());
        }
    }
}
