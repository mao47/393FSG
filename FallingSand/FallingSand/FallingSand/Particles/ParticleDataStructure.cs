using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FallingSand.Particles
{
    public class ParticleDataStructure
    {
        Particle[,] particleField;
        List<Particle> particles;
        List<Particle> addList;
        List<Particle> deleteList;
        int maxParticles;//Max Number
        int width, height;

        public ParticleDataStructure(int width, int height, int max)
        {
            this.width = width;
            this.height = height;
            addList = new List<Particle>();
            deleteList = new List<Particle>();
            particleField = new Particle[width, height];
            particles = new List<Particle>(max);
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
        public int particleDeleteCount()
        {
            return deleteList.Count;
        }

        public Particle particleAt(int x, int y)
        {
            if (x > 0 && x < particleField.GetLength(0)
                && y > 0 && y < particleField.GetLength(1))
                return particleField[x, y];
            return null;
        }
        public bool newParticle(Particle p)
        {
            if (!(p.position.X < 0 || p.position.X >= width || p.position.Y < 0 || p.position.Y >= height) && particles.Count < maxParticles)
            {
                addList.Add(p);
                return true;
            }
            return false;
        }

        public void deleteParticle(Particle p)
        {
            if (p != null)
            {
                p.Dead = true;
                particleField[(int)p.position.X, (int)p.position.Y] = null;
                deleteList.Add(p);
            }
        }

        /// <summary>
        /// Call to change the location of a particle. DO NOT set the position manually
        /// </summary>
        /// <param name="p"></param>
        /// <param name="newX"></param>
        /// <param name="newY"></param>
        public void moveParticle(Particle p, int newX, int newY)
        {
            if (newX < 0 || newX >= width || newY < 0 || newY >= height)
            {
                deleteList.Add(p);
                return;
            }
            particleField[(int)p.position.X, (int)p.position.Y] = null;
            particleField[newX, newY] = p;
            p.position = new Vector2(newX, newY);
        }


        public void moveSwapParticle(Particle p, int newX, int newY)
        {
            if (newX < 0 || newX >= width || newY < 0 || newY >= height)
            {
                deleteList.Add(p);
                return;
            }
            Particle old = particleField[newX, newY];
            old.position = new Vector2(p.position.X, p.position.Y);
            particleField[(int)p.position.X, (int)p.position.Y] = old;
            
            particleField[newX, newY] = p;
            p.position = new Vector2(newX, newY);
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
                //var w = particleField.GetLength(0);
                if (r < 0 || r >= particleField.GetLength(0))
                    continue;
                for (int c = y - 1; c <= y + 1; c++)
                {
                    //var h = particleField.GetLength(1);
                    if (c < 0 || c >= particleField.GetLength(1))
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
            //remove particles in deletelist in one pass
            int particlesToRemove = 100;
            var remove = deleteList.Take(particlesToRemove);
            particles.RemoveAll(p => remove.Contains(p));
            //foreach (Particle p in deleteList)
            //{
            //    particleField[(int)p.position.X, (int)p.position.Y] = null;
            //}
            //deleteList.Clear();
            deleteList = deleteList.Skip(particlesToRemove).ToList();
            
            foreach (Particle p in addList)
            {
                if (particleField[(int)p.position.X, (int)p.position.Y] == null)
                {
                    particleField[(int)p.position.X, (int)p.position.Y] = p;
                    particles.Add(p);
                }
                else
                {
                    //deleteList.Add(p);
                }
            }
            addList.Clear();
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
