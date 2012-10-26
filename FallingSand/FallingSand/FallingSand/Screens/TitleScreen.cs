using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FallingSand.Screens.Menus;

namespace FallingSand.Screens
{
    class TitleScreen : Screen
    {

        /// <summary>
        /// This is the time (DateTime, not GameClock) 
        /// that the screen is created.
        /// </summary>
        private long initialTime;

        /// <summary>
        /// This is the menu used for the pause screen.
        /// </summary>
        private TitleMenu menu;

        /// <summary>
        /// Where to write "Record Robot"
        /// </summary>
        private Vector2 textDrawPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="TitleScreen"/> class.
        /// </summary>
        public TitleScreen(ScreenContainer container)
            : base(container)
        {
            //Game1.screens.IsTitle = true;
            //Game1.screens.IsPaused = false;
            // Note: Do not use GameClock, it will be paused!
            this.initialTime = DateTime.Now.Ticks;
            this.menu = new TitleMenu(new Vector2(50, 175), 50);

            this.textDrawPosition = new Vector2(50, 100);

        }

        /// <summary>
        /// Updates this instance. This makes sure that GameClock is paused,
        /// and it also updates the menu.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //this.initialTime = Game1.screens.screenChanged;
            // Only pause the gameclock if the screen is not fading out.
            if (!this.FadingOut)
            {
                //GameClock.Pause();
            }
            else
            {
                //GameClock.Unpause();
            }

            this.menu.Update(gameTime);
        }

        private Vector2 robotLocation = new Vector2(200, 65);

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public override void Draw()
        {
            // Write "RecordRobot" at the center of the screen.
            FSGGame.spriteBatch.Begin();
            //FSGGame.spriteBatch.Draw(Textures.TitleRobot, robotLocation, Color.White);
            FSGGame.spriteBatch.DrawString(FSGGame.Font, "S A N D", this.textDrawPosition, Color.White);
            FSGGame.spriteBatch.End();
            // Draw menu
            this.menu.Draw();
        }
    }
}
