using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FallingSand.Achievement
{
    public class DemoAchievement : AchievementBase
    {
        private int numUpdatesToWait;
        private int counter;
        public DemoAchievement() 
            : base() 
        {
            counter = 0;
            numUpdatesToWait = 30 * 1000 / 40;
            name = "Demo Achievement";
        }
        public override void processParticle(Particles.Particle p)
        {
            
            if (p.type == Particles.Particle_Type.Fire)
            {
                counter++;
            }
        }
        public override bool Update()
        {
            if (Unlocked) return false;

            if (counter >= numUpdatesToWait)
            {
                Unlocked = true;
                return true;
            }
            counter = 0;
            return false;
        }
        public override AchievementNotification GetNotification()
        {
            return new AchievementNotification { Name = "Up in Flames", Message = "Make a fire larger than 1000 particles" };
        }
    }
}
