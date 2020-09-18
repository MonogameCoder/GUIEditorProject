using Microsoft.Xna.Framework;
using System;
using System.Xml.Serialization;
using static _GUIProject.AssetManager;

namespace _GUIProject.UI
{
   
    public class CheckBox : Button
    {       
      
        public Point Offset { get; set; }

        private bool _selected;
        [XmlIgnore]
        public bool Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                IsClicked = _selected;
            }
        }
        [XmlAttribute]
        public override string Text
        {
            get { return Caption.Text; }
            set { Caption.Text = value; }
        }
        private Point _defaultSize;
        
        [XmlIgnore]
        public override Point DefaultSize 
        {
            get { return _defaultSize + Caption.TextSize; }
            set { _defaultSize = value; }
        }

        [XmlIgnore]
        public override Point Size
        {
            get { return Rect.Size; }
            set { Rect = new Rectangle(Position, value); }
        }

        [XmlIgnore]
        public override int Height
        {
            get { return (Size + Caption.TextSize).Y; }
            set { Rect = new Rectangle(Position, new Point(Size.X, value)); }
        }

        [XmlIgnore]
        public override int Width
        {
            get { return (Size + Caption.TextSize).X; }
            set { Rect = new Rectangle(Position, new Point(value, Size.Y)); }
        }
        public CheckBox() : base("DefaultCheckboxTX", OverlayOption.CHECKBOX, DrawPriority.HIGH)
        {
            LoadAttributes();
        }
        public CheckBox(string textureName, OverlayOption layerOption, DrawPriority priority) : base(textureName, layerOption, priority)
        {
            LoadAttributes();
        }
        void LoadAttributes()
        {
            Offset = new Point(4, 8);
            TextColor = Color.White;
            Text = "";         
        }
        public override void Initialize()
        {
            base.Initialize();
            XPolicy = SizePolicy.FIXED;
            YPolicy = SizePolicy.FIXED;
            MoveState = MoveOption.DYNAMIC;
          
            Caption.Position = Position + Offset; 
            Active = true;
        }
        public override void InitPropertyPanel()
        {
            Property = new PropertyPanel(this);
            Property.AddProperties(PropertyPanel.PropertyOwner.CHECKBOX);           
            Property.SetupProperties();           
        }
        public override void ResetSize()
        {           
            Size = DefaultSize - Caption.TextSize;
            Caption.ResetSize();
        }
        public override void Resize(Point amount)
        {
            //Caption.Resize(amount);
            Size += amount;   
        }
        public override void Setup()
        {           
            base.Setup();
            Caption.Position = new Point((Right + Offset.X), Top + Offset.Y);
            DefaultSize = Rect.Size;
        } 
        public override UIObject HitTest(Point mousePosition)
        {
            UIObject result = null;

            if (Property != null && MainWindow.CurrentObject == this)
            {
                result = Property.HitTest(mousePosition);

                if (result != null)
                {
                    return result;
                }
            }

            if (Active)
            {
                Rectangle extended = new Rectangle(Position, Size + Caption.TextSize);
                if (extended.Contains(mousePosition.ToPoint()))
                {
                    IsMouseOver = true;
                    MouseEvent.Over();

                    return this;
                }
            }
            return base.HitTest(mousePosition);
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
