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
        private IBot bot;
        private int width;
        private int height;
        private Dictionary<int, Player> players = new Dictionary<int, Player>();

        public Minimap(IBot bot, int width, int height, Dictionary<int, Player> players)
        {
            this.bot = bot;
            this.width = width;
            this.height = height;
            this.players = players;
        }

        /*private void Clear(Bitmap bitmap, Color clearColor)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    bitmap.SetPixel(x, y, clearColor);
                }
            }
        }*/

        public void Update(BlockMap blockMap)
        {
            Bitmap bitmap = new Bitmap(width, height);
            //Clear(bitmap, Color.Black);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    IBlock background = blockMap.getBackgroundBlock(x, y);
                    IBlock foreground = blockMap.getBlock(x, y);
                    if (foreground != null && foreground.Id != 0 && MinimapColors.ColorCodes.ContainsKey(foreground.Id))
                    {
                        bitmap.SetPixel(x, y, MinimapColors.ColorCodes[foreground.Id]);
                        continue;
                    }
                    if (background != null && background.Id != 0 && MinimapColors.ColorCodes.ContainsKey(background.Id))
                    {
                        bitmap.SetPixel(x, y, MinimapColors.ColorCodes[background.Id]);
                        continue;
                    }
                    bitmap.SetPixel(x, y, Color.Black);
                }
            }
            Dictionary<int, Player> tempPlayers = new Dictionary<int, Player>(players);
            foreach (Player player in tempPlayers.Values)
            {
                if (player.BlockX >= 0 && player.BlockX < width)
                    if (player.BlockY >= 0 && player.BlockY < height)
                        bitmap.SetPixel(player.BlockX, player.BlockY, Color.White);
            }
            bot.MainForm.UpdateMinimap(bitmap);
        }
    }
}
