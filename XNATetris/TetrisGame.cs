
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XNATetris
{
    class TetrisGame : Microsoft.Xna.Framework.Game
    {
        //dirty quick access to common objects (mostly for drawing
        public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        public static SpriteFont FontSmall;
        public static SpriteFont FontBig;
        public static XNAShape ShapeManager;
        public static Texture2D Pixel;
        public static TetrisMenu menu;
        public static XNAHighScore highScore;
        public static XNASound soundPlayer;

        //real private stuff
        private TetrisGameplay gV;
        private Platform.Input input;

        //singleton construction. it is logical that only one game is lauched at runtime
        private static TetrisGame singleton;
        public static TetrisGame getSingleton()
        {
            if (singleton == null)
                singleton = new TetrisGame();
            return singleton;
        }
        private TetrisGame()
        {
            soundPlayer = new XNASound();
            highScore = new XNAHighScore();
            input = new Platform.Input();
            menu = new TetrisMenu();
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            FontSmall = Content.Load<SpriteFont>("SpriteFontSmall");
            FontBig = Content.Load<SpriteFont>("SpriteFontBig");
            Pixel = Content.Load<Texture2D>("graypixel");
            ShapeManager = new XNAShape(TetrisGame.graphics.GraphicsDevice);
            soundPlayer.LoadContent(Content);
            gV = new TetrisGameplay();
            gV.OnSizeChanged(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            gV = null;
            ShapeManager = null;
            Pixel.Dispose();
            FontBig = null;
            FontSmall = null;
            spriteBatch.Dispose();
            base.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            Platform.Input.ACTION action = input.QueryInput();
            if (TetrisGame.menu.IsMenuOff())
            {
                if (action == Platform.Input.ACTION.EXIT)
                    TetrisGame.menu.setMenuState(TetrisMenu.MENU_STATE.STATE_MENURESUME);
                else
                {
                    gV.handleInput(action);
                    gV.update();
                }
            }
            else //menu logic
            {
                menu.update_menu(action);
            }

            base.Update(gameTime);
        }



        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            TetrisGame.spriteBatch.Begin();

            gV.draw();
            menu.paint();

            TetrisGame.spriteBatch.End();

            base.Draw(gameTime);
        }

        public static void SetPlayerName(String newName)
        {
            getSingleton().gV.SetPlayerName(newName);
        }

        public static void RestartGame()
        {
            getSingleton().gV.restartGame();
        }

        public static void ExitGame()
        {
            getSingleton().Exit();
        }
    }
}
