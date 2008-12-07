using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNATetris
{
    class XNAShape
    {
        GraphicsDevice myDevice;

        public XNAShape(GraphicsDevice myDevice)
        {
            this.myDevice = myDevice;
        }


        //Calculates the distances and the angle and than draws a line
        public void DrawLine(Texture2D pixel, SpriteBatch sprite, Vector2 start, Vector2 end, Color color)
        {
            int distance = (int)Vector2.Distance(start, end);

            Vector2 connection = end - start;
            Vector2 baseVector = new Vector2(1, 0);

            float alpha = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);

            if (pixel != null)
                sprite.Draw(pixel, new Rectangle((int)start.X, (int)start.Y, distance, 1),
                    null, color, alpha, new Vector2(0, 0), SpriteEffects.None, 0);
        }

        //Draws a rect with the help of DrawLine
        public void DrawRect(Texture2D pixel, SpriteBatch sprite, Rectangle rect, Color color)
        {
            // | left
            DrawLine(pixel, sprite, new Vector2(rect.X, rect.Y), new Vector2(rect.X, rect.Y + rect.Height), color);
            // - top
            DrawLine(pixel, sprite, new Vector2(rect.X, rect.Y), new Vector2(rect.X + rect.Width, rect.Y), color);
            // - bottom
            DrawLine(pixel, sprite, new Vector2(rect.X, rect.Y + rect.Height),
                new Vector2(rect.X + rect.Width, rect.Y + rect.Height), color);
            // | right
            DrawLine(pixel, sprite, new Vector2(rect.X + rect.Width, rect.Y),
                new Vector2(rect.X + rect.Width, rect.Y + rect.Height), color);

        }
    }
}