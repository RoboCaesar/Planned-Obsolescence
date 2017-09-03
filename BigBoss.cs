using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskWars
{
    public partial class Enemies
    {
        public void BigBoss()
        {

        }
        private void BigBossLogic()
        {
            if (HP <= 0 && active == true) //placeholder code for the boss
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

            if (spawning == true) spawnTimer--;
            if (spawnTimer < 0) spawning = false;

            slowerTick++;
            if (slowerTick > 1) slowerTick = 0;

            if (lengthOfAction <= 0)
            {
                bounce = 0;
                yVelocity = 0;
                bouncing = false;
                int newAction = Game1.rnd.Next(0, 7); //Decide what this enemy guy should do.
                Frame = 1;
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
                    case 6:
                        currentAction = "Bullet Storm";
                        lengthOfAction = 60;
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
                    eMove("Down");
                    Moving = true;
                    lengthOfAction--;
                    break;
                case "Move Up":
                    eMove("Up");
                    Moving = true;
                    lengthOfAction--;
                    break;
                case "Move Left":
                    eMove("Left");
                    Moving = true;
                    lengthOfAction--;
                    break;
                case "Move Right":
                    eMove("Right");
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
                case "Bullet Storm": //An attack where the boss sends bullets generally in the direction of the player.
                    lengthOfAction--;
                    if (lengthOfAction % 10 == 0)
                    {
                        //have to use some trigonometry magic to calculate angle of attack. Super messy, trial and error involved. Don't use again.
                        double attackAngle = Math.Atan2((double)x + 22.0 - (double)Game1.Hunter.x, (double)Game1.Hunter.y - 30.0 - (double)y); //have to add 5 to Y, x for a certain offset fix
                        attackAngle -= Math.PI / 2;
                        attackAngle += Math.PI;
                        //if ((double)x + 4 < (double)Game1.Hunter.x) attackAngle += Math.PI;
                        Game1.Shot.Play(0.5f, 0.0f, 0.0f);
                        var cls = new Bullets((double)x + 4 + 18, (double)y - 5 + 35, 1, attackAngle, 2.0); //cls2 is a temp debris class
                        Game1.gameBullets.Add(cls);
                    }
                    Dir = "Down";
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
                Tick += 1;
                if (Tick > 10)
                {
                    if (Frame == 1) Frame = 3;
                    else Frame = 1;
                    Tick = 0;
                }
            }

        }
    }
}
