using System.Net;

namespace Common
{
    public class StringMessage : Message<string>
    {
        public StringMessage(string data, IPEndPoint sender)
        {
            Data = data;
            Sender = sender;
        }
    }
}
