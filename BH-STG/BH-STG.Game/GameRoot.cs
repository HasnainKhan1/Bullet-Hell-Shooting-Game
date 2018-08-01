using System.Collections.Generic;
using BHSTG.Domain.Model.BHSTGAggregate;
using BHSTG.Domain.Model.ValueObjects.BulletPatterns;
using BHSTG.GameLogicLayer.Factory;
using BHSTG.GameLogicLayer.Factory.ConcreteFactories;
using BHSTG.GameLogicLayer.GameManagers;
using BHSTG.SharedKernel.Enums.BHSTG.SharedKernel.Enums;
using BHSTG.SharedKernel.GameImages;
using BHSTG.Domain.Model.ValueObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using BHSTG.GameLogicLayer.DomainLogic.Repositories;

namespace BH_STG.Game
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameRoot : Microsoft.Xna.Framework.Game
    {
        MouseState mouseState, previousMouseState;
        KeyboardState keyboardState;
        Color color;
        List<BulletPatterns> playerBullets = new List<BulletPatterns>();
        public static TimeSpan timeBetweenShots = TimeSpan.FromMilliseconds(500);
        public static TimeSpan timeOfShot;

        GameStates gameState;

        Texture2D playGameText, menuBackground, gameOverBackground, selectLevel;
        //Buttons
        Button StartGameButton;
        Button LevelSelectButton;
        Button InstructionsButton;
        Button InGamePlayButton;
        Button InGameMainMenuButton;
        Button InGameRestartButton;
        Button level1;
        Button level2;
        Button level3;


        float screenwidth, screenheight;
    
        public PatternBuilder bulletPattern;

        private int _indexer;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        EntityManager gameObjectManager = new EntityManager();

        public static GameRoot Instance { get; private set; }
        public static Viewport Viewport { get { return Instance.GraphicsDevice.Viewport; } }
        public static Vector2 ScreenSize { get { return new Vector2(Viewport.Width, Viewport.Height); } }
        public static GameTime GameTime { get; private set; }

        public Vector2 Center { get; set; }

        public GameFactory GameFactory { get; set; }

        TimeSpan enemySpawnTime;
        TimeSpan previousSpawnTime;
        MainPlayer tempPlayer;

        SpriteFont font;
        float timer;
        public GameRoot()
        {
            Instance = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            GameFactory = new BHSTGFactory(this);
            this.IsMouseVisible = true;
            gameState = new GameStates();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            color = Color.White;
            screenheight = graphics.GraphicsDevice.Viewport.Height;
            screenwidth = graphics.GraphicsDevice.Viewport.Width;
            
            Center = new Vector2()
            {
                X = (float)Viewport.Width / 2,
                Y = (float)Viewport.Height / 2,
            };

            EntityManager.InitializeGame(this, "LevelOne");
            EntityManager.Add(MainPlayer.Instance);
         
            base.Initialize();

            _indexer = 1;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            // Load the player resources
            // for menu
            font = Content.Load<SpriteFont>("TimerFont");
            playGameText = Content.Load<Texture2D>("GameArt/playButton");
            menuBackground = Content.Load<Texture2D>("GameArt/menuBackground");
            gameOverBackground = Content.Load<Texture2D>("GameArt/gameover_background");
            selectLevel = Content.Load<Texture2D>("GameArt/select-level");


            //playGameButton = new Button(new Rectangle(300, 100, playGameText.Width, playGameText.Height), true);
            //playGameButton.load(Content, "GameArt/playButton");

            // MAIN MENU GameButtons
            StartGameButton = new Button(new Rectangle(300, 100, GameArt.StartGameButton.Width, GameArt.StartGameButton.Height), true);
            StartGameButton.load(Content, "GameArt/start-game-button");

            LevelSelectButton = new Button(new Rectangle(300, 200, GameArt.LevelSelectButton.Width, GameArt.LevelSelectButton.Height), true);
            LevelSelectButton.load(Content, "GameArt/level-select-button");

            InstructionsButton = new Button(new Rectangle(300, 300, GameArt.InstructionsButton.Width, GameArt.InstructionsButton.Height), true);
            InstructionsButton.load(Content, "GameArt/instructions-button");

            // In Game Buttons
            InGamePlayButton = new Button(new Rectangle(400, 200, GameArt.InGamePlayButton.Width, GameArt.InGamePlayButton.Height), true);
            InGamePlayButton.load(Content, "GameArt/play-game");

            InGameMainMenuButton = new Button(new Rectangle(200, 200, GameArt.InGameMainMenuButton.Width, GameArt.InGameMainMenuButton.Height), true);
            InGameMainMenuButton.load(Content, "GameArt/main-menu");

            InGameRestartButton = new Button(new Rectangle(300, 300, GameArt.InGameRestartButton.Width, GameArt.InGameRestartButton.Height), true);
            InGameRestartButton.load(Content, "GameArt/restart-game");

            level1 = new Button(new Rectangle(300, 200, GameArt.Level1.Width, GameArt.Level1.Height), true);
            level1.load(Content, "GameArt/restart-game");
            level2 = new Button(new Rectangle(300, 300, GameArt.Level2.Width, GameArt.Level2.Height), true);
            level2.load(Content, "GameArt/restart-game");
            level3 = new Button(new Rectangle(300, 400, GameArt.Level3.Width, GameArt.Level3.Height), true);
            level3.load(Content, "GameArt/level3");

            GameArt.Load(this);
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
            GameTime = gameTime;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            foreach(var entity in EntityManager.Entities)
            {
                if(entity is MainPlayer)
                {
                    tempPlayer = (MainPlayer)entity;
                }
            }
            // for menu
            mouseState = Mouse.GetState();
            keyboardState = Keyboard.GetState();

            switch (gameState.state)
            {
                case "MAINMENU":
                    timer = 0;
                    if (StartGameButton.update(new Vector2(mouseState.X, mouseState.Y)) == true && mouseState != previousMouseState && mouseState.LeftButton == ButtonState.Pressed)
                    {
                        gameState.changeToState("PLAYGAME");
                    }
                    else if(LevelSelectButton.update(new Vector2(mouseState.X, mouseState.Y)) == true && mouseState.LeftButton == ButtonState.Pressed)
                    {
                        gameState.changeToState("LEVEL");
                    }
                    break;

                case "PAUSE":
                    if (InGameMainMenuButton.update(new Vector2(mouseState.X, mouseState.Y)) == true && mouseState != previousMouseState && mouseState.LeftButton == ButtonState.Pressed)
                    {
                        gameState.changeToState("MAINMENU");
                        //EntityManager.Entities.Clear();
                        timer = 0;
                    }
                    if (InGamePlayButton.update(new Vector2(mouseState.X, mouseState.Y)) == true && mouseState != previousMouseState && mouseState.LeftButton == ButtonState.Pressed)
                    {
                        gameState.changeToState("PLAYGAME");
                    }
                    break;
                case "PLAYGAME":
                    //var tempPlayer = (MainPlayer)EntityManager.Entities[0];
                    if(tempPlayer.Lives <=0)
                    {
                        gameState.changeToState("GAMEOVER");
                        break;
                    }
                    if (keyboardState.IsKeyDown(Keys.P))
                    {
                        gameState.changeToState("PAUSE");
                        break;
                    }
                    else
                    {
                        if (keyboardState.IsKeyDown(Keys.Space) && gameTime.TotalGameTime - timeOfShot > timeBetweenShots)
                        {
                            timeOfShot = gameTime.TotalGameTime;
                            EntityManager.Add(PlayerBullet.Instance);
                        }

                        // we need to add certain enemies to entity manager here
                        
                        if (timer <= EntityManager.waveEndTimes[0])
                        {
                            if (timer >= EntityManager.enemyIntervals[0] - 0.01 && timer <= EntityManager.enemyIntervals[0] + 0.01)
                            {
                                // we are in interval of first wave
                                EntityManager.AddStandardEnemyQuater(150f, 20, MovePattern.QuarterTurnLeft);

                                for(; _indexer <= 10 && _indexer < EntityManager.EntitiesFromJson.Count; _indexer++)
                                    EntityManager.Entities.Add(EntityManager.EntitiesFromJson[_indexer]);

                            }
                        }
                        if (timer >= EntityManager.waveStartTimes[1] && timer <= EntityManager.waveEndTimes[1])
                        {
                            if (timer >= EntityManager.enemyIntervals[1] - 0.01 && timer <= EntityManager.enemyIntervals[1] + 0.01)
                            {
                                // we are in interval of second wave
                                EntityManager.AddStandardEnemyQuater(150f, 10, MovePattern.QuarterTurnRight);
                                EntityManager.AddButterflyEnemy("Random", 30);

                                //EntityManager.Entities.Add(EntityManager.EntitiesFromJson[_indexer]);
                                //_indexer++;

                                for (; _indexer <= 20 && _indexer < EntityManager.EntitiesFromJson.Count; _indexer++)
                                    EntityManager.Entities.Add(EntityManager.EntitiesFromJson[_indexer]);
                            }
                        }
                        if (timer >= EntityManager.waveStartTimes[2] && timer <= EntityManager.waveEndTimes[2])
                        {
                            if (timer >= EntityManager.enemyIntervals[2] - 0.01 && timer <= EntityManager.enemyIntervals[2] + 0.01)
                            {
                                // we are in interval of third wave
                                EntityManager.Add(GameFactory.CreateGameBoss());

                                //EntityManager.Entities.Add(EntityManager.EntitiesFromJson[_indexer]);
                                //_indexer++;

                                for (; _indexer < EntityManager.EntitiesFromJson.Count; _indexer++)
                                    EntityManager.Entities.Add(EntityManager.EntitiesFromJson[_indexer]);
                            }
                        }



                        //Stop game at 60 seconds
                        if (gameTime.TotalGameTime.TotalSeconds > 120)
                        {

                            EntityManager.ClearScreen();
                            gameState.changeToState("ENDGAME");
                        }

                        EntityManager.Update(gameTime, timer);
                        foreach (var bullet in playerBullets)
                        {
                            bullet.Update(gameTime, EntityManager.Entities);
                        }


                        base.Update(gameTime);
                    }
                    break;

                case "LEVEL":
                    if (level1.update(new Vector2(mouseState.X, mouseState.Y)) == true && mouseState != previousMouseState && mouseState.LeftButton == ButtonState.Pressed)
                    {
                        EntityManager.WavesParser = new GameWavesRepository(this, "LevelOne");
                        gameState.changeToState("PLAYGAME");
                    }
                    else if (level2.update(new Vector2(mouseState.X, mouseState.Y)) == true && mouseState != previousMouseState && mouseState.LeftButton == ButtonState.Pressed)
                    {
                        EntityManager.WavesParser = new GameWavesRepository(this, "LevelTwo");
                        EntityManager.InitializeGame(this, "LevelTwo");
                        gameState.changeToState("PLAYGAME");
                    }
                    else if (level3.update(new Vector2(mouseState.X, mouseState.Y)) == true && mouseState != previousMouseState && mouseState.LeftButton == ButtonState.Pressed)
                    {
                        EntityManager.WavesParser = new GameWavesRepository(this, "LevelThree");
                        EntityManager.InitializeGame(this, "LevelThree");
                        gameState.changeToState("PLAYGAME");

                    }
                    break;
                case "ENDGAME":
                    // display game over message then return to main menu
                    break;
            }
            if (gameState.state != "PAUSE" && gameState.state != "ENDGAME" && gameState.state != "GAMEOVER" && gameState.state != "LEVEL")
            {
                timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            previousMouseState = mouseState;
        }



        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);
            
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);

            foreach (var entity in EntityManager.Entities)
            {
                if (entity is MainPlayer)
                {
                    tempPlayer = (MainPlayer)entity;
                }
            }

            // implementing game states
            switch (gameState.state)
            {
                case "MAINMENU":
                    // display menu and buttons
                    spriteBatch.Draw(menuBackground, new Rectangle(0, 0, menuBackground.Width, menuBackground.Height), Color.White);
                    //spriteBatch.Draw(playGameText, new Rectangle(300, 100, playGameText.Width, playGameText.Height), Color.White);

                    // NEW MENU BUTTONS
                    spriteBatch.Draw(GameArt.StartGameButton, new Rectangle(300, 100, GameArt.StartGameButton.Width, GameArt.StartGameButton.Height), Color.White);
                    spriteBatch.Draw(GameArt.LevelSelectButton, new Rectangle(300, 200, GameArt.LevelSelectButton.Width, GameArt.LevelSelectButton.Height), Color.White);
                    spriteBatch.Draw(GameArt.InstructionsButton, new Rectangle(300, 300, GameArt.InstructionsButton.Width, GameArt.InstructionsButton.Height), Color.White);
                    break;
                    
                case "PLAYGAME":
                    EntityManager.Draw(spriteBatch);
                    spriteBatch.Draw(BHSTG.SharedKernel.GameImages.GameArt.GameBackground, Vector2.Zero, Color.White);                    
                    break;
                case "GAMEOVER":
                    // Display game over message, then return user to menu
                    spriteBatch.Draw(gameOverBackground, new Rectangle(0, 0, menuBackground.Width, menuBackground.Height), Color.White);
                    //var Player = (MainPlayer)EntityManager.Entities[0];
                    spriteBatch.DrawString(font, "Final Score: " + tempPlayer.Score, new Vector2(300, 200), Color.Crimson);
                    break;
                case "PAUSE":
                    spriteBatch.Draw(menuBackground, new Rectangle(0, 0, menuBackground.Width, menuBackground.Height), Color.White);
                    spriteBatch.Draw(GameArt.InGameMainMenuButton, new Rectangle(200, 200, GameArt.InGameMainMenuButton.Width, GameArt.InGameMainMenuButton.Height), Color.White);
                    spriteBatch.Draw(GameArt.InGamePlayButton, new Rectangle(400, 200, GameArt.InGamePlayButton.Width, GameArt.InGamePlayButton.Height), Color.White);
                    //spriteBatch.Draw(GameArt.InGameRestartButton, new Rectangle(500, 200, GameArt.InGameRestartButton.Width, GameArt.InGameRestartButton.Height), Color.White);
                    break;
                case "LEVEL":
                    spriteBatch.Draw(menuBackground, new Rectangle(0, 0, menuBackground.Width, menuBackground.Height), Color.White);
                    spriteBatch.Draw(selectLevel, new Rectangle(300, 100, selectLevel.Width, selectLevel.Height), Color.White);
                    spriteBatch.Draw(GameArt.Level1, new Rectangle(300, 200, GameArt.Level1.Width, GameArt.Level1.Height), Color.White);
                    spriteBatch.Draw(GameArt.Level2, new Rectangle(300, 300, GameArt.Level2.Width, GameArt.Level2.Height), Color.White);
                    spriteBatch.Draw(GameArt.Level3, new Rectangle(300, 400, GameArt.Level3.Width, GameArt.Level3.Height), Color.White);
                    break;
                case "ENDGAME":
                    spriteBatch.Draw(BHSTG.SharedKernel.GameImages.GameArt.GameBackground, Vector2.Zero, Color.White);
                    spriteBatch.DrawString(font, "Final Score: " + tempPlayer.Score, new Vector2(300, 200), Color.Crimson);
                    break;
            }            

            spriteBatch.End();
            base.Draw(gameTime);

            string str = "";
            if (tempPlayer.Invulnerable)
            {
                str = "Activated";
            }
            else
            {
                str = "Deactivated";
            }
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Time: " + timer.ToString(), new Vector2(25, 75), Color.Crimson);
            //spriteBatch.DrawString(font, "Lives: " + tempPlayer.Lives, new Vector2(25, 50), Color.Crimson);
            spriteBatch.DrawString(font, "Score: " + tempPlayer.Score, new Vector2(25, 25), Color.Crimson);
            spriteBatch.DrawString(font, "CM: " + str, new Vector2(25, 50), Color.Crimson);
            spriteBatch.DrawString(font, "Bombs: " + tempPlayer.BombsRemaining, new Vector2(25, 100), Color.Crimson);
            int coordinates = 3;
            for (int i = 0; i < tempPlayer.Lives; i++)
                spriteBatch.Draw(GameArt.StarLives, new Rectangle(coordinates += 20, 0, 25, 25), Color.Crimson);// display game backround
            spriteBatch.End();

        }
    }
}
