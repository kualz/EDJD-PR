using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public abstract class UdpBase
    {
        private bool connected;
        public UdpClient UdpClient { get; protected set; }

        protected UdpBase()
        {
            UdpClient = new UdpClient();
        }

        public async Task<StringMessage> Receive()
        {
            try
            {
                var result = await UdpClient.ReceiveAsync();

                return new StringMessage(
                    Encoding.ASCII.GetString(result.Buffer, 0, result.Buffer.Length),
                    result.RemoteEndPoint);
            }
            catch (Exception e)
            {
                SocketException socketException = e as SocketException;

                if (socketException != null && socketException.SocketErrorCode == SocketError.ConnectionReset)
                {
                    return new StringMessage("player_disconnected",null);
                }

                Console.WriteLine(e);
                return new StringMessage("", null);
            }
            
        }
    }
}
