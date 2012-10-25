﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FallingSand.GameElements.Particles
{
    public enum Particle_Type{
        Sand,
        Wall
    }

    struct Particle
    {
        public Vector2 position;
        public Vector2 direction;
        public float pointSize;
        public Particle_Type type;

        //From XNA book Chapter 14, unsure if needed.
        public static readonly VertexElement[] vertexElements =
        {
            new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position,0),
            new VertexElement(16, VertexElementFormat.Single, VertexElementUsage.PointSize,0),
        };
    }
}