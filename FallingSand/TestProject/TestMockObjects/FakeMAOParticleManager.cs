using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FallingSand.Particles;
using Microsoft.Xna.Framework;

namespace TestProject.TestMockObjects
{
    public class FakeMAOParticleManager : MAOParticleManager
    {
        public FakeMAOParticleManager(Rectangle bounds, int max, int particleSize)
            : base(bounds, max, particleSize, (p) => { })
        { }
        public int numberParticles()
        {
            return particleStorage.particleCount();
        }
        public int numberParticlesToAdd()
        {
            return particleStorage.particleAddCount();
        }
        
    }
}
