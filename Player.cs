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
    public class Player
    {

        public int x { get; set; }
        public int y { get; set; }
        public int Frame { get; set; }
        protected int Tick = 0;
        protected int animDir = 0;
        public bool Moving = false;
        protected string Dir = "Down";
        public bool attacking = false;
        public int attackStyle;
        protected int attackFrame = 0;

        public int HP = 30;
        public int score = 0;

        protected int preCharging;
        protected float weaponCharging = 0.0f;
        protected int walkingSoundTimer = 0;
        protected int chargingSoundTimer = 0;
        public Missiles[] Projectiles = new Missiles[8];

        public double bounce = 0;
        public double yVelocity = 0;
        public bool bouncing = false;

        public bool SpecialReady = true;
        private int SpecialCharge = 33;
        private int SpecialChargeTick = 0;

        //private int hitframe = 0;
        public bool hurtInvincible = false;
        public int hurtInvincibleTimer = 0;
        private int hurtframe = 0;

        //private int testCooldown = 0;

        private int KnockbackTime = 0;
        private bool BeingKnockedback = false;
        private string KnockbackDir = "Up";


        public int gemComboCounter = 0; //Need some gem logic here.
        public int gemComboCooldown = 0;
        private int gemPowerAnimTick = 0;

        public int bombsHeld = 0;

        public Player()
        {
            Frame = 0;

            for (int k = 0; k < Projectiles.Length; k++)
            {
                Projectiles[k] = new Missiles();
                Projectiles[k].active = false;
            }
        }


        public Rectangle GetSourceRectangle(int tileIndex, int frames=3, int size=30) //This allows us to find the correct sprite in the file
        {
            return new Rectangle((tileIndex % frames) * size, Convert.ToInt16(tileIndex / frames) * size, size, size);
        }

        public void Move(string direction,bool turnguy=true)
        {
            switch (direction) //player collision area is x + 3 to x + 12, y + 7 to y + 15
            {
                case "Up": //y - 1
                    if (GameMap.HaveCollision(x + 3, y + 6, 9, 8) == false) this.y -= 1;
                    //if (GameMap.HaveCollision(x, y-1, 15,15) == false) this.y -= 1;
                    //this.y -= 1;
                    if (turnguy == true) Dir = "Up";
                    break;
                case "Down": //y + 1
                    if (GameMap.HaveCollision(x + 3, y + 8, 9, 8) == false) this.y += 1;
                    if (turnguy == true) Dir = "Down";
                    break;
                case "Left": //x - 1
                    if (GameMap.HaveCollision(x + 2, y + 7, 9, 8) == false) this.x -= 1;
                    if (turnguy == true) Dir = "Left";
                    break;
                case "Right": //x + 1
                    if (GameMap.HaveCollision(x + 4, y + 7, 9, 8) == false) this.x += 1;
                    if (turnguy == true) Dir = "Right";
                    break;
                default:
                    break;
            }
        }

        //----------Important attack stuff here!!! -----------------
        public void Logic()
        {
            if (attackStyle != 3 && bouncing == true)
            {
                bounce += yVelocity; //Normal jump code. Actually, player doesn't even bounce usually (except for special attack)
                yVelocity -= 0.1;
                if (bounce < 0)
                {
                    bounce = 0;
                    yVelocity = 0;
                    bouncing = false;
                }
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
                    Game1.Walking.Play(0.3f, 0.0f, 0.0f);
                }
                if (gemComboCounter > 1) Move(Dir);
            }
            else
            {
                Frame = 1; Tick = 4;
            }

            SpecialChargeTick++; //Big special attack buildup

            if (SpecialChargeTick >= 10)
            {
                SpecialCharge++;
                SpecialChargeTick = 0;
            }
            if (SpecialCharge >= 33)
            {
                SpecialCharge = 33;
                SpecialReady = true;
            }


            bool foundHit = false;
            //Check for collision with enemy
            foreach (var cls in Game1.Opponents)
            {
                //This ugly list of if statements below...it's to check the four corners of the enemy hitbox. Sorry for the ugliness.
                //checkx and stuff is the coordinates for the player character. It's based on the collision detection for movement.
                int checkX = x + 3;
                int checkY = y + 7;
                int sizeX = 9;
                int sizeY = 8;

                if (cls.CheckPlayerCollision(x + 3, y + 7, sizeX, sizeY)) foundHit = true;
                /*
                if (cls.x + cls.colX >= checkX && cls.x + cls.colX <= checkX + sizeX && cls.y + cls.colY >= checkY && cls.y + cls.colY <= checkY + sizeY) foundHit = true;
                if (cls.x + cls.colX + cls.sizeX >= checkX && cls.x + cls.colX + cls.sizeX <= checkX + sizeX && cls.y + cls.colY >= checkY && cls.y + cls.colY <= checkY + sizeY) foundHit = true;
                if (cls.x + cls.colX >= checkX && cls.x + cls.colX <= checkX + sizeX && cls.y + cls.colY + cls.sizeY >= checkY && cls.y + cls.colY + cls.sizeY <= checkY + sizeY) foundHit = true;
                if (cls.x + cls.colX + cls.sizeX >= checkX && cls.x + cls.colX + cls.sizeX <= checkX + sizeX && cls.y + cls.colY + cls.sizeY >= checkY && cls.y + cls.colY + cls.sizeY <= checkY + sizeY) foundHit = true;
                */
            }

            if (foundHit == true)
            {
                if (hurtInvincible == false)
                {
                    HP -= 3; //How much damage is taken from a collision with a bad guy?
                    Game1.Hurt.Play(1.0f, 0.0f, 0.0f);
                }
                BeingKnockedback = true; //Got to knock the player back when hit!
                KnockbackTime = 0;
                if (Dir == "Up") KnockbackDir = "Down";
                if (Dir == "Down") KnockbackDir = "Up";
                if (Dir == "Left") KnockbackDir = "Right";
                if (Dir == "Right") KnockbackDir = "Left";

                hurtInvincible = true; //The player gets some temporary invincibility as well
                if (hurtInvincibleTimer > 40) hurtInvincibleTimer = 0;
            }

            if (BeingKnockedback) Knockback(); //Important! Moves the player back if hit.

            //Check HP and do something if it drops too low
            if (HP < 1)
            {
                HP = 0;
                Game1.gameOverMode = true;
                Game1.GameOverSound.Play();
                var clsa = new Explosions(x + 7, y, 4);
                Game1.gameExplosions.Add(clsa);
            }


            if (hurtInvincible == true)
            {
                hurtframe++;
                if (hurtframe > 5) hurtframe = 0;
                hurtInvincibleTimer++;
                if (hurtInvincibleTimer > 40) hurtInvincible = false;
            }


            if (attacking == true) //attack logic
            {
                if (attackStyle == 0) //Default basic attack
                {
                    attackFrame++;
                    CheckAttackHit();
                    if (attackFrame == 1) //shoot an attack wave
                    {
                        if (Dir == "Up") SendOutMissile(1, 1, x, y - 4);
                        if (Dir == "Down") SendOutMissile(1, 1, x, y + 8);
                        if (Dir == "Left") SendOutMissile(1, 1, x - 8, y);
                        if (Dir == "Right") SendOutMissile(1, 1, x + 8, y);
                    }
                    if (attackFrame > 11)
                    {
                        //If the attack button is still being held down at the end of the basic attack, initiate the charge build-up.
                        if (Keyboard.GetState().IsKeyDown(Keys.Z))
                        {
                            attackStyle = 1; attackFrame = 0; weaponCharging = 0.0f;
                            preCharging = 0;
                        }
                        else
                        {
                            attacking = false;
                        }
                    }
                }
                else if (attackStyle == 1) //charging up attack
                {
                    if (!Keyboard.GetState().IsKeyDown(Keys.Z))
                    {
                        //attacking = false;
                        if (weaponCharging >= 24)
                        {
                            SendOutMissile(0, 5, x + 7, y + 11); //Fire a missile!
                            Game1.ChargedShot.Play(0.2f, 0.0f, 0.0f);

                            attackFrame = 0;
                            attackStyle = 2; //This is the "hammer down" stage. Not really an attack.
                        }
                        else
                        {
                            attacking = false;
                        }
                    }
                    if (preCharging < 12) preCharging++;
                    if (preCharging >= 12) {
                        attackFrame++;
                        if (attackFrame > 5)
                        {
                            attackFrame = 0;
                            weaponCharging += 4f;
                            if (weaponCharging > 25) weaponCharging = 25.0f;

                        }
                        chargingSoundTimer++;
                        if (chargingSoundTimer > 14)
                        {
                            Game1.Charging.Play(0.2f, -0.4f + (float)(weaponCharging / 50), 0.0f);
                            chargingSoundTimer = 0;
                        }
                    }

                }
                else if (attackStyle == 2)
                {
                    attackFrame++;
                    if (attackFrame > 20) attacking = false;
                }
                else if (attackStyle == 3) //big attack!
                {
                    SpecialReady = false;
                    SpecialCharge = 0;
                    attackFrame++;
                    hurtInvincible = true;
                    if (attackFrame == 45)
                    {
                        Game1.SuperAttack.Play(0.4f, -0.0f, 0.0f);
                        //Explosions.CreateExplosion(x + 10, y + 15, 1);
                        var cls = new Explosions(x + 10, y + 15, 1);
                        Game1.gameExplosions.Add(cls);
                        CheckAttackHit();
                    }
                    bounce += yVelocity; //special bouncing code for big attack!
                    yVelocity -= 0.5;
                    if (bounce < 0)
                    {
                        bounce = 0;
                        yVelocity = 0 - 0.5*yVelocity;
                    }

                    if (attackFrame > 120) {
                        attacking = false;
                        hurtInvincible = false;
                        attackStyle = 0;
                        bounce = 0;
                    }
                }

            }
            //missiles code
            for (int k = 0; k < Projectiles.Length; k++)
            {
                if (Projectiles[k].active == true)
                {
                    Projectiles[k].Logic();
                }
            }

            //A bit of gem code
            gemComboCooldown--;
            if (gemComboCooldown < 0)
            {
                gemComboCooldown = 0;
                gemComboCounter = 0;
            }
            if (gemComboCounter > 0)
            {
                gemPowerAnimTick++;
                if (gemPowerAnimTick > 4)
                {
                    gemPowerAnimTick = 0;
                    var cls = new Explosions(x + 9, y + 5, 7);
                    Game1.gameExplosions.Add(cls);
                    //CheckAttackHit();
                    if (gemComboCounter > 5) HP++;
                    if (HP > 30) HP = 30;
                }

            }

        }

        public void DrawStatus()
        {
            Game1.spriteBatch.Draw(Game1.StatusBar, new Rectangle(0, Game1.SCREEN_H - 16, 80, 16), new Rectangle(0, 0, 80, 16), Color.White); //Status bar
            Game1.spriteBatch.Draw(Game1.StatusBar, new Rectangle(38, Game1.SCREEN_H - 12, SpecialCharge, 3), new Rectangle(83, 0, 3, 3), Color.White); //special charge bar
            Game1.spriteBatch.Draw(Game1.StatusBar, new Rectangle(15, Game1.SCREEN_H - 6, HP*2, 3), new Rectangle(83, 3, 3, 3), Color.White); //HP bar


            var scoreColor = new Color(255, 255, 255);
            if (Game1.Scorebounce > 0.8)
            {
                scoreColor = new Color(Game1.rnd.Next(0, 255), Game1.rnd.Next(0, 255), Game1.rnd.Next(0, 255));
            }

            Game1.spriteBatch.Draw(Game1.WindowSprites, new Rectangle(Game1.SCREEN_W - 146, Game1.SCREEN_H - 28, 146, 28), new Rectangle(1, 4, 14, 9), Color.White * 0.5f); //make a blue window for the info.
            Game1.spriteBatch.DrawString(Game1.gameFont, "SCORE: " + score, new Vector2(Game1.SCREEN_W - 139, Game1.SCREEN_H - 23 - (int)Game1.Scorebounce), Color.Black * 0.5f);
            Game1.spriteBatch.DrawString(Game1.gameFont, "SCORE: " + score, new Vector2(Game1.SCREEN_W - 140, Game1.SCREEN_H - 24 - (int)Game1.Scorebounce), scoreColor);
            if (GameMap.VictoryCondition == "Destroy Enemies")
            {
                if (GameMap.ObjectsRemaining > 0)
                {
                    Game1.spriteBatch.DrawString(Game1.gameFont, "Enemies remaining: " + GameMap.ObjectsRemaining, new Vector2(Game1.SCREEN_W - 139, Game1.SCREEN_H - 11), Color.Black * 0.5f);
                    Game1.spriteBatch.DrawString(Game1.gameFont, "Enemies remaining: " + GameMap.ObjectsRemaining, new Vector2(Game1.SCREEN_W - 140, Game1.SCREEN_H - 12), Color.White);
                }
                else
                {
                    Game1.spriteBatch.DrawString(Game1.gameFont, "Proceed to the exit.", new Vector2(Game1.SCREEN_W - 139, Game1.SCREEN_H - 11), Color.Black * 0.5f);
                    Game1.spriteBatch.DrawString(Game1.gameFont, "Proceed to the exit.", new Vector2(Game1.SCREEN_W - 140, Game1.SCREEN_H - 12), Color.White);
                }
            }
            if (GameMap.VictoryCondition == "Destroy Spawners")
            {
                if (GameMap.ObjectsRemaining > 0)
                {
                    Game1.spriteBatch.DrawString(Game1.gameFont, "Spawners remaining: " + GameMap.ObjectsRemaining, new Vector2(Game1.SCREEN_W - 139, Game1.SCREEN_H - 11), Color.Black * 0.5f);
                    Game1.spriteBatch.DrawString(Game1.gameFont, "Spawners remaining: " + GameMap.ObjectsRemaining, new Vector2(Game1.SCREEN_W - 140, Game1.SCREEN_H - 12), Color.White);
                }
                else
                {
                    Game1.spriteBatch.DrawString(Game1.gameFont, "You're free!", new Vector2(Game1.SCREEN_W - 139, Game1.SCREEN_H - 11), Color.Black * 0.5f);
                    Game1.spriteBatch.DrawString(Game1.gameFont, "You're free!", new Vector2(Game1.SCREEN_W - 140, Game1.SCREEN_H - 12), Color.White);
                }
            }
            if (GameMap.VictoryCondition == "Survive")
            {
                if (GameMap.ObjectsRemaining > 0)
                {
                    Game1.spriteBatch.DrawString(Game1.gameFont, "Time remaining: " + (GameMap.ObjectsRemaining / 60), new Vector2(Game1.SCREEN_W - 139, Game1.SCREEN_H - 11), Color.Black * 0.5f);
                    Game1.spriteBatch.DrawString(Game1.gameFont, "Time remaining: " + (GameMap.ObjectsRemaining / 60), new Vector2(Game1.SCREEN_W - 140, Game1.SCREEN_H - 12), Color.White);
                }
                else
                {
                    Game1.spriteBatch.DrawString(Game1.gameFont, "Get out of here!", new Vector2(Game1.SCREEN_W - 139, Game1.SCREEN_H - 11), Color.Black * 0.5f);
                    Game1.spriteBatch.DrawString(Game1.gameFont, "Get out of here!", new Vector2(Game1.SCREEN_W - 140, Game1.SCREEN_H - 12), Color.White);
                }
            }

            if (Game1.ScoreYVel != 0) Game1.Scorebounce += Game1.ScoreYVel;
            Game1.ScoreYVel -= 0.3;
            if (Game1.Scorebounce < 0)
            {
                Game1.Scorebounce = 0;
                Game1.ScoreYVel = 0 - 0.5 * Game1.ScoreYVel;
                if (Math.Abs(Game1.ScoreYVel) < 0.1)
                {
                    Game1.ScoreYVel = 0;
                    Game1.Scorebounce = 0;
                }
            }
            if (bombsHeld > 0)
            {
                for (int count = 0; count < bombsHeld; count++)
                {
                    Game1.spriteBatch.Draw(Game1.Gems, new Rectangle(Game1.SCREEN_W - 160 - count*12, Game1.SCREEN_H - 12, 10, 10),new Rectangle(0, 48, 16, 16), Color.White);
                }

            }



        }

        public void DrawPlayer()
        {
            for (int k = 0; k < Projectiles.Length; k++)
            {
                if (Projectiles[k].active == true)
                {
                    Projectiles[k].Draw();
                }
            }
            if (attacking == false)
            {
                //player collision area is x + 3 to x + 12, y + 7 to y + 15
                Game1.spriteBatch.Draw(Game1.Sprites, new Rectangle(x - 7, y - 11, 30, 30), GetSourceRectangle(12), Color.White);
                if (hurtInvincible == false) Game1.spriteBatch.Draw(Game1.Sprites, new Rectangle(x - 7, y - 14 - (int)bounce, 30, 30), GetSourceRectangle(Frame + 3 * Game1.DirDict[Dir]), Color.White);
                else
                {
                    if (hurtframe < 3)
                        Game1.spriteBatch.Draw(Game1.Sprites, new Rectangle(x - 7, y - 14 - (int)bounce, 30, 30), GetSourceRectangle(Frame + 3 * Game1.DirDict[Dir]), Color.White);
                    else Game1.spriteBatch.Draw(Game1.Sprites, new Rectangle(x - 7, y - 14 - (int)bounce, 30, 30), GetSourceRectangle(Frame + 3 * Game1.DirDict[Dir] + 15), Color.White);
                }
                //imageSource.Draw(Game1.Debris1, new Rectangle(x + 3, y + 7, 9, 8), GetSourceRectangle(6, 4, 16), Color.White * 1.0f);
            }
            else
            {
                if (attackStyle == 0)
                {

                    switch(Dir)
                    {
                        case "Down": //Those weird numbers are because of fine tuning the position of the attack wave. sorry!
                            Game1.spriteBatch.Draw(Game1.AttackWave, new Rectangle(x - 12, y + 14 + attackFrame - 12, 40, 16), new Rectangle(32,16,40,16), Color.White * 0.7f);
                            break;
                        case "Up":
                            Game1.spriteBatch.Draw(Game1.AttackWave, new Rectangle(x - 12, y - 16 - attackFrame + 12, 40, 16), new Rectangle(32, 0, 40, 16), Color.White * 0.7f);
                            break;
                        case "Left":
                            Game1.spriteBatch.Draw(Game1.AttackWave, new Rectangle(x - 12 - attackFrame + 12, y - 16, 16, 48), new Rectangle(16, 0, 16, 48), Color.White * 0.7f);
                            break;
                        case "Right":
                            Game1.spriteBatch.Draw(Game1.AttackWave, new Rectangle(x + 13 + attackFrame - 12, y - 16, 16, 48), new Rectangle(0, 0, 16, 48), Color.White * 0.7f);
                            break;
                    }

                    Game1.spriteBatch.Draw(Game1.Sprites, new Rectangle(x - 7, y - 11, 30, 30), GetSourceRectangle(12), Color.White);
                    if (Dir == "Up") Game1.spriteBatch.Draw(Game1.AttackSprites, new Rectangle(x - 7, y - 14, 30, 30), GetSourceRectangle(Convert.ToInt16(attackFrame / 3) + 4 * Game1.DirDict[Dir], 4), Color.White);
                    if (Dir == "Down") Game1.spriteBatch.Draw(Game1.AttackSprites, new Rectangle(x - 7, y, 30, 30), GetSourceRectangle(Convert.ToInt16(attackFrame / 3) + 4 * Game1.DirDict[Dir], 4), Color.White);
                    if (Dir == "Left") Game1.spriteBatch.Draw(Game1.AttackSprites, new Rectangle(x - 7, y - 7, 30, 30), GetSourceRectangle(Convert.ToInt16(attackFrame / 3) + 4 * Game1.DirDict[Dir], 4), Color.White);
                    if (Dir == "Right") Game1.spriteBatch.Draw(Game1.AttackSprites, new Rectangle(x - 7, y - 7, 30, 30), GetSourceRectangle(Convert.ToInt16(attackFrame / 3) + 4 * Game1.DirDict[Dir], 4), Color.White);
                }
                else if (attackStyle == 1)
                {
                    Game1.spriteBatch.Draw(Game1.Sprites, new Rectangle(x - 7, y - 11, 30, 30), GetSourceRectangle(12), Color.White);
                    if (preCharging >= 12)
                    {
                        if (Dir == "Up") Game1.spriteBatch.Draw(Game1.AttackSprites, new Rectangle(x - 7, y - 14, 30, 30), GetSourceRectangle(4 * (Game1.DirDict[Dir] + Convert.ToInt16(attackFrame / 3) * 4), 4), Color.White);
                        if (Dir == "Down") Game1.spriteBatch.Draw(Game1.AttackSprites, new Rectangle(x - 7, y, 30, 30), GetSourceRectangle(4 * (Game1.DirDict[Dir] + Convert.ToInt16(attackFrame / 3) * 4), 4), Color.White);
                        if (Dir == "Left") Game1.spriteBatch.Draw(Game1.AttackSprites, new Rectangle(x - 7, y - 7, 30, 30), GetSourceRectangle(4 * (Game1.DirDict[Dir] + Convert.ToInt16(attackFrame / 3) * 4), 4), Color.White);
                        if (Dir == "Right") Game1.spriteBatch.Draw(Game1.AttackSprites, new Rectangle(x - 7, y - 7, 30, 30), GetSourceRectangle(4 * (Game1.DirDict[Dir] + Convert.ToInt16(attackFrame / 3) * 4), 4), Color.White);
                    }
                    else
                    {
                        Game1.spriteBatch.Draw(Game1.Sprites, new Rectangle(x - 7, y - 14, 30, 30), GetSourceRectangle(3 * Game1.DirDict[Dir]), Color.White);
                    }
                }
                else if (attackStyle == 2)
                {
                    Game1.spriteBatch.Draw(Game1.Sprites, new Rectangle(x - 7, y - 11, 30, 30), GetSourceRectangle(12), Color.White); 
                    if (Dir == "Up") Game1.spriteBatch.Draw(Game1.AttackSprites, new Rectangle(x - 7, y - 14, 30, 30), GetSourceRectangle(24+ 5 * Game1.DirDict[Dir], 5), Color.White);
                    if (Dir == "Down") Game1.spriteBatch.Draw(Game1.AttackSprites, new Rectangle(x - 7, y, 30, 30), GetSourceRectangle(24+ 5 * Game1.DirDict[Dir], 5), Color.White);
                    if (Dir == "Left") Game1.spriteBatch.Draw(Game1.AttackSprites, new Rectangle(x - 7, y - 7, 30, 30), GetSourceRectangle(24+ 5 * Game1.DirDict[Dir], 5), Color.White);
                    if (Dir == "Right") Game1.spriteBatch.Draw(Game1.AttackSprites, new Rectangle(x - 7, y - 7, 30, 30), GetSourceRectangle(24+ 5 * Game1.DirDict[Dir], 5), Color.White);
                }
                else if (attackStyle == 3)
                {
                    //for fade effect use sin(3.14* (attackframe/120))
                    Game1.spriteBatch.Draw(Game1.SeriousOverlay, new Rectangle(0, 0, 320, 240), new Rectangle(5*attackFrame, 0, 320, 240), Color.White * Convert.ToSingle(Math.Sin(3.14 * ((double)attackFrame/120)) * 0.6f ));

                    Game1.spriteBatch.Draw(Game1.Sprites, new Rectangle(x - 7, y - 11, 30, 30), GetSourceRectangle(12), Color.White);
                    if (Dir == "Up") Game1.spriteBatch.Draw(Game1.AttackSprites, new Rectangle(x - 7, y - 14 - (int)bounce, 30, 30), GetSourceRectangle(5 * Game1.DirDict[Dir] + 4, 5), Color.White);
                    if (Dir == "Down") Game1.spriteBatch.Draw(Game1.AttackSprites, new Rectangle(x - 7, y - (int)bounce, 30, 30), GetSourceRectangle(5 * Game1.DirDict[Dir] + 4, 5), Color.White);
                    if (Dir == "Left") Game1.spriteBatch.Draw(Game1.AttackSprites, new Rectangle(x - 7, y - 7 - (int)bounce, 30, 30), GetSourceRectangle(5 * Game1.DirDict[Dir] + 4, 5), Color.White);
                    if (Dir == "Right") Game1.spriteBatch.Draw(Game1.AttackSprites, new Rectangle(x - 7, y - 7 - (int)bounce, 30, 30), GetSourceRectangle(5 * Game1.DirDict[Dir] + 4, 5), Color.White);
                }
            }
            // vGame1.spriteBatch.DrawString(Game1.gameFont, attackFrame + " ", new Vector2(5, 45), Color.White);
            //The code right below shows the attack box. Uncomment if you want to see that for debugging purposes!
            /*
            switch (Dir)
            { 
                case "Down":
                    Game1.spriteBatch.Draw(Game1.Debris1, new Rectangle(x - 12, y, 40, 30), GetSourceRectangle(13, 4, 16), Color.White * 1.0f);
                    break;
                case "Up":
                    Game1.spriteBatch.Draw(Game1.Debris1, new Rectangle(x - 12, y - 16, 40, 26), GetSourceRectangle(13, 4, 16), Color.White * 1.0f);
                    break;
                case "Left":
                    Game1.spriteBatch.Draw(Game1.Debris1, new Rectangle(x - 12, y - 16, 26, 48), GetSourceRectangle(13, 4, 16), Color.White * 1.0f);
                    break;
                case "Right":
                    Game1.spriteBatch.Draw(Game1.Debris1, new Rectangle(x + 3, y - 16, 26, 48), GetSourceRectangle(13, 4, 16), Color.White * 1.0f);
                    break;
            }*/

        }
        

        public void Attack(int type=0)
        {
            if (attacking == false)
            {
                if (type == 0) Game1.HammerHit.Play(0.3f, 0.0f, 0.0f);


                attacking = true;
                attackFrame = 0;
                attackStyle = type;
                if (type == 3)
                {
                    Game1.BigJump.Play(0.3f, 0.0f, 0.0f);
                    yVelocity = 10;
                }
            }
        }


        public void CheckAttackHit()
        {
            int checkX = 0;
            int checkY = 0;
            int sizeX = 0;
            int sizeY = 0;
            if (attackStyle == 0) //basic short range attack. Look to the missiles class for info on the ranged attacks
            {
                switch (Dir)
                {
                    case "Down": //Those random 12s you see? That's the original hitbox where only the drawn hammer reaches. I made it bigger.
                        checkX = x - 12;
                        checkY = y;
                        sizeX = 40;
                        sizeY = 30;// + 12;
                        break;
                    case "Up":
                        checkX = x - 12;
                        checkY = y - 15;// - 12;
                        sizeX = 40;
                        sizeY = 26;// + 12;
                        break;
                    case "Left":
                        checkX = x - 11;// - 12;
                        checkY = y - 16;
                        sizeX = 26;// + 12;
                        sizeY = 48;
                        break;
                    case "Right":
                        checkX = x + 3;
                        checkY = y - 16;
                        sizeX = 26;// + 12;
                        sizeY = 48;
                        break;
                }
            }
            else if (attackStyle == 3) //Big hammer attack!
            { //Hmm...don't like having a square hitbox. Might change this later.
                checkX = x - 55;
                checkY = y - 50;
                sizeX = 120;
                sizeY = sizeX;
            }

            foreach (var cls in Game1.Opponents)
            {
                //enemy collision area: set in the enemy class area -> "colX" and "colY" are the coords of the upper left corners, and sizeX and sizeY give the size of the collision box.
                bool foundHit = false; //This ugly list of if statements below...it's to check the four corners of the enemy hitbox. Sorry for the ugliness.

                if (cls.CheckPlayerCollision(checkX, checkY, sizeX, sizeY)) foundHit = true;
                /*
                if (cls.x + cls.colX >= checkX && cls.x + cls.colX <= checkX + sizeX && cls.y + cls.colY>= checkY && cls.y + cls.colY <= checkY + sizeY) foundHit = true;
                if (cls.x + cls.colX + cls.sizeX >= checkX && cls.x + cls.colX + cls.sizeX <= checkX + sizeX && cls.y + cls.colY >= checkY && cls.y + cls.colY <= checkY + sizeY) foundHit = true;
                if (cls.x + cls.colX >= checkX && cls.x + cls.colX <= checkX + sizeX && cls.y + cls.colY + cls.sizeY >= checkY && cls.y + cls.colY + cls.sizeY <= checkY + sizeY) foundHit = true;
                if (cls.x + cls.colX + cls.sizeX >= checkX && cls.x + cls.colX + cls.sizeX <= checkX + sizeX && cls.y + cls.colY + cls.sizeY >= checkY && cls.y + cls.colY + cls.sizeY <= checkY + sizeY) foundHit = true;
                */
                if (foundHit == true)
                {

                    int tempDamage = 0;
                    if (cls.hurtInvincible == false)
                    {

                        //How much damage is dealt to the enemy?
                        if (attackStyle == 0) tempDamage = 2 + Game1.Hunter.gemComboCounter;//cls.HP -= 2;
                        if (attackStyle == 3) tempDamage = 5;//cls.HP -= 5;
                        //Game1.Hurt.Play(1.0f, 0.0f, 0.0f);
                        var shouldWeKnockBackEnemy = true;
                        if (cls.type == 4) shouldWeKnockBackEnemy = false;
                        cls.DamageDealt(tempDamage, Dir, 14, shouldWeKnockBackEnemy);
                    }

                }
            }
            Game1.Opponents.RemoveAll(cls => cls.active == false);

        }

        private void Knockback()
        {
            Move(KnockbackDir, false);
            Move(KnockbackDir, false);
            Move(KnockbackDir, false);
            //Knockbacktick++;
            //if (Knockbacktick > 3) Knockbacktick = 0;
            KnockbackTime++;
            if (KnockbackTime > 5) BeingKnockedback = false;
        }
        public void DamageDealt(int AmountOfDamage, string cDir, bool Knockbackornot=true) //default invincible time is about as long as an average hammer attack
        {
            if (hurtInvincible == false)
            {
                HP -= AmountOfDamage;
                Game1.Hurt.Play(1.0f, 0.0f, 0.0f);
            }
        
            if(Knockbackornot == true)
            {
                BeingKnockedback = true; //Got to knock the enemy back when hit!
                KnockbackTime = 0;
                KnockbackDir = cDir;

            }

            hurtInvincible = true; //The enemy gets some temporary invincibility as well
            if (hurtInvincibleTimer > 40) hurtInvincibleTimer = 0;

        }
        public bool CheckPlayerCollision(int cx, int cy, int sizeX, int sizeY)
        {
            bool foundHit = false;
            int Pcx = x + 3; //player hitbox info
            int Pcy = y + 7;
            int Psx = 9; //sizes
            int Psy = 8;

            //The first four cases are to check for something that might be INSIDE the player collision box.
            if (cx >= Pcx && cx <= Pcx + Psx && cy >= Pcy && cy <= Pcy + Psy) foundHit = true;
            if (cx + sizeX >= Pcx && cx + sizeX <= Pcx + Psx && cy >= Pcy && cy <= Pcy + Psy) foundHit = true;
            if (cx >= Pcx && cx <= Pcx + Psx && cy + sizeY >= Pcy && cy + sizeY <= Pcy + Psy) foundHit = true;
            if (cx + sizeX >= Pcx && cx + sizeX <= Pcx + Psx && cy + sizeY >= Pcy && cy + sizeY <= Pcy + Psy) foundHit = true;
            //However, if the player's hitbox is inside something else, none of the statements above will return true, 
            //so we have to check again, with the variables switched around.
            if (Pcx >= cx && Pcx <= cx + sizeX && Pcy >= cy && Pcy <= cy + sizeY) foundHit = true;
            if (Pcx + Psx >= cx && Pcx + Psx <= cx + sizeX && Pcy >= cy && Pcy <= cy + sizeY) foundHit = true;
            if (Pcx >= cx && Pcx <= cx + sizeX && Pcy + Psy >= cy && Pcy + Psy <= cy + sizeY) foundHit = true;
            if (Pcx + Psx >= cx && Pcx + Psx <= cx + sizeX && Pcy + Psy >= cy && Pcy + Psy <= cy + sizeY) foundHit = true;

            return foundHit;
        }
        public void addBomb()
        {
            bombsHeld++;
            if (bombsHeld > 5) bombsHeld = 5;
        }
        public void removeBomb()
        {
            bombsHeld--;
            if (bombsHeld < 0) bombsHeld = 0;
        }
        public void placeBomb()
        {
            if (bombsHeld > 0)
            {
                var cls3 = new Debris(); //cls3 is a temp debris class
                Game1.gameDebris.Add(cls3);
                cls3.active = true;
                cls3.type = 4;
                cls3.yVelocity = 2;
                cls3.x = x;
                cls3.y = y + 3;
                removeBomb();
            }
        }

        public void SendOutMissile (int type, int speed, int sendX, int sendY)
        {
            for (int k = 0; k < Projectiles.Length; k++)
            {
                if (Projectiles[k].active == false)
                {
                    Projectiles[k].active = true;
                    Projectiles[k].x = (double)sendX;
                    Projectiles[k].y = (double)sendY;
                    Projectiles[k].speed = speed;
                    Projectiles[k].Dir = Dir;
                    Projectiles[k].type = type;
                    Projectiles[k].lifetime = 0;

                    break;
                }
            }
        }

    }
}
