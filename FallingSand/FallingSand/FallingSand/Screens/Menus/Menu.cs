﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FallingSand.GameElements;
using FallingSand.Inputs;

namespace FallingSand.Screens.Menus
{
    public class Menu : List<MenuEntry>
    {
        /// <summary>
        /// Gets the currently selected menu entry. This is the index
        /// of the menu entry that is highlighted.
        /// </summary>
        /// <value>The currently highlighted menu entry.</value>
        public int CurrentSelected { get; private set; }

        /// <summary>
        /// This is the time (DateTime, not GameClock) 
        /// that the screen is created.
        /// </summary>
        private long initialTime;

        /// <summary>
        /// This is the time of the last movement
        /// </summary>
        private long lastMove;


        /// <summary>
        /// This is the duration of the screen
        /// </summary>
        private long duration;

        public bool CanMoveAgain;

        /// <summary>
        /// The base this.position of the entire set of menu entries.
        /// </summary>
        /// <value>The position.</value>
        public Vector2 position { get; private set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="Menu"/> class.
        /// </summary>
        /// <param name="position">The this.position of the menu system.</param>
        public Menu(Vector2 position)
        {
            this.CanMoveAgain = false;
            this.position = new Vector2(position.X, position.Y);
            this.CurrentSelected = 0;
            // Note: Do not use GameClock, it will be paused!
            this.initialTime = DateTime.Now.Ticks;
            duration = 2500000;
            this.lastMove = initialTime + duration;

        }

        /// <summary>
        /// If the associated menu entry is not null, and
        /// exists in the list, the index of the currently 
        /// selected menu entry is changed to the index
        /// of the specified entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        private void TrySet(MenuEntry entry)
        {
            if (entry != null)
            {
                int temp = this.CurrentSelected;
                this.CurrentSelected = this.IndexOf(entry);

                if (this.CurrentSelected == -1)
                {
                    this.CurrentSelected = temp;
                }
            }
        }


        /// <summary>
        /// Updates this instance. This checks to see if the user has made an
        /// up or down selection, and if so, it tries to change the
        /// highlighted menu entry accordingly.
        /// </summary>
        public virtual void Update(GameTime gameTime)
        {
            
            if (FSGGame.controller.ContainsBool(ActionType.SelectionUp))
            {
                
                this.TrySet(this[this.CurrentSelected].UpperMenu);
            }

            else if (FSGGame.controller.ContainsBool(ActionType.SelectionDown))
            {
                this.TrySet(this[this.CurrentSelected].LowerMenu);
            }
            else //if (FSGGame.controller.ContainsBool(ActionType.MouseMoved))
            {
                Vector2 cursor = FSGGame.controller.CursorPosition();
                for (int i = 0; i < this.Count; i++)
                {
                    if (this.CurrentSelected != i)
                    {
                        Vector2 size = this[i].textSize / 2f;
                        Vector2 topLeft = this[i].position - size;
                        Vector2 bottomRight = this[i].position + size;
                        if (topLeft.X < cursor.X && bottomRight.X > cursor.X
                            && topLeft.Y < cursor.Y && bottomRight.Y > cursor.Y)
                        {
                            CurrentSelected = i;
                            break;
                        }
                    }
                }
            }


            //update newly highlighted option, and make sure others are not highlighted
            for (int i = 0; i < this.Count; i++)
            {
                if (this.CurrentSelected == i)
                {
                    this[this.CurrentSelected].Update(gameTime, true);
                        this[this.CurrentSelected].TryRunDelegate();
                }
                else
                    this[i].Update(gameTime, false);
            }

        }

        /// <summary>
        /// Draws each menu entry in the set.
        /// </summary>
        public virtual void Draw()
        {
            foreach (MenuEntry entry in this)
            {
                entry.Draw();
            }
        }
    }
}
