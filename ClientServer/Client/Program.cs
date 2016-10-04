using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client();
            try
            {
                client = Client.Connect("127.0.0.1", 32123);
                client.Send("connected");
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }
           
            
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
                        Console.WriteLine(ex);
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
