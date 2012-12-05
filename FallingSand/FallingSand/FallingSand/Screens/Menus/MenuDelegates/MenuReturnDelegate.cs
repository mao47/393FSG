using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FallingSand.Screens.Menus.MenuDelegates
{
    class MenuReturnDelegate : IMenuDelegate
    {
        public MenuReturnDelegate()
            : base()
        {

        }

        public void Run()
        {
            //hack a test screen together. not sure if a better way to do this.
            // ill look into it. -mao
            if (FSGGame.screens.Count > 0)
            {
                FSGGame.screens[FSGGame.screens.Count - 1].Disposed = true;
            }

            FSGGame.screens.Play(new TitleScreen(FSGGame.screens)); // The number passed here must be unique per game.
        }

    }
}
