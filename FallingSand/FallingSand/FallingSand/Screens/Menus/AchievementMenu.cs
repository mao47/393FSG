using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FallingSand.Screens.Menus.MenuDelegates;

namespace FallingSand.Screens.Menus
{
    class AchievementMenu : Menu
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TitleMenu"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="spacing">The spacing.</param>
        public AchievementMenu(Vector2 position, float spacing)
            : base(position)
        {
            MenuEntry titleReturn = new MenuEntry("Return", position + new Vector2(0, spacing * 4), new MenuReturnDelegate());
            titleReturn.UpperMenu = titleReturn;
            titleReturn.LowerMenu = titleReturn;
            this.Add(titleReturn);
        }
    }
}
