using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Core.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string PlayerName { get; set; }
        public string PlayerAddress { get; set; }
        public int Port { get; set; }
    }
}
