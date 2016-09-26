using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Controllers;

namespace Client
{
    class Client
    {
        static void Main(string[] args)
        {
            ClientController clientController = new ClientController();
            clientController.StartClient();
        }
    }
}
