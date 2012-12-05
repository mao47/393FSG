using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FallingSand.Particles;
using Microsoft.Xna.Framework.Input;
using FallingSand.Achievement;


namespace FallingSand.Screens
{
    public class ParticleTestScreen : Screen
    {
        private Random rand;
        public  MAOParticleManager pm;
        public AchievementManager am;
        public Particle_Type currentParticle;
        public int brushSize;
        private int[] selectPositions;
        public ParticleTestScreen(ScreenContainer container)
            : base(container)
        {
            am = FSGGame.achievementManager;
            pm = new MAOParticleManager(new Rectangle(0, 0, 800, 400), Constants.MaxParticles, Constants.ParticleSize, am.processParticle);
            
            brushSize = 3;
            rand = new Random(Environment.TickCount);
            //pm.addSource(new Vector2(400, 0), 1, Particle_Type.Sand,true);
            //pm.addSource(new Vector2(450, 0), 2, Particle_Type.Sand,true);
            //pm.addSource(new Vector2(500, 0), 3, Particle_Type.Sand,true);
            pm.addSource(new Vector2(550, 0), 1, Particle_Type.Water, true);
            //pm.addSource(new Vector2(5, 360), 1, Particle_Type.Fire, true);
            //pm.addSource(new Vector2(600, 0), 5, Particle_Type.Sand);
            currentParticle = Particle_Type.Sand;

            selectPositions = new int [7];
            selectPositions[0] = 17;
            selectPositions[1] = 117;
            selectPositions[2] = 217;
            selectPositions[3] = 317;
            selectPositions[4] = 417;
            selectPositions[5] = 517;
            selectPositions[6] = 617;
        }

        /// <summary>
        /// Updates this instance. This makes sure that GameClock is paused,
        /// and it also updates the menu.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            pm.Update(gameTime);
            am.Update(gameTime);
            getInput();             
        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public override void Draw()
        {
            FSGGame.spriteBatch.Begin();
            //FSGGame.spriteBatch.DrawString(FSGGame.Font, "Particles Demo", Vector2.One * 100f, Color.White);
            FSGGame.spriteBatch.Draw(FSGGame.select, new Vector2(selectPositions[(int)currentParticle], 425), Color.White);
            FSGGame.spriteBatch.DrawString(FSGGame.Font, "Sand", new Vector2(25, 425), Color.SandyBrown);
            FSGGame.spriteBatch.DrawString(FSGGame.Font, "Water", new Vector2(125, 425), Color.Aqua);
            FSGGame.spriteBatch.DrawString(FSGGame.Font, "Wall", new Vector2(225, 425), Color.Gray);
            FSGGame.spriteBatch.DrawString(FSGGame.Font, "Plant", new Vector2(325, 425), Color.YellowGreen);
            FSGGame.spriteBatch.DrawString(FSGGame.Font, "Fire", new Vector2(425, 425), Color.Firebrick);
            FSGGame.spriteBatch.DrawString(FSGGame.Font, "Remove", new Vector2(525, 425), Color.White);
            FSGGame.spriteBatch.DrawString(FSGGame.Font, "+", new Vector2(625, 410), Color.White);
            FSGGame.spriteBatch.DrawString(FSGGame.Font, "-", new Vector2(625, 440), Color.White);
            FSGGame.spriteBatch.DrawString(FSGGame.Font, "Size:", new Vector2(650, 425), Color.White);
            FSGGame.spriteBatch.Draw(FSGGame.white, new Rectangle(717, 425, 2 * brushSize, 2 * brushSize), Color.White);
            FSGGame.spriteBatch.DrawString(FSGGame.Font, "M", new Vector2(765, 415), Color.White);
            FSGGame.spriteBatch.DrawString(FSGGame.Font, "e", new Vector2(765, 427), Color.White);
            FSGGame.spriteBatch.DrawString(FSGGame.Font, "n", new Vector2(765, 439), Color.White);
            FSGGame.spriteBatch.DrawString(FSGGame.Font, "u", new Vector2(765, 451), Color.White);
            FSGGame.spriteBatch.End();

            pm.Draw();
            am.Draw();
            
        }

        private void getInput()
        {
            if (FSGGame.controller.ContainsBool(Inputs.ActionType.AButton))//mouseState.LeftButton == ButtonState.Pressed)//FSGGame.controller.ContainsBool(Inputs.ActionType.Select))
            {
                Vector2 temp = FSGGame.controller.CursorPosition();
                
                //mouse selection of particle type
                if (temp.Y >= 415)
                {
                    if (temp.X > 25 && temp.X < 115)
                        currentParticle = Particle_Type.Sand;
                    else if (temp.X > 125 && temp.X < 215)
                        currentParticle = Particle_Type.Water;
                    else if (temp.X > 225 && temp.X < 315)
                        currentParticle = Particle_Type.Wall;
                    else if (temp.X > 325 && temp.X < 415)
                        currentParticle = Particle_Type.Plant;
                    else if (temp.X > 425 && temp.X < 515)
                        currentParticle = Particle_Type.Fire;
                    else if (temp.X > 525 && temp.X < 615)
                        currentParticle = Particle_Type.Remove;
                    else if (temp.X > 625 && temp.X < 635)
                    {
                        if (temp.Y > 440 && temp.Y < 460 && brushSize >= 3)
                            brushSize--;
                        else if (temp.Y < 435 && temp.Y > 415 && brushSize <= 20)
                            brushSize++;
                    }
                    else if (temp.X > 760 && temp.X < 779)
                    {
                        this.Disposed = true;
                        FSGGame.screens.Play(new TitleScreen(FSGGame.screens));
                    }
                }

                else
                {
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
                                pm.removeParticleScreenCoord(temp2);
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
