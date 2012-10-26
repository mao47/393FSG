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
using FallingSand.Screens;
using FallingSand.Inputs;
using FallingSand.Particles;

namespace FallingSand
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class FSGGame : Microsoft.Xna.Framework.Game
    {
        public static List<Color> testColors = new List<Color> { Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Indigo, Color.Violet };
        public static int testColorIndex = 0;
        public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        public static ScreenContainer screens;
        public static ParticleManager partMan;
        public static SpriteFont Font;
        private static Texture2D pointer;
        public static Texture2D white;
        public static Controller controller;
        public FSGGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            screens = new ScreenContainer();
            partMan = new ParticleManager(graphics.GraphicsDevice, 10000, 1f);
            controller = new Controller(PlayerIndex.One);
            this.Components.Add(new GamerServicesComponent(this));
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
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            Font = Content.Load<SpriteFont>("Font1");
            pointer = Content.Load<Texture2D>("TestPointer");
            white = Content.Load<Texture2D>("1x1white");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            controller.Update();
            if (screens.Count == 0)
                screens.Play(new TitleScreen(screens));
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || ExitStatus)
                this.Exit();

            // TODO: Add your update logic here
            screens.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(testColors[testColorIndex % testColors.Count]);

            // TODO: Add your drawing code here
            screens.Draw();
            spriteBatch.Begin();
            spriteBatch.Draw(pointer, controller.CursorPosition(), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f); 
            //spriteBatch.Draw(pointer, controller.CursorPosition(), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public static bool ExitStatus { get; set; }
    }
}
