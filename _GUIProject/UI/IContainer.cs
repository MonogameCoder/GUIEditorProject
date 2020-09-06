using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

using static _GUIProject.UI.UIObject;

namespace _GUIProject.UI
{
   
    public interface IContainer :IObject
    {

        SortedSet<Slot<UIObject>> Slots { get; set; }
        Rectangle Rect { get; }
        Point Position { get; set; }       
        BasicSprite FrameBackground { get; set; }

        public Slot<UIObject> this[int index]
        {
            get { return Slots.ElementAt(index); }
        }
        public int Length
        {
            get { return Slots.Count; }
        }
        public static IContainer operator +(IContainer a, UIObject b)
        {
            for (int i = 0; i < a.Slots.Count; i++)
            {
                if (a.Slots.ElementAt(i).Item == b)
                {
                    Slot<UIObject> slot = a.Slots.ElementAt(i);
                    slot.Position = b.Position - a.Position;
                    break;
                }
            }
            return a;
        }
        Point SimulateInsert(Point dropLocation, UIObject item, DrawPriority priority = DrawPriority.HIGH);
        void AddItem(Point dropLocation, UIObject item, DrawPriority priority = DrawPriority.HIGH);
        void RemoveItem(UIObject item);
        void RemoveSlot(UIObject item);       
        bool Contains(UIObject item);
        bool Contains(Point position);    
        void UpdateSlot(UIObject item, Point newLocation);
        void UpdateLayout();
        void Show();
        void Hide();      
    }
}
