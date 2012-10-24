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
        }

    }
}
