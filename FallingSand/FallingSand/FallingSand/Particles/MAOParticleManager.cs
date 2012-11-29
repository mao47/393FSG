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
        static int roundTime = 40;//ms
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
                        p.velocity = new Vector2(0, Gravity);
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
                FSGGame.spriteBatch.Draw(white, p.position, p.pColor);
            FSGGame.spriteBatch.End();
        }

        public void addSource(Vector2 position, int period, Particle_Type type)
        {
            Source source = new Source(position, period, type);
            sources.Add(source);
        }

        public bool addParticle(Vector2 position, Vector2 velocity, Particle_Type type)
        {
            Particle p;
            if(type.Equals(Particle_Type.Sand))
                p = new Particle_Sand(position, velocity);
            else if (type.Equals(Particle_Type.Wall))
                p = new Particle_Wall(position, velocity);
            else
                p = new Particle_Sand(position, velocity);
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
            // these are used as flags for checking the first dip and checking for a wall which would block a particle
            bool checkLeft = false;
            bool leftObstacle = false;
            bool checkRight = false;
            bool rightObstacle = false;

            int counter = 1;    //used for incrementing where dips are checked
            List<Particle> collList = particleStorage.withinIndexExcludeSource((int)colP.position.X, (int)colP.position.Y);// new List<Particle>();


            if (particleStorage.particleAt((int)colP.position.X, (int)colP.position.Y + 1) != null)
            {
                colP.velocity = new Vector2(colP.velocity.X, 0);
                if (particleStorage.particleAt((int)colP.position.X - 1, (int)colP.position.Y + 1) != null)
                {
                    if (particleStorage.particleAt((int)colP.position.X + 1, (int)colP.position.Y + 1) == null)
                    {
                        colP.velocity = new Vector2(1, 1);
                    }
                    else
                        while ((!checkLeft && !checkRight) && (!rightObstacle || !leftObstacle))  //if the bottom three particle spaces check all exist, loop until we find the first dip UNLESS both end in a wall
                        {
                            //if there is an obstacle in line with the particle, it will stop checking that side for a dip
                            if(particleStorage.particleAt((int)colP.position.X + counter, (int)colP.position.Y) != null)
                                rightObstacle = true;
                            if (particleStorage.particleAt((int)colP.position.X - counter, (int)colP.position.Y) != null)
                                leftObstacle = true;

                            if(particleStorage.particleAt((int)colP.position.X + counter, (int)colP.position.Y + 1) == null && !rightObstacle)
                            {
                                checkRight = true;
                                colP.lockedDirection = true;
                                break;
                            }
                            if (particleStorage.particleAt((int)colP.position.X - counter, (int)colP.position.Y + 1) == null && !leftObstacle)
                            {
                                checkLeft = true;
                                colP.lockedDirection = true;
                                break;
                            }
                            counter++;
                        }
                    if (checkRight)
                        colP.velocity = new Vector2(1, 0);
                    if (checkLeft)
                        colP.velocity = new Vector2(-1, 0);
                }
                else if (particleStorage.particleAt((int)colP.position.X + 1, (int)colP.position.Y + 1) != null)
                {
                    if (particleStorage.particleAt((int)colP.position.X - 1, (int)colP.position.Y + 1) == null)
                    {
                        colP.velocity = new Vector2(-1, 1);
                    }
                }
            }

            foreach (Particle p in collList)
            {
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
