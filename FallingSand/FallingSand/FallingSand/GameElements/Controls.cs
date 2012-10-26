using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace FallingSand.GameElements
{
    /// <summary>
    /// OLD
    /// </summary>
    public class Controls
    {
        public static InputState GetInput()
        {
            var State = Keyboard.GetState();
            return new InputState(State);
        }
    }
}
