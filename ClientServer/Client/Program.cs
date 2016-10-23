using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            bool myTurn = false;
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

            bool live = false;
            Task.Factory.StartNew(async () => {
                while (true)
                {
                    try
                    {
                        var received = await client.Receive();
                        Console.WriteLine(received.Data);
                        if (received.Data == "Live") live = true;
                        if (received.Data == "It is your turn to play.") myTurn = true;


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            });


            do
            {
                
                if (live && myTurn)
                {
                    string read = Console.ReadLine();
                    client.Send(read);
                    myTurn = false;
                }
                if (!myTurn && live && Console.ReadLine()!= "!") Console.WriteLine("Wait for your turn");
                   
            } while (true);

            client.Disconnect();
        }
    }
}
