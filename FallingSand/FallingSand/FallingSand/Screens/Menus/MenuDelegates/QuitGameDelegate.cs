using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FallingSand.Screens.Menus.MenuDelegates
{
    class QuitGameDelegate : IMenuDelegate
    {
        public QuitGameDelegate() : base() { }

        public void Run()
        {
            FSGGame.ExitStatus = true;
        }
    }
}
