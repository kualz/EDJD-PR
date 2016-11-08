using System;
using System.Data;
using System.Diagnostics;
using System.Globalization;
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
            string[] GameBoard = new string[] {"1", "2", "3", "4", "5", "6", "7", "8", "9"};
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
            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    try
                    {
                        var received = await client.Receive();
                        //Console.WriteLine(received.Data);
                        if (received.Data == "Live") live = true;
                        if (received.Data == "It is your turn to play.") myTurn = true;
                        if (received.Data == "disconnect")
                        {
                            client.Disconnect();
                            Console.WriteLine("The other player left, Session closed.");
                        }
                        if (received.Data == "U there")
                            client.Send("yep");                     
                        if (received.Data == "Game Restarting...")
                        {
                            GameBoard = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
                        }
                        if (received.Data == "Remake")
                        {
                            Console.Clear();
                            GameBoard = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
                        }
                        if (received.Data == "Invalid move. Try again.") myTurn = true;
                        else
                        {
                            try
                            {
                                string[] move = received.Data.Split('|');
                                GameBoard[int.Parse(move[0]) - 1] = move[1];
                                Console.Clear();
                            }
                            catch (Exception)
                            {
                            }
                        }
                        if (live && received.Data != "It is your turn to play." && received.Data != "U there")
                        {
                            for (int x = 0; x < 9; x++)
                            {
                                Console.Write(GameBoard[x] + "|");

                                if (x == 2 || x == 5)
                                    Console.Write("\n");
                            }
                            Console.Write("\nLast Play:");
                        }
                        if (received.Data != "U there")
                                Console.WriteLine(received.Data);





                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            });

         
            do
            {
                if (live)
                {
                    if (myTurn)
                    {
                        
                        string read = Console.ReadLine();
                        if ((read == "1" || read == "2" || read == "3" || read == "4" ||
                            read == "5" || read == "6" || read == "7" || read == "8" ||
                            read == "9")&& GameBoard[int.Parse(read) - 1] != "0" && GameBoard[int.Parse(read) - 1] != "x")
                        {
                            client.Send(read);
                            myTurn = false;
                            Console.Clear();
                        }
                        else
                        {
                            Console.WriteLine("Invalid move");
                        }
                        
                    }
                    
                }

            } while (true);

            client.Disconnect();
        }
    }
}
