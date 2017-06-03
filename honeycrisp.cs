using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskWars
{
    public partial class Enemies
    {
        public void HoneyCrispLogic()
        {

            if (HP <= 0 && active == true && type == 3) //First, make sure the enemy is actually alive still.
            {
                active = false;
                Game1.Hunter.score += 40;
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
                    cls2.type = Game1.rnd.Next(-1, 2);
                }

                //bomb drop code
                var cls3 = new Debris(); //cls3 is a temp debris class
                Game1.gameDebris.Add(cls3);
                cls3.direction = Game1.rnd.NextDouble() * 2.00 * Math.PI;
                cls3.velocity = 0;
                cls3.yVelocity = Game1.rnd.NextDouble() * 3.00 + 3;
                cls3.x = x + 8;
                cls3.y = y + 8;
                cls3.active = true;
                cls3.type = 3;
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
                        currentAction = "Attack!";
                        lengthOfAction = 150;
                        /*//have to use some trigonometry magic to calculate angle of attack. Super messy, trial and error involved. Don't use again.
                        double attackAngle = Math.Atan2((double)x - (double)Game1.Hunter.x, (double)Game1.Hunter.y - (double)y + 10); //have to add 5 to Y, x for a certain offset fix
                        attackAngle -= Math.PI / 2;
                        attackAngle += Math.PI;
                        //if ((double)x + 4 < (double)Game1.Hunter.x) attackAngle += Math.PI;

                        var cls = new Bullets((double)x + 4, (double)y - 5, 0, attackAngle, 2.0); //cls2 is a temp debris class
                        Game1.gameBullets.Add(cls);*/
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
                    if (slowerTick == 0) eMove("Down", true, false);
                    Moving = true;
                    lengthOfAction--;
                    break;
                case "Move Up":
                    if (slowerTick == 0) eMove("Up", true, false);
                    Moving = true;
                    lengthOfAction--;
                    break;
                case "Move Left":
                    if (slowerTick == 0) eMove("Left", true, false);
                    Moving = true;
                    lengthOfAction--;
                    break;
                case "Move Right":
                    if (slowerTick == 0) eMove("Right", true, false);
                    Moving = true;
                    lengthOfAction--;
                    break;
                case "Attack!":
                    lengthOfAction--;
                    if (lengthOfAction == 20)
                    {
                        //have to use some trigonometry magic to calculate angle of attack. Super messy, trial and error involved. Don't use again.
                        double attackAngle = Math.Atan2((double)x - (double)Game1.Hunter.x, (double)Game1.Hunter.y - (double)y + 10); //have to add 5 to Y, x for a certain offset fix
                        attackAngle -= Math.PI / 2;
                        attackAngle += Math.PI;
                        //if ((double)x + 4 < (double)Game1.Hunter.x) attackAngle += Math.PI;

                        var cls = new Bullets((double)x + 4, (double)y - 5, 0, attackAngle, 2.0); //cls2 is a temp debris class
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
                Frame = 1; Tick = 4;
            }
        }
    }
}
