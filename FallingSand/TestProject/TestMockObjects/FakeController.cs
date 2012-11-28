using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FallingSand.Inputs;

namespace TestProject.TestMockObjects
{
    public class FakeController : FallingSand.Inputs.Controller {

        public FakeController() 
            : base(PlayerIndex.One) 
        { }
        
        public float mousex { private get; set; }
        public float mousey { private get; set; }
        public bool a { private get; set; }
        public bool b{ private get; set; }
        public bool select{ private get; set; }
        public bool back{ private get; set; }
        public bool mouseleft{ private get; set; }
        public bool mouseright{ private get; set; }
        public bool selectleft{ private get; set; } 
        public bool selectright{ private get; set; }
        public bool selectup{ private get; set; }
        public bool selectdown{ private get; set; }

        public override float ContainsFloat(ActionType action)
        {
            return 1;
        }
        public override bool ContainsBool(ActionType action)
        {
            switch (action)
            {
                case ActionType.AButton: return a;
                case ActionType.SelectionLeft: return selectleft;
                case ActionType.SelectionDown: return selectdown;
                case ActionType.SelectionUp: return selectup;
            }
            return false;
        }
        public override Vector2 CursorPosition()
        {
            return new Vector2(mousex, mousey);
        }

    }
}
