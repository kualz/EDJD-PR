using Common;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class Server : UdpBase
    {
        public IPEndPoint Address { get; private set; }

        public Server() : base()
        {
            Address = new IPEndPoint(IPAddress.Any, 32123);
        }

        public Server(IPEndPoint endpoint)
        {
            Address = endpoint;
            UdpClient = new UdpClient(Address);
        }

        public void Reply(StringMessage message)
        {
            var data = Encoding.ASCII.GetBytes(message.Data);
            UdpClient.Send(data, data.Length, message.Sender);
        }

        public void Shutdown()
        {
            UdpClient.Close();
        }
    }
}
