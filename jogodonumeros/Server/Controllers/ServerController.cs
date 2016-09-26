using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Core.Stores;
using Newtonsoft.Json;
using Server.Configs;

namespace Server.Controllers
{
    class ServerController
    {
        public ServerController()
        {
            GameStore.Instance.Game.PlayerList = new List<Player>();
            GameStore.Instance.Game.GameState = GameState.ConnectionClosed;
        }

        public void StartServer()
        {
            UdpClient udpClient = new UdpClient(ServerConfig.Port);
            GameStore.Instance.Game.GameState = GameState.ConnectionOpen;
            Console.WriteLine("Server Connections Oppened");
            while (true)
            {
                switch (GameStore.Instance.Game.GameState)
                {
                    case GameState.ConnectionClosed:
                        break;
                    case GameState.ConnectionOpen:
                        IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
                        byte[] receivedNetworkMessageBytes = udpClient.Receive(ref ipEndPoint);
                        string message = Encoding.Unicode.GetString(receivedNetworkMessageBytes);
                        NetworkMessage receivedNetworkMessage =
                            JsonConvert.DeserializeObject<NetworkMessage>(
                                Encoding.Unicode.GetString(receivedNetworkMessageBytes).ToString()
                                );

                        Player player = new Player()
                        {
                            Id = GameStore.Instance.Game.PlayerList.Count,
                            Port = ipEndPoint.Port,
                            PlayerAddress = ipEndPoint.Address.ToString(),
                            PlayerName = receivedNetworkMessage.Player.PlayerName
                        };
                        GameStore.Instance.Game.PlayerList.Add(player);
                        byte[] replyMessage = Encoding.Unicode.GetBytes(
                            JsonConvert.SerializeObject(
                                new NetworkMessage()
                                {
                                    Connected = true,
                                    Message = "Welcome to the game!",
                                    Player = player
                                })
                            );
                        udpClient.Send(replyMessage, replyMessage.Length, ipEndPoint);
                        if (GameStore.Instance.Game.PlayerList.Count == ServerConfig.MaxPlayers)
                        {
                            GameStore.Instance.Game.GameState = GameState.GameInitializing;
                        }
                        Console.WriteLine("Player"+ receivedNetworkMessage.Player.PlayerName + " entered the game");
                        Console.WriteLine("Players " + GameStore.Instance.Game.PlayerList.Count + "/" + ServerConfig.MaxPlayers);
                        break;
                    case GameState.GameInitializing:
                        Console.WriteLine("Game starting");
                        GameStore.Instance.Game.GameState = GameState.GameStarted;
          
                        break;
                    case GameState.GameStarted:


                        IPEndPoint ipEndPointGameStarted = new IPEndPoint(IPAddress.Any, 0);
                        byte[] receivedNetworkMessageBGameStarted = udpClient.Receive(ref ipEndPointGameStarted);
                        NetworkMessage receivedNetworkMessageGameStarted =
                           JsonConvert.DeserializeObject<NetworkMessage>(
                               Encoding.Unicode.GetString(receivedNetworkMessageBGameStarted).ToString()
                               );
                       

                        Console.WriteLine(receivedNetworkMessageGameStarted.Message +" / " + receivedNetworkMessageGameStarted.Player.PlayerName);
                        break;


                    case GameState.GameEnded:
                        
                        break;
                }
                
            }
        }
    }
}
