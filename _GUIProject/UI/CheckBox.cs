using Microsoft.Xna.Framework;

using System.Xml.Serialization;
using static _GUIProject.AssetManager;

namespace _GUIProject.UI
{
   
    public class CheckBox : Button
    {      
        
        public Point Offset { get; set; }

        private bool _selected;
        public bool Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                IsClicked = value;
            }
        }

        public CheckBox() : base("DefaultCheckboxTX", OverlayOption.CHECKBOX, DrawPriority.HIGH)
        {
            
        }
        public CheckBox(string textureName, OverlayOption layerOption, DrawPriority priority) : base(textureName, layerOption, priority)
        {
           
        }
        public override void Initialize()
        {
            base.Initialize();
            XPolicy = SizePolicy.FIXED;
            YPolicy = SizePolicy.FIXED;
            MoveState = MoveOption.DYNAMIC;

            Offset = new Point(4, 8);
            Caption.Position = Position + Offset;
            Text = "";                

            Active = true;
        }
        public override void InitPropertyPanel()
        {
            Property = new PropertyPanel(this);
            Property.AddProperties(PropertyPanel.PropertyOwner.CHECKBOX);
            Property.SetupProperties();
        }
        public override void Setup()
        {           
            base.Setup();
            Caption.Position = new Point((Right + Offset.X), Top + Offset.Y);
        }      
        
        public void SetCaption(Label caption, Point offset)
        {
            Caption = caption;
            Offset = offset;
            caption.Position = Position + offset;
        }
        public override UIObject HitTest(Point mousePosition)
        {
            
            if (Active)
            {
                return base.HitTest(mousePosition);
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
                if (!Editable)
                {
                    if (MouseGUI.Focus == this && MouseGUI.LeftWasReleased)
                    {
                        Selected = !Selected;
                        IsClicked = Selected;
                    }
                }
                
                Caption.Position = new Point((Right + Offset.X), Top + Offset.Y);
            }

        }
     
    }
}
