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
            string[] GameBoard = new string[]  { "_","_","_", "_", "_", "_" , "_", "_", "_" } ;
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

                    if (received.Data == "connected")
                    {
                        Players.Add(received.Sender);
                        server.Reply(new StringMessage("welcome, you are the player" + (pieceAux + 1) + " - " + GamePieces[pieceAux],received.Sender));
                    } 
                    //Console.WriteLine(received.Sender + ": " + received.Data);                 
                                   
                               
                    //GameBoard[Int32.Parse(received.Data) - 1] = "0";            



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
