using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FallingSand.Particles
{
    class Source
    {
        public Vector2 position;
        public int period;
        int counter;
        public Particle_Type type;

        static Random rnd = new Random();
        static float radius = 10;

        public Source(Vector2 pos, int cyclesBetween, Particle_Type outputType)
        {
            position = pos;
            period = cyclesBetween;
            type = outputType;
            counter = 0;
        }

        public bool isTimeForNewParticle()
        {
            counter++;
            if (counter < period)
                return false;
            counter = 0;
            return true;
        }

        public Vector2 getNewParticlePosition()
        {
            return new Vector2(position.X + radius * ((float)rnd.NextDouble() - .5f), position.Y + radius * ((float)rnd.NextDouble()) - .5f);
        }
    }
}
