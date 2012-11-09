using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FallingSand.Particles
{
    public enum Particle_Type{
        Sand,
        Wall
    }
    public class Particle
    {
        public Vector2 position;
        public Vector2 velocity;
        public Particle_Type type;
        private Color pColor;

        public Particle (Vector2 pos, Vector2 vel, Particle_Type ptype)
        {
            position = pos;
            velocity = vel;
            type = ptype;
            if (ptype == Particle_Type.Sand)
                pColor = Color.SandyBrown;
            else if (type == Particle_Type.Wall)
                pColor = Color.Gray;
            else
                pColor = Color.White;
        }

        public Color getColor()
        {
            return pColor;
        }

        public void setColor(Color collColor)//only for collision detection
        {
            pColor = collColor;
        }
    }
}
