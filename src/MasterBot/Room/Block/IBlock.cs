using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.Room.Block
{
    public interface IBlock
    {
        int Id { get; }
        int LayerId { get; }
        bool Background { get; }
    }
}
