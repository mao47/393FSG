using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FallingSand.Particles;
using Microsoft.Xna.Framework.Input;


namespace FallingSand.Screens
{
    public class ParticleTestScreen : Screen
    {
        private Random rand;
        public  MAOParticleManager pm;
        public Particle_Type currentParticle;
        public int brushSize;
        public ParticleTestScreen(ScreenContainer container)
            : base(container)
        {
            pm = new MAOParticleManager(new Rectangle(0, 0, 800, 400), 100000, 1);
            brushSize = 3;
            rand = new Random(Environment.TickCount);
            //pm.addSource(new Vector2(400, 0), 1, Particle_Type.Sand);
            //pm.addSource(new Vector2(450, 0), 2, Particle_Type.Sand);
            //pm.addSource(new Vector2(500, 0), 3, Particle_Type.Sand);
            pm.addSource(new Vector2(550, 0), 1, Particle_Type.Sand);
            //pm.addSource(new Vector2(600, 0), 5, Particle_Type.Sand);
            currentParticle = Particle_Type.Sand;
        }

        /// <summary>
        /// Updates this instance. This makes sure that GameClock is paused,
        /// and it also updates the menu.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            pm.Update(gameTime);
            getInput();             
        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public override void Draw()
        {
            FSGGame.spriteBatch.Begin();
            FSGGame.spriteBatch.DrawString(FSGGame.Font, "Particles Demo", Vector2.One * 100f, Color.White);
            FSGGame.spriteBatch.End();

            pm.Draw();
        }

        private void getInput()
        {
            if (FSGGame.controller.ContainsBool(Inputs.ActionType.AButton))//mouseState.LeftButton == ButtonState.Pressed)//FSGGame.controller.ContainsBool(Inputs.ActionType.Select))
            {
                Vector2 temp = FSGGame.controller.CursorPosition();
                temp.X -= brushSize;
                temp.Y -= brushSize;
                Vector2 temp2;
                if (currentParticle != Particle_Type.Wall)
                    for (int i = 0; i < brushSize; i++)
                    {
                        temp2 = new Vector2(temp.X + 10 * ((float)rand.NextDouble() - .5f), temp.Y + 10 * ((float)rand.NextDouble()) - .5f); //random location for spawning
                        pm.addParticle(temp2, Vector2.Zero, currentParticle);
                    }
                else
                {
                    for (int x = 0; x < 2 * brushSize; x++)
                    {

                        for (int y = 0; y < 2 * brushSize; y++)
                        {
                            temp2.X = temp.X + x;
                            temp2.Y = temp.Y + y;
                            pm.addParticle(temp2, Vector2.Zero, currentParticle);
                        }
                    }
                }
            }

            if (FSGGame.controller.ContainsBool(Inputs.ActionType.SelectionLeft))   //if left arrow is pressed
            {
                if (currentParticle > 0)
                    currentParticle--;
                else
                    currentParticle = Particle_Type.Wall;
            }

            if (FSGGame.controller.ContainsBool(Inputs.ActionType.SelectionUp) && brushSize <= 100)
                brushSize++;
            else if (FSGGame.controller.ContainsBool(Inputs.ActionType.SelectionDown) && brushSize >= 1)
                brushSize--;
        }
    }
}
