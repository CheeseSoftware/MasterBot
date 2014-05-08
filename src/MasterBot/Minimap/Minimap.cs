using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterBot.Room.Block;

namespace MasterBot.Minimap
{
    public class Minimap
    {
        IBot bot;
        Bitmap bitmap = null;

        public Bitmap Bitmap { get { return bitmap; } }

        public Minimap(IBot bot, int width, int height)
        {
            this.bot = bot;
            bitmap = new Bitmap(width, height);
            MinimapColors.CreateColorCodes();
            Clear(Color.Black);
        }

        private void Clear(Color clearColor)
        {
            for(int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    bitmap.SetPixel(x, y, clearColor);
                }
            }
        }

        public void Update(BlockMap blockMap)
        {
            Clear(Color.Black);
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    IBlock background = blockMap.getBackgroundBlock(x, y);
                    IBlock foreground = blockMap.getBlock(x, y);
                    if (foreground != null && MinimapColors.ColorCodes.ContainsKey(foreground.Id))
                    {
                        bitmap.SetPixel(x, y, MinimapColors.ColorCodes[foreground.Id]);
                        continue;
                    }
                    if(background != null && MinimapColors.ColorCodes.ContainsKey(background.Id))
                    {
                        bitmap.SetPixel(x, y, MinimapColors.ColorCodes[background.Id]);
                    }
                }
            }
            bot.MainForm.UpdateMinimap(bitmap);
        }
    }
}
