using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Bullet_Rebound
{
    public class KeyboardEventArgs : EventArgs
    { 
        public readonly KeyboardState CurrentState;
        public readonly KeyboardState PrevState;
        public readonly Keys Key;


        public KeyboardEventArgs(Keys key, KeyboardState currentKeyboardState, KeyboardState prevKeyboardState)
        {
            CurrentState = currentKeyboardState;
                PrevState = prevKeyboardState;
            Key = key;
        }
       
    }
}
