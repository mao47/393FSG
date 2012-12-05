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
        Water,
        Wall,
        Plant,
        Fire,
        Remove
    }
    public abstract class Particle
    {
        public Vector2 position { get; set; }
        public Vector2 velocity {  get; set; }
        public Particle_Type type {  get; private set; }
        public Color pColor {  get; private set; }
        public bool lockedDirection { get; set; }
        public bool remove { get; set; }
        public bool Dead { get; set; }

        public Particle (Vector2 pos, Vector2 vel, Particle_Type ptype, Color partColor)
        {
            Dead = false;
            position = pos;
            velocity = vel;
            type = ptype;
            pColor = partColor;
            lockedDirection = false;
            remove = false;
        }

        public virtual void Update() //For particles that dont move
        {
        }

        public virtual void Update(ParticleDataStructure particleStorage) //For particles that move
        {
            Vector2 newPosition = position + velocity;
            particleStorage.moveParticle(this, (int)newPosition.X, (int)newPosition.Y);
        }
    }
}
