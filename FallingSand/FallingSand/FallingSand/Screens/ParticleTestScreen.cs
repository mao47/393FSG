using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FallingSand.Particles;

namespace FallingSand.Screens
{
    class ParticleTestScreen : Screen
    {

        public ParticleTestScreen(ScreenContainer container)
            : base(container)
        {


        }
        ParticleManager manager;
        /// <summary>
        /// Updates this instance. This makes sure that GameClock is paused,
        /// and it also updates the menu.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (FSGGame.controller.ContainsBool(Inputs.ActionType.Select))
            {
                Vector2 temp = FSGGame.controller.CursorPosition();
                manager.addParticle(temp, Vector2.Zero, 1, Particle_Type.Sand);
            }

        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public override void Draw()
        {
            FSGGame.spriteBatch.Begin();
            FSGGame.spriteBatch.DrawString(FSGGame.Font, "LIKE A BAOWSSS", Vector2.One * 100f, Color.White);
            FSGGame.spriteBatch.End();
        }
    }
}
