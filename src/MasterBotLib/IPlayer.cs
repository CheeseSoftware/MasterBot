using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot
{
    public interface IPlayer : ICmdSource
    {
        #region IPlayer
        int Id { get; }

        void Send(string message);

        void SetMetadata(string key, object value);

        bool HasMetadata(string key);

        void RemoveMetadata(string key);

        object GetMetadata(string key);
        #endregion

        #region PhysicsPlayer
        int BlockX { get; set; }
        int BlockY { get; set; }
        double X { get; set; }
        double Y { get; set; }
        double SpeedX { get; set; }
        double SpeedY { get; set; }
        double ModifierX { get; set; }
        double ModifierY { get; set; }
        int Horizontal { get; set; }
        int Vertical { get; set; }
        int Coins { get; set; }
        bool Purple { get; set; }
        bool Levitation { get; set; }
        bool HasCrown { get; set; }
        bool HasCrownSilver { get; set; }
        int Smiley { get; set; }
        bool IsGod { get; set; }
        bool IsMod { get; set; }
        bool IsClubMember { get; set; }
        int Level { get; set; }
        bool Moved { get; }
        string Name  { get; set; }
        int OldBlockY { get; }
        int OldBlockX { get; }

        void tick();
        #endregion


    }
}
