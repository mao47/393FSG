using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FallingSand.Inputs
{
    public class ControllerPacket
    {
        public bool SelectionUp;
        public bool SelectionDown;
        public bool SelectionLeft;
        public bool SelectionRight;
        public bool Select;
        public bool GoBack;
        public bool XButton;
        public bool YButton;
        public bool AButton;
        public bool BButton;
        public bool XButtonFirst;
        public bool YButtonFirst;
        public bool AButtonFirst;
        public bool BButtonFirst;
        public bool RightBumper;
        public bool LeftBumper;
        public bool RightBumperFirst;
        public bool LeftBumperFirst;
        public bool Pause;
        public bool DPadUp;
        public bool DPadDown;
        public bool DPadLeft;
        public bool DPadRight;
        public float MoveHorizontal;
        public float MoveVertical;
        public float LookHorizontal;
        public float LookVertical;
        public float RightTrigger;
        public float LeftTrigger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerPacket"/> class.
        /// </summary>
        public ControllerPacket()
        {
            this.Clear();
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            this.Select = false;
            this.SelectionDown = false;
            this.SelectionLeft = false;
            this.SelectionRight = false;
            this.SelectionUp = false;
            this.GoBack = false;
            this.XButton = false;
            this.YButton = false;
            this.AButton = false;
            this.BButton = false;
            this.XButtonFirst = false;
            this.YButtonFirst = false;
            this.AButtonFirst = false;
            this.BButtonFirst = false;
            this.RightBumper = false;
            this.LeftBumper = false;
            this.RightBumperFirst = false;
            this.LeftBumperFirst = false;
            this.Pause = false;
            this.DPadDown = false;
            this.DPadLeft = false;
            this.DPadRight = false;
            this.DPadUp = false;
            this.MoveHorizontal = 0;
            this.MoveVertical = 0;
            this.LookHorizontal = 0;
            this.LookVertical = 0;
            this.RightTrigger = 0;
            this.LeftTrigger = 0;
        }
    }
}
