using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace _GUIProject.UI
{
    public class CheckBoxGroup 
    {
        private readonly List<CheckBox> _itemList;
        public CheckBoxGroup()
        {
            _itemList = new List<CheckBox>();        
        }
        public void Add(CheckBox checkbox)
        {
            _itemList.Add(checkbox);
        }
        public void AddRange(params CheckBox[] items)
        {
            foreach (CheckBox item in items)
            {
                _itemList.Add(item);
            }
        }

    

        public UIObject HitTest(Point mousePosition)
        {
            CheckBox result = null;
            foreach (CheckBox item in _itemList)
            {
                result = item.HitTest(mousePosition) as CheckBox;
            }
            return result;
        }
      
        public void SetSelected(Button currentBox)
        {
            foreach (CheckBox item in _itemList)
            {
                if (item == currentBox)
                    item.Selected = true;
                else
                    item.Selected = false;
            }        
        }    
    }
}
