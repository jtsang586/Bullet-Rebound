using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Bullet_Rebound
{
    class InputListener
    {
        // Current and previous keyboard states
        private KeyboardState PrevKeyboardState { get; set; }
        private KeyboardState CurrentKeyboardState { get; set; }
        private MouseState PrevMouseState { get; set; }
        private MouseState CurrentMouseState { get; set; }

        // List of keys to check for
        public HashSet<Keys> KeyList;
        public HashSet<MouseButton> ButtonList;

        //Keyboard event handlers
        //key is down
        public event EventHandler<KeyboardEventArgs> OnKeyDown = delegate { };
        //key was up and is now down
        public event EventHandler<KeyboardEventArgs> OnKeyPressed = delegate { };
        //key was down and is now up
        public event EventHandler<KeyboardEventArgs> OnKeyUp = delegate { };

        //Mouse event handlers
        public event EventHandler<MouseEventArgs> OnMouseButtonDown = delegate { };

        public InputListener()
        {
            CurrentKeyboardState = Keyboard.GetState();
            PrevKeyboardState = CurrentKeyboardState;

            CurrentMouseState = Mouse.GetState();
            PrevMouseState = CurrentMouseState;

            KeyList = new HashSet<Keys>();
            ButtonList = new HashSet<MouseButton>();
        }

        public void AddButton(MouseButton button)
        {
            ButtonList.Add(button);
        }

        public void AddKey(Keys key)
        {
            KeyList.Add(key);
        }

        public void Update()
        {
            PrevKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();

            PrevMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetState();
            FireKeyboardEvents();
            FireMouseEvents();
        }

        private void FireKeyboardEvents()
        {
            // Check through each key in the key list
            foreach (Keys key in KeyList)
            {
                // Is the key currently down?
                if (CurrentKeyboardState.IsKeyDown(key))
                {
                    // Fire the OnKeyDown event
                    if (OnKeyDown != null)
                        OnKeyDown(this, new KeyboardEventArgs(key, CurrentKeyboardState, PrevKeyboardState));
                }

                // Has the key been released? (Was down and is now up)
                if (PrevKeyboardState.IsKeyDown(key) && CurrentKeyboardState.IsKeyUp(key))
                {
                    // Fire the OnKeyUp event
                    if (OnKeyUp != null)
                        OnKeyUp(this, new KeyboardEventArgs(key, CurrentKeyboardState, PrevKeyboardState));
                }

                // Is the key pressed (was up and is now down)
                if (PrevKeyboardState.IsKeyUp(key) && CurrentKeyboardState.IsKeyDown(key))
                {
                    // Fire the OnKeyPressed event
                    if (OnKeyPressed != null)
                        OnKeyPressed(this, new KeyboardEventArgs(key, CurrentKeyboardState, PrevKeyboardState));
                }
            }

        }

        private void FireMouseEvents()
        {
            // Check through each key in the key list
            foreach (MouseButton button in ButtonList)
            {
                if (button == MouseButton.LEFT)
                {
                    // Is the left mouse button currently down?
                    if (CurrentMouseState.LeftButton == ButtonState.Pressed)
                    {
                        // Fire the OnMouseButtonDown event
                        if (OnMouseButtonDown != null)
                            OnMouseButtonDown(this, new MouseEventArgs(button, CurrentMouseState, PrevMouseState));
                    }
                }
            }

        }

    }
}
