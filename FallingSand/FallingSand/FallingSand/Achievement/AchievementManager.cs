using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FallingSand.Achievement
{
    public class AchievementManager
    {
        public List<AchievementBase> achievements;
        private List<AchievementNotification> notifications;
        private int displayTime = 5000;
        private Vector2 origin = new Vector2(0, 0);
        private Vector2 textSize;
        private const string message = "Achievement unlocked:";

        public void processParticle(Particles.Particle p)
        {
            foreach(var a in achievements)
            {
                a.processParticle(p);
            }
        }
        public AchievementManager()
        {
            achievements = new List<AchievementBase>();
            notifications = new List<AchievementNotification>();
        }
        public AchievementManager(List<AchievementBase> achievements)
        {
            notifications = new List<AchievementNotification>();
            this.achievements = achievements;
        }
        public void Init()
        {
            textSize = FSGGame.Font.MeasureString(message);
        }

        public void Attach(AchievementBase ach)
        {
            achievements.Add(ach);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var a in achievements)
            {
                if (a.Update())
                {
                    var notification = a.GetNotification();
                    notifications.Add(notification);
                }
            }
            foreach (var n in notifications)
            {
                n.Update(gameTime);
            }
            notifications.RemoveAll(n => n.Counter > displayTime);
        }

        public void Draw()
        {
            float x = 0;
            float y = 0;
            Vector2 temp = new Vector2(x, y);
            Vector2 temp2 = new Vector2(5, 5); //padding\
            

            if (notifications.Count > 0)
            {
                FSGGame.spriteBatch.Begin();
                
                
                FSGGame.spriteBatch.Draw(FSGGame.white, new Rectangle((int)temp.X, (int)temp.Y, (int)textSize.X, (int)textSize.Y), Color.LightGray);
                FSGGame.spriteBatch.DrawString(FSGGame.Font, message, temp + textSize/2f, Color.Black, 0, textSize / 2f, 1f, SpriteEffects.None, .8f);
                
                temp.Y = temp.Y + textSize.Y + temp2.Y;

                foreach (var n in notifications)
                { 
                    Vector2 msgSize = FSGGame.Font.MeasureString(n.Name);
                    
                    float scale = Math.Min(textSize.X / msgSize.X, 1f);
                    Vector2 position = new Vector2(temp.X + textSize.X / 2f * scale, temp.Y + textSize.Y / 2f);
                    FSGGame.spriteBatch.Draw(FSGGame.white, new Rectangle((int)temp.X, (int)temp.Y, (int)textSize.X, (int)textSize.Y), Color.LightGray);
                    FSGGame.spriteBatch.DrawString(FSGGame.Font, n.Name, position, Color.Black, 0, textSize / 2f, scale, SpriteEffects.None, .8f);
                    temp.Y = temp.Y + textSize.Y + temp2.Y;
                }

                FSGGame.spriteBatch.End();
            }
        }
    }
}
