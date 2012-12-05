using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FallingSand.Particles;

namespace FallingSand.Achievement.Achievements
{
    /// <summary>
    /// Actually this won't work since particles stay on screen. skipping for now
    /// </summary>
    public class TotalOfTypeAchievement : AchievementBase
    {
        int numReq;
        int count;
        string name, message;
        Particle_Type type;

        public TotalOfTypeAchievement(int numRequired, Particle_Type type, string name, string message)
            :base()
        {
            this.name = name;
            this.message = message;
            this.numReq = numRequired;
            this.type = type;
            count = 0;
        }
        public override void processParticle(Particles.Particle p)
        {
            if (p.type == type && !Unlocked)
            {
                count++;
            }
        }

        public override bool Update()
        {
            if (Unlocked) return false;
            if (count > numReq)
            {
                Unlocked = true;
                return true;
            }
            return false;
        }

        public override AchievementNotification GetNotification()
        {
            return new AchievementNotification { Name = name, Message = message };
        }
    }
}
