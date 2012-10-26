﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FallingSand.Particles
{
    public class ParticleManager
    {
        ArrayList particles = new ArrayList();
        ArrayList sources = new ArrayList();
        int maxParticles;//Max Number
        float size;
        VertexDeclaration vertexDeclaration;//May not be needed
        GraphicsDevice graphicsDevice;
        int lastRound = 0;
        Rectangle boundry;

        static Random rnd = new Random();
        static int roundTime = 100;//ms
        static int boundryBuffer = 10;//Buffer outside the boundry where the particles are still tracked
        static float Gravity = 1;//arbitrary, adjust as needed

        public Texture2D white;

        public ParticleManager(Rectangle boundries, GraphicsDevice gd, int maxPart, float particleSize)
        {
            boundry = boundries;
            graphicsDevice = gd;
            maxParticles = maxPart;//Not Currently used
            size = particleSize;//Not Currently used

            vertexDeclaration = new VertexDeclaration(Particle.vertexElements);
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
                        addParticle(s.position, new Vector2(0), size, s.type);
                    sources[i] = s;
                }
                for (int i = 0; i < particles.Count; i++)//Update the particles
                {
                    Particle p = (Particle)particles[i];
                    if (p.type != Particle_Type.Wall)//If not not (not a typo) affected by gravity
                        p.direction.Y += Gravity;
                    p.position += p.direction;
                    //Check if p is still in the viewing box
                    Rectangle surround = new Rectangle((int)p.position.X - boundryBuffer, (int)p.position.Y - boundryBuffer, 2 * boundryBuffer, 2 * boundryBuffer);
                    if (!boundry.Intersects(surround))
                    {
                        particles.RemoveAt(i);
                        i--;
                    }
                    else
                        particles[i] = p;
                }
                //TODO: Collision
                //TODO: Remove particles due to intereactions
            }
        }

        public void Draw()
        {//TODO: Different textrue/color for each particle
            FSGGame.spriteBatch.Begin();
            foreach (Particle p in particles)
                FSGGame.spriteBatch.Draw(white, p.position, Color.White);
            FSGGame.spriteBatch.End();
        }

        public void addSource(Vector2 position, int period, Particle_Type type)
        {
            Source source = new Source(position, period, type);
            sources.Add(source);
        }

        public void addParticle(Vector2 position, Vector2 direction, float size, Particle_Type type)
        {
            Particle p;
            p.position = position;
            p.direction = direction;
            p.pointSize = size;
            p.type = type;
            //Check if p is in the viewing box
            Rectangle surround = new Rectangle((int)p.position.X - boundryBuffer, (int)p.position.Y - boundryBuffer, 2 * boundryBuffer, 2 * boundryBuffer);
            if (boundry.Intersects(surround))
                particles.Add(p);
        }

        private void checkCollisions(Particle p)
        {
            
        }

    }
}
