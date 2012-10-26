using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FallingSand.Screens.Menus.MenuDelegates;
using FallingSand.GameElements;
using Microsoft.Xna.Framework.Graphics;

namespace FallingSand.Screens.Menus
{
    public class MenuEntry
    {
        /// <summary>
        /// This is the delegate holding the action or set of actions to be performed.
        /// </summary>
        private IMenuDelegate menuDelegate;

        private Color textColor;
        private OscillatingFloat oscScale;

        /// <summary>
        /// Gets the text to display for this entry.
        /// </summary>
        /// <value>The text to display.</value>
        public string text { get; set; }

        /// <summary>
        /// Gets or sets the upper menu, which is the menu entry that will
        /// be highlighted if this menu entry is highlighted and the player
        /// presses Up. If this is null, then no action will be taken.
        /// </summary>
        /// <value>The upper menu.</value>
        public MenuEntry UpperMenu { get; set; }

        /// <summary>
        /// Gets or sets the lower menu, which is the menu entry that will
        /// be highlighted if this menu entry is highlighted and the player
        /// presses Down. If this is null, then no action will be taken.
        /// </summary>
        /// <value>The lower menu.</value>
        public MenuEntry LowerMenu { get; set; }

        /// <summary>
        /// Gets the position of the menu entry.
        /// </summary>
        /// <value>The position.</value>
        public Vector2 position { get; private set; }

        /// <summary>
        /// Rectanglular size of text (at default size and 0 rotation)
        /// </summary>
        public Vector2 textSize { get; private set; }

        public MenuEntry(string text, Vector2 position, IMenuDelegate menuDelegate)
        {
            this.menuDelegate = menuDelegate;
            this.text = text;
            this.position = position;
            this.textColor = Color.White;
            this.textSize = FSGGame.Font.MeasureString(this.text);
            oscScale = new OscillatingFloat(1, (float).15, (float).5);
        }

        /// <summary>
        /// Tries to run the delegate. This should be called when the delegate is
        /// highlighted. If so, the delegated action is performed.
        /// </summary>
        public void TryRunDelegate()
        {
            //if (Controls.GetInput().Enter())
            if(FSGGame.controller.ContainsBool(Inputs.ActionType.Select))
            {
                this.menuDelegate.Run();
            }
            

        }

        public virtual void Update(GameTime gameTime, bool highlighted)
        {
            if (highlighted)
            {
                //show that its highlighted
                this.textColor = Color.Black;
                oscScale.Update(gameTime);
            }
            else
            {
                if (Math.Abs(oscScale.Value - oscScale.Middle) > .01)
                    oscScale.Update(gameTime);

                this.textColor = Color.White;
            }
        }

        public virtual void Draw()
        {
            FSGGame.spriteBatch.Begin();
            FSGGame.spriteBatch.DrawString(FSGGame.Font, this.text, this.position, textColor, 0, this.textSize/2f, oscScale.Value, SpriteEffects.None, 0);
            FSGGame.spriteBatch.End();
        }
    }
}
