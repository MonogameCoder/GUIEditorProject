using System.Collections.Generic;

namespace _GUIProject.UI
{
    public class TextBoxGroup
    {
        private List<BasicSprite> _itemList;

        public TextBoxGroup()
        {
            _itemList = new List<BasicSprite>();
        }
        public void Add(TextBox checkbox)
        {
            _itemList.Add(checkbox);
        }
       
        public void SetSelected(BasicSprite currentBox)
        {
            foreach (TextBox item in _itemList)
            {
                if (item == currentBox)
                    item.Selected = true;
                else
                    item.Selected = false;
            }
        }
        public List<BasicSprite> Items
        {
            get { return _itemList; }
        }
   
    }
}
