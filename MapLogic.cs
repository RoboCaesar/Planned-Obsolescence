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
    public static class GameMap
    {
        private static int mapAnimationCount = 0;
        private static int chimesRemaining = 10;
        public static int spawnFrequency = 500;
        public static int spawnTimer = 0;
        public static int spawnFlash = -1; //-1 implies that nothing is spawning there.
        public static int ObjectsRemaining = 10;
        public static string biome = "grasslands";
        public static string LevelMessage = "Hi";
        public static string VictoryCondition = "Destroy Enemies";
        public static int cleartile = 0;
        //private static int spawnDestroyReward = 0; //The reward you get for destroying an enemy spawner.
        private static int LevelDifficulty = 0; //Only for arena battle levels.
        public static int scoreAtStartOfLevel = 0;
        public static int bombsAtStartOfLevel = 0;
        public static int hpAtStartOfLevel = 30;

        public static int springreset = 0;

        public static int[,] Index;// = new int[,];
        /*{
            {51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,},
            {51,50,50, 0, 0, 0,51, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,50,50,51,},
            {51,50, 0, 0, 0, 0,51, 0, 0, 0, 0, 1, 0, 0, 3, 0, 0, 0,50,51,},
            {51, 0, 0, 0, 0, 0,51, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,51,},
            {51, 0, 0,50, 0,50,51,50, 0, 0, 0, 0,50, 0,50, 0,50, 0,54,51,},
            {51, 0, 0, 0, 0, 0,51, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,52,51,},
            {51, 0, 0, 0,51, 0,51, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,51,},
            {51, 0, 0, 0,51, 0,51, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,51,},
            {51, 0, 0, 0,51, 0,51, 0, 0, 0, 0, 0, 0, 0, 0, 0,17, 0, 0,51,},
            {51,51,51,51,51, 0,51, 0,51, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,51,},
            {51, 0, 0, 0,51, 0,51, 0,51, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,51,},
            {51, 0, 0, 0,51, 0,51, 0,51, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,51,},
            {51,50, 0, 0, 0, 0, 0, 0,51, 0, 0, 0, 0, 0, 0, 0, 0, 0,50,51,},
            {51,50,50, 0, 0, 0, 0, 0,51, 0, 0, 0, 0, 0, 0, 0, 0,50,50,51,},
            {51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,}
        };*/

        public static void LoadSpecialEntities()
        {
            for (int y1 = 0; y1 < Game1.MAPSIZE_H; y1++) //any map scanning logic goes here!
            {
                for (int x1 = 0; x1 < Game1.MAPSIZE_W; x1++)
                {
                    if (Index[y1, x1] >= 2 && Index[y1, x1] <= 5 ) //Create Monster spawner entity
                    {
                        var cls = new Enemies(x1*16, y1*16, 4);
                        Game1.Opponents.Add(cls);
                    }
                }
            }
        }

        public static void RestartLevel()
        {
            GameMap.LoadLevel(Game1.CurrentLevel);
            Windows.DisplayingMessage = true;
            Game1.loadingFrame = 300;
            Game1.loading = true;
            Game1.loadMode = "in";
            Game1.paused = false;
            Game1.Hunter.score = GameMap.scoreAtStartOfLevel;
            Game1.Hunter.HP = GameMap.hpAtStartOfLevel;
            Game1.Hunter.bombsHeld = GameMap.bombsAtStartOfLevel;
            Windows.MessageFrame = -1;
            Game1.gameOverMode = false;
            chimesRemaining = 10;
        }

        public static void LoadLevel(int whichlevel)
        {
            CleanUp();
            Windows.DisplayingMessage = true;
            switch (whichlevel)
            {
                case 1:
                    Index = new int[,]
                        {
                        {51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,},
                        {51, 0, 0, 0,50,50, 0, 0, 0, 0,51, 0, 0,50, 0,50, 0, 0, 0,51,},
                        {51, 0, 0, 0, 0,50, 0, 0, 0, 0,51, 0, 0, 0, 0, 0, 0, 0, 0,51,},
                        {51, 0, 0, 0, 0,50, 0, 0, 0,62,51, 0, 0, 0, 0, 0, 0, 0,50,51,},
                        {51,50, 0, 0, 0,50, 0, 0, 0,62,51, 0, 8, 0, 0, 0, 0, 0, 0,51,},
                        {51,50,50,50, 0,50, 0, 0, 0,40,40, 0, 8, 0, 0, 0, 0, 0, 0,51,},
                        {51, 0, 0, 0, 0,50, 0, 0, 0,62,51,51,51,51,51,51,51, 0,51,51,},
                        {51, 0, 0, 0,50,50, 0, 0, 0, 0,51,62, 0, 0, 0, 0, 0, 0, 0,51,},
                        {51, 0, 0,50, 0, 0,50, 0, 0, 0,51, 0, 0, 0, 0, 0, 0, 0, 0,51,},
                        {51, 0, 0, 0, 0, 0,50, 0, 0, 0,51, 0, 0,50, 0, 0, 0, 0,50,51,},
                        {51, 0, 0, 0, 0,73,72,72, 0, 0,51, 0, 0, 0, 0, 0,51,71,51,51,},
                        {51, 0, 0, 0, 0,17, 8, 0, 0, 0,51, 0, 0, 0, 0, 0,51,42,42,51,},
                        {51,62, 0, 0, 0,72,72,72, 0, 0,51, 0,13, 0, 0, 0,51,42,52,51,},
                        {51,62, 0, 0, 0, 0,50, 0, 0, 0,51, 0, 0, 0, 0, 0,51,42,42,51,},
                        {51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,}
                        };
                    biome = "grasslands";
                    LevelMessage = "Hello Recruit!  Please destroy 5 floppy disks to move on.";
                    VictoryCondition = "Destroy Enemies";
                    ObjectsRemaining = 5;
                    cleartile = 0;
                    Game1.Hunter.x = 2 * 16; Game1.Hunter.y = 4 * 16;

                    var cls = new Enemies(2 * 16, 11 * 16, 1); //Add our enemies!
                    Game1.Opponents.Add(cls);
                    cls = new Enemies(8 * 16, 9 * 16, 1);
                    Game1.Opponents.Add(cls);
                    cls = new Enemies(7 * 16, 3 * 16, 1);
                    Game1.Opponents.Add(cls);
                    cls = new Enemies(13 * 16, 3 * 16, 1);
                    Game1.Opponents.Add(cls);
                    cls = new Enemies(14 * 16, 12 * 16, 1);
                    Game1.Opponents.Add(cls);

                    break;
                case 2:
                    Index = new int[,]
                        {
                        {51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,},
                        {51, 0, 0, 0, 0, 0, 0,62,62,62,50, 0,50, 0,50, 0,50, 2, 0,51,},
                        {51, 0,52, 0, 0, 0, 0,62,62,62,50, 0,50, 0,50, 0,50, 0, 0,51,},
                        {51, 0, 0, 0, 0, 0, 0, 0, 0, 0,50, 0,50, 0,50, 0, 0, 0, 0,51,},
                        {51, 0, 0, 0, 0, 0, 0, 0, 0, 0,50, 8,50, 8,50, 8, 8, 8, 8,51,},
                        {51, 0, 0, 0, 0, 0, 0, 0, 0, 0,50, 8,50, 8,50, 8, 8, 8, 8,51,},
                        {51, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,51,},
                        {51, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,51,},
                        {51, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,51,},
                        {51,51,51,51,51,51,51, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8,51,},
                        {51,42,42,42,42,13,51, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 8, 8,51,},
                        {51,42,42,42,42,42,71, 0, 0, 0, 0, 0, 0, 8, 8, 8, 8, 8, 8,51,},
                        {51,42,42,42,42,42,51, 0, 0, 0, 0, 0, 8, 8, 8, 0, 0, 0, 0,51,},
                        {51,42,42,42,42,42,51, 0, 0, 0, 0, 8, 8, 8, 0, 0, 0, 0, 0,51,},
                        {51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,}
                        };
                    biome = "grasslands";
                    LevelMessage = "Target practice! Charge your attack!";
                    VictoryCondition = "Destroy Enemies";
                    ObjectsRemaining = 6;
                    cleartile = 0; //a simple grass tile for transparent graphics.
                    //var cls = new Enemies(200, 100, 3);
                    //Game1.Opponents.Add(cls);
                    Game1.Hunter.x = 2 * 16; Game1.Hunter.y = 12 * 16;


                    cls = new Enemies(2 * 16, 5 * 16, 1);
                    Game1.Opponents.Add(cls);
                    cls = new Enemies(11 * 16, 2 * 16, 1);
                    Game1.Opponents.Add(cls);
                    cls = new Enemies(13 * 16, 2 * 16, 1);
                    Game1.Opponents.Add(cls);
                    cls = new Enemies(16 * 16, 12 * 16, 2);
                    Game1.Opponents.Add(cls);

                    break;
                case 3:
                    Index = new int[,]
                        {
                        {50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,50,},
                        {50,50, 0, 0,50, 0,50, 0,51,51,51,51,51,51,51,51,51,51,51,50,},
                        {50, 0, 0, 0, 0,50, 0, 0,51,43,43,43,51,42,42,42,42,42,51,50,},
                        {50, 0, 0, 0, 0,50, 0, 0,51,51,51,51,51,42,52,42,42,42,51,50,},
                        {50, 0, 0, 0,50,51,51,51,51,42,42,42,51,42,42,42,42,42,51,50,},
                        {50,50, 0,50, 0,51,62,43,51,42,51,42,51,51,51,51,51,42,51,50,},
                        {50, 8, 0, 8, 0,51,43,43,51,42,51,42,51,43,43,43,51,42,51,50,},
                        {50, 0, 0, 8, 0,51,43,43,51,42,51,42,51,43,43,43,51,42,51,50,},
                        {50, 0, 8, 8, 0,51,43,43,51,42,51,42,51,43,43,43,51,42,51,50,},
                        {50, 0, 8, 8,51,51,51,51,51,42,51,42,51,43,43,43,51,42,51,50,},
                        {50, 0, 0, 0,42,42,42,42,42,42,51,42,51,62,43,43,51,42,51,50,},
                        {50, 8, 8, 8,51,51,51,51,51,51,51,42,51,51,51,51,51,42,51,50,},
                        {50, 0, 0, 0, 0,51,43,43,43,43,51,42,42,42,42,42,42,42,51,50,},
                        {50,50, 0, 0,50,51,43,43,43,43,51,51,51,51,51,51,51,51,51,50,},
                        {50,50,50,50,50,51,51,51,51,51,51,50,50,50,50,50,50,50,50,50,}
                        };
                    biome = "grasslands";
                    LevelMessage = "Try your special attack.";
                    VictoryCondition = "Destroy Enemies";
                    ObjectsRemaining = 6;
                    cleartile = 43; //checkered floor tile

                    Game1.Hunter.x = 2 * 16; Game1.Hunter.y = 3 * 16;

                    cls = new Enemies(10 * 16, 2 * 16, 1);
                    Game1.Opponents.Add(cls);
                    cls = new Enemies(7 * 16, 7 * 16, 1);
                    Game1.Opponents.Add(cls);
                    cls = new Enemies(7 * 16, 6 * 16, 1);
                    Game1.Opponents.Add(cls);
                    cls = new Enemies(14 * 16, 8 * 16, 3);
                    Game1.Opponents.Add(cls);
                    cls = new Enemies(14 * 16, 7 * 16, 1);
                    Game1.Opponents.Add(cls);
                    cls = new Enemies(7 * 16, 13 * 16 + 7, 3);
                    Game1.Opponents.Add(cls);
                    cls = new Enemies(14 * 16, 2 * 16, 2);
                    Game1.Opponents.Add(cls);

                    break;
                case 4:
                    Index = new int[,]
                        {
                        {51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,},
                        {51,62,62,11,51, 0,51, 0, 0, 3, 0,52, 0,51, 0,70,42,42,42,51,},
                        {51, 2, 0, 0,51, 0,51, 0, 0, 0, 0, 0, 0,51, 0,51,42,42,42,51,},
                        {51, 0, 0, 0,51, 0,51, 0, 0, 0, 0, 0, 0,51, 0,51,42,42,42,51,},
                        {51, 0, 0, 0,51, 0, 0,51,51,70,51,51,51, 0, 0,51,42,42, 2,51,},
                        {51, 0, 0, 0,51, 0, 0, 0,71,71,71, 0, 0, 0, 0,51,42,42,42,51,},
                        {51, 0, 0, 0,51, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,51,42,42,42,51,},
                        {51,51,43,51,51,50, 0, 0,50,50,50, 0, 0, 0, 0,51,51,42,51,51,},
                        {51,43,43,43,51, 0,50, 0, 0, 0, 0,50,50,50,50,51,42,42,42,51,},
                        {51,43,43,43,51, 0, 0, 0, 0, 0, 0, 0, 0,50,50,51,42,42,42,51,},
                        {51,43,43,43,43, 0, 0, 0, 0, 0, 0, 0, 0, 0,50,51,42,42,42,51,},
                        {51,43,43,43,43, 0,50, 0, 0, 0, 0, 0, 0, 0, 0,51,42,42,42,51,},
                        {51,43,43,43,51, 0, 0, 0, 0, 0, 0, 0, 0, 0,50,51,42, 2,42,51,},
                        {51,43,43,43,51,62, 0, 0, 0, 0, 0, 0,50, 0,50,51,42,42,13,51,},
                        {51,51,51,51,51,50,50,50,50,50,50,50,50,50,50,51,51,51,51,51,}
                        };
                    biome = "grasslands";
                    LevelMessage = "Too much obsolete tech! Destroy the spawners!";
                    VictoryCondition = "Destroy Spawners";
                    ObjectsRemaining = 4;
                    cleartile = 0; //a simple grass tile for transparent graphics.
                    //var cls = new Enemies(200, 100, 3);
                    //Game1.Opponents.Add(cls);
                    Game1.Hunter.x = 10 * 16; Game1.Hunter.y = 12 * 16;

                    break;
                case 5:
                    Index = new int[,]
                        {
                        {51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,},
                        {63,64, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,63,},
                        {63,63,52, 0, 0, 0, 0,28, 0, 0, 0, 0,28, 0, 0, 0,64,64, 0,63,},
                        {63, 0,64, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,64, 0,63,},
                        {63,64, 0, 0, 0, 0, 0,51,51,51,51,51,51, 0, 0, 0, 0, 0, 0,63,},
                        {63,63, 0, 0,17, 8,17,23,24,25,26,27,23,17, 8,17,64, 0, 0,63,},
                        {63,63, 0, 0, 0, 8, 0,23, 0, 0, 0, 0,23, 0, 8, 0, 0, 0, 0,63,},
                        {63, 0, 0, 0, 0, 8, 0,23, 0, 0, 0, 0,23, 0, 8, 0, 0, 0, 0,63,},
                        {63, 0, 0, 0, 0, 8, 0,23, 0, 0, 0, 0,23, 0, 8, 0, 0, 0, 0,63,},
                        {63,17, 0, 0,17, 8,17,23,23,23,23,23,23,17, 8,17, 0, 0,17,63,},
                        {63, 8,63,23,63, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,63,23,63, 8,63,},
                        {63,17, 0, 0,63, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,63, 0, 0,17,63,},
                        {63, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,63,},
                        {63, 0,28, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,28, 0,63,},
                        {51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,},
                        };
                    biome = "snow";
                    LevelMessage = "Just...don't die!";
                    VictoryCondition = "Survive";
                    ObjectsRemaining = 3600; //The amount of time in frames-> 2 minutes
                    cleartile = 22; //a simple snow tile for transparent graphics.
                    //var cls = new Enemies(200, 100, 3);
                    //Game1.Opponents.Add(cls);
                    Game1.Hunter.x = 10 * 16; Game1.Hunter.y = 12 * 16;

                    spawnFrequency = 200;
                    LevelDifficulty = 2;

                    break;
                case 6:
                    Index = new int[,]
                        {
                        {51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,},
                        {63, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,62, 0, 0, 0, 0, 0, 0, 0,63,},
                        {63, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,62, 0, 0, 0, 0, 0, 4, 0,63,},
                        {63, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,62, 0, 0, 0, 0, 0, 0, 0,63,},
                        {63, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,62, 0, 0, 0, 0, 0, 0, 0,63,},
                        {63, 0, 0, 0, 0, 0, 0, 0, 0,62, 0,62, 0, 0, 0, 0, 0, 0, 0,63,},
                        {63, 0, 0, 0,63, 0, 0, 0, 0,62, 0,62, 0, 0, 0, 0, 0, 0, 0,63,},
                        {63, 0, 0, 0, 0, 0, 0, 0, 0,62,23,62, 0, 0, 0,63,63, 0, 0,63,},
                        {63, 0, 0, 0, 0, 0, 0, 0, 0,62,62,62, 0, 0,63,63,63, 0, 0,63,},
                        {51,51,51,51,51,51, 0, 0, 0, 0, 0,62, 0, 0,63, 0, 0, 0, 0,63,},
                        {51,42,42,42,42,51, 0, 0, 0, 0, 0,62, 0, 0, 0, 0, 0, 0, 0,63,},
                        {51,42,42,42,42,70, 0, 0, 0, 0, 0,62, 0, 0, 0, 0,23,23,23,63,},
                        {51,42,42,42,42,51, 0,63, 0, 0, 0,62, 0, 0, 0, 0,23,52,23,63,},
                        {51,42,42,42,11,51, 0, 0, 0, 0, 0,62, 0, 0, 0, 0,23,23,23,63,},
                        {51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,},
                        };
                    biome = "snow";
                    LevelMessage = "Place a bomb with (c) next to the barrels!";
                    VictoryCondition = "Destroy Enemies";
                    ObjectsRemaining = 6; //6 enemies to destroy
                    cleartile = 22; //a simple snow tile for transparent graphics.
                    cls = new Enemies(2 * 16, 4 * 16, 3);
                    Game1.Opponents.Add(cls);
                    cls = new Enemies(4 * 16, 4 * 16, 3);
                    Game1.Opponents.Add(cls);
                    cls = new Enemies(2 * 16, 2 * 16, 3);
                    Game1.Opponents.Add(cls);
                    cls = new Enemies(4 * 16, 2 * 16, 3);
                    Game1.Opponents.Add(cls);
                    Game1.Hunter.x = 2 * 16; Game1.Hunter.y = 12 * 16;
                    break;
                case 7:
                    Index = new int[,]
                        {
                        {51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,},
                        {51,13, 0,62,51, 0, 0, 0, 0,51,51,52,51, 0, 0,62,62, 0,15,51,},
                        {51, 0, 0, 0,51, 0, 0, 0, 0,21,20,42,51, 0, 0,62,62, 0, 0,51,},
                        {51, 0, 0,40,51, 0, 0, 0, 0,51,51,42,51, 0, 0,62,62, 0, 0,51,},
                        {51, 0, 0,62,51, 0, 0,51,17,51,51,51,51, 0, 0,62,62,62,62,51,},
                        {51,17,51,51,51, 0, 0,74, 8, 0,51, 0, 0, 0, 0,62,62,62,62,51,},
                        {74, 8,74, 0, 0, 0, 0,74, 0, 0,51, 0, 4, 0, 0, 0, 0, 0, 0,51,},
                        {74,17,74, 0, 0, 0,72,75, 0, 0,51, 0, 0, 0, 0, 0, 0, 0, 0,51,},
                        {74, 8,74, 0, 0, 0,20, 0,13, 0,51,51,51, 0, 0, 0, 0, 0, 0,51,},
                        {74,17,74, 0,71, 0,72,51,17, 0, 0, 0,51, 0, 0, 0, 0, 0, 0,51,},
                        {74, 8,74,51,70,51,51,75, 8,51,51, 0,51, 0, 0, 0, 0, 0, 0,51,},
                        {74,17,74, 0, 0, 0,62, 0,51, 0, 0, 0,51,51,51, 0,51,51,51,51,},
                        {75, 8,75, 0, 0, 0,62,11,51, 2,51,51,51,42,42,42,42,42,42,51,},
                        {51,17, 0, 0, 0, 0,62, 0,51, 0, 0, 0, 0,42,42,42,42,42,42,51,},
                        {51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,},
                        };
                    biome = "dungeon";
                    LevelMessage = "You need some explosives here.";
                    VictoryCondition = "Destroy Enemies";
                    ObjectsRemaining = 1;
                    cleartile = 44; //dungeon tile
                    cls = new Enemies(4*16, 12*16, 3);
                    Game1.Opponents.Add(cls);
                    cls = new Enemies(6 * 16, 2 * 16, 3);
                    Game1.Opponents.Add(cls);
                    Game1.Hunter.x = 2 * 16; Game1.Hunter.y = 3 * 16;
                    break;
                case 8:
                    Index = new int[,]
                        {
                        {74,51,51,51,51,76,76,76,76,76,76,76,76,76,76,51,51,51,51,74,},
                        {74, 0, 0, 0, 0,45,45,45,45,29,45,45,45,45,45, 0, 0, 0, 0,74,},
                        {74, 0,29, 0, 0,45,45,45,45,45,45,45,45,45,45, 0, 0,29, 0,74,},
                        {74, 0, 0, 0, 0,76,76,45,76,76,76,76,45,76,76, 0, 0, 0, 0,74,},
                        {74, 0, 0, 0, 0,45,45,45,45,45,45,45,45,45,45, 0, 0, 0, 0,74,},
                        {74, 0, 0, 0, 0,45,45,45,45,45,45,45,45,45,45, 0, 0, 0, 0,74,},
                        {74, 0, 0, 0, 0,76,76,45,76,76,76,76,45,76,76, 0, 0, 0, 0,74,},
                        {74, 0, 0, 0,17, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,62,62,74,},
                        {74, 0, 0, 0, 0, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,62,11,74,},
                        {74, 0, 0, 0, 0, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,62,62,74,},
                        {74, 0, 0, 0, 0, 8,17, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,74,},
                        {74, 0, 0, 0, 0, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,74,},
                        {74, 0,29, 0, 0, 8, 0, 0, 0, 0,70, 0, 0, 0, 0, 0, 0,29, 0,74,},
                        {74, 0, 0, 0,17, 8, 0, 0, 0,51,52,51, 0, 0, 0, 0, 0, 0, 0,74,},
                        {75,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,74,},
                        };
                    biome = "dungeon";
                    LevelMessage = "Uh oh! Another big fight!";
                    VictoryCondition = "Survive";
                    ObjectsRemaining = 3600; //The amount of time in frames-> 2 minutes
                    cleartile = 44; //a simple snow tile for transparent graphics.
                    //var cls = new Enemies(200, 100, 3);
                    //Game1.Opponents.Add(cls);
                    Game1.Hunter.x = 10 * 16; Game1.Hunter.y = 10 * 16;

                    spawnFrequency = 250;
                    LevelDifficulty = 3;

                    break;
                case 9:
                    Index = new int[,]
                        {
                        {51,81,80,81,80,81,80,81,80,81,80,81,80,81,80,81,80,81,80,51,},
                        {51,80,81,80,81,80,81,80,35,36,37,38,81,80,81,80,81,80,81,51,},
                        {51,86,86,86,86,86,86,86,86,86,86,86,86,86,86,86,86,86,86,51,},
                        {51, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,51,},
                        {51, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,51,},
                        {51, 0, 0,97,98,97,98, 0, 0, 0, 0, 0, 0,87,88,97,88, 0, 0,51,},
                        {51, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,51,},
                        {51, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,51,},
                        {51, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,51,},
                        {51, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,51,},
                        {51, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,51,},
                        {51, 0, 0,87,88,87,88, 0, 0, 0, 0, 0, 0,97,98,87,98, 0, 0,51,},
                        {51, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,51,},
                        {51, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,51,},
                        {51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,51,},
                        };
                    biome = "bossLair";
                    LevelMessage = "Your nemesis has arrived.";
                    VictoryCondition = "Defeat Boss";
                    ObjectsRemaining = 1; //The amount of time in frames-> 2 minutes
                    cleartile = 45; //a simple snow tile for transparent graphics.
                    //var cls = new Enemies(200, 100, 3);
                    //Game1.Opponents.Add(cls);
                    Game1.Hunter.x = 10 * 16; Game1.Hunter.y = 10 * 16;

                    spawnFrequency = 250;
                    LevelDifficulty = 3;
                    cls = new Enemies(10 * 16, 5 * 16, 5);
                    Game1.Opponents.Add(cls);

                    break;
                default:
                    break;
            }
            chimesRemaining = 10;
            LoadSpecialEntities();
        }

        public static bool HaveCollision(int x, int y, int sizex, int sizey, bool avoidDanger=false, bool bigEntity=false)
        {
            bool checking = false;
            if (bigEntity == false) //If it's a small thing, we don't need to do anything fancy - Just check a few points on the map.
            {
                if (Index[Convert.ToInt16(y / 16), Convert.ToInt16(x / 16)] > 49) checking = true;
                if (Index[Convert.ToInt16((y + sizey) / 16), Convert.ToInt16(x / 16)] > 49) checking = true;
                if (Index[Convert.ToInt16(y / 16), Convert.ToInt16((x + sizex) / 16)] > 49) checking = true;
                if (Index[Convert.ToInt16((y + sizey) / 16), Convert.ToInt16((x + sizex) / 16)] > 49) checking = true;
                if (avoidDanger == true)
                {
                    if (Index[Convert.ToInt16(y / 16), Convert.ToInt16(x / 16)] == 8) checking = true;
                    if (Index[Convert.ToInt16((y + sizey) / 16), Convert.ToInt16(x / 16)] == 8) checking = true;
                    if (Index[Convert.ToInt16(y / 16), Convert.ToInt16((x + sizex) / 16)] == 8) checking = true;
                    if (Index[Convert.ToInt16((y + sizey) / 16), Convert.ToInt16((x + sizex) / 16)] == 8) checking = true;
                }
            }
            else //However, for bigger things, we ought to do a small loop. I had to implement this because otherwise the giant final boss could walk through tables.
            {
                for (var cy = 0; cy <= (sizey / 16); cy++)
                {
                    for (var cx = 0; cx <= (sizex / 16); cx++)
                    {
                        if (Index[Convert.ToInt16(y / 16) + cy, Convert.ToInt16(x / 16) + cx] > 49) checking = true;
                    }
                }
            }

            return checking;
        }

        public static bool IsOnTileTrigger(int mapx, int mapy, int cx, int cy, int sizeX, int sizeY) //code copied from player collision data
        {
            bool foundHit = false;
            int Pcx = (mapx * Game1.TILE_W) + 6; //map triggerbox info
            int Pcy = (mapy * Game1.TILE_H) + 6;
            int Psx = 3; //sizes. Note that it's small!
            int Psy = 3;

            if (cx >= Pcx && cx <= Pcx + Psx && cy >= Pcy && cy <= Pcy + Psy) foundHit = true;
            if (cx + sizeX >= Pcx && cx + sizeX <= Pcx + Psx && cy >= Pcy && cy <= Pcy + Psy) foundHit = true;
            if (cx >= Pcx && cx <= Pcx + Psx && cy + sizeY >= Pcy && cy + sizeY <= Pcy + Psy) foundHit = true;
            if (cx + sizeX >= Pcx && cx + sizeX <= Pcx + Psx && cy + sizeY >= Pcy && cy + sizeY <= Pcy + Psy) foundHit = true;

            return foundHit;
        }

        public static void Draw(string layer="background")
        {

            int exitX = -50;
            int exitY = -50;
            if (layer == "background")
            {
                for (int y = 0; y < 15; y++)
                {
                    for (int x = 0; x < 20; x++)
                    {
                        if (Index[y,x] >= 40 && Index[y, x] < 50) Game1.spriteBatch.Draw(Tile.TileSetTexture,new Rectangle(x * 16, y * 16, 16, 16),Tile.GetSourceRectangle(cleartile),Color.White);
                        if (Index[y, x] >= 60 && Index[y, x] < 70) Game1.spriteBatch.Draw(Tile.TileSetTexture, new Rectangle(x * 16, y * 16, 16, 16), Tile.GetSourceRectangle(cleartile), Color.White);


                        Game1.spriteBatch.Draw(
                            Tile.TileSetTexture,
                            new Rectangle(x * 16, y * 16, 16, 16),
                            Tile.GetSourceRectangle(GameMap.Index[y, x]),
                            Color.White);
                        if (Index[y,x] == 0 && biome == "dungeon") Game1.spriteBatch.Draw(Tile.TileSetTexture, new Rectangle(x * 16, y * 16, 16, 16), Tile.GetSourceRectangle(44), Color.White);
                        if (Index[y, x] == 0 && biome == "snow") Game1.spriteBatch.Draw(Tile.TileSetTexture, new Rectangle(x * 16, y * 16, 16, 16), Tile.GetSourceRectangle(22), Color.White);
                        if (Index[y, x] == 0 && biome == "bossLair") Game1.spriteBatch.Draw(Tile.TileSetTexture, new Rectangle(x * 16, y * 16, 16, 16), Tile.GetSourceRectangle(45), Color.White);
                        //if (GameMap.Index[y, x] == 20)
                        //{
                        //    Game1.spriteBatch.Draw(Tile.TileSetTexture, new Rectangle(x * 16, (y - 1) * 16, 16, 16), Tile.GetSourceRectangle(60), Color.White);
                        //    Game1.spriteBatch.Draw(Tile.TileSetTexture, new Rectangle(x * 16, y * 16, 16, 16), Tile.GetSourceRectangle(61), Color.White);
                        //}
                        /*if (GameMap.Index[y, x] == 1 && spawnFlash > -1)
                        {
                            spawnFlash++;
                            Game1.spriteBatch.Draw(Tile.TileSetTexture, new Rectangle(x * 16, y * 16, 16, 16), Tile.GetSourceRectangle(2),
                             Color.White * Convert.ToSingle(Math.Sin(3.14 * ((double)spawnFlash / 30))));
                            if (spawnFlash > 30) spawnFlash = -1;
                        }
                        */
                        if (GameMap.Index[y, x] == 51 && biome == "grasslands") Game1.spriteBatch.Draw(Tile.TileSetTexture, new Rectangle(x * 16, y * 16, 16, 16), Tile.GetSourceRectangle(56), Color.White);
                        if (GameMap.Index[y, x] == 51 && biome == "snow") Game1.spriteBatch.Draw(Tile.TileSetTexture, new Rectangle(x * 16, y * 16, 16, 16), Tile.GetSourceRectangle(66), Color.White);
                        if (GameMap.Index[y, x] == 51 && biome == "bossLair") Game1.spriteBatch.Draw(Tile.TileSetTexture, new Rectangle(x * 16, y * 16, 16, 16), Tile.GetSourceRectangle(68), Color.White);
                        if (GameMap.Index[y, x] == 51 && biome == "dungeon") Game1.spriteBatch.Draw(Tile.TileSetTexture, new Rectangle(x * 16, y * 16, 16, 16), Tile.GetSourceRectangle(58), Color.White);

                    }
                }
            }
            else if (layer == "foreground")
            {
                for (int y = 0; y < 15; y++)
                {
                    for (int x = 0; x < 20; x++)
                    {

                        if (GameMap.Index[y, x] == 51)
                        {
                            if (biome == "grasslands")
                            {
                                //Game1.spriteBatch.Draw(Tile.TileSetTexture, new Rectangle(x * 16, y * 16, 16, 16), Tile.GetSourceRectangle(56), Color.White);
                                Game1.spriteBatch.Draw(Tile.TileSetTexture, new Rectangle(x * 16, y * 16 - 8, 16, 16), Tile.GetSourceRectangle(55), Color.White);
                                if (y < Game1.MAPSIZE_H - 1 && x < Game1.MAPSIZE_W - 1) //Draw shadows!
                                {
                                    if (GameMap.Index[y, x + 1] != 51)
                                    {
                                        if (GameMap.Index[y + 1, x] == 51) Game1.spriteBatch.Draw(Tile.TileSetTexture, new Rectangle((x + 1) * 16, y * 16, 16, 16), Tile.GetSourceRectangle(90), Color.White * 0.3f);
                                        else Game1.spriteBatch.Draw(Tile.TileSetTexture, new Rectangle((x + 1) * 16, y * 16, 16, 16), Tile.GetSourceRectangle(91), Color.White * 0.3f);
                                    }
                                }

                            }
                            if (biome == "dungeon")
                            {
                                //Game1.spriteBatch.Draw(Tile.TileSetTexture, new Rectangle(x * 16, y * 16, 16, 16), Tile.GetSourceRectangle(56), Color.White);
                                Game1.spriteBatch.Draw(Tile.TileSetTexture, new Rectangle(x * 16, y * 16 - 8, 16, 16), Tile.GetSourceRectangle(57), Color.White);
                                if (y < Game1.MAPSIZE_H - 1 && x < Game1.MAPSIZE_W - 1) //Draw shadows!
                                {
                                    if (GameMap.Index[y, x + 1] != 51)
                                    {
                                        if (GameMap.Index[y + 1, x] == 51) Game1.spriteBatch.Draw(Tile.TileSetTexture, new Rectangle((x + 1) * 16, y * 16, 16, 16), Tile.GetSourceRectangle(90), Color.White * 0.3f);
                                        else Game1.spriteBatch.Draw(Tile.TileSetTexture, new Rectangle((x + 1) * 16, y * 16, 16, 16), Tile.GetSourceRectangle(91), Color.White * 0.3f);
                                    }
                                }

                            }
                            if (biome == "snow")
                            {
                                //Game1.spriteBatch.Draw(Tile.TileSetTexture, new Rectangle(x * 16, y * 16, 16, 16), Tile.GetSourceRectangle(56), Color.White);
                                Game1.spriteBatch.Draw(Tile.TileSetTexture, new Rectangle(x * 16, y * 16 - 8, 16, 16), Tile.GetSourceRectangle(65), Color.White);
                                if (y < Game1.MAPSIZE_H - 1 && x < Game1.MAPSIZE_W - 1) //Draw shadows!
                                {
                                    if (GameMap.Index[y, x + 1] != 51)
                                    {
                                        if (GameMap.Index[y + 1, x] == 51) Game1.spriteBatch.Draw(Tile.TileSetTexture, new Rectangle((x + 1) * 16, y * 16, 16, 16), Tile.GetSourceRectangle(90), Color.White * 0.3f);
                                        else Game1.spriteBatch.Draw(Tile.TileSetTexture, new Rectangle((x + 1) * 16, y * 16, 16, 16), Tile.GetSourceRectangle(91), Color.White * 0.3f);
                                    }
                                }

                            }
                            if (biome == "bossLair")
                            {
                                //Game1.spriteBatch.Draw(Tile.TileSetTexture, new Rectangle(x * 16, y * 16, 16, 16), Tile.GetSourceRectangle(56), Color.White);
                                Game1.spriteBatch.Draw(Tile.TileSetTexture, new Rectangle(x * 16, y * 16 - 8, 16, 16), Tile.GetSourceRectangle(67), Color.White);
                                if (y < Game1.MAPSIZE_H - 1 && x < Game1.MAPSIZE_W - 1) //Draw shadows!
                                {
                                    if (GameMap.Index[y, x + 1] != 51)
                                    {
                                        if (GameMap.Index[y + 1, x] == 51) Game1.spriteBatch.Draw(Tile.TileSetTexture, new Rectangle((x + 1) * 16, y * 16, 16, 16), Tile.GetSourceRectangle(90), Color.White * 0.3f);
                                        else Game1.spriteBatch.Draw(Tile.TileSetTexture, new Rectangle((x + 1) * 16, y * 16, 16, 16), Tile.GetSourceRectangle(91), Color.White * 0.3f);
                                    }
                                }

                            }
                        }
                        if (GameMap.Index[y, x] >= 70 && Index[y, x] < 80)
                        {

                            //Game1.spriteBatch.Draw(Tile.TileSetTexture, new Rectangle(x * 16, y * 16, 16, 16), Tile.GetSourceRectangle(56), Color.White);
                            //Game1.spriteBatch.Draw(Tile.TileSetTexture, new Rectangle(x * 16, y * 16, 16, 16), Tile.GetSourceRectangle(55), Color.White);
                            if (y < Game1.MAPSIZE_H - 1 && x < Game1.MAPSIZE_W - 1) //Draw shadows!
                            {
                                if (GameMap.Index[y, x + 1] != 51 && !(GameMap.Index[y, x + 1] >= 70 && Index[y, x + 1] < 80))
                                {
                                    if (GameMap.Index[y + 1, x] >= 70 && Index[y, x] < 80) Game1.spriteBatch.Draw(Tile.TileSetTexture, new Rectangle((x + 1) * 16, y * 16, 16, 16), Tile.GetSourceRectangle(92), Color.White * 0.3f);
                                    else Game1.spriteBatch.Draw(Tile.TileSetTexture, new Rectangle((x + 1) * 16, y * 16, 16, 16), Tile.GetSourceRectangle(93), Color.White * 0.3f);
                                }
                            }

                        }
                        if (ObjectsRemaining <= 0 && Index[y, x] == 41)
                        {
                            exitX = x * 16;
                            exitY = y * 16 + 16;
                        }
                    }
                }
                //Draw the exit arrow if the player has finished.
                if (ObjectsRemaining <= 0)
                {
                    if (mapAnimationCount % 3 == 0) Game1.spriteBatch.Draw(Game1.levelFinished, new Rectangle(exitX, exitY, 16, 24), Color.White);
                }
            }
        }

        public static void Logic()
        {
            mapAnimationCount++;
            if (mapAnimationCount > 35) mapAnimationCount = 0;

            if (ObjectsRemaining <= 0 && chimesRemaining > 0)
            {
                if (mapAnimationCount == 35)
                {
                    Game1.levelFinishedChime.Play(0.4f, 0.0f, 0.0f);
                    chimesRemaining--;
                }

            }

            if (Index[(int)((Game1.Hunter.y + 12) / Game1.TILE_H), (int)((Game1.Hunter.x + 3) / Game1.TILE_W)] == 17 &&  //check to see if player is on spring.
                Index[(int)((Game1.Hunter.y + 10 + 4) / Game1.TILE_H), (int)((Game1.Hunter.x + 3 + 9) / Game1.TILE_W)] == 17 &&
                Game1.Hunter.bouncing == false) //The spring!
            {
                //Good Lord, hate typing this long code sometimes
                //Have to check if the player is in the small trigger region, otherwise, the spring would activate in inappropriate places.
                //if (IsOnTileTrigger((int)((Game1.Hunter.x + 3) / Game1.TILE_W), (int)((Game1.Hunter.y + 7) / Game1.TILE_H), Game1.Hunter.x + 3, Game1.Hunter.y + 7, 9, 8) == true)
                //{
                Game1.Hunter.yVelocity = 2;
                Game1.Hunter.bouncing = true;
                springreset = 20;
                Index[(int)((Game1.Hunter.y + 12) / Game1.TILE_H), (int)((Game1.Hunter.x + 3) / Game1.TILE_W)] = 19;
                Game1.Jump.Play();
                //}
            }

            //Ouch...spikes
            if (Index[(int)((Game1.Hunter.y + 14) / Game1.TILE_H), (int)((Game1.Hunter.x + 8) / Game1.TILE_W)] == 8 && Game1.Hunter.hurtInvincible == false && Game1.Hunter.bouncing == false)
            {
                Game1.Hunter.DamageDealt(7, "Up", false);
            }

            if (Index[(int)((Game1.Hunter.y + 14) / Game1.TILE_H), (int)((Game1.Hunter.x + 8) / Game1.TILE_W)] == 11 && Game1.Hunter.bouncing == false) //red button
            {
                Game1.isScreenShaking = true;
                Game1.Press.Play(0.5f, 0.0f, 0.0f);
                Index[(int)((Game1.Hunter.y + 14) / Game1.TILE_H), (int)((Game1.Hunter.x + 8) / Game1.TILE_W)] = 12;
                for (int y1 = 0; y1 < Game1.MAPSIZE_H; y1++) //any map scanning logic goes here!
                {
                    for (int x1 = 0; x1 < Game1.MAPSIZE_W; x1++)
                    {
                        switch (Index[y1, x1])
                        {
                            case 20:
                                Index[y1, x1] = 70;
                                break;
                            case 70:
                                Index[y1, x1] = 20;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            if (Index[(int)((Game1.Hunter.y + 14) / Game1.TILE_H), (int)((Game1.Hunter.x + 8) / Game1.TILE_W)] == 13 && Game1.Hunter.bouncing == false) //blue button
            {
                Game1.isScreenShaking = true;
                Game1.Press.Play(0.5f, 0.0f, 0.0f);
                Index[(int)((Game1.Hunter.y + 14) / Game1.TILE_H), (int)((Game1.Hunter.x + 8) / Game1.TILE_W)] = 14;
                for (int y1 = 0; y1 < Game1.MAPSIZE_H; y1++) //any map scanning logic goes here!
                {
                    for (int x1 = 0; x1 < Game1.MAPSIZE_W; x1++)
                    {
                        switch (Index[y1, x1])
                        {
                            case 21:
                                Index[y1, x1] = 71;
                                break;
                            case 71:
                                Index[y1, x1] = 21;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            if (Index[(int)((Game1.Hunter.y + 14) / Game1.TILE_H), (int)((Game1.Hunter.x + 8) / Game1.TILE_W)] == 15 && Game1.Hunter.bouncing == false) //yellow button
            {
                Game1.isScreenShaking = true;
                Game1.Press.Play(0.5f, 0.0f, 0.0f);
                Index[(int)((Game1.Hunter.y + 14) / Game1.TILE_H), (int)((Game1.Hunter.x + 8) / Game1.TILE_W)] = 16;
                for (int y1 = 0; y1 < Game1.MAPSIZE_H; y1++) 
                {
                    for (int x1 = 0; x1 < Game1.MAPSIZE_W; x1++)
                    {
                        switch (Index[y1, x1])
                        {
                            case 21:
                                Index[y1, x1] = 71;
                                break;
                            case 71:
                                Index[y1, x1] = 21;
                                break;
                            case 20:
                                Index[y1, x1] = 70;
                                break;
                            case 70:
                                Index[y1, x1] = 20;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            //!!!!!!!!!!!!!!!!!!!!!
            //Arena level!!!
            if (VictoryCondition == "Survive")
            {
                if (ObjectsRemaining > 0) ObjectsRemaining--;
                spawnTimer--;
                if (spawnTimer < 0)
                { //Have to make the enemies spawn more frequently as time goes on. 
                    spawnFrequency = (int)(spawnFrequency * 0.95);
                    if (spawnFrequency < 10) spawnFrequency = 60;
                    spawnTimer = spawnFrequency;
                }
                if (ObjectsRemaining <= 10) spawnTimer = 1000; //Never let more enemies spawn after the objective is complete!
            }
            if (Index[(int)((Game1.Hunter.y + 14) / Game1.TILE_H), (int)((Game1.Hunter.x + 8) / Game1.TILE_W)] == 41 && Game1.Hunter.bouncing == false) //Winning tile! ********
            {
                Game1.LevelingUp = true; //initiate the transition into the next level.
            }

            if (springreset > 0) springreset--;

            for (int y = 0; y < Game1.MAPSIZE_H; y++) //any map scanning logic goes here!
            {
                for (int x = 0; x < Game1.MAPSIZE_W; x++)
                {
                    if (Index[y, x] == 76 || Index[y, x] == 77) //server racks have to change colors sometimes!
                    {
                        if (Game1.rnd.Next(0, 10) == 9)
                        {
                            if (Index[y, x] == 76) Index[y, x] = 77;
                            else Index[y, x] = 76;
                        }
                    }

                    if (Index[y, x] >= 80 && Index[y, x] <= 85) //The audience is animated a bit.
                    {
                        if (Game1.rnd.Next(0, 10) == 9)
                        {
                            if (Index[y, x] == 80 || Index[y, x] == 82 || Index[y, x] == 84) Index[y, x] = 80 + Game1.rnd.Next(0, 3) * 2;
                            else Index[y, x] = 81 + Game1.rnd.Next(0, 3) * 2;
                        }
                    }

                    if (Index[y, x] == 19 && springreset < 10) Index[y, x] = 18; //reset the spring slowly!
                    if (Index[y, x] == 18 && springreset < 1) Index[y, x] = 17;
                    if (Index[y, x] == 12 && !(y == (int)((Game1.Hunter.y + 14) / Game1.TILE_H) && x == (int)((Game1.Hunter.x + 8) / Game1.TILE_W)))
                    {
                        Index[y, x] = 11;
                    }
                    if (Index[y, x] == 14 && !(y == (int)((Game1.Hunter.y + 14) / Game1.TILE_H) && x == (int)((Game1.Hunter.x + 8) / Game1.TILE_W)))
                    {
                        Index[y, x] = 13;
                    }
                    if (Index[y, x] == 16 && !(y == (int)((Game1.Hunter.y + 14) / Game1.TILE_H) && x == (int)((Game1.Hunter.x + 8) / Game1.TILE_W))) {
                        Index[y, x] = 15;
                    }
                    if (Index[y, x] == 52 && ObjectsRemaining <= 0) { //opening the exit door.
                        Index[y, x] = 41;
                        ObjectsRemaining = 0;
                        var clsa = new Explosions(x * 16, y * 16, 6);
                        Game1.gameExplosions.Add(clsa);
                        Game1.isScreenShaking = true;
                        Game1.KilledEnemy.Play(0.4f, 0.0f, 0.0f);
                    }

                    //arena level logic here. Have to figure out which enemies to spawn and stuff.
                    if ((Index[y, x] == 28 || Index[y, x] == 29) && VictoryCondition == "Survive" && spawnTimer == 0)
                    {
                        if (LevelDifficulty == 2)
                        {
                            var whichEnemyToSpawn = Game1.rnd.Next(-2, 3);
                            if (whichEnemyToSpawn > 0)
                            {
                                var spawnedEnemy = new Enemies(x * 16, y * 16, whichEnemyToSpawn);
                                spawnedEnemy.spawning = true;
                                spawnedEnemy.spawnTimer = 30;
                                Game1.Opponents.Add(spawnedEnemy);
                            }
                        }
                        if (LevelDifficulty == 3)
                        {
                            var whichEnemyToSpawn = Game1.rnd.Next(-2, 4);
                            if (whichEnemyToSpawn > 0)
                            {
                                var spawnedEnemy = new Enemies(x * 16, y * 16, whichEnemyToSpawn);
                                spawnedEnemy.spawning = true;
                                spawnedEnemy.spawnTimer = 30;
                                Game1.Opponents.Add(spawnedEnemy);
                            }
                        }
                    }
                }
            }
            /*
                    spawnTimer += 1;
            if (spawnTimer >= spawnFrequency) //Enemy spawners
            {
                spawnTimer = 0;
                spawnFlash = 0; //Setting it to 0 causes the flash effect on the map tile.
                for (int y = 0; y < 15; y++)
                {
                    for (int x = 0; x < 20; x++)
                    {
                        if (GameMap.Index[y, x] == 2) //basic floppy disk
                        {

                            var cls = new Enemies(x*16, y*16 + 5);
                            Game1.Opponents.Add(cls);
                            cls.spawning = true;
                            cls.spawnTimer = 30;
                            cls.currentAction = "Do Nothing";
                            cls.lengthOfAction = 100;
                        }
                        if (GameMap.Index[y, x] == 3) //fire floppy
                        {

                            var cls = new Enemies(x * 16, y * 16 + 5, 2);
                            Game1.Opponents.Add(cls);
                            cls.spawning = true;
                            cls.spawnTimer = 30;
                            cls.currentAction = "Do Nothing";
                            cls.lengthOfAction = 100;
                        }
                    }
                }

            }
            */
        }

        public static void CleanUp()
        {
            Game1.Opponents.Clear();
            Game1.gameBullets.Clear();
            Game1.gameDebris.Clear();
            Game1.gameExplosions.Clear();

            Game1.Hunter.bouncing = false;
            Game1.Hunter.bounce = 0;
            Game1.Hunter.yVelocity = 0;
        }


    }
}
