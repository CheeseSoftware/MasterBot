using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.SubBot.Houses
{
    public class HousePlayer
    {
        IPlayer player;
        HashSet<string> trustedPlayers = new HashSet<string>();

        public HousePlayer(IPlayer player)
        {
            this.player = player;
        }

        public static HousePlayer Get(IPlayer player)
        {
            if (player.HasMetadata("HousePlayer"))
                return player.GetMetadata("HousePlayer") as HousePlayer;

            HousePlayer housePlayer = new HousePlayer(player);
            player.SetMetadata("HousePlayer", housePlayer);

            return housePlayer;
        }

        public void Trust(string playerName)
        {
            if (!trustedPlayers.Contains(playerName.ToLower()))
                trustedPlayers.Add(playerName.ToLower());
        }

        public void Untrust(string playerName)
        {
            if (trustedPlayers.Contains(playerName.ToLower()))
                trustedPlayers.Remove(playerName.ToLower());
        }

        public bool IsTrusted(string playerName)
        {
            return trustedPlayers.Contains(playerName.ToLower());
        }

        public void PrintTrusted()
        {
            string text = "You trust ";

            if (trustedPlayers.Count == 0)
                text += "nobody!";

            foreach(string str in trustedPlayers)
            {
                if (str == trustedPlayers.First())
                    text += str;
                else if (str == trustedPlayers.Last())
                    text += " and " + str + ".";
                else
                    text += ", " + str;
            }

            player.Reply(text);
        }
    }
}
