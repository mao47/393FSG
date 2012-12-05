using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FallingSand.Achievement.Achievements
{
    public class PyroAchievement : AchievementBase
    {
        int totalFireReq = Constants.FireDuration * Constants.PyroReq;
        int fireCount;

        public PyroAchievement()
            :base()
        {
            fireCount = 0;
            this.name = "Pyro";
        }
        public override void processParticle(Particles.Particle p)
        {
            if (p.type == Particles.Particle_Type.Fire)
            {
                fireCount++;
            }
        }

        public override bool Update()
        {
            if (Unlocked) return false;
            if (fireCount > totalFireReq)
            {
                Unlocked = true;
                return true;
            }
            return false;
        }

        public override AchievementNotification GetNotification()
        {
            return new AchievementNotification { Name = "Pyro", Message = "Create a million fire particles" };
        }
    }
}
