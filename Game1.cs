using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;
using System.Xml.Serialization;
using System.Xml;
using System.IO.IsolatedStorage;
using System.IO;
using GameInfo;

namespace Bullet_Rebound
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        enum GameStates { TitleScreen, Playing, PlayerDead, GameOver };
        GameStates gameState = GameStates.TitleScreen;
        Texture2D titleScreen;
        Texture2D spriteSheet;
        Texture2D spriteSheet1;
        Texture2D ShieldTexture;
        Texture2D explosionTexture;
        Texture2D Enemies;
        ExplosionManager explosionManager;
        CollisionManager collisionManager;
        Space space;
        PlayerManager playerManager;        
        EnemyManager enemyManager;
        CommandManager commandManager;
        PowerupLife life;
        InteractiveMusic interactiveMusic;
        SpriteFont pericles14;        
        private Vector2 offScreen = new Vector2(-500, -500);
        private float playerDeathDelayTime = 6f;
        private float playerDeathTimer = 0f;
        private float titleScreenTimer = 0f;
        private float titleScreenDelayTime = 1f;
        
        private int playerStartingLives = 0;
        private Vector2 scoreLocation = new Vector2(20, 10);
        private Vector2 livesLocation = new Vector2(20, 25);
        
        HighScoreData data;            
        public string HighScoresFilename = "Highscores.dat";
        PlayerInfo[] playerinfo;
                
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            commandManager = new CommandManager();
            interactiveMusic = new InteractiveMusic();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //creates a highscore .dat file using he scores below if there isn't one.
            string fullpath = "highscores.dat";
            if (!File.Exists(fullpath))
            {
                data = new HighScoreData(5);
                data.Score[0] = 2500;
                data.Score[1] = 1500;
                data.Score[2] = 1000;
                data.Score[3] = 500;
                data.Score[4] = 300;

                SaveHighScores(data, HighScoresFilename, null);

                IsFixedTimeStep = false;
            }
            // TODO: Add your initialization logic here

            
            base.Initialize();
            InitializeBindings();
            interactiveMusic.musicsystem.setParameterValue(1, 1);
        }

        private void InitializeBindings()
        {
            //key bindings for movement 
            commandManager.AddKeyboardBinding(Keys.W, playerManager.Up);
            commandManager.AddKeyboardBinding(Keys.A, playerManager.Left);
            commandManager.AddKeyboardBinding(Keys.S, playerManager.Down);
            commandManager.AddKeyboardBinding(Keys.D, playerManager.Right);
            commandManager.AddKeyboardBinding(Keys.Space, playerManager.fire);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //playerinfo = Content.Load<PlayerInfo[]>(@"Content/playerdata");
            
            titleScreen = Content.Load<Texture2D>(@"Textures\TitleScreen");
            spriteSheet = Content.Load<Texture2D>(@"Textures\SpriteSheet");
            spriteSheet1 = Content.Load<Texture2D>(@"Textures\SpriteSheet1");
            Enemies = Content.Load<Texture2D>(@"Textures\Enemies");
            explosionTexture = Content.Load<Texture2D>(@"Textures\explosion");
            ShieldTexture = Content.Load<Texture2D>(@"Textures\ShieldTexture");
            pericles14 = Content.Load<SpriteFont>(@"Fonts\Pericles14");

            space = new Space(
                this.Window.ClientBounds.Width,
                this.Window.ClientBounds.Height, 200,
                new Vector2(0, 30f), spriteSheet, new Rectangle(0, 450, 2, 2));

            playerManager = new PlayerManager(
                spriteSheet1, new Rectangle(
                    0, 0, 48, 48), 2,
                    new Rectangle(0, 0,
                        this.Window.ClientBounds.Width,
                        this.Window.ClientBounds.Height), null);


            enemyManager = new EnemyManager(
                spriteSheet, new Rectangle(0, 200, 50, 50),
                6, playerManager, new Rectangle(0, 0,
                    this.Window.ClientBounds.Width,
                    this.Window.ClientBounds.Height));

            explosionManager = new ExplosionManager(
                spriteSheet, new Rectangle(0, 100, 50, 50),
                3, new Rectangle(0, 450, 2, 2));

            life = new PowerupLife(
                Enemies, new Rectangle (160, 550, 15, 15),
            13, new Rectangle (0, 0,
                this.Window.ClientBounds.Width,
                this.Window.ClientBounds.Height));

            collisionManager = new CollisionManager(                
                playerManager,
                enemyManager,
                explosionManager,
                life);


            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            interactiveMusic.UnloadContent();
            // TODO: Unload any non ContentManager content here
        }

                      

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            Vector2 location = playerManager.playerSprite.Location;
            //mkaes sure the player does not go outside the screen.
            location.X = MathHelper.Clamp(location.X, 0, GraphicsDevice.Viewport.Width - playerManager.playerSprite.Source.Width);
            location.Y = MathHelper.Clamp(location.Y, 0, GraphicsDevice.Viewport.Height - playerManager.playerSprite.Source.Height);

            playerManager.playerSprite.Location = location;

            switch (gameState)
            {
                case GameStates.TitleScreen:
                    titleScreenTimer +=
                        (float)gameTime.ElapsedGameTime.TotalSeconds;
                    interactiveMusic.cue_titleScreen.begin();
                        interactiveMusic.eventsystem.update();
                    //if timer finishes then automatically goes into player state.
                    if (titleScreenTimer >= titleScreenDelayTime)
                    {
                        if ((Keyboard.GetState().IsKeyDown(Keys.Space)) ||
                            (GamePad.GetState(PlayerIndex.One).Buttons.A 
                            == ButtonState.Pressed))
                        {
                            playerManager.Lives = playerStartingLives;
                            playerManager.PlayerScore = 0;
                            resetGame();
                            gameState = GameStates.Playing;
                            interactiveMusic.musicsystem.setParameterValue(3, 5);
                            interactiveMusic.eventsystem.update();
                        }
                    }                                                    

                    break;

                case GameStates.Playing:

                    space.Update(gameTime);
                    commandManager.Update();
                    playerManager.Update(gameTime);
                    enemyManager.Update(gameTime);
                    explosionManager.Update(gameTime);
                    life.Update(gameTime);
                    collisionManager.CheckCollisions();
                    interactiveMusic.cue_main.begin();
                    interactiveMusic.eventsystem.update();

                    if (playerManager.PlayerScore == 1500)
                    {
                        interactiveMusic.musicsystem.setParameterValue(2, 6);
                        interactiveMusic.eventsystem.update();
                    }

                    if (playerManager.PlayerScore == 2500)
                    {
                        interactiveMusic.musicsystem.setParameterValue(2, 10);
                        interactiveMusic.eventsystem.update();                   
                    }

                    if (playerManager.Destroyed)

                    {
                        playerDeathTimer = 0f;
                        enemyManager.Active = false;
                        playerManager.Lives --;
                        if (playerManager.Lives < 0)
                        {
                            gameState = GameStates.GameOver;
                        }
                        else
                        {
                            gameState = GameStates.PlayerDead;
                        }
                    }

                    break;

                case GameStates.PlayerDead:
                        playerDeathTimer +=
                            (float)gameTime.ElapsedGameTime.TotalSeconds;

                    space.Update(gameTime);
                    commandManager.Update();
                    enemyManager.Update(gameTime);
                    playerManager.Update(gameTime);
                    explosionManager.Update(gameTime);
                    life.Update(gameTime);

                    if (playerDeathTimer >= playerDeathDelayTime)
                    {
                        resetGame();
                        gameState = GameStates.Playing;
                    }
                    break;
                    
                case GameStates.GameOver:
                    playerDeathTimer +=
                        (float)gameTime.ElapsedGameTime.TotalSeconds;
                    space.Update(gameTime);
                    commandManager.Update();
                    enemyManager.Update(gameTime);
                    playerManager.PlayerShotManager.Update(gameTime);
                    explosionManager.Update(gameTime);
                    life.Update(gameTime);
                    SaveHighScore();
                    
                    interactiveMusic.cue_main.end();
                            interactiveMusic.musicsystem.setParameterValue(1, 0);
                            interactiveMusic.musicsystem.setParameterValue(2, 0);
                            interactiveMusic.cue_gameOver.begin();
                            interactiveMusic.eventsystem.update();

                    if (playerDeathTimer >= playerDeathDelayTime)
                    {
                        gameState = GameStates.TitleScreen;
                        interactiveMusic.cue_gameOver.end();
                        interactiveMusic.musicsystem.setParameterValue(3, 0);
                        interactiveMusic.musicsystem.setParameterValue(1, 1);
                        interactiveMusic.eventsystem.update();
                    }
                    
                    break;
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }



         

        public struct HighScoreData
        {
            public long[] Score;
            public int Count;            

            public HighScoreData(int count)
            {
                Score = new long[count];

                Count = count;
            }
        }

        public static void SaveHighScores(HighScoreData data, string filename, StorageDevice device)
        {
            string fullpath = "highscores.dat";
            FileStream stream = File.Open(fullpath, FileMode.OpenOrCreate);
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(HighScoreData));
                serializer.Serialize(stream, data);
            }
            finally
            {
                stream.Close();
            }
        }

        public static HighScoreData LoadHighScores(string filename)
        {
            HighScoreData data;
            string fullpath = "highscores.dat";
            FileStream stream = File.Open(fullpath, FileMode.OpenOrCreate,
                FileAccess.Read);
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(HighScoreData));
                data = (HighScoreData)serializer.Deserialize(stream);
            }
            finally
            {
                stream.Close();
            }
            return (data);
        }

        private void SaveHighScore()
        {
            HighScoreData data = LoadHighScores(HighScoresFilename);

            int scoreIndex = -1;
            for (int i = data.Count -1; i > -1; i--)
            {
                if (playerManager.PlayerScore >= data.Score[i])
                {
                    scoreIndex = i;
                }
            }
            if (scoreIndex > -1)
            {
                for (int i = data.Count - 1; i> scoreIndex; i--)
                {
                    data.Score[i] = data.Score[i];                    
                }
                data.Score[scoreIndex] = playerManager.PlayerScore;

                SaveHighScores (data, HighScoresFilename, null);
            }
        }

        public string makeHighScoreString()
        {
            HighScoreData data2 = LoadHighScores(HighScoresFilename);
            string scoreBoardString = "HighScore : \n\n";
            for (int i = 0; i < 5; i++)
            {
                scoreBoardString = scoreBoardString + data2.Score[i] + "\n";
            }
            return scoreBoardString;
        }


        private void resetGame()
        {
            enemyManager.Enemies.Clear();
            enemyManager.Active = true;
            playerManager.PlayerShotManager.Shots.Clear();
            enemyManager.EnemyShotManager.Shots.Clear();
            playerManager.Destroyed = false;
        }
        


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            if (gameState == GameStates.TitleScreen)
            {
                spriteBatch.Draw(titleScreen,
                    new Rectangle(0, 0, this.Window.ClientBounds.Width,
                        this.Window.ClientBounds.Height), Color.White);
            }

            if ((gameState == GameStates.Playing) ||
                (gameState == GameStates.PlayerDead) ||
                (gameState == GameStates.GameOver))
            {
                space.Draw(spriteBatch);
                playerManager.Draw(spriteBatch);
                enemyManager.Draw(spriteBatch);
                explosionManager.Draw(spriteBatch);
                life.Draw(spriteBatch);               

                spriteBatch.DrawString(
                    pericles14,
                    "Score : " + playerManager.PlayerScore.ToString(),
                    scoreLocation, Color.White);

                if (playerManager.Lives >= 0)
                {
                    spriteBatch.DrawString(pericles14,
                        "Lives Remaining: " +
                        playerManager.Lives.ToString(),
                        livesLocation, Color.White);
                }
            }

            if ((gameState == GameStates.GameOver))
            {
                spriteBatch.DrawString(
                    pericles14, "G A M E  O V E R! ",
                    new Vector2(
                        this.Window.ClientBounds.Width / 2 -
                        pericles14.MeasureString
                        ("G A M E  O V E R! ").X / 2, 50),
                        Color.White);

                spriteBatch.DrawString(pericles14, makeHighScoreString(),
                    new Vector2(320, 150), Color.Red);
                SaveHighScore();
            }

            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
