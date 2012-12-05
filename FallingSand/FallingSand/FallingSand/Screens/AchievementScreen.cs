using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FallingSand.Screens.Menus;
using FallingSand.Achievement;

namespace FallingSand.Screens
{
    class AchievementScreen : Screen
    {

        /// <summary>
        /// This is the time (DateTime, not GameClock) 
        /// that the screen is created.
        /// </summary>
        private long initialTime;

        /// <summary>
        /// This is the menu used for the pause screen.
        /// </summary>
        private AchievementMenu menu;

        /// <summary>
        /// Where to write "Record Robot"
        /// </summary>
        private Vector2 textDrawPosition;

        /// <summary>
        /// Spacing for achievements
        /// </summary>
        private int xSpacing;

        /// <summary>
        /// Spacing for achievements
        /// </summary>
        private int ySpacing;

        /// <summary>
        /// Initializes a new instance of the <see cref="TitleScreen"/> class.
        /// </summary>
        public AchievementScreen(ScreenContainer container)
            : base(container)
        {
            //Game1.screens.IsTitle = true;
            //Game1.screens.IsPaused = false;
            // Note: Do not use GameClock, it will be paused!
            this.initialTime = DateTime.Now.Ticks;
            this.menu = new AchievementMenu(new Vector2(50, 175), 50);
            xSpacing = 200;
            ySpacing = 50;
            this.textDrawPosition = new Vector2(50, 50);

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
            int countX = 0;
            int countY = 0;
            // Write "RecordRobot" at the center of the screen.
            FSGGame.spriteBatch.Begin();
            //FSGGame.spriteBatch.Draw(Textures.TitleRobot, robotLocation, Color.White);
            foreach (AchievementBase a in FSGGame.achievementManager.achievements)
            {
                if (a.Unlocked)
                {
                    FSGGame.spriteBatch.DrawString(FSGGame.Font, a.name, this.textDrawPosition + new Vector2(xSpacing * countX, ySpacing * countY), Color.White);
                }
                else
                    FSGGame.spriteBatch.DrawString(FSGGame.Font, a.name, this.textDrawPosition + new Vector2(xSpacing * countX, ySpacing * countY), Color.Gray);

                countY++;
                if (countY > 4)
                {
                    countY = 0;
                    countX++;
                }
            }
            FSGGame.spriteBatch.End();
            // Draw menu
            this.menu.Draw();
        }
    }
}
