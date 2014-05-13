using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using MasterBot.SubBot;

namespace MasterBot
{
    public interface IMainForm
    {
        TabControl BotTabPage { get; }

        void UpdateSubbotsDatasource(Dictionary<string, ASubBot> source);
        void Console(string text, params Color[] color);
        void UpdateMinimap(Bitmap bitmap);
    }
}
