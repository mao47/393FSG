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


        [Test]
        public void TestAddParticle()
        {

            var pm = new MAOParticleManager(new Rectangle(0, 0, 800, 400),  100000, 1);

            var add1 = pm.addParticle(new Vector2(10f), Vector2.Zero, Particle_Type.Sand);
            var add2 = pm.addParticle(new Vector2(-10f), Vector2.Zero, Particle_Type.Sand);

            Assert.IsTrue(add1);
            Assert.IsFalse(add2);

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
            GameTime gt = new GameTime(new TimeSpan(0, 0, 0), new TimeSpan(0,0,0,0,1)); // one millisecond

            
            var screen = new ParticleTestScreen(new ScreenContainer());
            screen.pm = pm;
            screen.Update(gt);  //update to put particles on add list
            screen.Update(gt);  //update to put particles in data structure
            
            //at least one particle was added (can be more from brush)
            Assert.Greater(pm.numberParticles(), 0);
            
        }

        [Test]

        public void TestParticleSizeChange()
        {
            var pm = new FakeMAOParticleManager(new Rectangle(0, 0, 800, 400), 100000, 1);

            FSGGame.controller = new FakeController();
            var c = FSGGame.controller as FakeController;
            c.selectleft = true;
            GameTime gt = new GameTime(new TimeSpan(0, 0, 0), new TimeSpan(0, 0, 0, 0, 1)); // one millisecond


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
            GameTime gt = new GameTime(new TimeSpan(0, 0, 0), new TimeSpan(0, 0, 0, 0, 1)); // one millisecond


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

    public class FakeMAOParticleManager : MAOParticleManager
    {
        public FakeMAOParticleManager(Rectangle bounds, int max, int particleSize)
            : base(bounds, max, particleSize)
        { }
        public int numberParticles() { return particleStorage.particleCount(); }
        
    }
    public class FakeController : FallingSand.Inputs.Controller {

        public FakeController() : base(PlayerIndex.One) 
        { }
        public float mousex { private get; set; }
        public float mousey { private get; set; }
        public bool a { private get; set; }
        public bool b{ private get; set; }
        public bool select{ private get; set; }
        public bool back{ private get; set; }
        public bool mouseleft{ private get; set; }
        public bool mouseright{ private get; set; }
        public bool selectleft{ private get; set; } 
        public bool selectright{ private get; set; }
        public bool selectup{ private get; set; }
        public bool selectdown{ private get; set; }

        public override float  ContainsFloat(ActionType action ) { return 1; }
        public override bool ContainsBool(ActionType action)
        {
            switch (action)
            {
                case ActionType.AButton: return a;
                case ActionType.SelectionLeft: return selectleft;
                case ActionType.SelectionDown: return selectdown;
                case ActionType.SelectionUp: return selectup;
            }
            return false;
        }
        public override Vector2 CursorPosition() { return new Vector2(mousex, mousey); }

    }
}
