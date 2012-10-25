using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FallingSand.GameElements.Particles
{
    class ParticleManager
    {
        ArrayList particles = new ArrayList();
        int maxParticles;
        float size;
        VertexDeclaration vertexDeclaration;
        GraphicsDevice graphicsDevice;
        static Random rnd = new Random();

        public ParticleManager(GraphicsDevice gd, int maxPart, float particleSize)
        {
            graphicsDevice = gd;
            maxParticles = maxPart;
            size = particleSize;

            vertexDeclaration = new VertexDeclaration(Particle.vertexElements);
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < particles.Count; i++)
            {
                Particle p = (Particle)particles[i];
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

        public void Draw()
        {
            //This line is from the book, PoinList isn't there for some reason
            //graphicsDevice.DrawUserPrimitives<Particle>(PrimitiveType.PointList, particles.ToArray(), 0, particles.Count);
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
