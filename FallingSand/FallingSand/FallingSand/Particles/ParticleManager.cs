using System;
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
        int maxParticles;
        float size;
        VertexDeclaration vertexDeclaration;
        GraphicsDevice graphicsDevice;
        int lastRound = 0;

        static Random rnd = new Random();
        static int roundTime = 100;//ms
        static float Gravity = 1;

        public Texture2D white;

        public ParticleManager(GraphicsDevice gd, int maxPart, float particleSize)
        {
            graphicsDevice = gd;
            maxParticles = maxPart;
            size = particleSize;

            vertexDeclaration = new VertexDeclaration(Particle.vertexElements);
            white = FSGGame.white;
        }

        public void Update(GameTime gameTime)
        {
            lastRound += gameTime.ElapsedGameTime.Milliseconds;
            while (lastRound >= roundTime)
            {
                lastRound -= roundTime;
                for (int i = 0; i < sources.Count; i++)
                {
                    Source s = (Source)sources[i];
                    if (s.isTimeForNewParticle())
                        addParticle(s.position, new Vector2(0), size, s.type);
                    sources[i] = s;
                }
                for (int i = 0; i < particles.Count; i++)
                {
                    Particle p = (Particle)particles[i];
                    if (p.type != Particle_Type.Wall)
                        p.direction.Y += Gravity;
                    p.position += p.direction;
                    //TODO: Check if p is still there
                    if (false)
                    {
                        particles.RemoveAt(i);
                        i--;
                    }
                    else
                        particles[i] = p;
                }
                //TODO: Collision
            }
        }

        public void Draw()
        {
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
            particles.Add(p);
        }

    }
}
