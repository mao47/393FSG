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
        int period, counter;
        public Particle_Type type;

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
    }
}
