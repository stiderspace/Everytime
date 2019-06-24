using Smod2.API;
using System;

namespace EveryTime
{
    public class PlayerData
    {
  
        public DateTime loginTime { get; }
        public Player player { get; }


        public PlayerData(Player player, DateTime loginTime)
        {
            this.player = player;
            this.loginTime = loginTime;
        }
    }
}