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
        private int[] selectPositions;
        public ParticleTestScreen(ScreenContainer container)
            : base(container)
        {
            pm = new MAOParticleManager(new Rectangle(0, 0, 800, 400), 100000, 4);
            brushSize = 3;
            rand = new Random(Environment.TickCount);
            //pm.addSource(new Vector2(400, 0), 1, Particle_Type.Sand);
            //pm.addSource(new Vector2(450, 0), 2, Particle_Type.Sand);
            //pm.addSource(new Vector2(500, 0), 3, Particle_Type.Sand);
            pm.addSource(new Vector2(550, 0), 1, Particle_Type.Water);
            //pm.addSource(new Vector2(600, 0), 5, Particle_Type.Sand);
            currentParticle = Particle_Type.Sand;

            selectPositions = new int [5];
            selectPositions[0] = 35;
            selectPositions[1] = 135;
            selectPositions[2] = 235;
            selectPositions[3] = 340;
            selectPositions[4] = 445;
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
            FSGGame.spriteBatch.Draw(FSGGame.select, new Vector2(selectPositions[(int)currentParticle], 425), Color.White);
            FSGGame.spriteBatch.DrawString(FSGGame.Font, "Sand", new Vector2(50, 425), Color.SandyBrown);
            FSGGame.spriteBatch.DrawString(FSGGame.Font, "Water", new Vector2(150, 425), Color.Aqua);
            FSGGame.spriteBatch.DrawString(FSGGame.Font, "Wall", new Vector2(250, 425), Color.Gray);
            FSGGame.spriteBatch.DrawString(FSGGame.Font, "Plant", new Vector2(350, 425), Color.YellowGreen);
            FSGGame.spriteBatch.DrawString(FSGGame.Font, "Remove", new Vector2(450, 425), Color.White);
            FSGGame.spriteBatch.DrawString(FSGGame.Font, "Size:", new Vector2(600, 425), Color.White);
            FSGGame.spriteBatch.Draw(FSGGame.white, new Rectangle(700, 425, 2 * brushSize, 2 * brushSize), Color.White);
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

                if (currentParticle == Particle_Type.Remove)
                    for (int x = 0; x < 2 * brushSize; x++)
                    {
                        for (int y = 0; y < 2 * brushSize; y++)
                        {
                            temp2.X = temp.X + x;
                            temp2.Y = temp.Y + y;
                            pm.removeParticle(temp2);
                        }
                    }
                else if (currentParticle != Particle_Type.Wall && currentParticle != Particle_Type.Plant)
                {
                    temp.X += brushSize;
                    temp.Y += brushSize;
                    for (int i = 0; i < brushSize; i++)
                    {
                        temp2 = new Vector2(temp.X + 10 * ((float)rand.NextDouble() - .5f), temp.Y + 10 * ((float)rand.NextDouble()) - .5f); //random location for spawning
                        pm.addParticle(temp2, Vector2.Zero, currentParticle, true, true);
                    }
                }
                else
                {
                    for (int x = 0; x < 2 * brushSize; x++)
                    {
                        for (int y = 0; y < 2 * brushSize; y++)
                        {
                            temp2.X = temp.X + x;
                            temp2.Y = temp.Y + y;
                            pm.addParticle(temp2, Vector2.Zero, currentParticle, true, false);
                        }
                    }
                }
            }

            if (FSGGame.controller.ContainsBool(Inputs.ActionType.SelectionRight))   //if right arrow is pressed
            {
                if (currentParticle != Particle_Type.Remove)
                    currentParticle++;
                else
                    currentParticle = Particle_Type.Sand;
            }
            else if (FSGGame.controller.ContainsBool(Inputs.ActionType.SelectionLeft))   //if left arrow is pressed
            {
                if (currentParticle > 0)
                    currentParticle--;
                else
                    currentParticle = Particle_Type.Remove;
            }

            if (FSGGame.controller.ContainsBool(Inputs.ActionType.SelectionUp) && brushSize <= 20)
                brushSize++;
            else if (FSGGame.controller.ContainsBool(Inputs.ActionType.SelectionDown) && brushSize >= 3)
                brushSize--;
        }
    }
}
