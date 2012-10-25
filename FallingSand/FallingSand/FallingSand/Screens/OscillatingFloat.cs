using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FallingSand.Screens
{
    class OscillatingFloat
    {
        public float Middle { get; set; }
        public float Value { get; set; }
        /// <summary>
        /// in seconds
        /// </summary>
        public float Period { get; set; }
        public float Deviation { get; set; }
        
        private double angle;

        public OscillatingFloat(float middle, float maxDeviation, float period)
        {
            Value = middle;
            Middle = middle;
            Period = period;
            Deviation = maxDeviation;
            angle = 0;
        }

        public void Update(GameTime gameTime)
        {
            double twopi = (2*Math.PI);
            angle = (angle % twopi) + gameTime.ElapsedGameTime.TotalSeconds*twopi/(double)Period;
            Value = (float)Math.Sin(angle) * Deviation + Middle;
           
        }
    }
}
