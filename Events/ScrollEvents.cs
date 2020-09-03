using _GUIProject.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using static _GUIProject.UI.ScrollBar;

namespace _GUIProject.Events
{
    public class ScrollEventArgs : EventArgs
    {
        public UIObject Owner { get; set; }
        public int ScrollValue { get; set; }
        public ScrollDirection Direction { get; set; }
        public ScrollEventArgs(UIObject sender, ScrollDirection direction, int scrollValue)
        {
            Owner = sender;
            Direction = direction;
            ScrollValue = scrollValue;
        }
    }

    public class ScrollEvents
    {

        public UIObject eventOwner;

        public event EventHandler<ScrollEventArgs> onScroll;

        public ScrollEvents()
        {
        }
        public ScrollEvents(UIObject owner)
        {
            eventOwner = owner;

        }
        public void OnScroll(UIObject target, ScrollDirection direction, int scrollValue )
        {
            onScroll.Invoke(target,  new ScrollEventArgs(target,direction, scrollValue));
        }
    }
}
