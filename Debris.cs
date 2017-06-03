using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
/// <summary>
/// You might be wondering...why is this separate from the explosions class?
/// Well, I want to test out lists here. A lot of this game is just a way for me to practice
/// using C# and getting comfortable with it.
/// </summary>

namespace DiskWars
{
    public class Debris
    {
        public double x;
        public double y; //the velocity and direction determines these coordinates, as you might guess
        public int type;

        public double bounce; //Remember, this value is about how high the object is off the ground.
        public double yVelocity; //self-explanatory, I think...

        public double direction; //in radians
        public double velocity;

        public int size; //always square-shaped debris. Not in the mood to make rectangular or circular stuff.
        public bool active; //If the object is active, then it gets drawn and movement/bounce logic is applied to it.
        private int frame; //frame of the animation
        public float RotationAngle = 0f; //also in radians
        public string RotationDir = "Right";
        private int flickerTick = 0;

        private int gemFrame = 0; //Some gem logic. Yes, the gems are in the debris class. Also used for bombdrops


        public Debris()
        {
            active = false;
            x = 0; y = 0; type = 0; bounce = 0; direction = 0; velocity = 0;
            size = 16; frame = 0;
        }


        public Rectangle GetSourceRectangle(int tileIndex, int frames = 4, int size = 16) //This allows us to find the correct sprite in the file
        {
            return new Rectangle((tileIndex % frames) * size, Convert.ToInt16(tileIndex / frames) * size, size, size);
        }

        public void Draw()
        {
            if (type == -1) //broken part with trail of fire behind it.
            {
                if (frame < 250)
                {
                    Game1.spriteBatch.Draw(Game1.Debris1, new Vector2((int)x - (int)(0.5 * size), (int)y - (int)(0.5 * size)), GetSourceRectangle(Game1.rnd.Next(0, 3)), Color.Black * 0.3f, RotationAngle, new Vector2(size / 2, size / 2), 1.0f, SpriteEffects.None, 0f);
                    Game1.spriteBatch.Draw(Game1.Debris1, new Vector2((int)x - (int)(0.5 * size), (int)y - (int)(0.5 * size) - (int)bounce), GetSourceRectangle(Game1.rnd.Next(0, 3)), Color.White, RotationAngle, new Vector2(size / 2, size / 2), 1.0f, SpriteEffects.None, 0f);
                }
                else
                {
                    if (flickerTick < 2) Game1.spriteBatch.Draw(Game1.Debris1, new Vector2((int)x - (int)(0.5 * size), (int)y - (int)(0.5 * size)), GetSourceRectangle(Game1.rnd.Next(0, 3)), Color.Black * 0.3f, RotationAngle, new Vector2(size / 2, size / 2), 1.0f, SpriteEffects.None, 0f);
                    if (flickerTick < 2) Game1.spriteBatch.Draw(Game1.Debris1, new Vector2((int)x - (int)(0.5 * size), (int)y - (int)(0.5 * size) - (int)bounce), GetSourceRectangle(Game1.rnd.Next(0, 3)), Color.White, RotationAngle, new Vector2(size / 2, size / 2), 1.0f, SpriteEffects.None, 0f);
                }
            }
            if (type == 0 || type == 1) //broken floppy disk parts
            {
                if (frame < 250)
                {
                    Game1.spriteBatch.Draw(Game1.Debris1, new Vector2((int)x - (int)(0.5 * size), (int)y - (int)(0.5 * size)), GetSourceRectangle(Game1.rnd.Next(0, 3) + type * 3), Color.Black * 0.3f, RotationAngle, new Vector2(size / 2, size / 2), 1.0f, SpriteEffects.None, 0f);
                    Game1.spriteBatch.Draw(Game1.Debris1, new Vector2((int)x - (int)(0.5 * size), (int)y - (int)(0.5 * size) - (int)bounce), GetSourceRectangle(Game1.rnd.Next(0, 3) + type * 3), Color.White, RotationAngle, new Vector2(size / 2, size / 2), 1.0f, SpriteEffects.None, 0f);
                }
                else
                {
                    if (flickerTick < 2) Game1.spriteBatch.Draw(Game1.Debris1, new Vector2((int)x - (int)(0.5 * size), (int)y - (int)(0.5 * size)), GetSourceRectangle(Game1.rnd.Next(0, 3) + type * 3), Color.Black * 0.3f, RotationAngle, new Vector2(size / 2, size / 2), 1.0f, SpriteEffects.None, 0f);
                    if (flickerTick < 2) Game1.spriteBatch.Draw(Game1.Debris1, new Vector2((int)x - (int)(0.5 * size), (int)y - (int)(0.5 * size) - (int)bounce), GetSourceRectangle(Game1.rnd.Next(0, 3) + type * 3), Color.White, RotationAngle, new Vector2(size / 2, size / 2), 1.0f, SpriteEffects.None, 0f);
                }
            }
            if (type == 2) //Those lovely gems that you pick up sometimes.
            {
                if (frame < 250)
                {
                    Game1.spriteBatch.Draw(Game1.Gems, new Rectangle((int)x, (int)y + 8, 16, 8),
                    new Rectangle(gemFrame * 16, 16, 16, 16), Color.DarkBlue * 0.3f);
                    Game1.spriteBatch.Draw(Game1.Gems, new Rectangle((int)x, (int)y - (int)bounce, 16, 16),
                    new Rectangle(gemFrame * 16, 16, 16, 16), Color.White * 0.6f);
                    Game1.spriteBatch.Draw(Game1.Gems, new Rectangle((int)x, (int)y - (int)bounce, 16, 16),
                    new Rectangle(gemFrame * 16, 0, 16, 16), Color.White);
                }
                else if (frame > 250 && flickerTick < 2)
                {
                    Game1.spriteBatch.Draw(Game1.Gems, new Rectangle((int)x, (int)y + 8, 16, 8),
                    new Rectangle(gemFrame * 16, 16, 16, 16), Color.DarkBlue * 0.3f);
                    Game1.spriteBatch.Draw(Game1.Gems, new Rectangle((int)x, (int)y - (int)bounce, 16, 16),
                    new Rectangle(gemFrame * 16, 16, 16, 16), Color.White * 0.6f);
                    Game1.spriteBatch.Draw(Game1.Gems, new Rectangle((int)x, (int)y - (int)bounce, 16, 16),
                    new Rectangle(gemFrame * 16, 0, 16, 16), Color.White);
                }
            }
            if (type == 3) //Bombs
            {

                Game1.spriteBatch.Draw(Game1.Gems, new Rectangle((int)x, (int)y + 8, 16, 8), //too lazy to rename gem sprites
                new Rectangle(gemFrame * 16, 64, 16, 16), Color.DarkBlue * 0.3f);
                Game1.spriteBatch.Draw(Game1.Gems, new Rectangle((int)x, (int)y - (int)bounce, 16, 16),
                new Rectangle(gemFrame * 16, 64, 16, 16), Color.White);
            }
            if (type == 4) //Placed bombs (can't pick these up)
            {
                Game1.spriteBatch.Draw(Game1.Gems, new Rectangle((int)x, (int)y - (int)bounce, 16, 16),
                new Rectangle(16+ 16 * (Game1.rnd.Next(0, 2)), 48, 16, 16), Color.White);
            }
        }

        public void Logic()
        {
            x += Math.Cos(direction) * velocity;
            y += Math.Sin(direction) * velocity;
            velocity *= 0.98;

            bounce += yVelocity;
            yVelocity -= 0.2;
            if (bounce < 0)
            {
                bounce = 0;
                yVelocity = 0 - 0.5 * yVelocity;
                //if (yVelocity < 0.1) yVelocity = 0;
            }

            if (RotationDir == "Left") {
                RotationAngle -= Convert.ToSingle(velocity)/25f;
            }
            if (RotationDir == "Right")
            {
                RotationAngle += Convert.ToSingle(velocity) / 25f;
            }

            if (x > 340 || y > 260 || x < -20 || y < -20)
            {
                active = false;
            }

            frame++;
            flickerTick++;
            if (flickerTick > 3) flickerTick = 0;
            if (flickerTick == 3)
            {
                gemFrame++;
                if (gemFrame > 7) gemFrame = 0;
            }

            if (type == -1)//fire trail debris
            {
                if (frame % 10 == 0)
                {
                    var clsa = new Explosions(x - 8, y - 8 - bounce, -1);
                    Game1.gameExplosions.Add(clsa);
                }
            }

            if (type == 4) //placed bomb
            {
                if (frame > 250)
                {
                    int newx = (int)((x / 16) + 0.5);
                    int newy = (int)((y / 16) + 0.5);

                    active = false;
                    Game1.KilledEnemy.Play(0.4f, 0.0f, 0.0f); //play explosion sound.
                    for (int exY = 0; exY < 3; exY++) //cause explosions in a 3x3 grid AROUND the bomb.
                    {
                        for (int exX = 0; exX < 3; exX++)
                        {
                            Game1.isScreenShaking = true;
                            var clsa = new Explosions(16*newx - 16 + exX*16 + 8, 16*newy - 16 + exY * 16, 4);
                            Game1.gameExplosions.Add(clsa);
                            if (GameMap.Index[newy - 1 + exY, newx - 1 + exX] == 62) GameMap.Index[newy - 1 + exY, newx - 1 + exX] = 40; //Change barrel to broken barrel if it's in the area.
                        }
                    }
                    //Now, we check to see if any players were in the explosion area. If so, they lose some HP.
                    foreach (var clsEnemy in Game1.Opponents)
                    {
                        if (clsEnemy.CheckPlayerCollision(16 * newx - 20, 16*newy - 20, 16*3 + 8, 16*3 + 8) == true) clsEnemy.DamageDealt(10, "Up", 14, false); 
                        //The checking area is a little bigger than a 3x3 grid.
                    }
                    if (Game1.Hunter.CheckPlayerCollision(16 * newx - 20, 16 * newy - 20, 16 * 3 + 8, 16 * 3 + 8) == true) Game1.Hunter.DamageDealt(10, "Up", false);
                }

            }

            if (type == 2 && bounce < 2) //Checking to see if the player is within pickup range of a gem.
            {
                int checkX = (int)x + 2;
                int checkY = (int)y + 2;
                int sizeX = 12;
                int sizeY = 12;
                bool pickupRange = false;


                if (Game1.Hunter.x + 3 >= checkX && Game1.Hunter.x + 3 <= checkX + sizeX && Game1.Hunter.y + 7>= checkY && Game1.Hunter.y + 7<= checkY + sizeY) pickupRange = true;
                if (Game1.Hunter.x + 12 >= checkX && Game1.Hunter.x + 12 <= checkX + sizeX && Game1.Hunter.y + 7>= checkY && Game1.Hunter.y + 7<= checkY + sizeY) pickupRange = true;
                if (Game1.Hunter.x + 3 >= checkX && Game1.Hunter.x + 3 <= checkX + sizeX && Game1.Hunter.y + 15 >= checkY && Game1.Hunter.y + 15 <= checkY + sizeY) pickupRange = true;
                if (Game1.Hunter.x + 12 >= checkX && Game1.Hunter.x + 12 <= checkX + sizeX && Game1.Hunter.y + 15 >= checkY && Game1.Hunter.y + 15 <= checkY + sizeY) pickupRange = true;
                if (pickupRange == true)
                {
                    active = false; //deactivate that gem if it's within pickup range!
                    Game1.Hunter.score += 60 + Game1.Hunter.gemComboCounter * 20;
                    var clsa = new Explosions(x, y, 3);
                    Game1.gameExplosions.Add(clsa);
                    Game1.GemPickup.Play(1.0f, -0.1f + (Convert.ToSingle(Game1.Hunter.gemComboCounter) / 10f), 0.0f); //and play the sound effect!
                    Game1.Hunter.gemComboCounter += 1;
                    if (Game1.Hunter.gemComboCounter > 8) Game1.Hunter.gemComboCounter = 8;
                    Game1.Hunter.gemComboCooldown = 190;
                    if (GameMap.VictoryCondition == "Get Gems") GameMap.ObjectsRemaining -= 1;
                }
            }
            if (type == 3 && bounce < 2) //Checking to see if the player is within pickup range of a bomb.
            {
                int checkX = (int)x + 2;
                int checkY = (int)y + 2;
                int sizeX = 12;
                int sizeY = 12;
                bool pickupRange = false;


                if (Game1.Hunter.x + 3 >= checkX && Game1.Hunter.x + 3 <= checkX + sizeX && Game1.Hunter.y + 7 >= checkY && Game1.Hunter.y + 7 <= checkY + sizeY) pickupRange = true;
                if (Game1.Hunter.x + 12 >= checkX && Game1.Hunter.x + 12 <= checkX + sizeX && Game1.Hunter.y + 7 >= checkY && Game1.Hunter.y + 7 <= checkY + sizeY) pickupRange = true;
                if (Game1.Hunter.x + 3 >= checkX && Game1.Hunter.x + 3 <= checkX + sizeX && Game1.Hunter.y + 15 >= checkY && Game1.Hunter.y + 15 <= checkY + sizeY) pickupRange = true;
                if (Game1.Hunter.x + 12 >= checkX && Game1.Hunter.x + 12 <= checkX + sizeX && Game1.Hunter.y + 15 >= checkY && Game1.Hunter.y + 15 <= checkY + sizeY) pickupRange = true;
                if (pickupRange == true)
                {
                    active = false; //deactivate that bomb if it's within pickup range!
                    Game1.Hunter.score += 50;
                    var clsa = new Explosions(x, y, 3);
                    Game1.gameExplosions.Add(clsa);
                    Game1.GemPickup.Play(1.0f, -0.1f, 0.0f); //and play the sound effect!
                    Game1.Hunter.addBomb();
                }
            }

            if (type == 3) frame = 1; //We don't let the bomb disappear because it's a required item in some levels.
            if (frame > 300) active = false; //The debris isn't allowed to stay on the screen forever.

        }
    }
}
