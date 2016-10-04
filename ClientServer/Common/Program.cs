using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = Client.Connect("127.0.0.1", 32123);
            
            Task.Factory.StartNew(async () => {
                while (true)
                {
                    try
                    {
                        var received = await client.Receive();
                        Console.WriteLine("Server Message: " + received.Data);
                    }
                    catch (Exception ex)
                    {
                        Debug.Write(ex);
                    }
                }
            });

            string read;
            do
            {
                read = Console.ReadLine();
                client.Send(read);
            } while (read != "quit");

            client.Disconnect();
        }
    }
}
