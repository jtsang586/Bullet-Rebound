using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Bullet_Rebound
{
    public delegate void GameAction (eButtonState buttonState, Vector2 amount);

    class CommandManager
    {
        private InputListener m_Input;

        private Dictionary<Keys, GameAction> m_KeyBindings = new Dictionary<Keys, GameAction>();

        public CommandManager()
        {
            m_Input = new InputListener();

            m_Input.OnKeyDown += this.OnKeyDown;
        }

        public void Update()
        {
            m_Input.Update();
        }

        public void OnKeyDown(object sender, KeyboardEventArgs e)
        {
            GameAction action = m_KeyBindings[e.Key];
            if (action != null)
            {
                action(eButtonState.DOWN, new Vector2(1, 0));
            }
        }

        public void AddKeyboardBinding (Keys key, GameAction action)
        {
            m_Input.AddKey(key);

            m_KeyBindings.Add(key, action);
        }
    }
}
