using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Bullet_Rebound
{

    class MouseEventArgs : EventArgs
    {
        public readonly MouseState CurrentState;
        public readonly MouseState PrevState;
        public readonly MouseButton Button;

        public MouseEventArgs(MouseButton button, MouseState currentState, MouseState prevState)
        {
            CurrentState = currentState;
            PrevState = prevState;
            Button = button;
        }

    }

    public enum MouseButton
    {
        NONE = 0X00,
        LEFT = 0X01,
        RIGHT = 0X02,
        MIDDLE = 0X04,
        XBUTTON1 = 0X08,
        XBUTTON2 = 0X10,
    }
}
   

           

