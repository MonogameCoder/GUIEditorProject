using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Runtime.Serialization;
using static _GUIProject.UI.IObject;

namespace _GUIProject.UI
{
  
    public class ToggleButton : Button
    {
        private bool _toggle;
        public bool Toggle
        {
            get { return _toggle; }
            set
            {
                _toggle = value;
                IsClicked = _toggle;
            }
        }        

        public ToggleButton(): base("DefaultToggleTX", OverlayOption.TOGGLE, DrawPriority.NORMAL)
        {

            Active = true;
            IsClicked = false;
        }
        public override void Initialize()
        {
            base.Initialize();
            Text = "";
            MoveState = MoveOption.DYNAMIC;
            XPolicy = SizePolicy.FIXED;
            YPolicy = SizePolicy.FIXED;
        }
        public override void InitPropertyPanel()
        {
            Property = new PropertyPanel(this);
            Property.AddProperties(PropertyPanel.PropertyOwner.TOGGLE);
            Property.SetupProperties();
        }
        public override UIObject HitTest(Point mousePosition)
        {
            if (Active)
            {
                if (Contains(mousePosition))
                {
                    return this;
                }
            }
            if (Property != null && MainWindow.CurrentObject == this)
            {
                return Property.HitTest(mousePosition);
            }

            return null;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Active)
            {              
                if (MouseGUI.Focus == this && MouseGUI.LeftWasReleased)
                {
                    if (!Editable)
                    {
                        Toggle = !Toggle;
                    }
                }
            }
        }    

    }
}
