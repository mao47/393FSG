using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FallingSand.Particles;
using Microsoft.Xna.Framework;

namespace TestProject.ParticleSystemTests
{
    [TestFixture]
    public class ParticleTests
    {
        [Test]
        public void TestGetColor()
        {
            var sand = new Particle(Vector2.Zero, Vector2.Zero, Particle_Type.Sand);
            var wall = new Particle(Vector2.Zero, Vector2.Zero, Particle_Type.Wall);

            var sandc = sand.getColor();
            var wallc = sand.getColor();

            Assert.AreNotEqual(sandc, Color.White);
            Assert.AreNotEqual(wallc, Color.White);
        }
    }
}
