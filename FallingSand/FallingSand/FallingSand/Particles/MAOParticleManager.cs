using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FallingSand.Particles
{
    class MAOParticleManager
    {
        Particle[,] particleField;
        List<Particle> particles = new List<Particle>();
        List<Source> sources = new List<Source>();
        int maxParticles;//Max Number
        GraphicsDevice graphicsDevice;
        int lastRound = 0;
        Rectangle boundry;

        static Random rnd = new Random();
        static int roundTime = 100;//ms
        static int boundryBuffer = 10;//Buffer outside the boundry where the particles are still tracked
        static float Gravity = 2;//arbitrary, adjust as needed

        public Texture2D white;

        public MAOParticleManager(Rectangle boundries, GraphicsDevice gd, int maxPart, float particleSize)
        {
            particleField = new Particle[1800, 1400];
            boundry = boundries;
            graphicsDevice = gd;
            maxParticles = maxPart;//Not Currently used

            white = FSGGame.white;
        }

        public void Update(GameTime gameTime)
        {
            lastRound += gameTime.ElapsedGameTime.Milliseconds;
            while (lastRound >= roundTime)//updates for the number of rounds needed (possibly should only go once)
            {
                lastRound -= roundTime;
                for (int i = 0; i < sources.Count; i++)//Add particles from sources
                {
                    Source s = (Source)sources[i];
                    if (s.isTimeForNewParticle())
                        addParticle(s.getNewParticlePosition(), new Vector2(0), s.type);
                    sources[i] = s;
                }
                for (int i = 0; i < particles.Count; i++)//Update the particles
                {
                    Particle p = (Particle)particles[i];
                    if (p.type != Particle_Type.Wall)//If not not (not a typo) affected by gravity
                    {
                        p.velocity.Y = Gravity;
                        this.checkCollisions(p);  //check collisions
                    }

                    p.position += p.velocity;
                    //Check if p is still in the viewing box
                    Rectangle surround = new Rectangle((int)p.position.X - boundryBuffer, (int)p.position.Y - boundryBuffer, 2 * boundryBuffer, 2 * boundryBuffer);
                    if (!boundry.Intersects(surround))
                    {
                        particles.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        particleField[(int)p.position.X, (int)p.position.Y] = p;
                        particles[i] = p;
                    }
                }
                //TODO: Collision
                //TODO: Remove particles due to intereactions
            }
        }

        public void Draw()
        {
            FSGGame.spriteBatch.Begin();
            foreach (Particle p in particles)
                FSGGame.spriteBatch.Draw(white, p.position, p.getColor());
            FSGGame.spriteBatch.End();
        }

        public void addSource(Vector2 position, int period, Particle_Type type)
        {
            Source source = new Source(position, period, type);
            sources.Add(source);
        }

        public void addParticle(Vector2 position, Vector2 velocity, Particle_Type type)
        {
            Particle p = new Particle(position, velocity, type);
            //Check if p is in the viewing box
            Rectangle surround = new Rectangle((int)p.position.X - boundryBuffer, (int)p.position.Y - boundryBuffer, 2 * boundryBuffer, 2 * boundryBuffer);
            if (boundry.Intersects(surround))
            {
                particles.Add(p);
                particleField[(int)p.position.X, (int)p.position.Y] = p;//todo fix
            }
        }

        private bool checkCollisions(Particle colP)
        {
            //IEnumerable<Particle> collQuery =
            //from p in particles
            //where Math.Abs(p.position.X - colP.position.X) < 1 && Math.Abs(p.position.Y - colP.position.Y) < 1 && p.type == Particle_Type.Wall
            //select p;

            List<Particle> collList = new List<Particle>();
            for (int r = (int)colP.position.X - 1; r <= (int)colP.position.X + 1; r++)
            {
                if (r < 0 || r > particleField.GetLength(0))
                    continue;
                for (int c = (int)colP.position.Y - 1; c <= (int)colP.position.Y + 1; c++)
                {
                    if (c < 0 || c > particleField.GetLength(1))
                        continue;
                    if (particleField[r, c] != null && (r != (int)colP.position.X && c != (int)colP.position.Y))
                        collList.Add(particleField[r, c]);

                }
            }


            foreach (Particle p in collList)
            {
                p.setColor(Color.Red);
                    //if (p.position.Y - colP.position.Y <= 1 && p.position.X -colP.position.X <= 1)
                    //{
                        isColliding(colP, p);
                        //return true;
                    //}
            }
            return true;
        }

        private bool isColliding(Particle pOrig, Particle collider)
        {
            pOrig.velocity.Y = 0;

            return true;
        }
    }
}
