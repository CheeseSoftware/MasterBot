using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.Room.Block
{
    public interface IBlock
    {
        int Id { get; }
        int Layer { get; }
        bool Background { get; }
        DateTime DatePlaced { get; set; }
        double TimeSincePlaced { get; }
        double TimeSinceSent { get; }
        Color Color { get; }
        IPlayer Placer { get; set; }
        bool Placed { get; }
        int TimesSent { get; }

        void PlaceNormally(IBot bot, int x, int y);
        void OnSend(IBot bot, int x, int y);
        void OnReceive(IBot bot, int x, int y);
    }
}
