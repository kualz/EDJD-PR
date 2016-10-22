using Common;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] GameBoard = new string[]  { "1","2","3", "4", "5", "6" , "7", "8", "9" } ;
            string[] GamePieces = new string[] { "x", "0" };
            int pieceAux = 0;
            List<IPEndPoint> Players = new List<IPEndPoint>();

            var server = new Server(new IPEndPoint(IPAddress.Any, 32123));
            
            Task.Factory.StartNew(async () => {
                while (true)
                {
                    Console.Clear();
                   
                        for (int x = 0; x < 9; x++)
                        {
                            Console.Write(GameBoard[x] + "|");

                            if (x == 2 || x == 5)
                             Console.Write("\n");
                    }                 

                    var received = await server.Receive();
                    if (Players.Count > 2)
                    {
                        server.Reply(new StringMessage("Server is full!", received.Sender));
                    }
                    if (received.Data == "connected" && Players.Count < 2)
                    {
                        Players.Add(received.Sender);
                        server.Reply(new StringMessage("welcome, you are the player" + (pieceAux + 1) + " - " + GamePieces[pieceAux], received.Sender));
                        pieceAux++;
                    }
                if (received.Data == "1" || received.Data == "2" || received.Data == "3" || received.Data == "4" || received.Data == "5" || received.Data == "6" || received.Data == "7" || received.Data == "8" || received.Data == "9")
                    {
                    if (received.Sender.Equals(Players[0]))
                            GameBoard[Int32.Parse(received.Data) - 1] = "x";
                    if (received.Sender.Equals(Players[1]))
                            GameBoard[Int32.Parse(received.Data) - 1] = "0";

                    //Check for Victory
                        if ((GameBoard[0] == GameBoard[1] && GameBoard[0] == GameBoard[2]) || (GameBoard[3] == GameBoard[4] && GameBoard[3] == GameBoard[5]) || (GameBoard[6] == GameBoard[7] && GameBoard[6] == GameBoard[8]) ||
                   (GameBoard[0] == GameBoard[3] && GameBoard[0] == GameBoard[6]) || (GameBoard[1] == GameBoard[4] && GameBoard[1] == GameBoard[7]) || (GameBoard[2] == GameBoard[5] && GameBoard[2] == GameBoard[8]) ||
                   (GameBoard[0] == GameBoard[4] && GameBoard[0] == GameBoard[8]) || (GameBoard[2] == GameBoard[4] && GameBoard[2] == GameBoard[6]))
                        {
                            foreach (IPEndPoint ip in Players)
                            {
                                server.Reply(new StringMessage("Player coiso wins", ip));
                            }
                            
                        }

                    //Check for Draw



                    }
                    if (received.Data == "shutdown")
                        break;
                }
            });

            string read;
            do
            {
                read = Console.ReadLine();
            } while (read != "quit");

            server.Shutdown();
        }
    }
}
