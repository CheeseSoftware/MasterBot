using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot
{
    public class PlayerPhysics
    {
        Stopwatch playerTickTimer = new Stopwatch();

        public PlayerPhysics()
            : base()
        {
            playerTickTimer.Start();
        }

        public override void onMessage(object sender, PlayerIOClient.Message m)
        {
            //throw new NotImplementedException();
        }

        public override void onDisconnect(object sender, string reason)
        {
            //throw new NotImplementedException();
        }

        public override void onCommand(object sender, string text, string[] args, int userId, Player player, string name, bool isBotMod)
        {
            //throw new NotImplementedException();
        }

        public override void Update()
        {
            if (OstBot.connected)
            {
                if (playerTickTimer.ElapsedMilliseconds >= Config.physics_ms_per_tick)
                {
                    playerTickTimer.Restart();
                    lock (OstBot.playerList)
                    {
                        foreach (Player player in OstBot.playerList.Values)
                        {
                            player.tick();
                        }
                    }
                }
            }
        }
    }
}
