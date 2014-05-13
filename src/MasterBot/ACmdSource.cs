using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterBot;

namespace MasterBot
{
    public abstract class ACmdSource : ICmdSource
    {
        protected IBot bot;

        public ACmdSource(IBot bot)
        {
            this.bot = bot;
        }

        public abstract void Reply(string message);
    }
}
