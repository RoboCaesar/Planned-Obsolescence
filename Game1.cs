using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;


namespace DiskWars
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    /*public struct explosions
    {
        public double x;
        public double y;
        public int type;
        public bool active;
        public int lifespanCount;
        public int size;
    }*/
    public class Game1 : Game
    {

        public const int SCREEN_W = 320;
        public const int SCREEN_H = 240;
        public const int TILE_W = 16;
        public const int TILE_H = 16;
        public const int MAPSIZE_W = 20;
        public const int MAPSIZE_H = 15;

        GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        RenderTarget2D nativeRenderTarget;

        public static Texture2D Sprites;   //Playable character sprites here.
        public static Texture2D AttackSprites;  //and here
        public static Texture2D FloppySprites;
        public static Texture2D FastFloppySprites;
        public static Texture2D HoneyCrispSprites; //that mac style computer
        public static Texture2D BossSprites;
        public static Texture2D BossHappyFace;

        public static Texture2D ExplosionSprites;
        public static Texture2D CrackedGround;
        public static Texture2D SeriousOverlay;
        public static Texture2D Debris1;
        public static Texture2D Gems;
        public static Texture2D AttackWave;
        public static Texture2D ElectricAttacks;
        public static Texture2D StatusBar; //That important thing in the lower left corner.
        public static Texture2D TitleImage;
        public static Texture2D WindowSprites;
        public static Texture2D GameOver;
        public static Texture2D SnowSprites;
        public static Texture2D levelFinished;
        public static Texture2D cdAttacks;

        //public GameMap Arena = new GameMap();

        //characters
        public static Player Hunter;
        public static List<Enemies> Opponents;
        public static List<Enemies> TempOpponents; //For when you want an enemy object to create another enemy without messing up the list.
        public static List<Bullets> gameBullets;
        public static List<Snow> gameSnow;

        public static SoundEffect HammerHit;
        public static SoundEffect Charging;
        public static SoundEffect ChargedShot;
        public static SoundEffect Walking;
        public static SoundEffect BigJump;
        public static SoundEffect SuperAttack;
        public static SoundEffect FloppyBounce;
        public static SoundEffect HitEnemy;
        public static SoundEffect KilledEnemy;
        public static SoundEffect GemPickup;
        public static SoundEffect Hurt;
        public static SoundEffect Jump;
        public static SoundEffect Press;
        public static SoundEffect GameOverSound;
        public static SoundEffect levelFinishedChime;
        public static SoundEffect Shot;

        public static SpriteFont gameFont;
        public static SpriteFont gameFont2;
        public static Random rnd = new Random();

        public static List<Debris> gameDebris;
        public static List<Explosions> gameExplosions;

        public static IDictionary<string, int> DirDict = new Dictionary<string, int>() //Need this so program can find correct sprites for directionality from image files.
        {
            {"Down", 0}, {"Up", 1}, {"Left", 2}, {"Right", 3}
        };
        public static bool isScreenShaking = false;

        public static bool WasAnEnemyKilledThisFrame = false;
        public static bool PleaseAddEnemies = false;


        public static string gameMode = "Menu";
        public static int menuOption = 0;
        public static KeyboardState EmptyState = Keyboard.GetState();
        public bool freeToMove = false;
        public static bool loading = false;
        public static int loadingFrame = 0;
        public static int loadingAnimTick = 0;
        public static string loadMode = "out";
        public static bool paused = false;
        public int pausedframe = 0;
        public static bool gameOverMode = false;
        public int gameOverAnimFrame = 0;

        public static bool LevelingUp = false;
        public int LevelingUpAnimFrame = 0;
        public static int CurrentLevel = 1;

        public static int ScoreChangeTracker;
        public static double Scorebounce = 0;
        public static double ScoreYVel = 0;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 960;  
            graphics.PreferredBackBufferHeight = 720; 
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            nativeRenderTarget = new RenderTarget2D(GraphicsDevice, 320, 240);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Tile.TileSetTexture = Content.Load<Texture2D>("images/tiles");
            ExplosionSprites = Content.Load<Texture2D>("images/explosions");
            Hunter = new Player();

            Sprites = Content.Load<Texture2D>("images/hunter");
            FloppySprites = Content.Load<Texture2D>("images/floppyguy");
            FastFloppySprites = Content.Load<Texture2D>("images/fastfloppy");
            HoneyCrispSprites = Content.Load<Texture2D>("images/oldhoneycrisp");
            BossSprites = Content.Load<Texture2D>("images/boss");
            BossHappyFace = Content.Load<Texture2D>("images/boss_happyfaces");

            AttackSprites = Content.Load<Texture2D>("images/hunterattack");
            CrackedGround = Content.Load<Texture2D>("images/nuclearexplosion");
            SeriousOverlay = Content.Load<Texture2D>("images/scrollingoverlay");
            Debris1 = Content.Load<Texture2D>("images/smalldebris");
            Gems = Content.Load<Texture2D>("images/pickups");
            AttackWave = Content.Load<Texture2D>("images/attackwave");
            ElectricAttacks = Content.Load<Texture2D>("images/electricattacks");
            StatusBar = Content.Load<Texture2D>("images/statusbar");
            TitleImage = Content.Load<Texture2D>("images/title");
            WindowSprites = Content.Load<Texture2D>("images/windows");
            GameOver = Content.Load<Texture2D>("images/gameover");
            SnowSprites = Content.Load<Texture2D>("images/snow");
            levelFinished = Content.Load<Texture2D>("images/levelfinished");
            cdAttacks = Content.Load<Texture2D>("images/cdattacks");


            HammerHit = Content.Load<SoundEffect>("sounds/hit");
            Charging = Content.Load<SoundEffect>("sounds/charging");
            Walking = Content.Load<SoundEffect>("sounds/walking");
            ChargedShot = Content.Load<SoundEffect>("sounds/powershot");
            BigJump = Content.Load<SoundEffect>("sounds/bigjump");
            SuperAttack = Content.Load<SoundEffect>("sounds/SuperAttack");
            FloppyBounce = Content.Load<SoundEffect>("sounds/Floppy");
            HitEnemy = Content.Load<SoundEffect>("sounds/broken");
            KilledEnemy = Content.Load<SoundEffect>("sounds/explode3");
            GemPickup = Content.Load<SoundEffect>("sounds/gempickup2");
            Hurt = Content.Load<SoundEffect>("sounds/hurt");
            Jump = Content.Load<SoundEffect>("sounds/jump2");
            Press = Content.Load<SoundEffect>("sounds/press");
            GameOverSound = Content.Load<SoundEffect>("sounds/gameover");
            levelFinishedChime = Content.Load<SoundEffect>("sounds/levelfinished");
            Shot = Content.Load<SoundEffect>("sounds/shot");

            gameFont = Content.Load<SpriteFont>("ThisGame");
            gameFont2 = Content.Load<SpriteFont>("Message");

            /*for (int k = 0; k < Explosions.ExpArray.Length; k++)
            {
                Explosions.ExpArray[k].active = false;
            }*/

            gameDebris = new List<Debris>();
            gameExplosions = new List<Explosions>();

            Opponents = new List<Enemies>();
            TempOpponents = new List<Enemies>();
            gameBullets = new List<Bullets>();
            gameSnow = new List<Snow>();
            //var cls = new Enemies(200,200, 3);
            //Opponents.Add(cls);
            for (int snowCount = 0; snowCount < 100; snowCount++)
            {
                var cls = new Snow();
                gameSnow.Add(cls);
            }







            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {


            Game1.isScreenShaking = false;
            switch (gameMode)
            {

                case "Menu": //Main game menu

                    if (loading == false)
                    {
                        if (Keyboard.GetState() == EmptyState) //This lets us select another menu option if a key isn't being pressed.
                        {
                            freeToMove = true;
                        }
                        if (freeToMove == true)
                        {
                            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                            {
                                if (menuOption == 0)
                                {
                                    CurrentLevel = 1;
                                    GameMap.LoadLevel(1);
                                    loading = true;
                                    loadMode = "out";
                                    loadingFrame = 0;
                                    //gameMode = "In-game";
                                    Game1.GemPickup.Play(1.0f, 0.0f, 0.0f);
                                }
                                if (menuOption == 2)
                                {
                                    Game1.GemPickup.Play(1.0f, 0.0f, 0.0f);
                                    Exit();
                                }
                                freeToMove = false;
                            }
                            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                            {
                                menuOption--;
                                if (menuOption < 0) menuOption = 2;
                                freeToMove = false;
                            }
                            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                            {
                                menuOption++;
                                if (menuOption > 2) menuOption = 0;
                                freeToMove = false;
                            }
                        }
                    }

                    else if (loading == true)
                    {
                        if (loadMode == "out")
                        {
                            loadingFrame += 4;
                            if (loadingFrame > 299)
                            {
                                loadMode = "in";
                                gameMode = "In-game";
                                gameOverMode = false;
                                gameOverAnimFrame = 0;
                                paused = false;
                                Hunter.HP = 30;
                                Hunter.bombsHeld = 0;
                                Hunter.score = 0;
                                GameMap.scoreAtStartOfLevel = Hunter.score;
                                GameMap.hpAtStartOfLevel = Hunter.HP;
                                GameMap.bombsAtStartOfLevel = Hunter.bombsHeld;
                                loading = true; //have to keep it true for transitioning in to the next screen.
                                break;
                            }
                        }
                        if (loadMode == "in")
                        {
                            loadingFrame -= 4;
                            if (loadingFrame < 0)
                            {
                                loadingFrame = 0;
                                //gameMode = "In-game";
                                loading = false; //have to keep it true for transitioning in to the next screen.
                            }
                        }

                    }

                    break;
                case "In-game":
                    WasAnEnemyKilledThisFrame = false;
                    Hunter.Moving = false;
                    PleaseAddEnemies = false;
                    ScoreChangeTracker = Hunter.score;

                    if (paused == false) GameMap.Logic();
                    if (Keyboard.GetState() == EmptyState) //This lets us select another menu option if a key isn't being pressed.
                    {
                        freeToMove = true;
                    }
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        if (freeToMove == true && gameOverMode == false)
                        {
                            switch (paused)
                            {
                                case false:
                                    paused = true;
                                    pausedframe = 0;
                                    break;
                                case true:
                                    paused = false;
                                    break;
                            }
                        }
                        else if (gameOverMode == true)
                        {
                            //if (loading == false) gameMode = "Menu";
                            switch (paused)
                            {
                                case false:
                                    paused = true;
                                    pausedframe = 0;
                                    break;
                                case true:
                                    paused = false;
                                    break;
                            }
                        }
                        freeToMove = false; //In this context, you want to have the player press a key again before you can unpause!
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.Q) && paused == true)
                    {
                        Exit();
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.M) && paused == true)
                    {
                        if (loading == false) gameMode = "Menu";
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.M) && gameOverMode == true)
                    {
                        if (loading == false) gameMode = "Menu";
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.L) && paused == false)
                    {
                        LevelingUp = true; CurrentLevel = 8;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.C) && freeToMove == true && paused == false)
                    {
                        Game1.Hunter.placeBomb();
                        freeToMove = false;
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.R) && paused == true)
                    {
                        GameMap.RestartLevel();
                        /*
                        GameMap.LoadLevel(CurrentLevel);
                        Windows.DisplayingMessage = true;
                        loadingFrame = 300;
                        loading = true;
                        loadMode = "in";
                        paused = false;
                        Hunter.score = GameMap.scoreAtStartOfLevel;
                        Windows.MessageFrame = -1;
                        */
                    }


                    if (Keyboard.GetState().IsKeyDown(Keys.Z) && paused == false && gameOverMode == false && LevelingUp == false)
                    {
                        Hunter.Attack();
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.X) && paused == false && gameOverMode == false && LevelingUp == false)
                    {
                        if (Hunter.SpecialReady == true) Hunter.Attack(3);
                    }
                    if (Hunter.attacking == false && paused == false && gameOverMode == false && LevelingUp == false)
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.Up))
                        {
                            Hunter.Move("Up"); Hunter.Moving = true;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.Down))
                        {
                            Hunter.Move("Down"); Hunter.Moving = true;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.Left))
                        {
                            Hunter.Move("Left"); Hunter.Moving = true;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.Right))
                        {
                            Hunter.Move("Right"); Hunter.Moving = true;
                        }
                    }

                    if (paused == false)
                    {
                        if (gameOverMode == false && LevelingUp == false) Hunter.Logic();


                        foreach (var cls in Opponents)
                        {
                            cls.Moving = false;
                            cls.eLogic();
                        }

                        if (PleaseAddEnemies == true)
                        {
                            Opponents = Opponents.Concat(TempOpponents).ToList();
                            Game1.TempOpponents.Clear();
                            PleaseAddEnemies = false;
                        }

                        Opponents.RemoveAll(cls => cls.active == false);

                        foreach (var cls in gameBullets)
                        {
                            cls.Logic();
                        }
                        gameBullets.RemoveAll(cls => cls.active == false);

                        foreach (var cls in gameDebris)
                        {
                            cls.Logic();
                        }
                        gameDebris.RemoveAll(cls => cls.active == false);

                        foreach (var cls in gameExplosions)
                        {
                            cls.Logic();
                        }
                        gameDebris.RemoveAll(cls => cls.active == false);

                        if (GameMap.biome == "snow")
                        {
                            foreach (var cls in gameSnow)
                            {
                                cls.Logic();
                            }
                        }

                        if (WasAnEnemyKilledThisFrame == true) KilledEnemy.Play(0.4f, 0.0f, 0.0f);
                    }


                    if (ScoreChangeTracker != Hunter.score)
                    {
                        ScoreYVel = -3;
                    }

                    if (loading == true)
                    {
                        if (loadMode == "out")
                        {
                            Hunter.hurtInvincible = true; //keep the player invincible during this!
                            Hunter.hurtInvincibleTimer = 2;
                            loadingFrame += 4;
                            if (loadingFrame > 299)
                            {
                                if (gameOverMode == true) GameMap.RestartLevel();//gameMode = "Menu";
                                if (LevelingUp == true)
                                {
                                    GameMap.scoreAtStartOfLevel = Hunter.score;
                                    GameMap.hpAtStartOfLevel = Hunter.HP;
                                    GameMap.bombsAtStartOfLevel = Hunter.bombsHeld;
                                    GameMap.LoadLevel(CurrentLevel + 1);
                                    CurrentLevel++;
                                }

                                gameOverMode = false;
                                gameOverAnimFrame = 0;
                                LevelingUp = false;
                                paused = false;
                                loading = true; //have to keep it true for transitioning in to the next screen.
                                loadMode = "in";
                                LevelingUpAnimFrame = 0;
                                break;
                            }
                        }
                        if (loadMode == "in")
                        {
                            loadingFrame -= 4;
                            if (loadingFrame < 0)
                            {
                                loadingFrame = 0;
                                //gameMode = "In-game";
                                loading = false; //have to keep it true for transitioning in to the next screen.
                            }
                        }
                    }
                    if (paused == true) pausedframe++;
                    if (pausedframe > 20) pausedframe = 20;

                    if (gameOverMode == true)
                    {
                        gameOverAnimFrame++;
                        if (gameOverAnimFrame == 525)
                        {
                            loading = true;
                            loadMode = "out";
                            loadingFrame = 0;
                        }
                    }

                    if (LevelingUp == true)
                    {
                        LevelingUpAnimFrame++;
                        if (LevelingUpAnimFrame == 225)
                        {

                            loading = true;
                            loadMode = "out";
                            loadingFrame = 0;
                        }
                        Hunter.hurtInvincible = true; //keep the player invincible during this!
                        Hunter.hurtInvincibleTimer = 2;
                    }



                    break;
                default:
                    Exit();
                    break;
            }

            base.Update(gameTime);
        }



        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkViolet);

            // TODO: Add your drawing code here
            GraphicsDevice.SetRenderTarget(nativeRenderTarget);


            spriteBatch.Begin();//SpriteSortMode.Immediate, BlendState.AlphaBlend);

            switch(gameMode)
            {
                case "Menu":
                    GraphicsDevice.Clear(new Color(20,13,41));
                    spriteBatch.Draw(TitleImage, new Rectangle(10, 0, 300, 146), new Rectangle(0,0,800,390), Color.White);
                    Windows.Draw(110, 140, 100, 70);
                    spriteBatch.DrawString(gameFont, "New Game\n\nOptions\n\nExit", new Vector2(131, 147), Color.Black * 0.5f);
                    spriteBatch.DrawString(gameFont, "New Game\n\nOptions\n\nExit", new Vector2(130, 146), Color.White);
                    spriteBatch.Draw(WindowSprites, new Rectangle(120, 148 + 22*menuOption, 6, 8), new Rectangle(33, 0, 6, 8), Color.White);

                    spriteBatch.DrawString(gameFont, "ThomasSoft 2017", new Vector2(3, 229), Color.Black * 0.5f);
                    spriteBatch.DrawString(gameFont, "ThomasSoft 2017", new Vector2(2, 228), Color.Gray);

                    if (loading == true)
                    {

                        for (int fr = 0; fr < loadingFrame + 1; fr++)
                        {
                            spriteBatch.Draw(Tile.TileSetTexture, new Rectangle(16*(fr % 20), 16*(int)(fr / 20), 16, 16), new Rectangle(16, 112, 16, 16), Color.White);
                        }
                    }
                    break;
                case "In-game":
                    GameMap.Draw();
                    //Explosions.Draw(1);
                    //spriteBatch.Draw(Hunter.Sprites, new Rectangle(Hunter.x - 7, Hunter.y - 14, 30, 30), Hunter.GetSourceRectangle(Hunter.Frame), Color.White);

                    foreach (var cls in gameExplosions)
                    {
                        cls.Draw(1);
                    }

                    foreach (var cls in gameDebris)
                    {
                        if (cls.bounce < 2 || cls.type != 2) cls.Draw();
                    }
                    if (gameOverMode == false && LevelingUp == false)
                    {
                        if (Hunter.bounce < 1 && Hunter.attackStyle != 3) Hunter.DrawPlayer();
                    }




                    foreach (var cls in Opponents)
                    {
                        cls.Draw();
                    }
                    GameMap.Draw("foreground");

                    //Now here we draw things we want ABOVE the high tiles
                    if (gameOverMode == false && LevelingUp == false)
                    {
                        if (Hunter.bounce >= 1 || Hunter.attackStyle == 3) Hunter.DrawPlayer();
                    }

                    foreach (var cls in gameDebris)
                    {
                        if (cls.bounce >= 2 || cls.type == 2) cls.Draw();
                    }
                    if (GameMap.biome == "snow") Game1.spriteBatch.Draw(Game1.WindowSprites, new Rectangle(0, 0, SCREEN_W, SCREEN_W), new Rectangle(1, 4, 14, 9), Color.White * 0.3f);
                    foreach (var cls in gameExplosions)
                    {
                        cls.Draw();
                    }
                    foreach (var cls in gameBullets)
                    {
                        cls.Draw();
                    }
                    //Windows.Draw(3, 3, 160, 24);
                    if (GameMap.biome == "snow")
                    {
                        //Game1.spriteBatch.Draw(Game1.WindowSprites, new Rectangle(0, 0, SCREEN_W, SCREEN_W), new Rectangle(1, 4, 14, 9), Color.White * 0.3f);
                        foreach (var cls in gameSnow)
                        {
                            cls.Draw();
                        }
                    }

                    Hunter.DrawStatus();
                    if (Windows.DisplayingMessage == true && loading == false) Windows.DisplayMessage(GameMap.LevelMessage); //The level info message


                    if (paused == true)
                    {
                        Vector2 stringDim = Game1.gameFont.MeasureString(GameMap.LevelMessage);
                        spriteBatch.Draw(Tile.TileSetTexture, new Rectangle(0,0,320,240), new Rectangle(2, 147, 4, 4), Color.White * 0.5f);
                        Windows.Draw(6, 26- pausedframe, 174, 4 + pausedframe * 2);
                        Windows.Draw(6, 62 - (pausedframe/2), (int)stringDim.X + 12, 4 + pausedframe);
                        if (pausedframe > 16)
                        {
                            spriteBatch.DrawString(gameFont, "Game is currently paused.\nPress 'M' to return to the menu.\nPress 'R' to restart the level.", new Vector2(13, 11), Color.Black * 0.5f);
                            spriteBatch.DrawString(gameFont, "Game is currently paused.\nPress 'M' to return to the menu.\nPress 'R' to restart the level.", new Vector2(12, 10), Color.White);
                            spriteBatch.DrawString(gameFont, GameMap.LevelMessage, new Vector2(13, 59), Color.Black * 0.5f);
                            spriteBatch.DrawString(gameFont, GameMap.LevelMessage, new Vector2(12, 58), Color.YellowGreen);
                        }
                    }

                    if (gameOverMode == true) //GAME OVER!!!
                    {

                        if (gameOverAnimFrame < 65)
                        {
                            spriteBatch.Draw(Tile.TileSetTexture, new Rectangle(0, 0, 320, 240), new Rectangle(2, 147, 4, 4), Color.DarkRed * (Convert.ToSingle(gameOverAnimFrame)/130f));
                            spriteBatch.Draw(GameOver, new Rectangle(5, 90 + 65 - gameOverAnimFrame, 315, gameOverAnimFrame), new Rectangle(0, 0, 630, 130), Color.White);
                        }
                        else
                        {
                            spriteBatch.Draw(Tile.TileSetTexture, new Rectangle(0, 0, 320, 240), new Rectangle(2, 147, 4, 4), Color.DarkRed * 0.5f);
                            spriteBatch.Draw(GameOver, new Rectangle(5, 90, 315, 65), new Rectangle(0, 0, 630, 130), Color.White);
                        }
                    }

                    if (LevelingUp == true) //A new level!
                    {

                        if (LevelingUpAnimFrame < 65)
                        {
                            spriteBatch.Draw(Tile.TileSetTexture, new Rectangle(0, 0, 320, 240), new Rectangle(2, 147, 4, 4), Color.LightSteelBlue * (Convert.ToSingle(LevelingUpAnimFrame) / 130f));
                            //    spriteBatch.Draw(GameOver, new Rectangle(5, 90 + 65 - gameOverAnimFrame, 315, gameOverAnimFrame), new Rectangle(0, 0, 630, 130), Color.White);
                            spriteBatch.Draw(Sprites, new Rectangle(Hunter.x - 7, Hunter.y - 14, 30, 30), new Rectangle(60, 0, 30, 30), Color.White);
                        }
                        else
                        {
                            spriteBatch.Draw(Tile.TileSetTexture, new Rectangle(0, 0, 320, 240), new Rectangle(2, 147, 4, 4), Color.LightSteelBlue * 0.5f);
                            //spriteBatch.Draw(GameOver, new Rectangle(5, 90, 315, 65), new Rectangle(0, 0, 630, 130), Color.White);
                            if (LevelingUpAnimFrame % 2 == 0)
                            {
                                spriteBatch.Draw(Tile.TileSetTexture, new Rectangle(Hunter.x, Hunter.y + 65 - LevelingUpAnimFrame, 16, 16), new Rectangle(16, 96, 16, 16), Color.White * 0.6f);
                                spriteBatch.Draw(Tile.TileSetTexture, new Rectangle(Hunter.x, 0, 16, Hunter.y + 65 - LevelingUpAnimFrame), new Rectangle(16, 97, 16, 9), Color.White * 0.6f);
                            }
                            spriteBatch.Draw(Sprites, new Rectangle(Hunter.x - 7, Hunter.y - 14 + 65 - LevelingUpAnimFrame, 30, 30), new Rectangle(60,0,30,30), Color.White);

                        }

                    }


                    if (loading == true)
                    {
                        for (int fr = 0; fr < loadingFrame + 1; fr++)
                        {
                            spriteBatch.Draw(Tile.TileSetTexture, new Rectangle(16 * (fr % 20) + 8, 16 * (int)(fr / 20) + 8, 16, 16), new Rectangle(16, 112, 16, 16), Color.Black * 0.4f);
                        }
                        for (int fr = 0; fr < loadingFrame + 1; fr++)
                        {
                            spriteBatch.Draw(Tile.TileSetTexture, new Rectangle(16 * (fr % 20), 16 * (int)(fr / 20), 16, 16), new Rectangle(16, 112, 16, 16), Color.White);
                        }
                    }


                    break;
            }


            spriteBatch.End();



            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            if (isScreenShaking == false)
            {
                spriteBatch.Draw(nativeRenderTarget, new Rectangle(0, 0, 960, 720), Color.White);
            }
            else
            {
                spriteBatch.Draw(nativeRenderTarget, new Rectangle(2 * rnd.Next(-2, 3), 2 * rnd.Next(-1, 2), 960, 720), Color.White);
            }



            spriteBatch.End();
            base.Draw(gameTime);
        }


    }
}
