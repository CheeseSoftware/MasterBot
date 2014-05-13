using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterBot.Room.Block;
using MasterBot.SubBot;
using System.Timers;
using System.Threading;
using System.Diagnostics;

namespace MasterBot.Minimap
{
    public class Minimap : ASubBot
    {
        private IBot bot;
        private int width;
        private int height;
        private Bitmap bitmap;
        private Thread updateThread;
        private Queue<KeyValuePair<Point, Color>> pixelsToSet = new Queue<KeyValuePair<Point, Color>>();
        private Stopwatch minimapUpdateStopwatch = new Stopwatch();
        private int minimapUpdateDelay = 20;

        public Minimap(IBot bot, int width, int height)
            : base(bot)
        {
            this.bot = bot;
            this.width = width;
            this.height = height;
            this.bitmap = new Bitmap(width, height);
            updateThread = new Thread(UpdateMinimap);
            updateThread.Start();
        }

        private void UpdateMinimap()
        {
            minimapUpdateStopwatch.Start();
            while (true)
            {
                while (pixelsToSet.Count > 0)
                {
                    lock (pixelsToSet)
                    {
                        KeyValuePair<Point, Color> pixel = pixelsToSet.Dequeue();
                        bitmap.SetPixel(pixel.Key.X, pixel.Key.Y, pixel.Value);
                    }
                }
                if (minimapUpdateStopwatch.ElapsedMilliseconds >= minimapUpdateDelay)
                {
                    bot.MainForm.UpdateMinimap(bitmap);
                    minimapUpdateStopwatch.Restart();
                }
                Thread.Sleep(5);
            }
        }

        public void DrawPlayer(Player player)
        {
            lock (pixelsToSet)
            {
                if (player.OldBlockX != -1 && player.OldBlockY != -1)
                    pixelsToSet.Enqueue(new KeyValuePair<Point, Color>(new Point(player.OldBlockX, player.OldBlockY), bot.Room.BlockMap.getColor(player.OldBlockX, player.OldBlockY)));
                pixelsToSet.Enqueue(new KeyValuePair<Point, Color>(new Point(player.BlockX, player.BlockY), Color.White));
            }
        }

        public void Die()
        {
            updateThread.Abort();
            bitmap = new Bitmap(width, height);
            bot.MainForm.UpdateMinimap(bitmap);
        }

        public override void onEnable()
        {
        }

        public override void onDisable()
        {
        }

        public override void onConnect()
        {
            updateThread = new Thread(UpdateMinimap);
            updateThread.Start();
        }

        public override void onDisconnect(string reason)
        {
            Die();
        }

        public override void onMessage(PlayerIOClient.Message m)
        {
        }

        public override void onCommand(string cmd, string[] args, ICmdSource cmdSource)
        {
        }

        public override void onBlockChange(int x, int y, IBlock newBlock, IBlock oldBlock)
        {
            if (newBlock.Id != oldBlock.Id)
            {
                if (newBlock.Id == 0 && newBlock.Layer == 0)
                {
                    pixelsToSet.Enqueue(new KeyValuePair<Point, Color>(new Point(x, y), bot.Room.getBlock(1, x, y).Color));
                }
                else if ((newBlock.Layer == 0 || (newBlock.Layer == 1 && bot.Room.getBlock(0, x, y).Id == 0)) && MinimapColors.ColorCodes.ContainsKey(newBlock.Id))
                {
                    lock (pixelsToSet)
                    {
                        if (newBlock.Layer == 0 && newBlock.Color == Color.Transparent)
                            pixelsToSet.Enqueue(new KeyValuePair<Point, Color>(new Point(x, y), bot.Room.getBlock(1, x, y).Color));
                        else
                            pixelsToSet.Enqueue(new KeyValuePair<Point, Color>(new Point(x, y), newBlock.Color));
                    }
                }
            }
        }

        public override void onTick()
        {
        }

        public override bool HasTab
        {
            get { return true; }
        }
    }
}
