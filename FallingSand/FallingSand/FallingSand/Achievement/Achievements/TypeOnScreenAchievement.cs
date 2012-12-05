using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FallingSand.Particles;

namespace FallingSand.Achievement.Achievements
{
    public class TypeOnScreenAchievement : AchievementBase
    {
        private int numReq;
        private int counter;
        //private string name;
        private string message;
        private Particle_Type type;
        public TypeOnScreenAchievement(int numRequired, Particle_Type type, string name, string message) 
            : base() 
        {
            counter = 0;
            numReq = numRequired;
            this.name = name;
            this.message = message;
            this.type = type;
        }
        public override void processParticle(Particles.Particle p)
        {
            if (p.type == this.type && !Unlocked)
            {
                counter++;
            }
        }
        public override bool Update()
        {
            if (Unlocked) return false;

            if (counter >= numReq)
            {
                Unlocked = true;
                return true;
            }
            counter = 0;
            return false;
        }
        public override AchievementNotification GetNotification()
        {
            return new AchievementNotification { Name = this.name, Message = this.message };
        }
    }
}
