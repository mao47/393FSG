using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace FallingSand.GameElements
{
    public class InputState
    {
        private KeyboardState state;

        public InputState(KeyboardState state)
        {
            this.state = state;
        }
        
        
        //returns true if enter is pressed
        //for use by menus
        public bool Enter()
        {
            if (state.IsKeyDown(Keys.Enter))
                return true;
            else
                return false;
        }

        //returns true if back is pressed for menus
        public bool Back()
        {
            if (state.IsKeyDown(Keys.Escape) || state.IsKeyDown(Keys.Back))
                return true;
            else
                return false;
        }

        public bool PauseGame()
        {
            if (state.IsKeyDown(Keys.Escape))
                return true;
            else
                return false;

        }

        public bool Down()
        {
            if (state.IsKeyDown(Keys.Down))
                return true;
            return false;
        }

        public bool Up()
        {
            if (state.IsKeyDown(Keys.Up))
                return true;
            return false;
        }
    }
}
