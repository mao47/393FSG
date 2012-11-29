using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FallingSand.Particles
{
    public class Particle_Water : Particle
    {
        public Particle_Water(Vector2 pos, Vector2 vel)
            : base(pos, vel, Particle_Type.Water, Color.Aqua)
        { }

        public override void Update()
        {
            base.Update();
        }
    }
}

