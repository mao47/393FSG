using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FallingSand.Particles
{
    public class Particle_Wall : Particle
    {
        public Particle_Wall (Vector2 pos, Vector2 vel)
            : base(pos, vel, Particle_Type.Wall, Color.Gray)
        {        }

        public override void Update(ParticleDataStructure particleStorage)
        {
            base.Update();
        }
    }
}
