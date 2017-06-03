using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace DiskWars
{

    public class Explosions
    {




        public double x;
        public double y;
        public int type;
        public bool active;
        public int lifespanCount;
        public int size = 32;
        public int slowdowntick = 0;
        //public static explosions[] ExpArray = new explosions[200];


        public void Draw(int undermode=0)
        {
            if (active == true)
            {
                if (type == -1 && undermode == 0) //explosion with fire trail. Added later. That's why it has the weird designation of "-1"
                {
                    Game1.spriteBatch.Draw(Game1.ExplosionSprites, new Rectangle((int)x - 4, (int)y - 4, 8, 8),
                        new Rectangle(lifespanCount*8, 128, 8, 8), Color.White);
                }
                if (type == 0 && undermode == 0) //normal explosion
                {
                    Game1.spriteBatch.Draw(Game1.ExplosionSprites, new Rectangle((int)x - 16, (int)y - 16, 32, 32), 
                        GetSourceRectangle((int)(lifespanCount)), Color.White);
                }
                if (type == 1 && undermode == 1) //cracked ground from big hammer
                {
                    Game1.spriteBatch.Draw(Game1.CrackedGround, new Rectangle((int)x - 75, (int)y - 75, 150, 150), GetSourceRectangle((int)(lifespanCount/4), 12,150), Color.White * 1.0f);
                }
                if (type == 2 && undermode == 0) //other normal explosion
                {
                    Game1.spriteBatch.Draw(Game1.ExplosionSprites, new Rectangle((int)x - 16, (int)y - 16, 32, 32),
                        GetSourceRectangle((int)(lifespanCount) + 14, 14), Color.White);
                }
                if (type == 3 && undermode == 0) //gem disappearing
                {
                    Game1.spriteBatch.Draw(Game1.Gems, new Rectangle((int)x, (int)y, 16, 16),
                        GetSourceRectangle((int)(lifespanCount) + 16, 8, 16), Color.White);
                }
                if (type == 4 && undermode == 0) //normal explosion number 3
                {
                    Game1.spriteBatch.Draw(Game1.ExplosionSprites, new Rectangle((int)x - 16, (int)y - 16, 32, 32),
                        GetSourceRectangle((int)(lifespanCount) + 28, 14), Color.White);
                }
                if (type == 5 && undermode == 0) //electrical explosion
                {
                    Game1.spriteBatch.Draw(Game1.ElectricAttacks, new Rectangle((int)x - 8, (int)y - 8, 16, 16),
                        GetSourceRectangle((int)(lifespanCount) + 10, 10, 16), Color.White);
                }
                if (type == 6 && undermode == 0) //Tile explosion
                {
                    Game1.spriteBatch.Draw(Game1.ExplosionSprites, new Rectangle((int)x - 8, (int)y - 16, 32, 32),
                        GetSourceRectangle((int)(lifespanCount) + 42, 14), Color.White);
                }
                if (type == 7 && undermode == 0) //electrical explosion 2
                {
                    Game1.spriteBatch.Draw(Game1.ElectricAttacks, new Rectangle((int)x - 8, (int)y - 8, 16, 16),
                        GetSourceRectangle((int)(lifespanCount) + 20, 10, 16), Color.White);
                }
            }
        }

        private Rectangle GetSourceRectangle(int tileIndex, int frames=12, int size=32) //This allows us to find the correct sprite in the file
        {
            return new Rectangle((tileIndex % frames) * size, Convert.ToInt16(tileIndex / frames) * size, size, size);
        }

        public void Logic()
        {
            slowdowntick++;
            if (slowdowntick > 1) slowdowntick = 0;
            if (active == true)
            {
                if (type == -1)
                {
                    lifespanCount++;
                    if (lifespanCount > 13) active = false;
                }
                if (type == 0)
                {
                    lifespanCount++;
                    y -= 2;
                    if (lifespanCount > 11) active = false;
                }
                if (type == 1)
                {
                    Game1.isScreenShaking = true;
                    lifespanCount++;
                    if (lifespanCount > 47)
                    {
                        Game1.isScreenShaking = false;
                        active = false;
                    }
                }
                if (type == 2)
                {
                    if (slowdowntick == 0) lifespanCount++;
                    y -= 1;
                    if (lifespanCount > 13) active = false;
                }
                if (type == 3)
                {
                    if (slowdowntick == 0) lifespanCount++;
                    y -= 1;
                    if (lifespanCount > 7) active = false;
                }
                if (type == 4) //another normal explosion. Doesn't float up
                {
                    if (slowdowntick == 0) lifespanCount++;
                    if (lifespanCount > 13) active = false;
                }
                if (type == 5) //electrical explosion
                {
                    if (slowdowntick == 0) lifespanCount++;
                    if (lifespanCount > 9) active = false;
                }
                if (type == 6)
                {
                    if (slowdowntick == 0) lifespanCount++;
                    y -= 1;
                    if (lifespanCount > 13) active = false;
                }
                if (type == 7) //electrical explosion 2
                {
                    lifespanCount++;
                    if (lifespanCount > 9) active = false;
                }
            }
        }

        public Explosions(double cx = 0, double cy = 0, int ctype = 0)
        {
            active = true;
            x = cx;
            y = cy;
            type = ctype;
            lifespanCount = 0;
            if (type == 0) size = 32;
            if (type == 5) size = 16;
            if (type == 7) size = 16;
        }

    }
}
