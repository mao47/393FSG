﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FallingSand.Screens.Menus.MenuDelegates;

namespace FallingSand.Screens.Menus
{
    class TitleMenu : Menu
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TitleMenu"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="spacing">The spacing.</param>
        public TitleMenu(Vector2 position, float spacing)
            : base(position)
        {
            MenuEntry resume = new MenuEntry("Start", position, new StartDelegate());

           // MenuEntry howto = new MenuEntry("How to Play", position + new Vector2(0, spacing), new HowToPlayDelegate());

           // MenuEntry toSettings = new MenuEntry("Settings", position + new Vector2(0, spacing * 2), new SettingsDelegate());
            
            MenuEntry quit = new MenuEntry("Quit", position + new Vector2(0, spacing * 3), new QuitGameDelegate());

            resume.UpperMenu = quit;
            resume.LowerMenu = quit;//howto;

            //howto.UpperMenu = resume;
            //howto.LowerMenu = toSettings;

            //toSettings.UpperMenu = howto;
            //toSettings.LowerMenu = quit;

            quit.UpperMenu = resume;// toSettings;
            quit.LowerMenu = resume;

            this.Add(resume);
            //this.Add(howto);
            //this.Add(toSettings);
            this.Add(quit);
        }
    }
}
