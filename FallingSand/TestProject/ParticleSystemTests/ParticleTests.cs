using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FallingSand.Particles;
using Microsoft.Xna.Framework;
using FallingSand;
using FallingSand.Inputs;
using FallingSand.Screens;
using TestProject.TestMockObjects;

namespace TestProject.ParticleSystemTests
{
    [TestFixture]
    public class ParticleTests
    {

        /// <summary>
        /// Test getColor method functions
        /// </summary>
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

        /// <summary>
        /// Test particles will be added only when within bounds of array
        /// </summary>
        [Test]
        public void TestAddParticle()
        {

            var pm = new MAOParticleManager(new Rectangle(0, 0, 800, 400),  100000, 1);

            var add1 = pm.addParticle(new Vector2(10f), Vector2.Zero, Particle_Type.Sand);
            var add2 = pm.addParticle(new Vector2(-10f), Vector2.Zero, Particle_Type.Sand);

            Assert.IsTrue(add1);
            Assert.IsFalse(add2);

        }

        /// <summary>
        /// Test that when "A" button is held, particles are continuously added to screen
        /// </summary>
        [Test]
        public void TestHoldPlaceParticle()
        {
            var pm = new FakeMAOParticleManager(new Rectangle(0, 0, 800, 400), 100000, 1);
            int lastCount = 0;
            int lastAdd = 0;

            FSGGame.controller = new FakeController();
            var c = FSGGame.controller as FakeController;
            c.mousex = 50;
            c.mousey = 50;
            c.a = true;
            GameTime gt = new GameTime(new TimeSpan(0, 0, 0), new TimeSpan(0, 0, 0, 0, 100)); // one millisecond


            var screen = new ParticleTestScreen(new ScreenContainer());
            screen.pm = pm;
            for (int i = 0; i < 5; i++)
            {
                //create some movement to avoid clogging the screen
                c.mousex = 50 + 5*i;
                c.mousey = 50 + i;
                lastAdd = pm.numberParticlesToAdd();
                lastCount = pm.numberParticles();
                screen.Update(gt);
                Assert.Greater(pm.numberParticlesToAdd(), 0);
                /*
                 * total particles should be less than or equal the amount contained + added in the
                 * last step. (there may be overlap in random placement or particles drifting
                 * offscreen to decrease below this number)
                 */
                Assert.LessOrEqual( pm.numberParticles(),lastAdd+lastCount);

            }
        }

        [Test]
        public void TestPlaceParticle()
        {
            var pm = new FakeMAOParticleManager(new Rectangle(0, 0, 800, 400), 100000, 1);

            FSGGame.controller = new FakeController();
            var c = FSGGame.controller as FakeController;
            c.mousex = 50;
            c.mousey = 50;
            c.a = true;
            GameTime gt = new GameTime(new TimeSpan(0, 0, 0), new TimeSpan(0,0,0,0,100)); // one millisecond

            
            var screen = new ParticleTestScreen(new ScreenContainer());
            screen.pm = pm;
            screen.Update(gt);  //update to put particles on add list
            Assert.Greater(pm.numberParticlesToAdd(), 0);
            c.a = false;
            screen.Update(gt);  //update to put particles in data structure
            //at least one particle was added (can be more from brush)
            Assert.Greater(pm.numberParticles(), 0);
            Assert.AreEqual(0, pm.numberParticlesToAdd());
        }

        [Test]

        public void TestParticleSizeChange()
        {
            var pm = new FakeMAOParticleManager(new Rectangle(0, 0, 800, 400), 100000, 1);

            FSGGame.controller = new FakeController();
            var c = FSGGame.controller as FakeController;
            c.selectleft = true;
            GameTime gt = new GameTime(new TimeSpan(0, 0, 0), new TimeSpan(0, 0, 0, 0, 100)); // one millisecond


            var screen = new ParticleTestScreen(new ScreenContainer());
            screen.pm = pm;
            screen.Update(gt);

            //successfully changed to wall
            Assert.AreEqual(screen.currentParticle, Particle_Type.Wall);

            screen.Update(gt);
            //changed back to sand since left is down again
            Assert.AreEqual(screen.currentParticle, Particle_Type.Sand);
        }

        [Test]

        public void TestChangeParticleType()
        {
            var pm = new FakeMAOParticleManager(new Rectangle(0, 0, 800, 400), 100000, 1);

            FSGGame.controller = new FakeController();
            var c = FSGGame.controller as FakeController;
            c.selectup = true;
            GameTime gt = new GameTime(new TimeSpan(0, 0, 0), new TimeSpan(0, 0, 0, 0, 100)); // one millisecond


            var screen = new ParticleTestScreen(new ScreenContainer());
            screen.pm = pm;
            screen.Update(gt);

            //at least one particle was added (can be more from brush)
            Assert.Greater(screen.brushSize, 3);
            c.selectup = false;
            c.selectdown = true;
            screen.Update(gt);
            Assert.AreEqual(screen.brushSize, 3);

        }
        
    }


    
}
