using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskWars
{
    public partial class Enemies
    {

        private void ChasePlayer() //Calculates which way the floppy disk should go to chase the player.
        {
            int sx = 0, sy = 0; //scanning x and y coordinates
            int[,] chaseMap = new int[Game1.MAPSIZE_H, Game1.MAPSIZE_W]; //size of map
            bool scanFinished = false; //letting us know if we've finished scanning everything and creating a path for the floppy disk. Hope it's fast enough!
            bool StillNeedToScanMore = false;
            int newValue = -1; //a temporary value telling us what we'll assign to a spot on the map.

            for (sy = 0; sy < Game1.MAPSIZE_H; sy++)
            {
                for (sx = 0; sx < Game1.MAPSIZE_W; sx++)
                {
                    if (GameMap.Index[sy, sx] >= 50 || GameMap.Index[sy,sx] == 8) chaseMap[sy, sx] = 500; //500 is an arbitrary value meaning NOTHING gets through here.
                    else chaseMap[sy, sx] = -1; //-1 means we haven't done assigned a priority path yet
                }
            }

            //Set the player's position on the map to 0. Remember, the player's position is not based on the grid, so we have to convert it to a suitable coordinate system!
            chaseMap[(int)((Game1.Hunter.y + 7) / Game1.TILE_H), (int)((Game1.Hunter.x + 5)/ Game1.TILE_W)] = 0; //Remember, now we don't want to mess with the original map!

            while (scanFinished == false)
            {
                StillNeedToScanMore = false;

                for (sy = 0; sy < Game1.MAPSIZE_H; sy++)
                {
                    for (sx = 0; sx < Game1.MAPSIZE_W; sx++)
                    {
                        newValue = 499;
                        if (chaseMap[sy,sx] == -1) //Apologies for the ugly code. I just wanted to get it to work.
                        {
                            if (chaseMap[sy - 1, sx] != -1 && chaseMap[sy - 1, sx] != 500) newValue = chaseMap[sy - 1, sx] + 1;
                            if (chaseMap[sy + 1, sx] != -1 && chaseMap[sy + 1, sx] != 500)
                            {
                                if (newValue > chaseMap[sy + 1, sx]) newValue = chaseMap[sy + 1, sx] + 1;
                            }
                            if (chaseMap[sy, sx - 1] != -1 && chaseMap[sy, sx - 1] != 500)
                            {
                                if (newValue > chaseMap[sy, sx - 1]) newValue = chaseMap[sy, sx - 1] + 1;
                            }
                            if (chaseMap[sy, sx + 1] != -1 && chaseMap[sy, sx + 1] != 500)
                            {
                                if (newValue > chaseMap[sy, sx + 1]) newValue = chaseMap[sy, sx + 1] + 1;
                            }
                            if (newValue != -1 && newValue != 499)
                            {
                                chaseMap[sy, sx] = newValue;
                                StillNeedToScanMore = true;
                            }
                        }
                    }
                }
                if (StillNeedToScanMore == false) scanFinished = true;
            }

            //Now that we've generated a direction map, we need to tell the floppy disk which direction to go in to catch the player.
            if (chaseMap[(int)(y / Game1.TILE_H), (int)(x / Game1.TILE_W)] != -1)
            {
                if (chaseMap[(int)(y / Game1.TILE_H) - 1, (int)(x / Game1.TILE_W)] < chaseMap[(int)(y / Game1.TILE_H), (int)(x / Game1.TILE_W)]) Dir = "Up";
                if (chaseMap[(int)(y / Game1.TILE_H) + 1, (int)(x / Game1.TILE_W)] < chaseMap[(int)(y / Game1.TILE_H), (int)(x / Game1.TILE_W)]) Dir = "Down";
                if (chaseMap[(int)(y / Game1.TILE_H), (int)(x / Game1.TILE_W) - 1] < chaseMap[(int)(y / Game1.TILE_H), (int)(x / Game1.TILE_W)]) Dir = "Left";
                if (chaseMap[(int)(y / Game1.TILE_H), (int)(x / Game1.TILE_W) + 1] < chaseMap[(int)(y / Game1.TILE_H), (int)(x / Game1.TILE_W)]) Dir = "Right";

                Moving = true;
                if (eMove(Dir) == false) //try moving. If it doesn't work, nudge the dude a bit so he can get through
                {
                    if (Dir == "Up" || Dir == "Down")
                    {
                        if (x % Game1.TILE_W != 0) eMove("Left", false); //left or up moves the guy towards an area where the modulus is 0
                    }
                    if (Dir == "Left" || Dir == "Right")
                    {
                        if (y % Game1.TILE_H != 0) eMove("Up", false);
                    }
                }
            }
            else //If there's no place for the floppy to go, it moves around randomly
            {
                var randomMove = Game1.rnd.Next(0, 5);
                switch (randomMove)
                {
                    case 0:
                        lengthOfAction--;
                        Moving = false;
                        break;
                    case 1:
                        if (slowerTick == 0) eMove("Down");
                        Moving = true;
                        //lengthOfAction--;
                        break;
                    case 2:
                        if (slowerTick == 0) eMove("Up");
                        Moving = true;
                        //lengthOfAction--;
                        break;
                    case 3:
                        if (slowerTick == 0) eMove("Left");
                        Moving = true;
                        //lengthOfAction--;
                        break;
                    case 4:
                        if (slowerTick == 0) eMove("Right");
                        Moving = true;
                        //lengthOfAction--;
                        break;
                    default:
                        break;
                }
            }

        }

        public void FireFloppyLogic()
        {
            Moving = false;
            if (spawning == true) spawnTimer--;
            if (spawnTimer < 0) spawning = false;

            slowerTick++;
            if (slowerTick > 1) slowerTick = 0;

            if (HP <= 0 && active == true && type == 2) //First, make sure the enemy is actually alive still.
            {
                active = false;
                Game1.Hunter.score += 20;
                if (GameMap.VictoryCondition == "Destroy Enemies") GameMap.ObjectsRemaining -= 1;
                //Explosions.CreateExplosion(x + 7, y, 0);
                var clsa = new Explosions(x + 7, y, 2);
                Game1.gameExplosions.Add(clsa);

                Game1.WasAnEnemyKilledThisFrame = true;
                for (int k = 0; k < 3; k++)
                {
                    var cls2 = new Debris(); //cls2 is a temp debris class
                    Game1.gameDebris.Add(cls2);

                    cls2.direction = Game1.rnd.NextDouble() * 2.00 * Math.PI;
                    cls2.velocity = Game1.rnd.NextDouble() * 1.00;
                    cls2.yVelocity = Game1.rnd.NextDouble() * 3.00 + 2;
                    cls2.x = x + 8;
                    cls2.y = y + 8;
                    cls2.active = true;
                    cls2.RotationAngle = Convert.ToSingle(Game1.rnd.NextDouble() * 2 * Math.PI);
                    var tempdir = Game1.rnd.Next(0, 2);
                    if (tempdir == 0) cls2.RotationDir = "Left";
                    if (tempdir == 1) cls2.RotationDir = "Right";
                    cls2.type = -1;
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

            ChasePlayer();

            if (Math.Abs(x + 7 - Game1.Hunter.x - 8) < 16 && Math.Abs(y + 4 - Game1.Hunter.y - 11) < 16)
            {
                active = false;
                if (GameMap.VictoryCondition == "Destroy Enemies") GameMap.ObjectsRemaining -= 1;
                //Explosions.CreateExplosion(x + 7, y, 0);
                Game1.isScreenShaking = true;
                var clsa = new Explosions(x + 7, y, 4);
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
                Game1.Hunter.DamageDealt(7, "Up", false);
            }


            if (Moving == true) //movement logic
            {
                Tick += 1;
                if (Tick > 2)
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
