using System.Collections.Generic;

namespace _GUIProject.UI
{
    public class ButtonGroup 
    {     
       
        List<Button> _itemsList;
        public ButtonGroup()
        {
            _itemsList = new List<Button>();        
        }
        public void Add(Button checkbox)
        {
            _itemsList.Add(checkbox);
        }
    
        public void SetSelected(Button current_btn)
        {
            foreach (Button item in _itemsList)
            {
                if (item == current_btn)
                    item.Active  = false;
                else
                    item.Active = true;
            }        
        }
       
    }
}
