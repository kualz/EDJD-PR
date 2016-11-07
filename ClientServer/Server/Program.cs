using Common;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Timers;

namespace Server
{
    class Program
    {
        static bool sendRemake = false;
        static void Main(string[] args)
        {
            string[] GameBoard = new string[]  { "1","2","3", "4", "5", "6" , "7", "8", "9" } ;
            string[] GamePieces = new string[] { "x", "0" };
            int pieceAux = 0, playerAux = 0;
            List<IPEndPoint> Players = new List<IPEndPoint>();
            bool Live = false;
            bool remake = false;
            Timer remakeTimer = new Timer();
            remakeTimer.Interval = 5000;
            remakeTimer.Elapsed += new ElapsedEventHandler(onTimedEvent);
            

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
                        server.Reply(new StringMessage("welcome, you are the player" + (pieceAux + 1) + " - " + GamePieces[pieceAux] +  "\n" + "Wait for the Live Signal.", received.Sender));
                        pieceAux++;
                    }
                    if (Live == false && Players.Count == 2)
                    {
                        Live = true;
                        foreach (var ipEnd in Players)
                        {
                            server.Reply(new StringMessage("Live",ipEnd));           
                                                       
                        }
                        server.Reply(new StringMessage("It is your turn to play.", Players[playerAux]));
                    }
                    if (Live)
                    {
                        if (received.Data == "1" || received.Data == "2" || received.Data == "3" || received.Data == "4" ||
                            received.Data == "5" || received.Data == "6" || received.Data == "7" || received.Data == "8" ||
                            received.Data == "9")
                        {
                            if (received.Sender.Equals(Players[0]))
                            {
                                GameBoard[int.Parse(received.Data) - 1] = "x";
                                foreach (var ip in Players)
                                {
                                    server.Reply(new StringMessage(received.Data + "|x", ip));
                                }
                            }
                            if (received.Sender.Equals(Players[1]))
                            {

                                GameBoard[int.Parse(received.Data) - 1] = "0";
                                foreach (var ip in Players)
                                {
                                    server.Reply(new StringMessage(received.Data + "|0", ip));
                                }
                            }
                            

                            //Check for Victory
                            if ((GameBoard[0] == GameBoard[1] && GameBoard[0] == GameBoard[2]) ||
                                (GameBoard[3] == GameBoard[4] && GameBoard[3] == GameBoard[5]) ||
                                (GameBoard[6] == GameBoard[7] && GameBoard[6] == GameBoard[8]) ||
                                (GameBoard[0] == GameBoard[3] && GameBoard[0] == GameBoard[6]) ||
                                (GameBoard[1] == GameBoard[4] && GameBoard[1] == GameBoard[7]) ||
                                (GameBoard[2] == GameBoard[5] && GameBoard[2] == GameBoard[8]) ||
                                (GameBoard[0] == GameBoard[4] && GameBoard[0] == GameBoard[8]) ||
                                (GameBoard[2] == GameBoard[4] && GameBoard[2] == GameBoard[6]))
                            {
                                foreach (var ip in Players)
                                {
                                    server.Reply(new StringMessage("Player " + (playerAux+1) + " - "+ GamePieces[playerAux] + " wins", ip));
                                }
                                remake = true;

                            }

                            playerAux++;
                            if (playerAux > 1) playerAux = 0;
                            server.Reply(new StringMessage("It is your turn to play.", Players[playerAux]));

                            //Check for Draw

                            //remake
                            if (remake)
                            {
                                GameBoard = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
                                foreach (var ip in Players)
                                {
                                    server.Reply(new StringMessage("Game Restarting...", ip));
                                }
                                remakeTimer.Enabled = true;
                            }

                        }
                        else
                        {
                            if (Players[playerAux].Equals( received.Sender))
                                server.Reply(new StringMessage("Invalid move. Try again.", received.Sender));
                        }
                    }



                    if (received.Data == "shutdown")
                        break;
                }
            });

            string read;
            do
            {
                if (sendRemake)
                {
                    sendRemake = false;
                    remake = false;
                    remakeTimer.Enabled = false;
                    foreach (var ip in Players)
                    {
                        server.Reply(new StringMessage("Remake", ip));
                    }
                }
            } while (true);




            server.Shutdown();
        }
        static void onTimedEvent(object source, ElapsedEventArgs e)
        {
            sendRemake = true;
        }
    }
}
