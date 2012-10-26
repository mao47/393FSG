using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FallingSand.Screens.Menus.MenuDelegates
{
    class StartDelegate : IMenuDelegate
    {
        public StartDelegate()
            : base()
        {

        }

        public void Run()
        {
            if (FSGGame.testColorIndex == FSGGame.testColors.Count)
                FSGGame.testColorIndex = 0;
            else
                FSGGame.testColorIndex++;


            //hack a test screen together. not sure if a better way to do this.
            // ill look into it. -mao
            if (FSGGame.screens.Count > 0)
            {
                FSGGame.screens[FSGGame.screens.Count - 1].Disposed = true;
            }

            FSGGame.screens.Play(new ParticleTestScreen(FSGGame.screens)); // The number passed here must be unique per game.
        }

    }
}
