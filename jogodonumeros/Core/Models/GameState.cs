using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public enum GameState
    {
        ConnectionOpen,
        ConnectionClosed,
        GameInitializing,
        GameStarted,
        GameEnded
    }
}
