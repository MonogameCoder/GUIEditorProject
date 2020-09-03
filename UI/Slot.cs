using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static _GUIProject.UI.UIObject;

namespace _GUIProject.UI
{
    public class Slot<T> :  IComparable
    {
        DrawPriority Priority { get; set; }
        public Point Position { get; set; }    
        public T Item { get; set; }
        public long Index { get; private set; }
        public static long GlobalIndex { get; private set; } = 0;
    
        public Slot(Point position, T item, DrawPriority priority)
        {
            Item = item;
            Position = position;
            Priority = priority;
           
            Index = GlobalIndex;
            GlobalIndex++;
        }
     
        public int CompareTo(object other)
        {
            int result = Priority.CompareTo((other as Slot<UIObject>).Priority);

            if (result == 0)
            {
                result = (Item as UIObject).Priority.CompareTo((other as Slot<UIObject>).Item.Priority);                    
            }
            if (result == 0)
            {
                result = Index.CompareTo((other as Slot<UIObject>).Index);
            }
            // This should never be true, but its there anyways
            if (result == 0)
            {
                result = GetHashCode().CompareTo(other.GetHashCode());
            }

            return result;
        }
    }
}
