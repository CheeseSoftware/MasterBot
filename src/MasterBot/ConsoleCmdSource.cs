using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot
{
    class ConsoleCmdSource : ACmdSource
    {
        public ConsoleCmdSource(IBot bot)
            : base(bot)
        {

        }

        public override void Reply(string message)
        {
            bot.MainForm.Console(message);
        }

    }
}
