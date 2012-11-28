using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FallingSand.Particles
{
    public class ParticleDataStructure
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

        public int particleCount()
        {
            return particles.Count;
        }
        public int particleAddCount()
        {
            return addList.Count;
        }

        public bool newParticle(Particle p)
        {
            if (particles.Count < maxParticles)
            {
                addList.Add(p);
                return true;
            }
            return false;
        }

        public void deleteParticle(Particle p)
        {
            deleteList.Add(p);
        }

        /// <summary>
        /// Call to change the location of a particle. DO NOT set the position manually
        /// </summary>
        /// <param name="p"></param>
        /// <param name="newX"></param>
        /// <param name="newY"></param>
        public void moveParticle(Particle p, int newX, int newY)
        {
            particleField[(int)p.position.X, (int)p.position.Y] = null;
            particleField[newX, newY] = p;
            p.position.X = (int)newX;
            p.position.Y = (int)newY;
        }

        /// <summary>
        /// Returns a list of all the particles within a chebyshev distance of 1 from the given x,y position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Checks the add and remove lists to officially make the changes to the matrix and list
        /// </summary>
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

        /// <summary>
        /// Returns the list as an IEnumerable for client code to iterate over
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Particle> myEnumerable()
        {
            return particles;
        }
    }
}
