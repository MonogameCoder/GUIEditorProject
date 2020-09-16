using System.Collections.Generic;

namespace _GUIProject.UI
{
    public class TextBoxGroup
    {
        private readonly List<Sprite> _itemList;

        public TextBoxGroup()
        {
            _itemList = new List<Sprite>();
        }
        public void Add(TextBox checkbox)
        {
            _itemList.Add(checkbox);
        }
       
        public void SetSelected(Sprite currentBox)
        {
            foreach (TextBox item in _itemList)
            {
                if (item == currentBox)
                    item.Selected = true;
                else
                    item.Selected = false;
            }
        }
        public List<Sprite> Items
        {
            get { return _itemList; }
        }
   
    }
}
