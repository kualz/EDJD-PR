using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Core.Stores;
using Server.Configs;
using Server.Controllers;

namespace Server
{
    class Server
    {
        static void Main(string[] args)
        {
            ServerController serverController = new ServerController();

            serverController.StartServer();
        }
    }
}
