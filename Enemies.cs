using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace DiskWars
{
    public partial class Enemies
    {

        public int type; //default floppy enemy if 1

        public string currentAction = "Do Nothing";
        public int lengthOfAction = 0;
        private int slowerTick = 0;
        public bool active = true;

        public bool spawning = false; //need a variable for the spawning effect. My God...so many variables. I'd better finish this quickly or I won't be able to read my own code.
        public int spawnTimer = 0;

        public int x { get; set; }
        public int y { get; set; }
        public int colX { get; set; } //These five variables help us know the collision box coordinates of the enemy. 
        public int colY { get; set; } //Useful when dealing with different types seamlessly
        public int sizeX { get; set; }
        public int sizeY { get; set; }
        public int dimensions;

        public int Frame { get; set; }
        protected int Tick = 0;
        protected int animDir = 0;
        public bool Moving = false;
        protected string Dir = "Down";
        public bool attacking = false;
        protected int attackStyle;
        protected int attackFrame = 0;

        public int HP = 1;

        protected int preCharging;
        protected float weaponCharging = 0.0f;
        protected int walkingSoundTimer = 0;
        protected int chargingSoundTimer = 0;
        public Missiles[] Projectiles = new Missiles[4];

        public double bounce = 0;
        protected double yVelocity = 0;
        public bool bouncing = false;


        public bool hurtInvincible = false;
        private int hurtInvincibleTimer = 0;
        private int hurtframe = 0;

        //private int testCooldown = 0;

        public int KnockbackTime = 0;
        public bool BeingKnockedback = false;
        public string KnockbackDir = "Up";

        public int spawnflash = -1;
        public int spawnfrequency = 400;
        public int enemyspawntimer = 0;


        public Enemies(int cx, int cy, int selectType = 1)
        {
            type = selectType;
            x = cx;
            y = cy;
            if (type == 1) //floppy size info. So important!
            {
                colX = 2;
                colY = 0;
                sizeX = 11;
                sizeY = 8;
                HP = 1;
                dimensions = 16; //it's always a square shape!
            }
            if (type == 2) //fast reddish floppy disk
            {
                colX = 2;
                colY = 0;
                sizeX = 11;
                sizeY = 8;
                HP = 1;
                dimensions = 16; //it's always a square shape!
            }
            if (type == 3) //old "mac"
            {
                colX = 3;
                colY = 3;
                sizeX = 14;
                sizeY = 8;
                HP = 10;
                dimensions = 20; //it's always a square shape!
            }
            if (type == 4) //Enemy spawner tile. It doesn't move, and you can't see it unless you're attacking it.
            {
                colX = 1;
                colY = 1;
                sizeX = 14;
                sizeY = 14;
                HP = 30; //A bit tough to destroy.
                dimensions = 16;
            }
        }

        public Rectangle GetSourceRectangle(int tileIndex, int frames = 3, int size = 30) //This allows us to find the correct sprite in the file
        {
            return new Rectangle((tileIndex % frames) * size, Convert.ToInt16(tileIndex / frames) * size, size, size);
        }
        public void Draw()
        {
            switch(type)
            {
                case 1: //standard floppy
                    if (attacking == false)
                    {  //collision area: x + 2, size 11, y, size 8
                        if (spawning == false)
                        {
                            Game1.spriteBatch.Draw(Game1.Sprites, new Rectangle(x - 7, y - 18, 30, 30), GetSourceRectangle(12), Color.White);
                            Game1.spriteBatch.Draw(Game1.FloppySprites, new Rectangle(x, y - 7 - (int)bounce, 16, 16), GetSourceRectangle(Frame + 3 * Game1.DirDict[Dir], 3, 16), Color.White);
                        }
                        else
                        {
                            for (int k = 0; k < 16; k++)
                            {
                                if (k % 2 == 0)
                                {
                                    Game1.spriteBatch.Draw(Game1.FloppySprites, new Rectangle(x + (spawnTimer / 3), y - 7 - (int)bounce + k, 16, 1),
                                    new Rectangle((Frame + 3 * Game1.DirDict[Dir] % 3) * 16, Convert.ToInt16(Frame + 3 * Game1.DirDict[Dir] / 3) * 16 + k, 16, 1), Color.White * (Convert.ToSingle(30 - spawnTimer) / 30f));
                                }
                                else
                                {
                                    Game1.spriteBatch.Draw(Game1.FloppySprites, new Rectangle(x - (spawnTimer / 3), y - 7 - (int)bounce + k, 16, 1),
                                    new Rectangle((Frame + 3 * Game1.DirDict[Dir] % 3) * 16, Convert.ToInt16(Frame + 3 * Game1.DirDict[Dir] / 3) * 16 + k, 16, 1), Color.White * (Convert.ToSingle(30 - spawnTimer) / 30f));
                                }
                            }
                        }
                    }
                    break;
                case 2: //fast fire floppy
                    if (attacking == false)
                    {  //collision area: x + 2, size 11, y, size 8
                        if (spawning == false)
                        {
                            Game1.spriteBatch.Draw(Game1.Sprites, new Rectangle(x - 7, y - 18, 30, 30), GetSourceRectangle(12), Color.White);
                            Game1.spriteBatch.Draw(Game1.FastFloppySprites, new Rectangle(x, y - 7 - (int)bounce, 16, 16), GetSourceRectangle(Frame + 3 * Game1.DirDict[Dir], 3, 16), Color.White);
                        }
                        else
                        {
                            for (int k = 0; k < 16; k++)
                            {
                                if (k % 2 == 0)
                                {
                                    Game1.spriteBatch.Draw(Game1.FastFloppySprites, new Rectangle(x + (spawnTimer / 3), y - 7 - (int)bounce + k, 16, 1),
                                    new Rectangle((Frame + 3 * Game1.DirDict[Dir] % 3) * 16, Convert.ToInt16(Frame + 3 * Game1.DirDict[Dir] / 3) * 16 + k, 16, 1), Color.White * (Convert.ToSingle(30 - spawnTimer) / 30f));
                                }
                                else
                                {
                                    Game1.spriteBatch.Draw(Game1.FastFloppySprites, new Rectangle(x - (spawnTimer / 3), y - 7 - (int)bounce + k, 16, 1),
                                    new Rectangle((Frame + 3 * Game1.DirDict[Dir] % 3) * 16, Convert.ToInt16(Frame + 3 * Game1.DirDict[Dir] / 3) * 16 + k, 16, 1), Color.White * (Convert.ToSingle(30 - spawnTimer) / 30f));
                                }
                            }
                        }
                    }
                    break;
                case 3: //old mac "Honeycrisp"
                    if (attacking == false)
                    {  //collision area: x + 2, size 11, y, size 8
                        if (hurtInvincible == false)
                        {
                            if (spawning == false)
                            {
                                Game1.spriteBatch.Draw(Game1.Sprites, new Rectangle(x - 7, y - 18, 30, 30), GetSourceRectangle(12), Color.White);
                                Game1.spriteBatch.Draw(Game1.HoneyCrispSprites, new Rectangle(x, y - 7 - (int)bounce, 20, 20), GetSourceRectangle(Frame + 3 * Game1.DirDict[Dir], 3, 20), Color.White);
                            }
                            else
                            {
                                for (int k = 0; k < 20; k++)
                                {
                                    if (k % 2 == 0)
                                    {
                                        Game1.spriteBatch.Draw(Game1.HoneyCrispSprites, new Rectangle(x + (spawnTimer / 3), y - 7 - (int)bounce + k, 16, 1),
                                        new Rectangle((Frame + 3 * Game1.DirDict[Dir] % 3) * 20, Convert.ToInt16(Frame + 3 * Game1.DirDict[Dir] / 3) * 20 + k, 20, 1), Color.White * (Convert.ToSingle(30 - spawnTimer) / 30f));
                                    }
                                    else
                                    {
                                        Game1.spriteBatch.Draw(Game1.HoneyCrispSprites, new Rectangle(x - (spawnTimer / 3), y - 7 - (int)bounce + k, 16, 1),
                                        new Rectangle((Frame + 3 * Game1.DirDict[Dir] % 3) * 20, Convert.ToInt16(Frame + 3 * Game1.DirDict[Dir] / 3) * 20 + k, 20, 1), Color.White * (Convert.ToSingle(30 - spawnTimer) / 30f));
                                    }
                                }
                            }
                        }

                        else {
                            Game1.spriteBatch.Draw(Game1.Sprites, new Rectangle(x - 7, y - 18, 30, 30), GetSourceRectangle(12), Color.White);
                            if (hurtframe < 3)
                                Game1.spriteBatch.Draw(Game1.HoneyCrispSprites, new Rectangle(x, y - 7 - (int)bounce, 20, 20), GetSourceRectangle(Frame + 3 * Game1.DirDict[Dir], 3, 20), Color.White);
                            else Game1.spriteBatch.Draw(Game1.HoneyCrispSprites, new Rectangle(x, y - 7 - (int)bounce, 20, 20), GetSourceRectangle(Frame + 3 * Game1.DirDict[Dir] + 12, 3, 20), Color.White);
                        }
                    }
                    if (currentAction == "Attack!")
                    {
                        Game1.spriteBatch.Draw(Game1.ElectricAttacks, new Rectangle((int)x, (int)y - 7 - (int)bounce, 16, 16), new Rectangle(Game1.rnd.Next(0,9) * 16, 0, 16, 16), Color.White);
                    }
                    break;
                case 4: //enemy spawner tile...I think
                    if (hurtInvincible == true)
                    {
                        Game1.spriteBatch.Draw(Game1.ExplosionSprites, new Rectangle((int)x, (int)y, 16, 16),
                        new Rectangle(8, 112, 16, 16), Color.White);
                    }
                    if (spawnflash > -1)
                    {
                        spawnflash++;
                        Game1.spriteBatch.Draw(Tile.TileSetTexture, new Rectangle(x, y, 16, 16), Tile.GetSourceRectangle(1),
                         Color.White * Convert.ToSingle(Math.Sin(3.14 * ((double)spawnflash / 30))));
                        if (spawnflash > 30) spawnflash = -1;
                    }
  
                    break;
            }

        }


        public void eLogic() //just the logic for the dumb floppy disk below. Look at other files for other enemy's logic data
        {

            if (HP <= 0 && active == true && type == 1) //First, make sure the enemy is actually alive still.
            {
                active = false;
                Game1.Hunter.score += 5;
                if (GameMap.VictoryCondition == "Destroy Enemies") GameMap.ObjectsRemaining -= 1;
                //Explosions.CreateExplosion(x + 7, y, 0);
                var clsa = new Explosions(x + 7, y, 2);
                Game1.gameExplosions.Add(clsa);

                Game1.WasAnEnemyKilledThisFrame = true;
                for (int k = 0; k < 4; k++)
                {
                    var cls2 = new Debris(); //cls2 is a temp debris class
                    Game1.gameDebris.Add(cls2);

                    cls2.direction = Game1.rnd.NextDouble() * 2.00 * Math.PI;
                    cls2.velocity = Game1.rnd.NextDouble() * 3.00;
                    cls2.yVelocity = Game1.rnd.NextDouble() * 3.00 + 2;
                    cls2.x = x + 8;
                    cls2.y = y + 8;
                    cls2.active = true;
                    cls2.RotationAngle = Convert.ToSingle(Game1.rnd.NextDouble() * 2 * Math.PI);
                    var tempdir = Game1.rnd.Next(0, 2);
                    if (tempdir == 0) cls2.RotationDir = "Left";
                    if (tempdir == 1) cls2.RotationDir = "Right";
                    cls2.type = Game1.rnd.Next(0, 2);
                }

                //gem drop code
                var cls3 = new Debris(); //cls3 is a temp debris class
                Game1.gameDebris.Add(cls3);
                cls3.direction = Game1.rnd.NextDouble() * 2.00 * Math.PI;
                cls3.velocity = Game1.rnd.NextDouble() * 0.30;
                cls3.yVelocity = Game1.rnd.NextDouble() * 3.00 + 2;
                cls3.x = x + 8;
                cls3.y = y + 8;
                cls3.active = true;
                cls3.type = 2;
                return;
            }

            if (HP <= 0 && active == true && type == 4) //spawner death
            {
                active = false;
                Game1.Hunter.score += 50;
                var clsa = new Explosions(x, y, 6);
                Game1.gameExplosions.Add(clsa);
                if (GameMap.VictoryCondition == "Destroy Spawners") GameMap.ObjectsRemaining -= 1;
                Game1.WasAnEnemyKilledThisFrame = true;
                GameMap.Index[y / 16, x / 16] = 7;
                return;
            }

            if (type == 4) enemyspawntimer += 1;
            if (type == 4 && enemyspawntimer >= spawnfrequency) //Enemy spawners
            {
                enemyspawntimer = 0;
                spawnflash = 0; //Setting it to 0 causes the flash effect on the map tile.
                Game1.PleaseAddEnemies = true;
                if (GameMap.Index[y/Game1.TILE_H, x/Game1.TILE_W] == 2) //basic floppy disk
                {

                    var clsb = new Enemies(x, y + 6, 1);
                    Game1.TempOpponents.Add(clsb);
                    clsb.spawning = true;
                    clsb.spawnTimer = 30;
                    clsb.currentAction = "Do Nothing";
                    clsb.lengthOfAction = 100;
                }
                if (GameMap.Index[y / Game1.TILE_H, x / Game1.TILE_W] == 3) //fire floppy
                {

                    var cls = new Enemies(x, y + 5, 2);
                    Game1.TempOpponents.Add(cls);
                    cls.spawning = true;
                    cls.spawnTimer = 30;
                    cls.currentAction = "Do Nothing";
                    cls.lengthOfAction = 100;
                    spawnfrequency = 600;
                }
                if (GameMap.Index[y / Game1.TILE_H, x / Game1.TILE_W] == 4) //honeycrisp spawner
                {

                    var cls = new Enemies(x, y + 5, 3);
                    Game1.TempOpponents.Add(cls);
                    cls.spawning = true;
                    cls.spawnTimer = 30;
                    cls.currentAction = "Do Nothing";
                    cls.lengthOfAction = 100;
                    spawnfrequency = 1200;
                }


            }


            if (type == 1) {
                if (spawning == true) spawnTimer--;
                if (spawnTimer < 0) spawning = false;

                slowerTick++;
                if (slowerTick > 1) slowerTick = 0;

                if (lengthOfAction <= 0)
                {
                    bounce = 0;
                    yVelocity = 0;
                    bouncing = false;
                    int newAction = Game1.rnd.Next(0, 6); //Decide what this enemy guy should do.
                    switch (newAction)
                    {

                        case 0:
                            currentAction = "Do Nothing";
                            lengthOfAction = Game1.rnd.Next(30, 200);
                            Moving = false;
                            break;
                        case 1:
                            currentAction = "Move Down";
                            lengthOfAction = Game1.rnd.Next(30, 200);
                            break;
                        case 2:
                            currentAction = "Move Up";
                            lengthOfAction = Game1.rnd.Next(30, 200);
                            break;
                        case 3:
                            currentAction = "Move Left";
                            lengthOfAction = Game1.rnd.Next(30, 200);
                            break;
                        case 4:
                            currentAction = "Move Right";
                            lengthOfAction = Game1.rnd.Next(30, 200);
                            break;
                        case 5:
                            currentAction = "Bounce";
                            lengthOfAction = 30;
                            bounce = 0;
                            yVelocity = 3;
                            bouncing = true;
                            Game1.FloppyBounce.Play(0.3f, 0.0f, 0.0f);
                            break;
                    }
                }

                switch (currentAction)
                {
                    case "Do Nothing":
                        lengthOfAction--;
                        Moving = false;
                        break;
                    case "Move Down":
                        if (slowerTick == 0) eMove("Down");
                        Moving = true;
                        lengthOfAction--;
                        break;
                    case "Move Up":
                        if (slowerTick == 0) eMove("Up");
                        Moving = true;
                        lengthOfAction--;
                        break;
                    case "Move Left":
                        if (slowerTick == 0) eMove("Left");
                        Moving = true;
                        lengthOfAction--;
                        break;
                    case "Move Right":
                        if (slowerTick == 0) eMove("Right");
                        Moving = true;
                        lengthOfAction--;
                        break;
                    case "Bounce":
                        lengthOfAction--;
                        bounce += yVelocity;
                        yVelocity -= 0.5;
                        if (bounce < 0)
                        {
                            bounce = 0;
                            yVelocity = 0;
                        }
                        break;
                }
                if (Moving == true) //movement logic
                {
                    Tick += 1;
                    if (Tick > 5)
                    {
                        Tick = 0;

                        if (animDir == 0)
                        {
                            Frame++;
                            if (Frame >= 2) animDir = 1;
                        }
                        else
                        {
                            Frame--;
                            if (Frame <= 0) animDir = 0;
                        }
                    }
                    walkingSoundTimer++;
                    if (walkingSoundTimer > 9)
                    {
                        walkingSoundTimer = 0;
                        //Game1.Walking.Play();
                    }
                }
                else
                {
                    Frame = 1; Tick = 4;
                }

            }
            else if (type == 2)
            {
                FireFloppyLogic();
            }
            else if (type == 3)
            {
                HoneyCrispLogic();
            }

            if (BeingKnockedback) Knockback();
            if (hurtInvincible)
            {
                hurtframe++;
                if (hurtframe > 5) hurtframe = 0;
                hurtInvincibleTimer++;
                if (hurtInvincibleTimer > 17) hurtInvincible = false;
            }


        }



        public bool eMove(string direction, bool turnguy= true, bool affect_lengthofAction=true)
        {
            bool successfullyMoved = false;
            bool resetlengthofAction = false;
            switch (direction) //collision area: x + 2, size 11, y, size 8
            {
                case "Up": //y - 1
                    if (GameMap.HaveCollision(x + colX, y + colY - 1, sizeX, sizeY, true) == false) { this.y -= 1; successfullyMoved = true; }
                    else resetlengthofAction = true;
                    if (turnguy == true) Dir = "Up";
                    break;
                case "Down": //y + 1
                    if (GameMap.HaveCollision(x + colX, y + colY + 1, sizeX, sizeY, true) == false) { this.y += 1; successfullyMoved = true; }

                    else resetlengthofAction = true;
                    if (turnguy == true) Dir = "Down";
                    break;
                case "Left": //x - 1
                    if (GameMap.HaveCollision(x + colX - 1, y + colY, sizeX, sizeY, true) == false) { this.x -= 1; successfullyMoved = true; }
                    else resetlengthofAction = true;
                    if (turnguy == true) Dir = "Left";
                    break;
                case "Right": //x + 1
                    if (GameMap.HaveCollision(x + colX + 1, y + colY, sizeX, sizeY, true) == false) { this.x += 1; successfullyMoved = true; }
                    else resetlengthofAction = true;
                    if (turnguy == true) Dir = "Right";
                    break;
                default:
                    break;
            }
            if (resetlengthofAction == true && affect_lengthofAction == true) lengthOfAction = 0;
            return successfullyMoved;
        }


        public void DamageDealt(int AmountOfDamage, string cDir, int invinc_time=14, bool Knockbackornot = true) //default invincible time is about as long as an average hammer attack
        {
            if (hurtInvincible == false) HP -= AmountOfDamage;
            BeingKnockedback = Knockbackornot; //Got to knock the enemy back when hit!
            KnockbackTime = 0;
            KnockbackDir = cDir;


            hurtInvincible = true; //The enemy gets some temporary invincibility as well
            if (hurtInvincibleTimer > invinc_time) hurtInvincibleTimer = 0;

        }


        private void Knockback()
        {
            eMove(KnockbackDir, false, false);
            eMove(KnockbackDir, false, false);
            eMove(KnockbackDir, false, false);
            eMove(KnockbackDir, false, false);
            //Knockbacktick++;
            //if (Knockbacktick > 3) Knockbacktick = 0;
            KnockbackTime++;
            if (KnockbackTime > 5) BeingKnockedback = false;
        }

        public bool CheckPlayerCollision(int cx, int cy, int sizeX2, int sizeY2)
        {
            bool foundHit = false;

            //You may wonder...why don't I just use the variable names that the enemy object already has? Well, I copied this code from the player class and I'm lazy sometimes.
            int Pcx = x + colX;
            int Pcy = y+ colY;
            int Psx = sizeX; //sizes
            int Psy = sizeY;

            if (cx >= Pcx && cx <= Pcx + Psx && cy >= Pcy && cy <= Pcy + Psy) foundHit = true;
            if (cx + sizeX2 >= Pcx && cx + sizeX2 <= Pcx + Psx && cy >= Pcy && cy <= Pcy + Psy) foundHit = true;
            if (cx >= Pcx && cx <= Pcx + Psx && cy + sizeY2 >= Pcy && cy + sizeY2 <= Pcy + Psy) foundHit = true;
            if (cx + sizeX2 >= Pcx && cx + sizeX2 <= Pcx + Psx && cy + sizeY2 >= Pcy && cy + sizeY2 <= Pcy + Psy) foundHit = true;

            //However, if the player's hitbox is inside something else, none of the statements above will return true, 
            //so we have to check again, with the variables switched around.
            if (Pcx >= cx && Pcx <= cx + sizeX2 && Pcy >= cy && Pcy <= cy + sizeY2) foundHit = true;
            if (Pcx + Psx >= cx && Pcx + Psx <= cx + sizeX2 && Pcy >= cy && Pcy <= cy + sizeY2) foundHit = true;
            if (Pcx >= cx && Pcx <= cx + sizeX2 && Pcy + Psy >= cy && Pcy + Psy <= cy + sizeY2) foundHit = true;
            if (Pcx + Psx >= cx && Pcx + Psx <= cx + sizeX2 && Pcy + Psy >= cy && Pcy + Psy <= cy + sizeY2) foundHit = true;

            return foundHit; //Returns true if a collision is detected.
        }

    }
}
