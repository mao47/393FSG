using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FallingSand.Achievement
{
    public class AchievementNotification
    {
        public string Message { get; set; }
        public string Name { get; set; }

        public int Counter;

        public AchievementNotification()
        {
            Counter = 0;
        }
        public void Update(GameTime gametime)
        {
            Counter += gametime.ElapsedGameTime.Milliseconds;
        }
    }
}
