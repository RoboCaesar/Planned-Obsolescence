using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DiskWars
{
    public class Bullets //Like the debris class, but some specialization
    {
        public double x;
        public double y; //the velocity and direction determines these coordinates, as you might guess
        public int type;

        public double direction; //in radians
        public double velocity;

        public int size; //always square-shaped debris. Not in the mood to make rectangular or circular stuff.
        public bool active; //If the object is active, then it gets drawn and movement/bounce logic is applied to it.
        private int frame; //frame of the animation
        private int lengthOfExistence = 0;
        private int flickerTick = 0;


        public Bullets(double cx, double cy, int ctype, double cdir, double cvelocity)
        {
            active = true;
            x = cx; y = cy; type = ctype; direction = cdir; velocity = cvelocity;
            if (type == 0) size = 16;
            frame = 0;
        }

        //public Rectangle GetSourceRectangle(int tileIndex, int frames = 10, int size = 16) //This allows us to find the correct sprite in the file
        //{
        //    return new Rectangle((tileIndex % frames) * size, Convert.ToInt16(tileIndex / frames) * size, size, size);
        //}

        public void Draw()
        {
            if (type == 0) //electric attack
            {

                Game1.spriteBatch.Draw(Game1.ElectricAttacks, new Rectangle((int)x - 4, (int)y - 5, 16, 16), new Rectangle(frame * 16, 0, 16, 16), Color.White);
            }
            if (type == 1) //cd attack
            {

                Game1.spriteBatch.Draw(Game1.cdAttacks, new Rectangle((int)x - 4, (int)y - 5, 16, 16), new Rectangle(frame * 16, 0, 16, 16), Color.White);
            }
        }

        public void Logic()
        {
            x += Math.Cos(direction) * velocity;
            y += Math.Sin(direction) * velocity;

            if (type == 0)  //default screen attack
            {
                if (flickerTick == 0) frame++;
                if (frame > 9) frame = 0;
                lengthOfExistence++;
                if (GameMap.HaveCollision((int)x + 4, (int)y + 3, 2, 2) == true && lengthOfExistence > 8) //The enemy's bullets can penetrate walls for the first moments of their existence.
                {
                    active = false;
                    var cls = new Explosions(x + 4, y + 4, 5);
                    Game1.gameExplosions.Add(cls);
                }

                int checkX = (int)x;
                int checkY = (int)y;
                int sizeX = 10;
                int sizeY = 8;
                if (Game1.Hunter.CheckPlayerCollision(checkX, checkY, sizeX, sizeY) == true)
                {
                    active = false;
                    var cls = new Explosions(x + 4, y + 4, 5);
                    Game1.gameExplosions.Add(cls);
                    Game1.Hunter.DamageDealt(5, "Up", false);
                }

                flickerTick++;
                if (flickerTick > 1) flickerTick = 0;
            }
            if (type == 1)  //default screen attack
            {
                if (flickerTick == 0) frame++;
                if (frame > 13) frame = 0;
                lengthOfExistence++;
                if (GameMap.HaveCollision((int)x + 4, (int)y + 3, 2, 2) == true && lengthOfExistence > 8) //The enemy's bullets can penetrate walls for the first moments of their existence.
                {
                    active = false;
                    var cls = new Explosions(x + 4, y + 4, 5);
                    Game1.gameExplosions.Add(cls);
                }

                int checkX = (int)x;
                int checkY = (int)y;
                int sizeX = 10;
                int sizeY = 8;
                if (Game1.Hunter.CheckPlayerCollision(checkX, checkY, sizeX, sizeY) == true)
                {
                    active = false;
                    var cls = new Explosions(x + 4, y + 4, 5);
                    Game1.gameExplosions.Add(cls);
                    Game1.Hunter.DamageDealt(5, "Up", false);
                }

                flickerTick++;
                if (flickerTick > 1) flickerTick = 0;
            }
        }
    }
}
