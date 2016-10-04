using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public abstract class UdpBase
    {
        public UdpClient UdpClient { get; protected set; }

        protected UdpBase()
        {
            UdpClient = new UdpClient();
        }

        public async Task<StringMessage> Receive()
        {
            var result = await UdpClient.ReceiveAsync();

            return new StringMessage(
                Encoding.ASCII.GetString(result.Buffer, 0, result.Buffer.Length), 
                result.RemoteEndPoint);
        }
    }
}
