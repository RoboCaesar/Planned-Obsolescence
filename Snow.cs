using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DiskWars
{
    public class Snow
    {
        public double x { get; set; }
        public double y { get; set; }
        private double yVelocity;
        private int type = 0; //just for drawing purposes
        
        public Snow()
        {
            type = Game1.rnd.Next(0, 3);
            x = (double)Game1.rnd.Next(0, Game1.SCREEN_W);
            y = (double)Game1.rnd.Next(0, Game1.SCREEN_H);
            yVelocity = Game1.rnd.NextDouble() * 8.00 + 2;
        }
        public void Logic()
        {
            y += yVelocity;
            if (y > Game1.SCREEN_H)
            {
                y = -4;
                yVelocity = Game1.rnd.NextDouble() * 4.00 + 1;
            }
            x += 1 + (double)Game1.rnd.Next(0, 2);
            if (x > Game1.SCREEN_W) x = -4;
        }
        public void Draw()
        {
            if (type == 0) //tiny snowflake
            {
                Game1.spriteBatch.Draw(Game1.SnowSprites, new Rectangle((int)x, (int)y, 4, 4), new Rectangle(0, 0, 4, 4), Color.White);
            }
            if (type == 1) // snowflakes
            {
                Game1.spriteBatch.Draw(Game1.SnowSprites, new Rectangle((int)x, (int)y, 4, 4), new Rectangle(4, 0, 4, 4), Color.White);
            }
            if (type == 2) 
            {
                Game1.spriteBatch.Draw(Game1.SnowSprites, new Rectangle((int)x, (int)y, 4, 4), new Rectangle(4, 4, 4, 4), Color.White);
            }
            if (type == 3) 
            {
                Game1.spriteBatch.Draw(Game1.SnowSprites, new Rectangle((int)x, (int)y, 4, 4), new Rectangle(0, 4, 4, 4), Color.White);
            }
        }
    }
}
