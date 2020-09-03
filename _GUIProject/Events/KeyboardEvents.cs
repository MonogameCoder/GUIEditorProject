using _GUIProject.UI;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _GUIProject.Events
{
    public class KeyboardArgs : EventArgs
    {
        public UIObject Owner { get; set; }
        public KeyboardState KeyboardState { get; set; }
        public Action Instruction { get; set; }
        public KeyboardArgs(UIObject sender, KeyboardState state, Action action)
        {
            Owner = sender;
            KeyboardState = state;
            Instruction = action;
        }
    }

    public class KeyboardEvents
    {
        public UIObject eventOwner;

        public event EventHandler<KeyboardArgs> onKeyPressed;
        public event EventHandler<KeyboardArgs> onKeyReleased;

        public KeyboardEvents(UIObject owner)
        {
            eventOwner = owner;
        }

        public void Pressed()
        {
            if (onKeyPressed != null)
            {
                onKeyPressed(this, onKeyPressed.Target as KeyboardArgs);
            }
        }
        public void Released()
        {
            if (onKeyReleased != null)
            {
                onKeyReleased(this, onKeyReleased.Target as KeyboardArgs);
            }

        }

    }
}
