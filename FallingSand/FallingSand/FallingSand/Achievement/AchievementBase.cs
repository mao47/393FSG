using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FallingSand.Achievement
{
    public abstract class AchievementBase
    {
        public bool Unlocked { get; protected set; }
        public abstract void processParticle(Particles.Particle p);
        public String name { get; protected set; }
        public AchievementBase()
        {
            Unlocked = false;
        }
        public abstract bool Update();
        public abstract AchievementNotification GetNotification();
    }
}
