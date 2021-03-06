﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace MasterBot.Minimap
{
    public static class MinimapColors
    {
        private static int backgroundBricksBMD = 0;
        private static int forgroundBricksBMD = 1;
        private static int decorationsBMD = 2;
        private static int waveBMD = 0;
        private static int waterBMD = 0;
        private static int switchBMD = 0;
        private static int secretBricksBMD = 0;
        private static int doorstimeBMD = 0;
        private static int doorsBMD = 0;
        private static int completeBMD = 0;
        private static int coinBMD = 0;
        private static int cakeBMD = 0;
        private static int bonusCoinBMD = 0;
        private static int mudbubbleBMD = 0;
        private static int hasardBMD = 0;
        private static int fireHazardBMD = 0;
        private static int mudBMD = 0;
        private static int glowylinebluestraightBMD = 0;
        private static int glowylineblueslopeBMD = 0;
        private static int glowylinegreenslopeBMD = 0;
        private static int glowylinegreenstraightBMD = 0;
        private static int glowylineyellowstraightBMD = 0;
        private static int glowylineyellowslopeBMD = 0;
        private static Dictionary<int, Color> colorCodes = new Dictionary<int, Color>();

        public static Dictionary<int, Color> ColorCodes { get { return colorCodes; } }

        public static Color UIntToColor(uint color)
        {
            byte a = (byte)(color >> 24);
            byte r = (byte)(color >> 16);
            byte g = (byte)(color >> 8);
            byte b = (byte)(color >> 0);
            return Color.FromArgb(a, r, g, b);
        }

        private static uint ColorToUInt(Color color)
        {
            return (uint)((color.A << 24) | (color.R << 16) |
                          (color.G << 8) | (color.B << 0));
        }

        private static Color getDominantColor(Bitmap bmp)
        {
            int r = 0;
            int g = 0;
            int b = 0;

            int total = 0;

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color clr = bmp.GetPixel(x, y);

                    r += clr.R;
                    g += clr.G;
                    b += clr.B;

                    total++;
                }
            }

            r /= total;
            g /= total;
            b /= total;

            return Color.FromArgb(r, g, b);
        }

        private static void createBrick(int id, int layer, int bmd, string bla3, int blocktype, bool bla5, bool bla6, int xOffset = 0, uint colorid = 0)
        {
            if (colorid != 0 && id != 0)
            {
                colorCodes.Add(id, UIntToColor(colorid));
            }
            else if (layer == ItemLayer.BACKGROUND)
            {
                try
                {
                    Bitmap bitmap1 = new Bitmap("image 994.png");
                    Rectangle size = new Rectangle(16 * xOffset, 0, 16, 16);
                    Bitmap bitmap2 = bitmap1.Clone(size, new PixelFormat());
                    colorCodes.Add(id, getDominantColor(bitmap2));
                }
                catch (Exception)
                {
                }
            }
            else if (layer == ItemLayer.FORGROUND)
            {
                try
                {
                    Bitmap bitmap1 = new Bitmap("image 979.png");
                    Rectangle size = new Rectangle(16 * xOffset, 0, 16, 16);
                    Bitmap bitmap2 = bitmap1.Clone(size, new PixelFormat());
                    colorCodes.Add(id, getDominantColor(bitmap2));
                }
                catch (Exception)
                {
                    
                }
            }
            else if (layer == ItemLayer.DECORATION || layer == ItemLayer.ABOVE)
            {
                if (!colorCodes.ContainsKey(id))
                    ColorCodes.Add(id, Color.Transparent);
            }
            /*else if (layer == ItemLayer.DECORATION && id != 1000)
            {
                Bitmap bitmap1 = new Bitmap("image 1000.png");
                Rectangle size = new Rectangle(16 * xOffset, 0, 16, 16);
                Bitmap bitmap2 = bitmap1.Clone(size, new PixelFormat());
                colorCodes.Add(id, getDominantColor(bitmap2));
            }*/
        }

        public static void CreateColorCodes()
        {
            //var _loc_1:* = new ItemBrickPackage("basic", "Basic Blocks");
            createBrick(9, ItemLayer.FORGROUND, forgroundBricksBMD, "", ItemTab.BLOCK, false, true, 9, 4285427310);
            createBrick(10, ItemLayer.FORGROUND, forgroundBricksBMD, "", ItemTab.BLOCK, false, true, 10, 4281684648);
            createBrick(11, ItemLayer.FORGROUND, forgroundBricksBMD, "", ItemTab.BLOCK, false, true, 11, 4288099751);
            createBrick(12, ItemLayer.FORGROUND, forgroundBricksBMD, "", ItemTab.BLOCK, false, true, 12, 4289213780);
            createBrick(13, ItemLayer.FORGROUND, forgroundBricksBMD, "", ItemTab.BLOCK, false, true, 13, 4287866933);
            createBrick(14, ItemLayer.FORGROUND, forgroundBricksBMD, "", ItemTab.BLOCK, false, true, 14, 4282558518);
            createBrick(15, ItemLayer.FORGROUND, forgroundBricksBMD, "", ItemTab.BLOCK, false, true, 15, 4281704102);
            createBrick(182, ItemLayer.FORGROUND, forgroundBricksBMD, "", ItemTab.BLOCK, false, true, 156, 4279834905);
            //var _loc_2:* = new ItemBrickPackage("beta", "Beta Blocks");
            createBrick(37, ItemLayer.FORGROUND, forgroundBricksBMD, "pro", ItemTab.BLOCK, false, true, 37, 4291715791);
            createBrick(38, ItemLayer.FORGROUND, forgroundBricksBMD, "pro", ItemTab.BLOCK, false, true, 38, 4283091074);
            createBrick(39, ItemLayer.FORGROUND, forgroundBricksBMD, "pro", ItemTab.BLOCK, false, true, 39, 4283270342);
            createBrick(40, ItemLayer.FORGROUND, forgroundBricksBMD, "pro", ItemTab.BLOCK, false, true, 40, 4291782224);
            createBrick(41, ItemLayer.FORGROUND, forgroundBricksBMD, "pro", ItemTab.BLOCK, false, true, 41, 4291995973);
            createBrick(42, ItemLayer.FORGROUND, forgroundBricksBMD, "pro", ItemTab.BLOCK, false, true, 42, 4288321167);
            //var _loc_3:* = new ItemBrickPackage("brick", "Brick Blocks");
            createBrick(16, ItemLayer.FORGROUND, forgroundBricksBMD, "", ItemTab.BLOCK, false, true, 16, 4287315465);
            createBrick(17, ItemLayer.FORGROUND, forgroundBricksBMD, "", ItemTab.BLOCK, false, true, 17, 4280577869);
            createBrick(18, ItemLayer.FORGROUND, forgroundBricksBMD, "", ItemTab.BLOCK, false, true, 18, 4283311215);
            createBrick(19, ItemLayer.FORGROUND, forgroundBricksBMD, "", ItemTab.BLOCK, false, true, 19, 4282614544);
            createBrick(20, ItemLayer.FORGROUND, forgroundBricksBMD, "", ItemTab.BLOCK, false, true, 20, 4285473833);
            createBrick(21, ItemLayer.FORGROUND, forgroundBricksBMD, "", ItemTab.BLOCK, false, true, 21, 4285488420);
            //var _loc_4:* = new ItemBrickPackage("metal", "Metal Blocks");
            createBrick(29, ItemLayer.FORGROUND, forgroundBricksBMD, "", ItemTab.BLOCK, false, true, 29, 4288783269);
            createBrick(30, ItemLayer.FORGROUND, forgroundBricksBMD, "", ItemTab.BLOCK, false, true, 30, 4292835905);
            createBrick(31, ItemLayer.FORGROUND, forgroundBricksBMD, "", ItemTab.BLOCK, false, true, 31, 4293962023);
            //var _loc_5:* = new ItemBrickPackage("grass", "Grass Blocks");
            createBrick(34, ItemLayer.DECORATION, forgroundBricksBMD, "", ItemTab.BLOCK, false, true, 34, 4282737427);
            createBrick(35, ItemLayer.FORGROUND, forgroundBricksBMD, "", ItemTab.BLOCK, false, true, 35, 4282737427);
            createBrick(36, ItemLayer.DECORATION, forgroundBricksBMD, "", ItemTab.BLOCK, false, true, 36, 4282737427);
            //var _loc_6:* = new ItemBrickPackage("special", "Special Blocks");
            createBrick(22, ItemLayer.DECORATION, forgroundBricksBMD, "", ItemTab.BLOCK, false, true, 22, 4287191826);
            createBrick(32, ItemLayer.DECORATION, forgroundBricksBMD, "", ItemTab.BLOCK, false, true, 32, 4291792930);
            createBrick(33, ItemLayer.FORGROUND, forgroundBricksBMD, "", ItemTab.BLOCK, false, true, 33, 4278190080);
            createBrick(44, ItemLayer.FORGROUND, forgroundBricksBMD, "brickblackblock", ItemTab.BLOCK, false, true, 44, 16777216);
            //var _loc_7:* = new ItemBrickPackage("factory", "Factory Blocks");
            createBrick(45, ItemLayer.FORGROUND, forgroundBricksBMD, "brickfactorypack", ItemTab.BLOCK, false, true, 45, 4285686091);
            createBrick(46, ItemLayer.FORGROUND, forgroundBricksBMD, "brickfactorypack", ItemTab.BLOCK, false, true, 46, 4285426528);
            createBrick(47, ItemLayer.FORGROUND, forgroundBricksBMD, "brickfactorypack", ItemTab.BLOCK, false, true, 47, 4287525711);
            createBrick(48, ItemLayer.FORGROUND, forgroundBricksBMD, "brickfactorypack", ItemTab.BLOCK, false, true, 48, 4286533419);
            createBrick(49, ItemLayer.FORGROUND, forgroundBricksBMD, "brickfactorypack", ItemTab.BLOCK, false, true, 49, 4285887861);
            //var _loc_8:* = new ItemBrickPackage("secrets", "Secret Blocks");
            createBrick(50, ItemLayer.DECORATION, secretBricksBMD, "bricksecret", ItemTab.BLOCK, true, true, 0, 0);
            createBrick(243, ItemLayer.DECORATION, secretBricksBMD, "bricksecret", ItemTab.BLOCK, true, true, 1, 16777216);
            //var _loc_9:* = new ItemBrickPackage("glass", "Glass Blocks");
            createBrick(51, ItemLayer.FORGROUND, forgroundBricksBMD, "brickglass", ItemTab.BLOCK, false, true, 51, 4294480537);
            createBrick(52, ItemLayer.FORGROUND, forgroundBricksBMD, "brickglass", ItemTab.BLOCK, false, true, 52, 4293495798);
            createBrick(53, ItemLayer.FORGROUND, forgroundBricksBMD, "brickglass", ItemTab.BLOCK, false, true, 53, 4289169910);
            createBrick(54, ItemLayer.FORGROUND, forgroundBricksBMD, "brickglass", ItemTab.BLOCK, false, true, 54, 4286487030);
            createBrick(55, ItemLayer.FORGROUND, forgroundBricksBMD, "brickglass", ItemTab.BLOCK, false, true, 55, 4288011510);
            createBrick(56, ItemLayer.FORGROUND, forgroundBricksBMD, "brickglass", ItemTab.BLOCK, false, true, 56, 4287822762);
            createBrick(57, ItemLayer.FORGROUND, forgroundBricksBMD, "brickglass", ItemTab.BLOCK, false, true, 57, 4294498956);
            createBrick(58, ItemLayer.FORGROUND, forgroundBricksBMD, "brickglass", ItemTab.BLOCK, false, true, 58, 4294359700);
            //var _loc_10:* = new ItemBrickPackage("minerals", "Mineral Blocks");
            createBrick(70, ItemLayer.FORGROUND, forgroundBricksBMD, "brickminiral", ItemTab.BLOCK, false, true, 70, 4293787648);
            createBrick(71, ItemLayer.FORGROUND, forgroundBricksBMD, "brickminiral", ItemTab.BLOCK, false, true, 71, 4293787886);
            createBrick(72, ItemLayer.FORGROUND, forgroundBricksBMD, "brickminiral", ItemTab.BLOCK, false, true, 72, 4278190318);
            createBrick(73, ItemLayer.FORGROUND, forgroundBricksBMD, "brickminiral", ItemTab.BLOCK, false, true, 73, 4278251246);
            createBrick(74, ItemLayer.FORGROUND, forgroundBricksBMD, "brickminiral", ItemTab.BLOCK, false, true, 74, 4278251008);
            createBrick(75, ItemLayer.FORGROUND, forgroundBricksBMD, "brickminiral", ItemTab.BLOCK, false, true, 75, 4293848576);
            createBrick(76, ItemLayer.FORGROUND, forgroundBricksBMD, "brickminiral", ItemTab.BLOCK, false, true, 76, 4293818112);
            //var _loc_11:* = new ItemBrickPackage("christmas 2011", "Christmas 2011 Blocks");
            createBrick(78, ItemLayer.FORGROUND, forgroundBricksBMD, "brickxmas2011", ItemTab.BLOCK, false, true, 78);
            createBrick(79, ItemLayer.FORGROUND, forgroundBricksBMD, "brickxmas2011", ItemTab.BLOCK, false, true, 79);
            createBrick(80, ItemLayer.FORGROUND, forgroundBricksBMD, "brickxmas2011", ItemTab.BLOCK, false, true, 80);
            createBrick(81, ItemLayer.FORGROUND, forgroundBricksBMD, "brickxmas2011", ItemTab.BLOCK, false, true, 81);
            createBrick(82, ItemLayer.FORGROUND, forgroundBricksBMD, "brickxmas2011", ItemTab.BLOCK, false, true, 82);
            //var _loc_12:* = new ItemBrickPackage("adm", "Admin Blocks");
            createBrick(1000, ItemLayer.DECORATION, forgroundBricksBMD, "", ItemTab.BLOCK, false, true, 1000);//, -1, true);
            //var _loc_13:* = new ItemBrickPackage("keys", "Key Blocks");
            createBrick(6, ItemLayer.DECORATION, forgroundBricksBMD, "", ItemTab.ACTION, false, false, 6, 4281080346);
            createBrick(7, ItemLayer.DECORATION, forgroundBricksBMD, "", ItemTab.ACTION, false, false, 7, 4279905306);
            createBrick(8, ItemLayer.DECORATION, forgroundBricksBMD, "", ItemTab.ACTION, false, false, 8, 4279900716);
            //var _loc_14:* = new ItemBrickPackage("switches", "Switch Blocks");
            createBrick(ItemId.SWITCH_PURPLE, ItemLayer.DECORATION, switchBMD, "brickswitchpurple", ItemTab.ACTION, true, true, 0);
            //var _loc_15:* = new ItemBrickPackage("gates", "Gate Blocks");
            createBrick(26, ItemLayer.DECORATION, forgroundBricksBMD, "", ItemTab.ACTION, false, false, 26, 4288425286);
            createBrick(27, ItemLayer.DECORATION, forgroundBricksBMD, "", ItemTab.ACTION, false, false, 27, 4281834544);
            createBrick(28, ItemLayer.DECORATION, forgroundBricksBMD, "", ItemTab.ACTION, false, false, 28, 4281156764);
            createBrick(ItemId.COINGATE, ItemLayer.DECORATION, forgroundBricksBMD, "brickcoingate", ItemTab.ACTION, true, false, 139, 4290285077);
            createBrick(ItemId.TIMEGATE, ItemLayer.DECORATION, doorstimeBMD, "bricktimeddoor", ItemTab.ACTION, true, false, 8);
            createBrick(ItemId.GATE_PURPLE, ItemLayer.DECORATION, doorsBMD, "brickswitchpurple", ItemTab.ACTION, true, false, 8);
            //var _loc_16:* = new ItemBrickPackage("doors", "Door Blocks");
            createBrick(23, ItemLayer.DECORATION, forgroundBricksBMD, "", ItemTab.ACTION, false, true, 23, 4288425286);
            createBrick(24, ItemLayer.DECORATION, forgroundBricksBMD, "", ItemTab.ACTION, false, true, 24, 4281834544);
            createBrick(25, ItemLayer.DECORATION, forgroundBricksBMD, "", ItemTab.ACTION, false, true, 25, 4281156764);
            createBrick(ItemId.COINDOOR, ItemLayer.DECORATION, forgroundBricksBMD, "brickcoindoor", ItemTab.ACTION, true, true, 43, 4290285077);
            createBrick(ItemId.TIMEDOOR, ItemLayer.DECORATION, doorstimeBMD, "bricktimeddoor", ItemTab.ACTION, true, true, 3);
            createBrick(ItemId.DOOR_PURPLE, ItemLayer.DECORATION, doorsBMD, "brickswitchpurple", ItemTab.ACTION, true, false, 9);
            //var _loc_17:* = new ItemBrickPackage("gravity", "Gravity Modifying Arrows");
            createBrick(0, ItemLayer.BACKGROUND, forgroundBricksBMD, "", ItemTab.ACTION, false, false, 0, 4278190080);
            createBrick(1, ItemLayer.DECORATION, forgroundBricksBMD, "", ItemTab.ACTION, false, false, 1, 0);
            createBrick(2, ItemLayer.DECORATION, forgroundBricksBMD, "", ItemTab.ACTION, false, false, 2, 0);
            createBrick(3, ItemLayer.DECORATION, forgroundBricksBMD, "", ItemTab.ACTION, false, false, 3, 0);
            createBrick(4, ItemLayer.DECORATION, forgroundBricksBMD, "", ItemTab.ACTION, false, false, 4, 0);
            //var _loc_18:* = new ItemBrickPackage("Boost", "Boost Arrows");
            createBrick(ItemId.SPEED_LEFT, ItemLayer.DECORATION, forgroundBricksBMD, "brickboost", ItemTab.ACTION, false, false, 157, 0);
            createBrick(ItemId.SPEED_RIGHT, ItemLayer.DECORATION, forgroundBricksBMD, "brickboost", ItemTab.ACTION, false, false, 158, 0);
            createBrick(ItemId.SPEED_UP, ItemLayer.DECORATION, forgroundBricksBMD, "brickboost", ItemTab.ACTION, false, false, 159, 0);
            createBrick(ItemId.SPEED_DOWN, ItemLayer.DECORATION, forgroundBricksBMD, "brickboost", ItemTab.ACTION, false, false, 160, 0);
            //var _loc_19:* = new ItemBrickPackage("music", "Music Blocks");
            createBrick(77, ItemLayer.DECORATION, forgroundBricksBMD, "bricknode", ItemTab.ACTION, false, false, 77, 0);
            createBrick(83, ItemLayer.DECORATION, forgroundBricksBMD, "brickdrums", ItemTab.ACTION, false, false, 83, 0);
            //var _loc_20:* = new ItemBrickPackage("coins", "Coin Blocks");
            createBrick(100, ItemLayer.ABOVE, coinBMD, "", ItemTab.ACTION, false, false, 0, 0);
            createBrick(101, ItemLayer.ABOVE, bonusCoinBMD, "", ItemTab.ACTION, false, false, 0, 0);
            createBrick(110, ItemLayer.DECORATION, coinBMD, "hidden", ItemTab.ACTION, false, false, 0, 0);
            createBrick(111, ItemLayer.DECORATION, bonusCoinBMD, "hidden", ItemTab.ACTION, false, false, 0, 0);
            //var _loc_21:* = new ItemBrickPackage("crowns", "Crown Awarders");
            createBrick(5, ItemLayer.DECORATION, forgroundBricksBMD, "", ItemTab.ACTION, false, true, 5, 4282595615);
            //var _loc_22:* = new ItemBrickPackage("tools", "Tool Blocks");
            createBrick(255, ItemLayer.DECORATION, decorationsBMD, "brickspawn", ItemTab.ACTION, true, true, 255 - 128);
            createBrick(ItemId.BRICK_COMPLETE, ItemLayer.ABOVE, completeBMD, "brickcomplete", ItemTab.ACTION, true, false, 0, 0);
            //var _loc_23:* = new ItemBrickPackage("hazards", "Hazard Blocks")
            createBrick(361, ItemLayer.DECORATION, hasardBMD, "brickspike", ItemTab.ACTION, true, false, 1, 0);
            createBrick(368, ItemLayer.ABOVE, fireHazardBMD, "brickfire", ItemTab.ACTION, true, false, 3, 0);
            //var _loc_23:* = new ItemBrickPackage("Ladders", "Ladder Blocks");
            createBrick(118, ItemLayer.DECORATION, forgroundBricksBMD, "brickcastle", ItemTab.ACTION, false, true, 135, 0);
            createBrick(120, ItemLayer.DECORATION, forgroundBricksBMD, "brickninja", ItemTab.ACTION, false, false, 98, 0);
            createBrick(98, ItemLayer.DECORATION, forgroundBricksBMD, "brickjungle", ItemTab.ACTION, false, true, 174, 0);
            createBrick(99, ItemLayer.DECORATION, forgroundBricksBMD, "brickjungle", ItemTab.ACTION, false, true, 175, 0);
            //var _loc_25:* = new ItemBrickPackage("liquids", "Liquid Blocks");
            createBrick(119, ItemLayer.ABOVE, waterBMD, "brickwater", ItemTab.ACTION, false, false, 0, 0);
            createBrick(369, ItemLayer.ABOVE, mudBMD, "brickswamp", ItemTab.ACTION, false, false, 0, 0);
            //var _loc_24:* = new ItemBrickPackage("portal", "Portal Block");
            createBrick(242, ItemLayer.DECORATION, decorationsBMD, "", ItemTab.ACTION, true, true, 242 - 128);
            //var _loc_25:* = new ItemBrickPackage("diamond", "Diamond Block");
            createBrick(ItemId.DIAMOND, ItemLayer.DECORATION, decorationsBMD, "brickdiamond", ItemTab.ACTION, true, true, 241 - 128);
            //var _loc_26:* = new ItemBrickPackage("cake", "Cake Block");
            createBrick(ItemId.CAKE, ItemLayer.DECORATION, cakeBMD, "brickcake", ItemTab.ACTION, true, true, 0, 4294902015);
            //var _loc_27:* = new ItemBrickPackage("christmas 2010", "Christmas 2010 Blocks");
            createBrick(249, ItemLayer.DECORATION, decorationsBMD, "brickchristmas2010", ItemTab.DECORATIVE, false, true, 249 - 128, 0);
            createBrick(250, ItemLayer.DECORATION, decorationsBMD, "brickchristmas2010", ItemTab.DECORATIVE, false, true, 250 - 128, 0);
            createBrick(251, ItemLayer.DECORATION, decorationsBMD, "brickchristmas2010", ItemTab.DECORATIVE, false, true, 251 - 128, 0);
            createBrick(252, ItemLayer.DECORATION, decorationsBMD, "brickchristmas2010", ItemTab.DECORATIVE, false, true, 252 - 128, 0);
            createBrick(253, ItemLayer.ABOVE, decorationsBMD, "brickchristmas2010", ItemTab.DECORATIVE, false, true, 253 - 128, 0);
            createBrick(254, ItemLayer.ABOVE, decorationsBMD, "brickchristmas2010", ItemTab.DECORATIVE, false, true, 254 - 128, 0);
            //var _loc_28:* = new ItemBrickPackage("new year 2010", "New Year 2010 Blocks");
            createBrick(244, ItemLayer.DECORATION, decorationsBMD, "mixednewyear2010", ItemTab.DECORATIVE, false, true, 244 - 128, 0);
            createBrick(245, ItemLayer.DECORATION, decorationsBMD, "mixednewyear2010", ItemTab.DECORATIVE, false, true, 245 - 128, 0);
            createBrick(246, ItemLayer.DECORATION, decorationsBMD, "mixednewyear2010", ItemTab.DECORATIVE, false, true, 246 - 128, 0);
            createBrick(247, ItemLayer.DECORATION, decorationsBMD, "mixednewyear2010", ItemTab.DECORATIVE, false, true, 247 - 128, 0);
            createBrick(248, ItemLayer.DECORATION, decorationsBMD, "mixednewyear2010", ItemTab.DECORATIVE, false, true, 248 - 128, 0);
            //var _loc_29:* = new ItemBrickPackage("spring 2011", "Spring 2011 Blocks");
            createBrick(233, ItemLayer.ABOVE, decorationsBMD, "brickspring2011", ItemTab.DECORATIVE, false, true, 233 - 128, 0);
            createBrick(234, ItemLayer.ABOVE, decorationsBMD, "brickspring2011", ItemTab.DECORATIVE, false, true, 234 - 128, 0);
            createBrick(235, ItemLayer.ABOVE, decorationsBMD, "brickspring2011", ItemTab.DECORATIVE, false, true, 235 - 128, 0);
            createBrick(236, ItemLayer.ABOVE, decorationsBMD, "brickspring2011", ItemTab.DECORATIVE, false, true, 236 - 128, 0);
            createBrick(237, ItemLayer.ABOVE, decorationsBMD, "brickspring2011", ItemTab.DECORATIVE, false, true, 237 - 128, 0);
            createBrick(238, ItemLayer.ABOVE, decorationsBMD, "brickspring2011", ItemTab.DECORATIVE, false, true, 238 - 128, 0);
            createBrick(239, ItemLayer.DECORATION, decorationsBMD, "brickspring2011", ItemTab.DECORATIVE, false, true, 239 - 128, 0);
            createBrick(240, ItemLayer.ABOVE, decorationsBMD, "brickspring2011", ItemTab.DECORATIVE, false, true, 240 - 128, 0);
            //var _loc_30:* = new ItemBrickPackage("Prizes", "Your Prices");
            createBrick(223, ItemLayer.DECORATION, decorationsBMD, "brickhwtrophy", ItemTab.DECORATIVE, false, false, 223 - 128, 0);
            //var _loc_31:* = new ItemBrickPackage("easter 2012", "Easter 2012 Blocks");
            createBrick(256, ItemLayer.DECORATION, decorationsBMD, "brickeaster2012", ItemTab.DECORATIVE, false, false, 256 - 128, 0);
            createBrick(257, ItemLayer.DECORATION, decorationsBMD, "brickeaster2012", ItemTab.DECORATIVE, false, false, 257 - 128, 0);
            createBrick(258, ItemLayer.DECORATION, decorationsBMD, "brickeaster2012", ItemTab.DECORATIVE, false, false, 258 - 128, 0);
            createBrick(259, ItemLayer.DECORATION, decorationsBMD, "brickeaster2012", ItemTab.DECORATIVE, false, false, 259 - 128, 0);
            createBrick(260, ItemLayer.DECORATION, decorationsBMD, "brickeaster2012", ItemTab.DECORATIVE, false, false, 260 - 128, 0);
            //var _loc_32:* = new ItemBrickPackage("basic", "Basic Background Blocks");
            createBrick(500, ItemLayer.BACKGROUND, backgroundBricksBMD, "", ItemTab.BACKGROUND, false, false, 500 - 500);
            createBrick(501, ItemLayer.BACKGROUND, backgroundBricksBMD, "", ItemTab.BACKGROUND, false, false, 501 - 500);
            createBrick(502, ItemLayer.BACKGROUND, backgroundBricksBMD, "", ItemTab.BACKGROUND, false, false, 502 - 500);
            createBrick(503, ItemLayer.BACKGROUND, backgroundBricksBMD, "", ItemTab.BACKGROUND, false, false, 503 - 500);
            createBrick(504, ItemLayer.BACKGROUND, backgroundBricksBMD, "", ItemTab.BACKGROUND, false, false, 504 - 500);
            createBrick(505, ItemLayer.BACKGROUND, backgroundBricksBMD, "", ItemTab.BACKGROUND, false, false, 505 - 500);
            createBrick(506, ItemLayer.BACKGROUND, backgroundBricksBMD, "", ItemTab.BACKGROUND, false, false, 506 - 500);
            // var _loc_33:* = new ItemBrickPackage("brick", "Brick Background Blocks");
            createBrick(507, ItemLayer.BACKGROUND, backgroundBricksBMD, "", ItemTab.BACKGROUND, false, false, 507 - 500);
            createBrick(508, ItemLayer.BACKGROUND, backgroundBricksBMD, "", ItemTab.BACKGROUND, false, false, 508 - 500);
            createBrick(509, ItemLayer.BACKGROUND, backgroundBricksBMD, "", ItemTab.BACKGROUND, false, false, 509 - 500);
            createBrick(510, ItemLayer.BACKGROUND, backgroundBricksBMD, "", ItemTab.BACKGROUND, false, false, 510 - 500);
            createBrick(511, ItemLayer.BACKGROUND, backgroundBricksBMD, "", ItemTab.BACKGROUND, false, false, 511 - 500);
            createBrick(512, ItemLayer.BACKGROUND, backgroundBricksBMD, "", ItemTab.BACKGROUND, false, false, 512 - 500);
            //var _loc_34:* = new ItemBrickPackage("checker", "Checker Background Blocks");
            createBrick(513, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickbgchecker", ItemTab.BACKGROUND, false, false, 513 - 500);
            createBrick(514, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickbgchecker", ItemTab.BACKGROUND, false, false, 514 - 500);
            createBrick(515, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickbgchecker", ItemTab.BACKGROUND, false, false, 515 - 500);
            createBrick(516, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickbgchecker", ItemTab.BACKGROUND, false, false, 516 - 500);
            createBrick(517, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickbgchecker", ItemTab.BACKGROUND, false, false, 517 - 500);
            createBrick(518, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickbgchecker", ItemTab.BACKGROUND, false, false, 518 - 500);
            createBrick(519, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickbgchecker", ItemTab.BACKGROUND, false, false, 519 - 500);
            //var _loc_35:* = new ItemBrickPackage("dark", "Dark Background Blocks");
            createBrick(520, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickbgdark", ItemTab.BACKGROUND, false, false, 520 - 500);
            createBrick(521, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickbgdark", ItemTab.BACKGROUND, false, false, 521 - 500);
            createBrick(522, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickbgdark", ItemTab.BACKGROUND, false, false, 522 - 500);
            createBrick(523, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickbgdark", ItemTab.BACKGROUND, false, false, 523 - 500);
            createBrick(524, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickbgdark", ItemTab.BACKGROUND, false, false, 524 - 500);
            createBrick(525, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickbgdark", ItemTab.BACKGROUND, false, false, 525 - 500);
            createBrick(526, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickbgdark", ItemTab.BACKGROUND, false, false, 526 - 500);
            //var _loc_36:* = new ItemBrickPackage("normal", "Normal Background Blocks");
            createBrick(610, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickbgnormal", ItemTab.BACKGROUND, false, true, 110);
            createBrick(611, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickbgnormal", ItemTab.BACKGROUND, false, true, 111);
            createBrick(612, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickbgnormal", ItemTab.BACKGROUND, false, true, 112);
            createBrick(613, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickbgnormal", ItemTab.BACKGROUND, false, true, 113);
            createBrick(614, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickbgnormal", ItemTab.BACKGROUND, false, true, 114);
            createBrick(615, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickbgnormal", ItemTab.BACKGROUND, false, true, 115);
            createBrick(616, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickbgnormal", ItemTab.BACKGROUND, false, true, 116);
            //var _loc_37:* = new ItemBrickPackage("pastel", "Pastel Background Blocks");
            createBrick(527, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickbgpastel", ItemTab.BACKGROUND, false, false, 527 - 500);
            createBrick(528, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickbgpastel", ItemTab.BACKGROUND, false, false, 528 - 500);
            createBrick(529, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickbgpastel", ItemTab.BACKGROUND, false, false, 529 - 500);
            createBrick(530, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickbgpastel", ItemTab.BACKGROUND, false, false, 530 - 500);
            createBrick(531, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickbgpastel", ItemTab.BACKGROUND, false, false, 531 - 500);
            createBrick(532, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickbgpastel", ItemTab.BACKGROUND, false, false, 532 - 500);
            //var _loc_38:* = new ItemBrickPackage("canvas", "Canvas Background Blocks");
            createBrick(533, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickbgcanvas", ItemTab.BACKGROUND, false, false, 533 - 500);
            createBrick(534, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickbgcanvas", ItemTab.BACKGROUND, false, false, 534 - 500);
            createBrick(535, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickbgcanvas", ItemTab.BACKGROUND, false, false, 535 - 500);
            createBrick(536, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickbgcanvas", ItemTab.BACKGROUND, false, false, 536 - 500);
            createBrick(537, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickbgcanvas", ItemTab.BACKGROUND, false, false, 537 - 500);
            createBrick(538, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickbgcanvas", ItemTab.BACKGROUND, false, false, 538 - 500);
            //var _loc_39:* = new ItemBrickPackage("carnival", "Carnival Blocks");
            createBrick(545, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickbgcarnival", ItemTab.BACKGROUND, false, true, 45);
            createBrick(546, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickbgcarnival", ItemTab.BACKGROUND, false, true, 46);
            createBrick(547, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickbgcarnival", ItemTab.BACKGROUND, false, true, 47);
            createBrick(548, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickbgcarnival", ItemTab.BACKGROUND, false, true, 48);
            createBrick(549, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickbgcarnival", ItemTab.BACKGROUND, false, true, 49);
            //var _loc_40:* = new ItemBrickPackage("candy", "Candy Blocks");
            createBrick(60, ItemLayer.FORGROUND, forgroundBricksBMD, "brickcandy", ItemTab.BLOCK, false, true, 60);
            createBrick(61, ItemLayer.DECORATION, forgroundBricksBMD, "brickcandy", ItemTab.BLOCK, false, true, 61);
            createBrick(62, ItemLayer.DECORATION, forgroundBricksBMD, "brickcandy", ItemTab.BLOCK, false, true, 62);
            createBrick(63, ItemLayer.DECORATION, forgroundBricksBMD, "brickcandy", ItemTab.BLOCK, false, true, 63);
            createBrick(64, ItemLayer.DECORATION, forgroundBricksBMD, "brickcandy", ItemTab.BLOCK, false, true, 64);
            createBrick(65, ItemLayer.DECORATION, forgroundBricksBMD, "brickcandy", ItemTab.BLOCK, false, true, 65);
            createBrick(66, ItemLayer.DECORATION, forgroundBricksBMD, "brickcandy", ItemTab.BLOCK, false, true, 66);
            createBrick(67, ItemLayer.DECORATION, forgroundBricksBMD, "brickcandy", ItemTab.BLOCK, false, true, 67);
            createBrick(227, ItemLayer.DECORATION, decorationsBMD, "brickcandy", ItemTab.DECORATIVE, false, true, 227 - 128, 0);
            createBrick(539, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickcandy", ItemTab.BACKGROUND, false, false, 539 - 500);
            createBrick(540, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickcandy", ItemTab.BACKGROUND, false, false, 540 - 500);
            //var _loc_41:* = new ItemBrickPackage("summer 2011", "Summer 2011 Blocks");
            createBrick(59, ItemLayer.FORGROUND, forgroundBricksBMD, "bricksummer2011", ItemTab.BLOCK, false, true, 59);
            createBrick(228, ItemLayer.ABOVE, decorationsBMD, "bricksummer2011", ItemTab.DECORATIVE, false, false, 228 - 128, 0);
            createBrick(229, ItemLayer.ABOVE, decorationsBMD, "bricksummer2011", ItemTab.DECORATIVE, false, false, 229 - 128, 0);
            createBrick(230, ItemLayer.ABOVE, decorationsBMD, "bricksummer2011", ItemTab.DECORATIVE, false, false, 230 - 128, 0);
            createBrick(231, ItemLayer.ABOVE, decorationsBMD, "bricksummer2011", ItemTab.DECORATIVE, false, false, 231 - 128, 0);
            createBrick(232, ItemLayer.ABOVE, decorationsBMD, "bricksummer2011", ItemTab.DECORATIVE, false, false, 232 - 128, 0);
            //var _loc_42:* = new ItemBrickPackage("halloween 2011", "Halloween 2011 Blocks");
            createBrick(68, ItemLayer.FORGROUND, forgroundBricksBMD, "brickhw2011", ItemTab.BLOCK, false, true, 68);
            createBrick(69, ItemLayer.FORGROUND, forgroundBricksBMD, "brickhw2011", ItemTab.BLOCK, false, true, 69);
            createBrick(224, ItemLayer.DECORATION, decorationsBMD, "brickhw2011", ItemTab.DECORATIVE, false, true, 224 - 128, 0);
            createBrick(225, ItemLayer.DECORATION, decorationsBMD, "brickhw2011", ItemTab.DECORATIVE, false, true, 225 - 128, 0);
            createBrick(226, ItemLayer.DECORATION, decorationsBMD, "brickhw2011", ItemTab.DECORATIVE, false, true, 226 - 128, 0);
            createBrick(541, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickhw2011", ItemTab.BACKGROUND, false, false, 541 - 500);
            createBrick(542, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickhw2011", ItemTab.BACKGROUND, false, false, 542 - 500);
            createBrick(543, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickhw2011", ItemTab.BACKGROUND, false, false, 543 - 500);
            createBrick(544, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickhw2011", ItemTab.BACKGROUND, false, false, 544 - 500);
            //var _loc_43:* = new ItemBrickPackage("christmas 2011", "Christmas 2011 Blocks");
            createBrick(218, ItemLayer.DECORATION, decorationsBMD, "brickxmas2011", ItemTab.DECORATIVE, false, true, 218 - 128, 0);
            createBrick(219, ItemLayer.DECORATION, decorationsBMD, "brickxmas2011", ItemTab.DECORATIVE, false, true, 219 - 128, 0);
            createBrick(220, ItemLayer.DECORATION, decorationsBMD, "brickxmas2011", ItemTab.DECORATIVE, false, true, 220 - 128, 0);
            createBrick(221, ItemLayer.DECORATION, decorationsBMD, "brickxmas2011", ItemTab.DECORATIVE, false, true, 221 - 128, 0);
            createBrick(222, ItemLayer.DECORATION, decorationsBMD, "brickxmas2011", ItemTab.DECORATIVE, false, true, 222 - 128, 0);
            //var _loc_44:* = new ItemBrickPackage("scifi", "SciFi Blocks");
            createBrick(84, ItemLayer.DECORATION, forgroundBricksBMD, "brickscifi", ItemTab.BLOCK, false, true, 84);
            createBrick(85, ItemLayer.DECORATION, forgroundBricksBMD, "brickscifi", ItemTab.BLOCK, false, true, 85);
            createBrick(86, ItemLayer.DECORATION, forgroundBricksBMD, "brickscifi", ItemTab.BLOCK, false, true, 86);
            createBrick(87, ItemLayer.DECORATION, forgroundBricksBMD, "brickscifi", ItemTab.BLOCK, false, true, 87, 4294967295);
            createBrick(88, ItemLayer.DECORATION, forgroundBricksBMD, "brickscifi", ItemTab.BLOCK, false, true, 88);
            createBrick(89, ItemLayer.DECORATION, forgroundBricksBMD, "brickscifi", ItemTab.BLOCK, false, true, 89);
            createBrick(90, ItemLayer.DECORATION, forgroundBricksBMD, "brickscifi", ItemTab.BLOCK, false, true, 90);
            createBrick(91, ItemLayer.DECORATION, forgroundBricksBMD, "brickscifi", ItemTab.BLOCK, false, true, 91);
            //var _loc_45:* = new ItemBrickPackage("prison", "Prison Blocks");
            createBrick(261, ItemLayer.ABOVE, decorationsBMD, "brickprison", ItemTab.DECORATIVE, false, false, 261 - 128, 0);
            createBrick(92, ItemLayer.FORGROUND, forgroundBricksBMD, "brickprison", ItemTab.BLOCK, false, true, 92);
            createBrick(550, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickprison", ItemTab.BACKGROUND, false, true, 50);
            createBrick(551, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickprison", ItemTab.BACKGROUND, false, true, 51);
            createBrick(552, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickprison", ItemTab.BACKGROUND, false, true, 52);
            createBrick(553, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickprison", ItemTab.BACKGROUND, false, true, 53);
            //var _loc_46:* = new ItemBrickPackage("windows", "Window Blocks");
            createBrick(262, ItemLayer.ABOVE, decorationsBMD, "brickwindow", ItemTab.DECORATIVE, false, false, 262 - 128, 0);
            createBrick(263, ItemLayer.ABOVE, decorationsBMD, "brickwindow", ItemTab.DECORATIVE, false, false, 263 - 128, 0);
            createBrick(264, ItemLayer.ABOVE, decorationsBMD, "brickwindow", ItemTab.DECORATIVE, false, false, 264 - 128, 0);
            createBrick(265, ItemLayer.ABOVE, decorationsBMD, "brickwindow", ItemTab.DECORATIVE, false, false, 265 - 128, 0);
            createBrick(266, ItemLayer.ABOVE, decorationsBMD, "brickwindow", ItemTab.DECORATIVE, false, false, 266 - 128, 0);
            createBrick(267, ItemLayer.ABOVE, decorationsBMD, "brickwindow", ItemTab.DECORATIVE, false, false, 267 - 128, 0);
            createBrick(268, ItemLayer.ABOVE, decorationsBMD, "brickwindow", ItemTab.DECORATIVE, false, false, 268 - 128, 0);
            createBrick(269, ItemLayer.ABOVE, decorationsBMD, "brickwindow", ItemTab.DECORATIVE, false, false, 269 - 128, 0);
            createBrick(270, ItemLayer.ABOVE, decorationsBMD, "brickwindow", ItemTab.DECORATIVE, false, false, 270 - 128, 0);
            // var _loc_47:* = new ItemBrickPackage("pirate", "Pirate Blocks");
            createBrick(93, ItemLayer.FORGROUND, forgroundBricksBMD, "brickpirate", ItemTab.BLOCK, false, true, 93);
            createBrick(94, ItemLayer.FORGROUND, forgroundBricksBMD, "brickpirate", ItemTab.BLOCK, false, true, 94);
            createBrick(271, ItemLayer.DECORATION, decorationsBMD, "brickpirate", ItemTab.DECORATIVE, false, false, 271 - 128, 0);
            createBrick(272, ItemLayer.DECORATION, decorationsBMD, "brickpirate", ItemTab.DECORATIVE, false, false, 272 - 128, 0);
            createBrick(554, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickpirate", ItemTab.BACKGROUND, false, false, 54);
            createBrick(555, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickpirate", ItemTab.BACKGROUND, false, false, 55);
            createBrick(556, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickpirate", ItemTab.BACKGROUND, false, false, 56);
            createBrick(557, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickpirate", ItemTab.BACKGROUND, false, false, 57);
            createBrick(558, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickpirate", ItemTab.BACKGROUND, false, false, 58);
            createBrick(559, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickpirate", ItemTab.BACKGROUND, false, false, 59);
            createBrick(560, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickpirate", ItemTab.BACKGROUND, false, false, 60);
            //var _loc_48:* = new ItemBrickPackage("vikings", "Viking Blocks");
            createBrick(95, ItemLayer.FORGROUND, forgroundBricksBMD, "brickviking", ItemTab.BLOCK, false, true, 95);
            createBrick(273, ItemLayer.DECORATION, decorationsBMD, "brickviking", ItemTab.DECORATIVE, false, false, 273 - 128, 0);
            createBrick(274, ItemLayer.DECORATION, decorationsBMD, "brickviking", ItemTab.DECORATIVE, false, false, 274 - 128, 0);
            createBrick(275, ItemLayer.DECORATION, decorationsBMD, "brickviking", ItemTab.DECORATIVE, false, false, 275 - 128, 0);
            createBrick(561, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickviking", ItemTab.BACKGROUND, false, false, 61);
            createBrick(562, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickviking", ItemTab.BACKGROUND, false, false, 62);
            createBrick(563, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickviking", ItemTab.BACKGROUND, false, false, 63);
            //var _loc_49:* = new ItemBrickPackage("ninja", "Ninja Blocks");
            createBrick(96, ItemLayer.DECORATION, forgroundBricksBMD, "brickninja", ItemTab.BLOCK, false, false, 96, 0);
            createBrick(97, ItemLayer.DECORATION, forgroundBricksBMD, "brickninja", ItemTab.BLOCK, false, false, 97, 0);
            createBrick(564, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickninja", ItemTab.BACKGROUND, false, true, 64);
            createBrick(565, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickninja", ItemTab.BACKGROUND, false, true, 65);
            createBrick(566, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickninja", ItemTab.BACKGROUND, false, true, 66);
            createBrick(567, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickninja", ItemTab.BACKGROUND, false, true, 67);
            createBrick(276, ItemLayer.DECORATION, decorationsBMD, "brickninja", ItemTab.DECORATIVE, false, false, 276 - 128, 0);
            createBrick(277, ItemLayer.DECORATION, decorationsBMD, "brickninja", ItemTab.DECORATIVE, false, false, 277 - 128, 0);
            createBrick(278, ItemLayer.DECORATION, decorationsBMD, "brickninja", ItemTab.DECORATIVE, false, false, 278 - 128, 0);
            createBrick(279, ItemLayer.DECORATION, decorationsBMD, "brickninja", ItemTab.DECORATIVE, false, false, 279 - 128, 0);
            createBrick(280, ItemLayer.DECORATION, decorationsBMD, "brickninja", ItemTab.DECORATIVE, false, false, 280 - 128, 0);
            createBrick(281, ItemLayer.DECORATION, decorationsBMD, "brickninja", ItemTab.DECORATIVE, false, false, 281 - 128, 0);
            createBrick(282, ItemLayer.DECORATION, decorationsBMD, "brickninja", ItemTab.DECORATIVE, false, false, 282 - 128, 0);
            createBrick(283, ItemLayer.DECORATION, decorationsBMD, "brickninja", ItemTab.DECORATIVE, false, false, 283 - 128, 0);
            createBrick(284, ItemLayer.DECORATION, decorationsBMD, "brickninja", ItemTab.DECORATIVE, false, false, 284 - 128, 0);
            //var _loc_50:* = new ItemBrickPackage("cowboy", "Cowboy Blocks");
            createBrick(122, ItemLayer.DECORATION, forgroundBricksBMD, "brickcowboy", ItemTab.BLOCK, false, false, 99, 0);
            createBrick(123, ItemLayer.DECORATION, forgroundBricksBMD, "brickcowboy", ItemTab.BLOCK, false, false, 100, 0);
            createBrick(124, ItemLayer.DECORATION, forgroundBricksBMD, "brickcowboy", ItemTab.BLOCK, false, false, 101, 0);
            createBrick(125, ItemLayer.DECORATION, forgroundBricksBMD, "brickcowboy", ItemTab.BLOCK, false, false, 102, 0);
            createBrick(126, ItemLayer.DECORATION, forgroundBricksBMD, "brickcowboy", ItemTab.BLOCK, false, false, 103, 0);
            createBrick(127, ItemLayer.DECORATION, forgroundBricksBMD, "brickcowboy", ItemTab.BLOCK, false, false, 104, 0);
            createBrick(568, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickcowboy", ItemTab.BACKGROUND, false, true, 68);
            createBrick(569, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickcowboy", ItemTab.BACKGROUND, false, true, 69);
            createBrick(570, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickcowboy", ItemTab.BACKGROUND, false, true, 70);
            createBrick(571, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickcowboy", ItemTab.BACKGROUND, false, true, 71);
            createBrick(572, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickcowboy", ItemTab.BACKGROUND, false, true, 72);
            createBrick(573, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickcowboy", ItemTab.BACKGROUND, false, true, 73);
            createBrick(285, ItemLayer.ABOVE, decorationsBMD, "brickcowboy", ItemTab.DECORATIVE, false, true, 285 - 128, 0);
            createBrick(286, ItemLayer.ABOVE, decorationsBMD, "brickcowboy", ItemTab.DECORATIVE, false, true, 286 - 128, 0);
            createBrick(287, ItemLayer.DECORATION, decorationsBMD, "brickcowboy", ItemTab.DECORATIVE, false, false, 287 - 128, 0);
            createBrick(288, ItemLayer.DECORATION, decorationsBMD, "brickcowboy", ItemTab.DECORATIVE, false, false, 288 - 128, 0);
            createBrick(289, ItemLayer.DECORATION, decorationsBMD, "brickcowboy", ItemTab.DECORATIVE, false, false, 289 - 128, 0);
            createBrick(290, ItemLayer.DECORATION, decorationsBMD, "brickcowboy", ItemTab.DECORATIVE, false, false, 290 - 128, 0);
            createBrick(291, ItemLayer.DECORATION, decorationsBMD, "brickcowboy", ItemTab.DECORATIVE, false, false, 291 - 128, 0);
            createBrick(292, ItemLayer.DECORATION, decorationsBMD, "brickcowboy", ItemTab.DECORATIVE, false, false, 292 - 128, 0);
            createBrick(293, ItemLayer.DECORATION, decorationsBMD, "brickcowboy", ItemTab.DECORATIVE, false, false, 293 - 128, 0);
            createBrick(294, ItemLayer.ABOVE, decorationsBMD, "brickcowboy", ItemTab.DECORATIVE, false, false, 294 - 128, 0);
            createBrick(295, ItemLayer.ABOVE, decorationsBMD, "brickcowboy", ItemTab.DECORATIVE, false, false, 295 - 128, 0);
            createBrick(296, ItemLayer.ABOVE, decorationsBMD, "brickcowboy", ItemTab.DECORATIVE, false, false, 296 - 128, 0);
            createBrick(297, ItemLayer.ABOVE, decorationsBMD, "brickcowboy", ItemTab.DECORATIVE, false, false, 297 - 128, 0);
            createBrick(298, ItemLayer.ABOVE, decorationsBMD, "brickcowboy", ItemTab.DECORATIVE, false, false, 298 - 128, 0);
            createBrick(299, ItemLayer.ABOVE, decorationsBMD, "brickcowboy", ItemTab.DECORATIVE, false, false, 299 - 128, 0);
            //var _loc_51:* = new ItemBrickPackage("plastic", "Plastic Blocks");
            /*createBrick(128, ItemLayer.DECORATION, forgroundBricksBMD, "brickplastic", ItemTab.BLOCK, false, true, 105);
            createBrick(129, ItemLayer.DECORATION, forgroundBricksBMD, "brickplastic", ItemTab.BLOCK, false, true, 106);
            createBrick(130, ItemLayer.DECORATION, forgroundBricksBMD, "brickplastic", ItemTab.BLOCK, false, true, 107);
            createBrick(131, ItemLayer.DECORATION, forgroundBricksBMD, "brickplastic", ItemTab.BLOCK, false, true, 108);
            createBrick(132, ItemLayer.DECORATION, forgroundBricksBMD, "brickplastic", ItemTab.BLOCK, false, true, 109);
            createBrick(133, ItemLayer.DECORATION, forgroundBricksBMD, "brickplastic", ItemTab.BLOCK, false, true, 110);
            createBrick(134, ItemLayer.DECORATION, forgroundBricksBMD, "brickplastic", ItemTab.BLOCK, false, true, 111);
            createBrick(135, ItemLayer.DECORATION, forgroundBricksBMD, "brickplastic", ItemTab.BLOCK, false, true, 112);*/
            createBrick(128, ItemLayer.FORGROUND, forgroundBricksBMD, "brickplastic", ItemTab.BLOCK, false, true, 105);
            createBrick(129, ItemLayer.FORGROUND, forgroundBricksBMD, "brickplastic", ItemTab.BLOCK, false, true, 106);
            createBrick(130, ItemLayer.FORGROUND, forgroundBricksBMD, "brickplastic", ItemTab.BLOCK, false, true, 107);
            createBrick(131, ItemLayer.FORGROUND, forgroundBricksBMD, "brickplastic", ItemTab.BLOCK, false, true, 108);
            createBrick(132, ItemLayer.FORGROUND, forgroundBricksBMD, "brickplastic", ItemTab.BLOCK, false, true, 109);
            createBrick(133, ItemLayer.FORGROUND, forgroundBricksBMD, "brickplastic", ItemTab.BLOCK, false, true, 110);
            createBrick(134, ItemLayer.FORGROUND, forgroundBricksBMD, "brickplastic", ItemTab.BLOCK, false, true, 111);
            createBrick(135, ItemLayer.FORGROUND, forgroundBricksBMD, "brickplastic", ItemTab.BLOCK, false, true, 112);
            //var _loc_52:* = new ItemBrickPackage("water", "Water Blocks");
            createBrick(ItemId.WATER, ItemLayer.ABOVE, waterBMD, "brickwater", ItemTab.ACTION, false, false, 0, 0);
            createBrick(ItemId.WAVE, ItemLayer.ABOVE, waveBMD, "brickwater", ItemTab.DECORATIVE, false, false, 0, 0);
            createBrick(574, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickwater", ItemTab.BACKGROUND, false, true, 74, 4285913831);
            createBrick(575, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickwater", ItemTab.BACKGROUND, false, true, 75, 4285913831);
            createBrick(576, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickwater", ItemTab.BACKGROUND, false, true, 76, 4285913831);
            createBrick(577, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickwater", ItemTab.BACKGROUND, false, true, 77, 4285913831);
            createBrick(578, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickwater", ItemTab.BACKGROUND, false, true, 78, 4285913831);
            //var _loc_53:* = new ItemBrickPackage("sand", "Sand Blocks");
            createBrick(137, ItemLayer.FORGROUND, forgroundBricksBMD, "bricksand", ItemTab.BLOCK, false, true, 114);
            createBrick(138, ItemLayer.FORGROUND, forgroundBricksBMD, "bricksand", ItemTab.BLOCK, false, true, 115);
            createBrick(139, ItemLayer.FORGROUND, forgroundBricksBMD, "bricksand", ItemTab.BLOCK, false, true, 116);
            createBrick(140, ItemLayer.FORGROUND, forgroundBricksBMD, "bricksand", ItemTab.BLOCK, false, true, 117);
            createBrick(141, ItemLayer.FORGROUND, forgroundBricksBMD, "bricksand", ItemTab.BLOCK, false, true, 118);
            createBrick(142, ItemLayer.FORGROUND, forgroundBricksBMD, "bricksand", ItemTab.BLOCK, false, true, 119);
            createBrick(579, ItemLayer.BACKGROUND, backgroundBricksBMD, "bricksand", ItemTab.BACKGROUND, false, false, 79);
            createBrick(580, ItemLayer.BACKGROUND, backgroundBricksBMD, "bricksand", ItemTab.BACKGROUND, false, false, 80);
            createBrick(581, ItemLayer.BACKGROUND, backgroundBricksBMD, "bricksand", ItemTab.BACKGROUND, false, false, 81);
            createBrick(582, ItemLayer.BACKGROUND, backgroundBricksBMD, "bricksand", ItemTab.BACKGROUND, false, false, 82);
            createBrick(583, ItemLayer.BACKGROUND, backgroundBricksBMD, "bricksand", ItemTab.BACKGROUND, false, false, 83);
            createBrick(584, ItemLayer.BACKGROUND, backgroundBricksBMD, "bricksand", ItemTab.BACKGROUND, false, false, 84);
            createBrick(301, ItemLayer.DECORATION, decorationsBMD, "bricksand", ItemTab.DECORATIVE, false, false, 300 - 128, 0);
            createBrick(302, ItemLayer.DECORATION, decorationsBMD, "bricksand", ItemTab.DECORATIVE, false, false, 301 - 128, 0);
            createBrick(303, ItemLayer.DECORATION, decorationsBMD, "bricksand", ItemTab.DECORATIVE, false, false, 302 - 128, 0);
            createBrick(304, ItemLayer.DECORATION, decorationsBMD, "bricksand", ItemTab.DECORATIVE, false, false, 303 - 128, 0);
            createBrick(305, ItemLayer.DECORATION, decorationsBMD, "bricksand", ItemTab.DECORATIVE, false, false, 304 - 128, 0);
            createBrick(306, ItemLayer.DECORATION, decorationsBMD, "bricksand", ItemTab.DECORATIVE, false, false, 305 - 128, 0);
            //var _loc_54:* = new ItemBrickPackage("summer 2012", "Summer 2012 Blocks");
            createBrick(307, ItemLayer.DECORATION, decorationsBMD, "bricksummer2012", ItemTab.DECORATIVE, false, false, 306 - 128, 0);
            createBrick(308, ItemLayer.DECORATION, decorationsBMD, "bricksummer2012", ItemTab.DECORATIVE, false, false, 307 - 128, 0);
            createBrick(309, ItemLayer.DECORATION, decorationsBMD, "bricksummer2012", ItemTab.DECORATIVE, false, false, 308 - 128, 0);
            createBrick(310, ItemLayer.DECORATION, decorationsBMD, "bricksummer2012", ItemTab.DECORATIVE, false, false, 309 - 128, 0);
            //var _loc_55:* = new ItemBrickPackage("cloud", "Cloud Blocks");
            createBrick(143, ItemLayer.FORGROUND, forgroundBricksBMD, "brickcloud", ItemTab.BLOCK, false, false, 120);
            createBrick(311, ItemLayer.ABOVE, decorationsBMD, "brickcloud", ItemTab.DECORATIVE, false, false, 310 - 128);
            createBrick(312, ItemLayer.ABOVE, decorationsBMD, "brickcloud", ItemTab.DECORATIVE, false, false, 311 - 128);
            createBrick(313, ItemLayer.ABOVE, decorationsBMD, "brickcloud", ItemTab.DECORATIVE, false, false, 312 - 128);
            createBrick(314, ItemLayer.ABOVE, decorationsBMD, "brickcloud", ItemTab.DECORATIVE, false, false, 313 - 128);
            createBrick(315, ItemLayer.ABOVE, decorationsBMD, "brickcloud", ItemTab.DECORATIVE, false, false, 314 - 128);
            createBrick(316, ItemLayer.ABOVE, decorationsBMD, "brickcloud", ItemTab.DECORATIVE, false, false, 315 - 128);
            createBrick(317, ItemLayer.ABOVE, decorationsBMD, "brickcloud", ItemTab.DECORATIVE, false, false, 316 - 128);
            createBrick(318, ItemLayer.ABOVE, decorationsBMD, "brickcloud", ItemTab.DECORATIVE, false, false, 317 - 128);
            //var _loc_56:* = new ItemBrickPackage("plate iron", "Plate Iron Blocks");
            createBrick(144, ItemLayer.FORGROUND, forgroundBricksBMD, "brickplateiron", ItemTab.BLOCK, false, false, 121);
            createBrick(145, ItemLayer.FORGROUND, forgroundBricksBMD, "brickplateiron", ItemTab.BLOCK, false, false, 122);
            createBrick(585, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickplateiron", ItemTab.BACKGROUND, false, false, 85);
            createBrick(586, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickplateiron", ItemTab.BACKGROUND, false, false, 86);
            createBrick(587, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickplateiron", ItemTab.BACKGROUND, false, false, 87);
            createBrick(588, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickplateiron", ItemTab.BACKGROUND, false, false, 88);
            createBrick(589, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickplateiron", ItemTab.BACKGROUND, false, false, 89);
            //var _loc_57:* = new ItemBrickPackage("signs", "Warning Sign Blocks");
            createBrick(319, ItemLayer.DECORATION, decorationsBMD, "bricksigns", ItemTab.DECORATIVE, false, false, 190, 0);
            createBrick(320, ItemLayer.DECORATION, decorationsBMD, "bricksigns", ItemTab.DECORATIVE, false, false, 191, 0);
            createBrick(321, ItemLayer.DECORATION, decorationsBMD, "bricksigns", ItemTab.DECORATIVE, false, false, 192, 0);
            createBrick(322, ItemLayer.DECORATION, decorationsBMD, "bricksigns", ItemTab.DECORATIVE, false, false, 193, 0);
            createBrick(323, ItemLayer.DECORATION, decorationsBMD, "bricksigns", ItemTab.DECORATIVE, false, false, 194, 0);
            createBrick(324, ItemLayer.DECORATION, decorationsBMD, "bricksigns", ItemTab.DECORATIVE, false, false, 195, 0);
            //var _loc_58:* = new ItemBrickPackage("industrial", "Industrial Blocks");
            createBrick(146, ItemLayer.ABOVE, forgroundBricksBMD, "brickindustrial", ItemTab.BLOCK, false, false, 123);
            createBrick(147, ItemLayer.ABOVE, forgroundBricksBMD, "brickindustrial", ItemTab.BLOCK, false, false, 124);
            createBrick(148, ItemLayer.ABOVE, forgroundBricksBMD, "brickindustrial", ItemTab.BLOCK, false, false, 125);
            createBrick(149, ItemLayer.FORGROUND, forgroundBricksBMD, "brickindustrial", ItemTab.BLOCK, false, false, 126);
            createBrick(150, ItemLayer.ABOVE, forgroundBricksBMD, "brickindustrial", ItemTab.BLOCK, false, false, 127);
            createBrick(151, ItemLayer.FORGROUND, forgroundBricksBMD, "brickindustrial", ItemTab.BLOCK, false, false, 128);
            createBrick(152, ItemLayer.FORGROUND, forgroundBricksBMD, "brickindustrial", ItemTab.BLOCK, false, false, 129);
            createBrick(153, ItemLayer.ABOVE, forgroundBricksBMD, "brickindustrial", ItemTab.BLOCK, false, false, 130);
            //var _loc_59:* = new ItemBrickPackage("timbered", "Timbered Blocks");
            createBrick(154, ItemLayer.ABOVE, forgroundBricksBMD, "bricktimbered", ItemTab.BLOCK, false, true, 131);
            createBrick(590, ItemLayer.BACKGROUND, backgroundBricksBMD, "bricktimbered", ItemTab.BACKGROUND, false, false, 90);
            createBrick(591, ItemLayer.BACKGROUND, backgroundBricksBMD, "bricktimbered", ItemTab.BACKGROUND, false, false, 91);
            createBrick(592, ItemLayer.BACKGROUND, backgroundBricksBMD, "bricktimbered", ItemTab.BACKGROUND, false, false, 92);
            createBrick(593, ItemLayer.BACKGROUND, backgroundBricksBMD, "bricktimbered", ItemTab.BACKGROUND, false, false, 93);
            createBrick(594, ItemLayer.BACKGROUND, backgroundBricksBMD, "bricktimbered", ItemTab.BACKGROUND, false, false, 94);
            createBrick(595, ItemLayer.BACKGROUND, backgroundBricksBMD, "bricktimbered", ItemTab.BACKGROUND, false, false, 95);
            createBrick(596, ItemLayer.BACKGROUND, backgroundBricksBMD, "bricktimbered", ItemTab.BACKGROUND, false, false, 96);
            createBrick(597, ItemLayer.BACKGROUND, backgroundBricksBMD, "bricktimbered", ItemTab.BACKGROUND, false, false, 97);
            createBrick(598, ItemLayer.BACKGROUND, backgroundBricksBMD, "bricktimbered", ItemTab.BACKGROUND, false, false, 98);
            //var _loc_60:* = new ItemBrickPackage("castle", "Castle Blocks");
            createBrick(158, ItemLayer.ABOVE, forgroundBricksBMD, "brickcastle", ItemTab.BLOCK, false, true, 132);
            createBrick(159, ItemLayer.FORGROUND, forgroundBricksBMD, "brickcastle", ItemTab.BLOCK, false, true, 133);
            createBrick(160, ItemLayer.FORGROUND, forgroundBricksBMD, "brickcastle", ItemTab.BLOCK, false, true, 134);
            createBrick(599, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickcastle", ItemTab.BACKGROUND, false, false, 99);
            createBrick(325, ItemLayer.DECORATION, decorationsBMD, "brickcastle", ItemTab.DECORATIVE, false, false, 196, 0);
            createBrick(326, ItemLayer.ABOVE, decorationsBMD, "brickcastle", ItemTab.DECORATIVE, false, false, 197);
            //var _loc_61:* = new ItemBrickPackage("medieval", "Medieval Blocks");
            createBrick(162, ItemLayer.ABOVE, forgroundBricksBMD, "brickmedieval", ItemTab.BLOCK, false, true, 136);
            createBrick(163, ItemLayer.ABOVE, forgroundBricksBMD, "brickmedieval", ItemTab.BLOCK, false, true, 137);
            createBrick(327, ItemLayer.DECORATION, decorationsBMD, "brickmedieval", ItemTab.DECORATIVE, false, false, 198, 0);
            createBrick(328, ItemLayer.DECORATION, decorationsBMD, "brickmedieval", ItemTab.DECORATIVE, false, false, 199, 0);
            createBrick(329, ItemLayer.DECORATION, decorationsBMD, "brickmedieval", ItemTab.DECORATIVE, false, false, 200, 0);
            createBrick(330, ItemLayer.DECORATION, decorationsBMD, "brickmedieval", ItemTab.DECORATIVE, false, false, 201, 0);
            createBrick(331, ItemLayer.DECORATION, decorationsBMD, "brickmedieval", ItemTab.DECORATIVE, false, false, 202, 0);
            createBrick(600, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickmedieval", ItemTab.BACKGROUND, false, false, 100);
            //var _loc_62:* = new ItemBrickPackage("pipes", "Pipe Blocks");
            createBrick(166, ItemLayer.FORGROUND, forgroundBricksBMD, "brickpipe", ItemTab.BLOCK, false, true, 140);
            createBrick(167, ItemLayer.FORGROUND, forgroundBricksBMD, "brickpipe", ItemTab.BLOCK, false, true, 141);
            createBrick(168, ItemLayer.FORGROUND, forgroundBricksBMD, "brickpipe", ItemTab.BLOCK, false, true, 142);
            createBrick(169, ItemLayer.FORGROUND, forgroundBricksBMD, "brickpipe", ItemTab.BLOCK, false, true, 143);
            createBrick(170, ItemLayer.FORGROUND, forgroundBricksBMD, "brickpipe", ItemTab.BLOCK, false, true, 144);
            createBrick(171, ItemLayer.FORGROUND, forgroundBricksBMD, "brickpipe", ItemTab.BLOCK, false, true, 145);
            //var _loc_63:* = new ItemBrickPackage("rocket", "Rocket Blocks");
            createBrick(172, ItemLayer.FORGROUND, forgroundBricksBMD, "brickrocket", ItemTab.BLOCK, false, true, 146);
            createBrick(173, ItemLayer.FORGROUND, forgroundBricksBMD, "brickrocket", ItemTab.BLOCK, false, true, 147);
            createBrick(174, ItemLayer.FORGROUND, forgroundBricksBMD, "brickrocket", ItemTab.BLOCK, false, true, 148);
            createBrick(175, ItemLayer.FORGROUND, forgroundBricksBMD, "brickrocket", ItemTab.BLOCK, false, true, 149);
            createBrick(601, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickrocket", ItemTab.BACKGROUND, false, true, 101);
            createBrick(602, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickrocket", ItemTab.BACKGROUND, false, true, 102);
            createBrick(603, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickrocket", ItemTab.BACKGROUND, false, true, 103);
            createBrick(604, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickrocket", ItemTab.BACKGROUND, false, true, 104);
            createBrick(332, ItemLayer.DECORATION, decorationsBMD, "brickrocket", ItemTab.DECORATIVE, false, false, 203, 0);
            createBrick(333, ItemLayer.DECORATION, decorationsBMD, "brickrocket", ItemTab.DECORATIVE, false, false, 204, 0);
            createBrick(334, ItemLayer.DECORATION, decorationsBMD, "brickrocket", ItemTab.DECORATIVE, false, false, 205, 0);
            createBrick(335, ItemLayer.DECORATION, decorationsBMD, "brickrocket", ItemTab.DECORATIVE, false, false, 206, 0);
            //var _loc_64:* = new ItemBrickPackage("mars", "Mars Blocks");
            createBrick(176, ItemLayer.FORGROUND, forgroundBricksBMD, "brickmars", ItemTab.BLOCK, false, true, 150, 4294945604);
            createBrick(177, ItemLayer.FORGROUND, forgroundBricksBMD, "brickmars", ItemTab.BLOCK, false, true, 151, 4292711483);
            createBrick(178, ItemLayer.FORGROUND, forgroundBricksBMD, "brickmars", ItemTab.BLOCK, false, true, 152, 4291200308);
            createBrick(179, ItemLayer.FORGROUND, forgroundBricksBMD, "brickmars", ItemTab.BLOCK, false, true, 153, 4287717671);
            createBrick(180, ItemLayer.ABOVE, forgroundBricksBMD, "brickmars", ItemTab.BLOCK, false, false, 154, 0);
            createBrick(181, ItemLayer.ABOVE, forgroundBricksBMD, "brickmars", ItemTab.BLOCK, false, false, 155, 0);
            createBrick(605, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickmars", ItemTab.BACKGROUND, false, true, 105, 4278460021);
            createBrick(606, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickmars", ItemTab.BACKGROUND, false, true, 106, 4278460021);
            createBrick(607, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickmars", ItemTab.BACKGROUND, false, true, 107, 4278460021);
            createBrick(336, ItemLayer.ABOVE, decorationsBMD, "brickmars", ItemTab.DECORATIVE, false, false, 207, 0);
            //var _loc_65:* = new ItemBrickPackage("monster", "Monster Blocks");
            createBrick(608, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickmonster", ItemTab.BACKGROUND, false, true, 108, 4288716897);
            createBrick(609, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickmonster", ItemTab.BACKGROUND, false, true, 109, 4285558852);
            createBrick(338, ItemLayer.ABOVE, decorationsBMD, "brickmonster", ItemTab.DECORATIVE, false, false, 208, 0);
            createBrick(339, ItemLayer.ABOVE, decorationsBMD, "brickmonster", ItemTab.DECORATIVE, false, false, 209, 0);
            createBrick(340, ItemLayer.ABOVE, decorationsBMD, "brickmonster", ItemTab.DECORATIVE, false, false, 210, 0);
            createBrick(341, ItemLayer.DECORATION, decorationsBMD, "brickmonster", ItemTab.DECORATIVE, false, false, 211, 0);
            createBrick(342, ItemLayer.DECORATION, decorationsBMD, "brickmonster", ItemTab.DECORATIVE, false, false, 212, 0);
            //var _loc_66:* = new ItemBrickPackage("fog", "Fog Blocks");
            createBrick(343, ItemLayer.ABOVE, decorationsBMD, "brickfog", ItemTab.DECORATIVE, false, false, 213, 0);
            createBrick(344, ItemLayer.ABOVE, decorationsBMD, "brickfog", ItemTab.DECORATIVE, false, false, 214, 0);
            createBrick(345, ItemLayer.ABOVE, decorationsBMD, "brickfog", ItemTab.DECORATIVE, false, false, 215, 0);
            createBrick(346, ItemLayer.ABOVE, decorationsBMD, "brickfog", ItemTab.DECORATIVE, false, false, 216, 0);
            createBrick(347, ItemLayer.ABOVE, decorationsBMD, "brickfog", ItemTab.DECORATIVE, false, false, 217, 0);
            createBrick(348, ItemLayer.ABOVE, decorationsBMD, "brickfog", ItemTab.DECORATIVE, false, false, 218, 0);
            createBrick(349, ItemLayer.ABOVE, decorationsBMD, "brickfog", ItemTab.DECORATIVE, false, false, 219, 0);
            createBrick(350, ItemLayer.ABOVE, decorationsBMD, "brickfog", ItemTab.DECORATIVE, false, false, 220, 0);
            createBrick(351, ItemLayer.ABOVE, decorationsBMD, "brickfog", ItemTab.DECORATIVE, false, false, 221, 0);
            //var _loc_67:* = new ItemBrickPackage("halloween 2012", "Halloween 2012 Blocks");
            createBrick(352, ItemLayer.ABOVE, decorationsBMD, "brickhw2012", ItemTab.DECORATIVE, false, true, 222, 0);
            createBrick(353, ItemLayer.DECORATION, decorationsBMD, "brickhw2012", ItemTab.DECORATIVE, false, true, 223, 0);
            createBrick(354, ItemLayer.DECORATION, decorationsBMD, "brickhw2012", ItemTab.DECORATIVE, false, true, 224, 0);
            createBrick(355, ItemLayer.DECORATION, decorationsBMD, "brickhw2012", ItemTab.DECORATIVE, false, true, 225, 0);
            createBrick(356, ItemLayer.DECORATION, decorationsBMD, "brickhw2012", ItemTab.DECORATIVE, false, true, 226, 0);
            //var _loc_68:* = new ItemBrickPackage("checker", "Checker Blocks");
            createBrick(186, ItemLayer.DECORATION, forgroundBricksBMD, "brickchecker", ItemTab.BLOCK, false, true, 161, 4285229931);
            createBrick(187, ItemLayer.DECORATION, forgroundBricksBMD, "brickchecker", ItemTab.BLOCK, false, true, 162, 4281291665);
            createBrick(188, ItemLayer.DECORATION, forgroundBricksBMD, "brickchecker", ItemTab.BLOCK, false, true, 163, 4286594449);
            createBrick(189, ItemLayer.DECORATION, forgroundBricksBMD, "brickchecker", ItemTab.BLOCK, false, true, 164, 4289206591);
            createBrick(190, ItemLayer.DECORATION, forgroundBricksBMD, "brickchecker", ItemTab.BLOCK, false, true, 165, 4289442611);
            createBrick(191, ItemLayer.DECORATION, forgroundBricksBMD, "brickchecker", ItemTab.BLOCK, false, true, 166, 4282753847);
            createBrick(192, ItemLayer.DECORATION, forgroundBricksBMD, "brickchecker", ItemTab.BLOCK, false, true, 167, 4282167980);
            //var _loc_69:* = new ItemBrickPackage("jungle ruins", "Jungle Ruin Blocks");
            createBrick(193, ItemLayer.ABOVE, forgroundBricksBMD, "brickjungleruins", ItemTab.BLOCK, false, true, 168);
            createBrick(194, ItemLayer.ABOVE, forgroundBricksBMD, "brickjungleruins", ItemTab.BLOCK, false, true, 169);
            createBrick(195, ItemLayer.FORGROUND, forgroundBricksBMD, "brickjungleruins", ItemTab.BLOCK, false, true, 170, 4288256378);
            createBrick(196, ItemLayer.FORGROUND, forgroundBricksBMD, "brickjungleruins", ItemTab.BLOCK, false, true, 171, 4289491041);
            createBrick(197, ItemLayer.FORGROUND, forgroundBricksBMD, "brickjungleruins", ItemTab.BLOCK, false, true, 172, 4284647578);
            createBrick(198, ItemLayer.FORGROUND, forgroundBricksBMD, "brickjungleruins", ItemTab.BLOCK, false, true, 173, 4287071297);
            createBrick(617, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickjungleruins", ItemTab.BACKGROUND, false, true, 117, 4284900945);
            createBrick(618, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickjungleruins", ItemTab.BACKGROUND, false, true, 118, 4286008900);
            createBrick(619, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickjungleruins", ItemTab.BACKGROUND, false, true, 119, 4282473062);
            createBrick(620, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickjungleruins", ItemTab.BACKGROUND, false, true, 120, 4285229108);
            //var _loc_70:* = new ItemBrickPackage("jungle", "Jungle Blocks");
            createBrick(199, ItemLayer.ABOVE, forgroundBricksBMD, "brickjungle", ItemTab.BLOCK, false, true, 176);
            createBrick(621, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickjungle", ItemTab.BACKGROUND, false, true, 121);
            createBrick(622, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickjungle", ItemTab.BACKGROUND, false, true, 122);
            createBrick(623, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickjungle", ItemTab.BACKGROUND, false, true, 123);
            createBrick(357, ItemLayer.ABOVE, decorationsBMD, "brickjungle", ItemTab.DECORATIVE, false, true, 227);
            createBrick(358, ItemLayer.DECORATION, decorationsBMD, "brickjungle", ItemTab.DECORATIVE, false, true, 228);
            createBrick(359, ItemLayer.DECORATION, decorationsBMD, "brickjungle", ItemTab.DECORATIVE, false, true, 229);
            //var _loc_73:* = new ItemBrickPackage("christmas 2012", "Christmas 2012 Blocks");
            createBrick(624, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickxmas2012", ItemTab.BACKGROUND, false, true, 124, 4292381209);
            createBrick(625, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickxmas2012", ItemTab.BACKGROUND, false, true, 125, 4283728909);
            createBrick(626, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickxmas2012", ItemTab.BACKGROUND, false, true, 126, 4280236504);
            createBrick(362, ItemLayer.DECORATION, decorationsBMD, "brickxmas2012", ItemTab.DECORATIVE, false, true, 230, 0);
            createBrick(363, ItemLayer.DECORATION, decorationsBMD, "brickxmas2012", ItemTab.DECORATIVE, false, true, 231, 0);
            createBrick(364, ItemLayer.DECORATION, decorationsBMD, "brickxmas2012", ItemTab.DECORATIVE, false, true, 232, 0);
            createBrick(365, ItemLayer.DECORATION, decorationsBMD, "brickxmas2012", ItemTab.DECORATIVE, false, true, 233, 0);
            createBrick(366, ItemLayer.DECORATION, decorationsBMD, "brickxmas2012", ItemTab.DECORATIVE, false, true, 234, 0);
            createBrick(367, ItemLayer.DECORATION, decorationsBMD, "brickxmas2012", ItemTab.DECORATIVE, false, true, 235, 0);
            //var _loc_74:* = new ItemBrickPackage("lava", "Lava Blocks");
            createBrick(202, ItemLayer.FORGROUND, forgroundBricksBMD, "bricklava", ItemTab.BLOCK, false, true, 177, 4294954558);
            createBrick(203, ItemLayer.FORGROUND, forgroundBricksBMD, "bricklava", ItemTab.BLOCK, false, true, 178, 4294612750);
            createBrick(204, ItemLayer.FORGROUND, forgroundBricksBMD, "bricklava", ItemTab.BLOCK, false, true, 179, 4294926080);
            createBrick(627, ItemLayer.BACKGROUND, backgroundBricksBMD, "bricklava", ItemTab.BACKGROUND, false, true, 127, 4291601203);
            createBrick(628, ItemLayer.BACKGROUND, backgroundBricksBMD, "bricklava", ItemTab.BACKGROUND, false, true, 128, 4291196171);
            createBrick(629, ItemLayer.BACKGROUND, backgroundBricksBMD, "bricklava", ItemTab.BACKGROUND, false, true, 129, 4290198016);
            //var _loc_75:* = new ItemBrickPackage("swamp", "Swamp Blocks");
            createBrick(370, ItemLayer.ABOVE, mudbubbleBMD, "brickswamp", ItemTab.DECORATIVE, false, false, 5, 0);
            createBrick(630, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickswamp", ItemTab.BACKGROUND, false, false, 130, 4284504612);
            createBrick(371, ItemLayer.ABOVE, decorationsBMD, "brickswamp", ItemTab.DECORATIVE, false, false, 236, 0);
            createBrick(372, ItemLayer.ABOVE, decorationsBMD, "brickswamp", ItemTab.DECORATIVE, false, false, 237, 0);
            createBrick(373, ItemLayer.DECORATION, decorationsBMD, "brickswamp", ItemTab.DECORATIVE, false, false, 238, 0);
            //var _loc_76:* = new ItemBrickPackage("scifi 2013", "Sci-fi 2013 Blocks");
            createBrick(375, ItemLayer.DECORATION, glowylineblueslopeBMD, "brickscifi2013", ItemTab.DECORATIVE, false, true, 1, 4289259775);
            createBrick(376, ItemLayer.DECORATION, glowylinebluestraightBMD, "brickscifi2013", ItemTab.DECORATIVE, false, true, 1, 4289259775);
            createBrick(379, ItemLayer.DECORATION, glowylinegreenslopeBMD, "brickscifi2013", ItemTab.DECORATIVE, false, true, 1, 4289789814);
            createBrick(380, ItemLayer.DECORATION, glowylinegreenstraightBMD, "brickscifi2013", ItemTab.DECORATIVE, false, true, 1, 4289789814);
            createBrick(377, ItemLayer.DECORATION, glowylineyellowslopeBMD, "brickscifi2013", ItemTab.DECORATIVE, false, true, 1, 4294891351);
            createBrick(378, ItemLayer.DECORATION, glowylineyellowstraightBMD, "brickscifi2013", ItemTab.DECORATIVE, false, true, 1, 4294891351);
            createBrick(637, ItemLayer.BACKGROUND, backgroundBricksBMD, "brickscifi2013", ItemTab.BACKGROUND, false, true, 131, 4285758849);
            //var _loc_77:* = new ItemBrickPackage("sparta", "Sparta Blocks");
            createBrick(382, ItemLayer.DECORATION, decorationsBMD, "bricksparta", ItemTab.DECORATIVE, false, true, 239);
            createBrick(383, ItemLayer.DECORATION, decorationsBMD, "bricksparta", ItemTab.DECORATIVE, false, true, 240);
            createBrick(384, ItemLayer.DECORATION, decorationsBMD, "bricksparta", ItemTab.DECORATIVE, false, true, 241);
            createBrick(208, ItemLayer.FORGROUND, forgroundBricksBMD, "bricksparta", ItemTab.BLOCK, false, true, 180, 4291678675);
            createBrick(209, ItemLayer.FORGROUND, forgroundBricksBMD, "bricksparta", ItemTab.BLOCK, false, true, 181, 4290895033);
            createBrick(210, ItemLayer.FORGROUND, forgroundBricksBMD, "bricksparta", ItemTab.BLOCK, false, true, 182, 4293248719);
            createBrick(211, ItemLayer.DECORATION, forgroundBricksBMD, "bricksparta", ItemTab.BLOCK, false, true, 183, 4290889673);
            createBrick(638, ItemLayer.BACKGROUND, backgroundBricksBMD, "bricksparta", ItemTab.BACKGROUND, false, false, 132, 4286020477);
            createBrick(639, ItemLayer.BACKGROUND, backgroundBricksBMD, "bricksparta", ItemTab.BACKGROUND, false, false, 133, 4285563247);
            createBrick(640, ItemLayer.BACKGROUND, backgroundBricksBMD, "bricksparta", ItemTab.BACKGROUND, false, false, 134, 4286805627);
            //var _loc_78:* = new ItemBrickPackage("sign", "BC Text Sign");
            createBrick(385, ItemLayer.DECORATION, decorationsBMD, "buildersclub", ItemTab.ACTION, true, true, 242);
            //var _loc_79:* = new ItemBrickPackage("farm", "Farm blocks");
            createBrick(386, ItemLayer.ABOVE, decorationsBMD, "brickfarm", ItemTab.DECORATIVE, false, true, 243);
            createBrick(387, ItemLayer.ABOVE, decorationsBMD, "brickfarm", ItemTab.DECORATIVE, false, true, 244);
            createBrick(388, ItemLayer.ABOVE, decorationsBMD, "brickfarm", ItemTab.DECORATIVE, false, true, 245);
            createBrick(389, ItemLayer.ABOVE, decorationsBMD, "brickfarm", ItemTab.DECORATIVE, false, true, 246);
            createBrick(212, ItemLayer.DECORATION, forgroundBricksBMD, "brickfarm", ItemTab.BLOCK, false, true, 184, 4291608181);
        }

    }
}
