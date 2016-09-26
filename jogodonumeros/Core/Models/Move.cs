using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Move
    {
        public int Id { get; set; }
        public Player TargetPlayer { get; set; }
        public DateTime MoveDateTime { get; set; }
        public int PlayedNumber { get; set; }
    }
}
