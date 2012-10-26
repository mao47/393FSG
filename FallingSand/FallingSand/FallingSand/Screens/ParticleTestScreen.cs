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
        private ParticleManager pm;
        private int brushSize = 5;

        public ParticleTestScreen(ScreenContainer container)
            : base(container)
        {
            pm = new ParticleManager(FSGGame.graphics.GraphicsDevice, 100000, 1);
            pm.addSource(new Vector2(400, 0), 1, Particle_Type.Sand);
            pm.addSource(new Vector2(450, 0), 2, Particle_Type.Sand);
            pm.addSource(new Vector2(500, 0), 3, Particle_Type.Sand);
            pm.addSource(new Vector2(550, 0), 4, Particle_Type.Sand);
            pm.addSource(new Vector2(600, 0), 5, Particle_Type.Sand);
        }

        /// <summary>
        /// Updates this instance. This makes sure that GameClock is paused,
        /// and it also updates the menu.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);





            pm.Update(gameTime);
            
            if (FSGGame.controller.ContainsBool(Inputs.ActionType.Select))
            {
                Vector2 temp = FSGGame.controller.CursorPosition();
                temp.X -= brushSize;
                temp.Y -= brushSize;
                Vector2 temp2;
                
                for (int x = 0; x < 2 * brushSize; x++)
                {

                    for (int y = 0; y < 2 * brushSize; y++)
                    {
                        temp2.X = temp.X + x;
                        temp2.Y = temp.Y + y;
                        pm.addParticle(temp2, Vector2.Zero, 1, Particle_Type.Sand);
                    }
                }
                
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

            pm.Draw();
        }
    }
}
