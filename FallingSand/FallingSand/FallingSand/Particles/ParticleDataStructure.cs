using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FallingSand.Particles
{
    class ParticleDataStructure
    {
        Particle[,] particleField;
        List<Particle> particles;
        List<Particle> addList;
        List<Particle> deleteList;
        int maxParticles;//Max Number

        public ParticleDataStructure(int length, int width, int max)
        {
            addList = new List<Particle>();
            deleteList = new List<Particle>();
            particleField = new Particle[length, width];
            particles = new List<Particle>();
            maxParticles = max;
        }

        public void newParticle(Particle p)
        {
            //todo: check max particles
            addList.Add(p);
        }
        public void deleteParticle(Particle p)
        {
            deleteList.Add(p);
        }
        public void moveParticle(Particle p, int newX, int newY)
        {
            particleField[(int)p.position.X, (int)p.position.Y] = null;
            particleField[newX, newY] = p;
            p.position.X = (int)newX;
            p.position.Y = (int)newY;
        }

        public List<Particle> withinIndexExcludeSource(int x, int y)
        {
            List<Particle> collList = new List<Particle>();
            for (int r = x - 1; r <= x + 1; r++)
            {
                if (r < 0 || r > particleField.GetLength(0))
                    continue;
                for (int c = y - 1; c <= y + 1; c++)
                {
                    if (c < 0 || c > particleField.GetLength(1))
                        continue;
                    if (particleField[r, c] != null && (r != x && c != y))
                        collList.Add(particleField[r, c]);

                }
            }
            return collList;
        }

        public void Update()
        {
            foreach (Particle p in addList)
            {
                if (particleField[(int)p.position.X, (int)p.position.Y] == null)
                {
                    particleField[(int)p.position.X, (int)p.position.Y] = p;
                    particles.Add(p);
                }
                else
                {
                    deleteList.Add(p);
                }
            }
            addList.Clear();
            //assume the delete list is much smaller than the list of all particles
            for (int i = 0; i < particles.Count; i++)
            {
                var p = particles[i];
                if (deleteList.Contains(p) && particleField[(int)p.position.X, (int)p.position.Y] == p)
                {
                    particleField[(int)p.position.X, (int)p.position.Y] = null;
                    particles.RemoveAt(i);
                    i--;
                }
            }
            deleteList.Clear();
            
        }

        public IEnumerable<Particle> myEnumerable()
        {
            return particles;
        }
    }
}
