using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using MasterBot.SubBot;
using MasterBot.Gui;

namespace MasterBot
{
    public interface IMainForm
    {
        TabControl BotTabPage { get; }
        IRichTextConsole Console { get; }

        void UpdateSubbotsDatasource(Dictionary<string, ASubBot> source);
        void UpdateMinimap(Bitmap bitmap);
    }
}
