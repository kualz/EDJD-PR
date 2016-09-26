using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Stores
{
    public sealed class PlayerStore
    {
        private static PlayerStore _instance;
        public Player Player { get; set; }

        private PlayerStore()
        {
            Player = new Player();
        }

        public static PlayerStore Instance => _instance ?? (_instance = new PlayerStore());
    }
}
