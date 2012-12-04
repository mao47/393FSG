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
        int particleSize;
        Rectangle boundry;
        protected ParticleDataStructure particleStorage;
        //static Random rnd = new Random();
        static int roundTime = 40;//ms
        static int boundryBuffer = 0;//Buffer outside the boundry where the particles are still tracked
        static float Gravity = 1;//arbitrary, adjust as needed
        static float epsilon = 0.001f;
        Random rnd;
        public Texture2D white;

        public MAOParticleManager(Rectangle boundries, int maxPart, int particleSize)
        {
            lastRound = 0;
            sources = new List<Source>();
            boundry = boundries;
            this.particleSize = particleSize;
            maxParticles = maxPart;

            particleStorage = new ParticleDataStructure(boundries.Width/particleSize, boundries.Height/particleSize, maxPart);

            white = FSGGame.white;
        }

        public void Update(GameTime gameTime)
        {
            rnd = new Random(gameTime.ElapsedGameTime.Milliseconds);
            lastRound += gameTime.ElapsedGameTime.Milliseconds;
            while (lastRound >= roundTime)//updates for the number of rounds needed (possibly should only go once)
            {
                lastRound -= roundTime;
                for (int i = 0; i < sources.Count; i++)//Add particles from sources
                {
                    Source s = (Source)sources[i];
                    //if (s.isTimeForNewParticle())
                     //   addParticle(s.getNewParticlePosition(), new Vector2(0), s.type);
                    for (int x = 0; x < 2 * s.period; x++)
                    {

                        for (int y = 0; y < 2 * s.period; y++)
                        {
                            addParticle(s.getNewParticlePosition(), Vector2.Zero, s.type, false, false);
                        }
                    }
                    sources[i] = s;
                }
                foreach(Particle p in particleStorage.myEnumerable())
                {
                    if (p.Dead)
                        continue;
                //for (int i = 0; i < particles.Count; i++)//Update the particles
                //{
                    //Particle p = (Particle)particles[i];
                    if (p.type != Particle_Type.Wall && p.type != Particle_Type.Plant && p.type != Particle_Type.Fire)//If not not (not a typo) affected by gravity
                    {
                        p.velocity = new Vector2(0, Gravity);
                        this.checkCollisions(p);  //check collisions
                    }
                    else if (p.type == Particle_Type.Fire)
                        handleFire(p);
                    p.Update(particleStorage);

                    if (p.remove)
                        particleStorage.deleteParticle(p);
                        //removeParticle(p.position);
                        //particleStorage.deleteParticle(particleStorage.particleAt((int)p.position.X, (int)p.position.Y));
                    //p.position += p.velocity;
                    //Check if p is still in the viewing box
                    //Rectangle surround = new Rectangle((int)p.position.X - boundryBuffer, (int)p.position.Y - boundryBuffer, 2 * boundryBuffer, 2 * boundryBuffer);
                    //if (!boundry.Intersects(surround))
                    //{
                    //    particleStorage.deleteParticle(p);
                    //    //particles.RemoveAt(i);
                    //    //i--;
                    //}
                    //else
                    //{
                    //    //particleField[(int)p.position.X, (int)p.position.Y] = p;
                    //    //particles[i] = p;
                    //}
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
            {
                if (p.Dead) continue;
                FSGGame.spriteBatch.Draw(white, p.position * particleSize + new Vector2(boundry.X, boundry.Y), null, p.pColor, 0, Vector2.Zero, particleSize, SpriteEffects.None, 0);
            }
            FSGGame.spriteBatch.End();
            //Console.WriteLine("total: " + particleStorage.particleCount() + ", add: " + particleStorage.particleAddCount() + ", delete: " + particleStorage.particleDeleteCount());
        }

        public void addSource(Vector2 position, int period, Particle_Type type, bool screenCoord)
        {
            if(screenCoord)
                position = (position + new Vector2(boundry.X, boundry.Y)) / particleSize;
            Source source = new Source(position, period, type);
            sources.Add(source);
        }

        public bool removeParticle(Vector2 position, bool screenCoord)
        {
            if (screenCoord)
                position = (position + new Vector2(boundry.X, boundry.Y)) / particleSize;
            particleStorage.deleteParticle(particleStorage.particleAt((int)position.X, (int)position.Y));
            return true;
        }
        public bool removeParticleScreenCoord(Vector2 screenPos)
        {
            return removeParticle(screenPos, true);
        }

        public bool addParticle(Vector2 position, Vector2 velocity, Particle_Type type, bool screenCoord, bool overlay)
        {
            if(screenCoord)
                position = (position + new Vector2(boundry.X, boundry.Y)) / particleSize;
            position.X = (int)position.X;
            position.Y = (int)position.Y;
            Particle p;
            if(type.Equals(Particle_Type.Sand))
                p = new Particle_Sand(position, velocity);
            else if (type.Equals(Particle_Type.Wall))
                p = new Particle_Wall(position, velocity);
            else if (type.Equals(Particle_Type.Water))
                p = new Particle_Water(position, velocity);
            else if (type.Equals(Particle_Type.Plant))
                p = new Particle_Plant(position, velocity);
            else if (type.Equals(Particle_Type.Fire))
                p = new Particle_Fire(position, velocity);
            else
                p = new Particle_Sand(position, velocity);
            //Check if p is in the viewing box
            Rectangle surround = new Rectangle((int)p.position.X - boundryBuffer, (int)p.position.Y - boundryBuffer, 2 * boundryBuffer, 2 * boundryBuffer);
            if (boundry.Intersects(surround))
            {
                //particles.Add(p);
                //particleField[(int)p.position.X, (int)p.position.Y] = p;//todo fix
                //return true;
                if (!overlay)
                {
                    if (particleStorage.particleAt((int)p.position.X, (int)p.position.Y) != null && particleStorage.particleAt((int)p.position.X, (int)p.position.Y).type == p.type)
                        return true;
                    else
                    {
                        particleStorage.deleteParticle(particleStorage.particleAt((int)p.position.X, (int)p.position.Y));
                        particleStorage.newParticle(p);
                        return true;
                    }
                }
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

            checkTurnToPlant(colP);

            if (particleStorage.particleAt((int)colP.position.X, (int)colP.position.Y + 1) != null)
            {
                //if particle is sand, and water is below, switch to cause sand to fall in water
                if (colP.type == Particle_Type.Sand && particleStorage.particleAt((int)colP.position.X, (int)colP.position.Y + 1).type == Particle_Type.Water)
                {
                    
                    //BAD, me no likey: colP.position = new Vector2(colP.position.X, colP.position.Y + 1);
                    particleStorage.moveSwapParticle(colP, (int)colP.position.X, (int)colP.position.Y + 1);
                    colP.velocity = Vector2.Zero;
                }

                else
                {
                    colP.velocity = new Vector2(colP.velocity.X, 0);

                    //future acid particle interaction?
                    //if (colP.type == Particle_Type.Acid && rnd.Next(2) == 0)
                    //{
                    //    Vector2 tempPos = particleStorage.particleAt((int)colP.position.X, (int)colP.position.Y + 1).position;
                    //    particleStorage.deleteParticle(particleStorage.particleAt((int)colP.position.X, (int)colP.position.Y + 1));
                    //    addParticle(tempPos, new Vector2(0), Particle_Type.Plant);
                    //}

                    //turns water into plant.


                    if (particleStorage.particleAt((int)colP.position.X - 1, (int)colP.position.Y + 1) != null) //there is a particle to the bottom left
                    {
                        if (particleStorage.particleAt((int)colP.position.X + 1, (int)colP.position.Y + 1) == null) //there is no particle to the right
                        {
                            colP.velocity = new Vector2(1, 1);
                        }
                        //used to make particles behave more liquidlike and less powder like
                        else if (!surrounded(colP))    //if the particle is surrounded by other particles, it shouldn't check
                            while ((!checkLeft && !checkRight) && (!rightObstacle || !leftObstacle))  //if the bottom three particle spaces check all exist, loop until we find the first dip UNLESS both end in a wall
                            {
                                //if there is an obstacle in line with the particle, it will stop checking that side for a dip
                                if (particleStorage.particleAt((int)colP.position.X + counter, (int)colP.position.Y) != null)
                                    //if(particleStorage.particleAt((int)colP.position.X + counter, (int)colP.position.Y).type == Particle_Type.Wall) //this drastically slows performance but speeds up liquid behavior
                                    rightObstacle = true;
                                if (particleStorage.particleAt((int)colP.position.X - counter, (int)colP.position.Y) != null)
                                    //if(particleStorage.particleAt((int)colP.position.X - counter, (int)colP.position.Y).type == Particle_Type.Wall) //this drastically slows performance but speeds up liquid behavior
                                    leftObstacle = true;

                                if (particleStorage.particleAt((int)colP.position.X + counter, (int)colP.position.Y + 1) == null && !rightObstacle)
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
                            colP.velocity = new Vector2(3, 0);
                        if (checkLeft)
                            colP.velocity = new Vector2(-3, 0);
                    }
                    else if (particleStorage.particleAt((int)colP.position.X + 1, (int)colP.position.Y + 1) != null)    //there is a particle to the right
                    {
                        if (particleStorage.particleAt((int)colP.position.X - 1, (int)colP.position.Y + 1) == null) //there is no particle to the left
                        {
                            colP.velocity = new Vector2(-1, 1);
                        }
                    }
                    else if (particleStorage.particleAt((int)colP.position.X, (int)colP.position.Y + 1) != null)
                    //there is a particle directly underneath, but no particles to either side underneath (solves pillar problem)
                    {
                        if (rnd.Next(1) == 0)
                            colP.velocity = new Vector2(1, 1);
                        else
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

        private bool handleFire(Particle p)
        {
            Vector2[] positionList = new Vector2[8];
            positionList[0] = new Vector2(p.position.X, p.position.Y - 1);//Right above first
            positionList[1] = new Vector2(p.position.X -1, p.position.Y - 1);//Then top corners
            positionList[2] = new Vector2(p.position.X + 1, p.position.Y - 1);
            positionList[3] = new Vector2(p.position.X - 1, p.position.Y);//Left and Right
            positionList[4] = new Vector2(p.position.X + 1, p.position.Y - 1);
            positionList[5] = new Vector2(p.position.X, p.position.Y + 1);//Right Below
            positionList[6] = new Vector2(p.position.X - 1, p.position.Y + 1);//Bottom corners
            positionList[7] = new Vector2(p.position.X + 1, p.position.Y + 1);

            bool isRemoved = false;
            foreach(Vector2 pos in positionList)
                if (/*!isRemoved &&*/ particleStorage.particleAt((int)pos.X, (int)pos.Y) != null)
                {
                    Particle other = particleStorage.particleAt((int)pos.X, (int)pos.Y);
                    //double random = rnd.NextDouble();
                    if (other.type == Particle_Type.Water)
                    {
                        particleStorage.deleteParticle(p);
                        particleStorage.deleteParticle(other);
                        isRemoved = true;
                    }
                    else if (other.type == Particle_Type.Plant)
                    {
                        
                        if (rnd.Next(10) == 0)//10% chance to delete plant
                        {
                            particleStorage.deleteParticle(other);
                            if(rnd.NextDouble() < 0.9)//90% chance to propagate
                                addParticle(other.position, Vector2.Zero, Particle_Type.Fire, false, true);
                        }
                    }
                    else if (other.type == Particle_Type.Wall || other.type == Particle_Type.Sand)
                    {
                        
                        if (rnd.Next(10) == 0)//10% chance to delete wall
                        {
                            particleStorage.deleteParticle(other);
                            if(rnd.Next(4) == 0) //25% chance to propagate
                                addParticle(other.position, Vector2.Zero, Particle_Type.Fire, false, true);
                        }
                    }
                }
            return true;
        }
        
        private void checkTurnToPlant(Particle colP)
        {
            if (colP.type != Particle_Type.Water)
                return;
            int deleteSize = particleStorage.particleDeleteCount();
            if (particleStorage.particleAt((int)colP.position.X, (int)colP.position.Y + 1) != null)//beleow
            {
                if (particleStorage.particleAt((int)colP.position.X, (int)colP.position.Y + 1).type == Particle_Type.Plant && rnd.Next(10) == 0)
                {
                    Vector2 tempPos = colP.position;
                    particleStorage.deleteParticle(colP);
                    addParticle(tempPos, new Vector2(0), Particle_Type.Plant, false, true);
                }
            }
            if (particleStorage.particleAt((int)colP.position.X, (int)colP.position.Y - 1) != null && deleteSize == particleStorage.particleDeleteCount())  //above
            {
                if (particleStorage.particleAt((int)colP.position.X, (int)colP.position.Y - 1).type == Particle_Type.Plant && rnd.Next(10) == 0)
                {
                    Vector2 tempPos = colP.position;
                    particleStorage.deleteParticle(colP);
                    addParticle(tempPos, new Vector2(0), Particle_Type.Plant, false, true);
                }
            }
            if (particleStorage.particleAt((int)colP.position.X + 1, (int)colP.position.Y) != null && deleteSize == particleStorage.particleDeleteCount())  //right
            {
                if (particleStorage.particleAt((int)colP.position.X + 1, (int)colP.position.Y).type == Particle_Type.Plant && rnd.Next(6) == 0)
                {
                    Vector2 tempPos = colP.position;
                    particleStorage.deleteParticle(colP);
                    addParticle(tempPos, new Vector2(0), Particle_Type.Plant, false, true);
                }
            }
            if (particleStorage.particleAt((int)colP.position.X - 1, (int)colP.position.Y) != null && deleteSize == particleStorage.particleDeleteCount())  //left
            {
                if (particleStorage.particleAt((int)colP.position.X - 1, (int)colP.position.Y).type == Particle_Type.Plant && rnd.Next(6) == 0)
                {
                    Vector2 tempPos = colP.position;
                    particleStorage.deleteParticle(colP);
                    addParticle(tempPos, new Vector2(0), Particle_Type.Plant, false, true);
                }
            }
        }

        private bool surrounded(Particle colP)
        {
            if (particleStorage.particleAt((int)colP.position.X, (int)colP.position.Y + 1) != null && particleStorage.particleAt((int)colP.position.X, (int)colP.position.Y + 1) != null &&//beleow
                particleStorage.particleAt((int)colP.position.X, (int)colP.position.Y - 1) != null && particleStorage.particleAt((int)colP.position.X, (int)colP.position.Y - 1) != null &&//above
                particleStorage.particleAt((int)colP.position.X + 1, (int)colP.position.Y) != null && particleStorage.particleAt((int)colP.position.X + 1, (int)colP.position.Y) != null && //right
                particleStorage.particleAt((int)colP.position.X - 1, (int)colP.position.Y) != null && particleStorage.particleAt((int)colP.position.X - 1, (int)colP.position.Y) != null)//left
                return true;
            else
                return false;

        }

        private bool isColliding(Particle pOrig, Particle collider)
        {
            //pOrig.velocity.Y = 0;

            return true;
        }
    }
}
