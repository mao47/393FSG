using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FallingSand.Particles
{
    public class MAOParticleManager
    {

        //Particle[,] particleField;
        //protected List<Particle> particles = new List<Particle>();

        int maxParticles;//Max Number



        List<Source> sources;

        int lastRound;

        Rectangle boundry;
        protected ParticleDataStructure particleStorage;
        static Random rnd = new Random();
        static int roundTime = 100;//ms
        static int boundryBuffer = 0;//Buffer outside the boundry where the particles are still tracked
        static float Gravity = 1;//arbitrary, adjust as needed
        static float epsilon = 0.001f;
        public Texture2D white;

        public MAOParticleManager(Rectangle boundries, int maxPart, float particleSize)
        {
            lastRound = 0;
            sources = new List<Source>();
            boundry = boundries;

            maxParticles = maxPart;//Not Currently used

            particleStorage = new ParticleDataStructure(1800, 1400, maxPart);

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
                foreach(Particle p in particleStorage.myEnumerable())
                {
                //for (int i = 0; i < particles.Count; i++)//Update the particles
                //{
                    //Particle p = (Particle)particles[i];
                    if (p.type != Particle_Type.Wall)//If not not (not a typo) affected by gravity
                    {
                        p.velocity.X = 0;
                        p.velocity.Y = Gravity;
                        this.checkCollisions(p);  //check collisions
                    }
                    Vector2 newPosition = p.position + p.velocity;
                    particleStorage.moveParticle(p, (int)newPosition.X, (int)newPosition.Y);
                    //p.position += p.velocity;
                    //Check if p is still in the viewing box
                    Rectangle surround = new Rectangle((int)p.position.X - boundryBuffer, (int)p.position.Y - boundryBuffer, 2 * boundryBuffer, 2 * boundryBuffer);
                    if (!boundry.Intersects(surround))
                    {
                        particleStorage.deleteParticle(p);
                        //particles.RemoveAt(i);
                        //i--;
                    }
                    else
                    {
                        //particleField[(int)p.position.X, (int)p.position.Y] = p;
                        //particles[i] = p;
                    }
                }
                //TODO: Collision
                //TODO: Remove particles due to intereactions
            }
            particleStorage.Update();
        }

        public void Draw()
        {
            FSGGame.spriteBatch.Begin();
            foreach (Particle p in particleStorage.myEnumerable())
                FSGGame.spriteBatch.Draw(white, p.position, p.getColor());
            FSGGame.spriteBatch.End();
        }

        public void addSource(Vector2 position, int period, Particle_Type type)
        {
            Source source = new Source(position, period, type);
            sources.Add(source);
        }

        public bool addParticle(Vector2 position, Vector2 velocity, Particle_Type type)
        {
            Particle p = new Particle(position, velocity, type);
            //Check if p is in the viewing box
            Rectangle surround = new Rectangle((int)p.position.X - boundryBuffer, (int)p.position.Y - boundryBuffer, 2 * boundryBuffer, 2 * boundryBuffer);
            if (boundry.Intersects(surround))
            {
//<<<<<<< HEAD
                //particles.Add(p);
                //particleField[(int)p.position.X, (int)p.position.Y] = p;//todo fix
                //return true;
//=======
                if (particleStorage.newParticle(p))
                    return true;
                //particles.Add(p);
                //particleField[(int)p.position.X, (int)p.position.Y] = p;//todo fix

            }
            return false;
        }

        private bool checkCollisions(Particle colP)
        {
            //IEnumerable<Particle> collQuery =
            //from p in particles
            //where Math.Abs(p.position.X - colP.position.X) < 1 && Math.Abs(p.position.Y - colP.position.Y) < 1 && p.type == Particle_Type.Wall
            //select p;

            List<Particle> collList = particleStorage.withinIndexExcludeSource((int)colP.position.X, (int)colP.position.Y);// new List<Particle>();
            //for (int r = (int)colP.position.X - 1; r <= (int)colP.position.X + 1; r++)
            //{
            //    if (r < 0 || r > particleField.GetLength(0))
            //        continue;
            //    for (int c = (int)colP.position.Y - 1; c <= (int)colP.position.Y + 1; c++)
            //    {
            //        if (c < 0 || c > particleField.GetLength(1))
            //            continue;
            //        if (particleField[r, c] != null && (r != (int)colP.position.X && c != (int)colP.position.Y))
            //            collList.Add(particleField[r, c]);

            //    }
            //}

            //var under = from p in collList
            //            where (colP.position.Y - 1) - p.position.Y < -epsilon 
            //            select p;
            //foreach (Particle p in under)
            //{
            //}
            if (particleStorage.particleAt((int)colP.position.X, (int)colP.position.Y + 1) != null)
            {
                colP.velocity.Y = 0;
                if (particleStorage.particleAt((int)colP.position.X - 1, (int)colP.position.Y + 1) != null)
                {
                    if (particleStorage.particleAt((int)colP.position.X + 1, (int)colP.position.Y + 1) == null)
                    {
                        colP.velocity.X = 1;
                        colP.velocity.Y = 1;
                    }
                }
                else if (particleStorage.particleAt((int)colP.position.X + 1, (int)colP.position.Y + 1) != null)
                {
                    if (particleStorage.particleAt((int)colP.position.X - 1, (int)colP.position.Y + 1) == null)
                    {
                        colP.velocity.X = -1;
                        colP.velocity.Y = 1;
                    }
                }
            }
            //if (collList.Any(p => (int)p.position.X == (int)colP.position.X && (int)p.position.Y - 1 - (int)colP.position.Y <= 0))
            //{
            //    colP.velocity.Y = 0;

            //    if (collList.Any(p1 => (int)p1.position.X - 1 == (int)colP.position.X && (int)p1.position.Y - 1 == (int)colP.position.Y))
            //    {
            //        if (!collList.Any(p2 => (int)p2.position.X + 1 == (int)colP.position.X && (int)p2.position.Y - 1 == (int)colP.position.Y))
            //        {
            //            colP.velocity.X = 1;
            //            colP.velocity.Y = 1;
            //        }
            //    }
            //    else if (collList.Any(p1 => (int)p1.position.X + 1 == (int)colP.position.X && (int)p1.position.Y - 1 == (int)colP.position.Y))
            //    {
            //        if (!collList.Any(p2 => (int)p2.position.X - 1 == (int)colP.position.X && (int)p2.position.Y - 1 == (int)colP.position.Y))
            //        {
            //            colP.velocity.X = -1;
            //            colP.velocity.Y = 1;
            //        }
            //    }
            //}
            foreach (Particle p in collList)
            {
                //p.setColor(Color.Red);
                //if (p.position.Y - colP.position.Y <= 1 && p.position.X -colP.position.X <= 1)

                isColliding(colP, p);
                if ((colP.position.Y - 1) - p.position.Y < -epsilon)//colP is above p
                {
                }
            }
            return true;
        }

        private bool isColliding(Particle pOrig, Particle collider)
        {
            //pOrig.velocity.Y = 0;

            return true;
        }
    }
}
