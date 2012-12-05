using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FallingSand.Screens.Menus;
using FallingSand.Particles;

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


        private MAOParticleManager pm;


        /// <summary>
        /// Initializes a new instance of the <see cref="TitleScreen"/> class.
        /// </summary>
        public TitleScreen(ScreenContainer container)
            : base(container)
        {
            pm = new MAOParticleManager(new Rectangle(0, 0, 900, 500), Constants.MaxParticles, Constants.ParticleSize, p => { return; });
            pm.addSource(new Vector2(900 *3/4 , 0), 1, Particle_Type.Water, true);
            pm.addSource(new Vector2(5,200),1,Particle_Type.Fire,true);
            int radius = 175;
            int xo = 0;
            int yo = 100;
            for(int x = xo-radius; x < xo+radius; x++)
            {
                for(int y = yo-radius; y < yo+radius; y++)
                {
                    if(Math.Sqrt(Math.Pow((double)(x-xo),2)+Math.Pow((double)(y-yo),2)) < (float)radius)
                        pm.addParticle(new Vector2(x, y), Vector2.Zero, Particle_Type.Plant, true, false);
                }
            }
            for (int x = 200, y = 200; x < 600 && y < 400; x+=2, y++)
            {
                pm.addParticle(new Vector2(x, y), Vector2.Zero, Particle_Type.Wall, true, true);
                pm.addParticle(new Vector2(x+1, y), Vector2.Zero, Particle_Type.Wall, true, true);
                pm.addParticle(new Vector2(x, y+1), Vector2.Zero, Particle_Type.Wall, true, true);
                pm.addParticle(new Vector2(x + 1, y+1), Vector2.Zero, Particle_Type.Wall, true, true);
            }
            
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
            //if(!this.FadingOut)
                pm.Update(gameTime);
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
            FSGGame.spriteBatch.DrawString(FSGGame.Font, "S A N D", this.textDrawPosition, Color.White,0,Vector2.Zero,1,Microsoft.Xna.Framework.Graphics.SpriteEffects.None,.9f);
            FSGGame.spriteBatch.End();
            // Draw menu
            this.menu.Draw();
            this.pm.Draw();
        }
    }
}
