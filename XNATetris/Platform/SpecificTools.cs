using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//functions specific to XNA or C# i did not now where esle to put
namespace XNATetris
{
    class SpecificTools
    {
        public enum Align
        {
            Left,
            Center,
            Right,
        }

        public static long GetSystemTime()
        {
            return System.DateTime.Now.Ticks/10000;
        }

        public static void QuickDrawString(string output, int x, int y, SpriteFont f, Color c, Align a)
        {
            QuickDrawString(output, x, y, f, c, a, false, Color.White);
        }

        public static void QuickDrawString(string output, int x, int y, SpriteFont f, Color c, Align a, bool withTag, Color tagColor )
        {
            if (a == Align.Right)
                x -= (int)f.MeasureString(output).X;
            else if (a == Align.Center)
                x -= (int)f.MeasureString(output).X/2;

            // Draw the tag
            if (withTag)
            {
                int padding = f == TetrisGame.FontBig ? 10 : 3;
                Rectangle r = new Rectangle(x - padding, y - padding, (int)f.MeasureString(output).X + padding * 2, (int)f.MeasureString(output).Y + padding*2);
                TetrisGame.spriteBatch.Draw(TetrisGame.Pixel, r, tagColor);
                TetrisGame.ShapeManager.DrawRect(TetrisGame.Pixel, TetrisGame.spriteBatch, r, c);
            }

            // Draw the string
            TetrisGame.spriteBatch.DrawString(f, output, new Vector2(x, y), c);
        }
    }
}
