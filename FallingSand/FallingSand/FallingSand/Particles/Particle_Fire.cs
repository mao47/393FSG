using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FallingSand.Particles
{
    public class Particle_Fire : Particle
    {
        private int roundsLeft;
        public Particle_Fire (Vector2 pos, Vector2 vel)
            : base(pos, vel, Particle_Type.Fire, Color.Firebrick)
        {
            roundsLeft = Constants.FireDuration;//hardcodded for now
        }

        public override void Update(ParticleDataStructure particleStorage)
        {
            base.Update(particleStorage);
            if (roundsLeft > 0)
                roundsLeft--;
            else
                remove = true;
        }
    }
}
