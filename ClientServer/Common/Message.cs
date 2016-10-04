using System.Net;

namespace Common
{
    public abstract class Message <T>
    {
        public IPEndPoint Sender { get; set; }
        public T Data { get; set; }
    }
}
