using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace DiskWars
{
    //This is the code for the fireballs that the player shoots, 
    //as well as the tiny extension of the player's basic hammer attack. (it's not as fun is the hammer has a really tiny range.)

    //Sorry for how this is handled! I've learned so much since I started making this game.
    public class Missiles
    {
        public double x=100;
        public double y=100;
        public string Dir;
        public int type = 0;
        private int sizex;
        private int sizey;
        public int speed;
        public bool active;// { get; set; }
        private int animTick = 0;
        public int lifetime = 0;

        public Missiles(int whichKind=0, int whereX=0, int whereY=0, string whichDir="Down", int whichSpeed = 1)
        {
            active = false; //You need to activate this instance somewhere else if you need to use it!
            x = (double)whereX;
            y = (double)whereY;
            type = whichKind;
            Dir = whichDir;
            speed = whichSpeed;

            if (type == 0)
            {
                sizex = 7;
                sizey = 7;
            }
            if (type == 1)
            {
                sizex = 16;
                sizey = 16;
            }
        }


        public void Logic()
        {
            bool collided = false;
            for (int k = 0; k < (speed + 1); k++) {
                if (type == 0) //The code below is so ugly I don't fully understand it anymore (5/24/17)
                {
                    //...so as I'm creating the logic code for the attack wave "missiles", I'm just going to make its own code separate.
                    switch (Dir)
                    {

                        case "Up": //y - 1
                            if (GameMap.HaveCollision(3 + (int)x - (sizex / 2), 3 + (int)y - ((int)(sizey / 2)) - 1, sizex - 3, sizey - 3) == false)
                            {
                                y -= 1;
                            }
                            else collided = true;
                            break;
                        case "Down": //y + 1
                            if (GameMap.HaveCollision(3 + (int)x - (sizex / 2), 3 + (int)y - ((int)(sizey / 2)) + 1, sizex - 3, sizey - 3) == false)
                            {
                                y += 1;
                            }
                            else collided = true;
                            break;
                        case "Left": //x - 1
                                     //I reverse sizex and sizey here for projectiles that are rectangular in shape.
                            if (GameMap.HaveCollision(3 + (int)x - (sizey / 2) - 1, 3 + (int)y - ((int)(sizex / 2)), sizey - 3, sizex - 3) == false)
                            {
                                x -= 1;
                            }
                            else collided = true;
                            break;
                        case "Right": //x + 1
                            if (GameMap.HaveCollision(3 + (int)x - (sizey / 2) + 1, 3 + (int)y - ((int)(sizex / 2)), sizey - 3, sizex - 3) == false)
                            {
                                x += 1;
                            }
                            else collided = true;
                            break;
                        default:
                            break;
                    }
                }
                if (type == 1) //attack wave logic
                {
                    switch (Dir)
                    {
                        case "Up": //y - 1
                            if (GameMap.HaveCollision(4 + (int)x, 4 + (int)y - 4 - 1, sizex - 4, sizey - 4) == false)
                            {
                                y -= 1;
                            }
                            else collided = true;
                            break;
                        case "Down": //y + 1
                            if (GameMap.HaveCollision(4 + (int)x, 4 + (int)y - 4 + 1, sizex - 4, sizey - 4) == false)
                            {
                                y += 1;
                            }
                            else collided = true;
                            break;
                        case "Left": //x - 1
                                     //I reverse sizex and sizey here for projectiles that are rectangular in shape.
                            if (GameMap.HaveCollision(4 + (int)x - 1, 4 + (int)y - 4, sizey - 4, sizex - 4) == false)
                            {
                                x -= 1;
                            }
                            else collided = true;
                            break;
                        case "Right": //x + 1
                            if (GameMap.HaveCollision(4 + (int)x + 1, 4 + (int)y - 4, sizey - 4, sizex - 4) == false)
                            {
                                x += 1;
                            }
                            else collided = true;
                            break;
                        default:
                            break;
                    }
                }

            }
            if (collided == true)
            {
                active = false;

                //Only cause that cool explosion if we're shooting a fireball.
                if (type == 0)
                {
                    var clsa = new Explosions(x, y, 0);
                    Game1.gameExplosions.Add(clsa);
                }
            }
            else
            {
                CheckAttackHit();
            }
            animTick++;
            lifetime++;
            if (type == 1)
            {
                if (lifetime > 10) active = false;
            }
            if (animTick > 8) animTick = 0;
        }


        private Rectangle GetSourceRectangle(int tileIndex, int frames = 3, int size = 30) //This allows us to find the correct sprite in the file
        {
            return new Rectangle((tileIndex % frames) * size, Convert.ToInt16(tileIndex / frames) * size, size, size);
        }

        public void Draw()
        {
            if (type == 0)
            {
                Game1.spriteBatch.Draw(Game1.AttackSprites, new Rectangle((int)x - 15, (int)y - 15, 30, 30), GetSourceRectangle(1 + 4 * (Game1.DirDict[Dir]) + 16 + ((int)animTick/3), 4), Color.White);
            }
            if (type == 1)
            {
                switch (Dir)
                {
                    case "Up":
                        Game1.spriteBatch.Draw(Game1.AttackWave, new Rectangle((int)x, (int)y, 16, 16), new Rectangle(32, 0, 40, 16), Color.White);
                        break;
                    case "Down":
                        Game1.spriteBatch.Draw(Game1.AttackWave, new Rectangle((int)x, (int)y, 16, 16), new Rectangle(32,16, 40, 16), Color.White);
                        break;
                    case "Left":
                        Game1.spriteBatch.Draw(Game1.AttackWave, new Rectangle((int)x, (int)y, 16, 16), new Rectangle(16, 0, 16, 48), Color.White);
                        break;
                    case "Right":
                        Game1.spriteBatch.Draw(Game1.AttackWave, new Rectangle((int)x, (int)y, 16, 16), new Rectangle(0, 0, 16, 48), Color.White);
                        break;
                }
            }
        }

        public void CheckAttackHit()
        {
            int checkX = 0;
            int checkY = 0;
            int sizeX = 0;
            int sizeY = 0;
            if (type == 0) //basic fireball
            {
                switch (Dir)
                {
                    case "Down": //Those random 15s you see? That's the original draw coords of the fireball. 
                        checkX = (int)x - 15 + 5;
                        checkY = (int)y - 15 + 10;
                        sizeX = 16;
                        sizeY = 20;
                        break;
                    case "Up":
                        checkX = (int)x - 15 + 5;
                        checkY = (int)y - 15;
                        sizeX = 16;
                        sizeY = 20;
                        break;
                    case "Left":
                        checkX = (int)x - 15;
                        checkY = (int)y - 15 + 7;
                        sizeX = 14;
                        sizeY = 11;
                        break;
                    case "Right":
                        checkX = (int)x - 15 + 16;
                        checkY = (int)y - 15 + 7;
                        sizeX = 14;
                        sizeY = 11;
                        break;
                }
            }
            if (type == 1) //small attack wave
            {
                checkX = (int)x;
                checkY = (int)y;
                sizeX = 16;
                sizeY = 16;
            }

            foreach (var cls in Game1.Opponents)
            {
                //floppy enemy collision area: x + 2, size 11, y, size 8
                bool foundHit = false; //This ugly list of if statements below...it's to check the four corners of the enemy hitbox. Sorry for the ugliness.
                if (cls.x + cls.colX >= checkX && cls.x + cls.colX <= checkX + sizeX && cls.y + cls.colY >= checkY && cls.y + cls.colY <= checkY + sizeY) foundHit = true;
                if (cls.x + cls.colX + cls.sizeX >= checkX && cls.x + cls.colX + cls.sizeX <= checkX + sizeX && cls.y + cls.colY >= checkY && cls.y + cls.colY <= checkY + sizeY) foundHit = true;
                if (cls.x + cls.colX >= checkX && cls.x + cls.colX <= checkX + sizeX && cls.y + cls.colY + cls.sizeY >= checkY && cls.y + cls.colY + cls.sizeY <= checkY + sizeY) foundHit = true;
                if (cls.x + cls.colX + cls.sizeX >= checkX && cls.x + cls.colX + cls.sizeX <= checkX + sizeX && cls.y + cls.colY + cls.sizeY >= checkY && cls.y + cls.colY + cls.sizeY <= checkY + sizeY) foundHit = true;

                if (foundHit == true)
                {
                    var shouldWeKnockBackEnemy = true;
                    if (cls.type == 4) shouldWeKnockBackEnemy = false;

                    //The fireball is more powerful than the normal hammer attack, because it's harder to aim and slow.
                    if (type == 0) cls.DamageDealt(2 + Game1.Hunter.gemComboCounter, Dir, 14, shouldWeKnockBackEnemy);
                    if (type == 1) cls.DamageDealt(8 + Game1.Hunter.gemComboCounter, Dir, 14, shouldWeKnockBackEnemy); 
                    active = false;
                    //Explosions.CreateExplosion(this.x, this.y, 0);
                    if (type == 0)
                    {
                        var clsa = new Explosions(x, y, 0);
                        Game1.gameExplosions.Add(clsa);
                    }
                }
            }
            Game1.Opponents.RemoveAll(cls => cls.active == false);

        }
    }
}
