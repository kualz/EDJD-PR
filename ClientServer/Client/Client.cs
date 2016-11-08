using System;
using Common;
using System.Net.Sockets;
using System.Text;


namespace Client
{
    public class Client : UdpBase
    {
        public static Client Connect(string hostname, int port)
        {
            Client connection = new Client();
            connection.UdpClient.Connect(hostname, port);
            return connection;
        }

        public void Send(string message)
        {
            try
            {
                var data = Encoding.ASCII.GetBytes(message);
                UdpClient.Send(data, data.Length);
            }
            catch (Exception e) { Console.WriteLine(e.Message); }

        }

        public void Disconnect()
        {
            UdpClient.Close();
        }
    }
}
