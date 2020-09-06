using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;


using Microsoft.Xna.Framework.Graphics;
using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Reflection;
using static _GUIProject.AssetManager;
using System.Diagnostics;

namespace _GUIProject.UI
{
    public class ComboBox : Button
    {       
        public enum GrowDirection
        {
            UP,
            DOWN
        }

        // This will be used in future implementations
        private GrowDirection _direction;

        private Frame _buttonsContainer;
        private Button _footTX;
        private Button _defaultItem;
        private ElementSelection _buttonSelection;
      
        readonly string _defaultTXName;
        readonly string _footTXName;
      
        public Point DefaultOffset { get; set; } = new Point(2, 3);
        public Point _offset;
        
        private BasicSprite _auxiliaryInfo;
        public ColorObject AuxilaryColor
        {
            get { return _auxiliaryInfo.ColorValue; }
            set
            {

                if (_auxiliaryInfo != null)
                {
                    _auxiliaryInfo.ColorValue = value;

                }
                Caption.Text = Text;
            }
        }
        
        public ComboBox() : base("DefaultComboBoxTX", OverlayOption.NORMAL, DrawPriority.LOW)
        {
            Parent = this;         

            _defaultTXName = "DefaultComboBoxBGTX";
            _footTXName = "DefaultComboBoxFootTX";      

            Active = true;         

        }
        public ComboBox(string baseTX, string itemTX, string footTX, DrawPriority priority) : base(baseTX, OverlayOption.NORMAL, priority)
        {
            Parent = this;
      
            _defaultTXName = itemTX;
            _footTXName = footTX;

            Active = true;
        }
        public override void InitPropertyPanel()
        {
            Property = new PropertyPanel(this);
            Property.AddProperties(PropertyPanel.PropertyOwner.COMBOBOX);
            Property.SetupProperties();
        }

        public override void Initialize()
        {
            base.Initialize();

            XPolicy = SizePolicy.EXPAND;
            YPolicy = SizePolicy.EXPAND;
            MoveState = MoveOption.DYNAMIC;
            _direction = GrowDirection.DOWN;

            _buttonSelection = new ElementSelection();
            _buttonSelection.Initialize();


            Caption.TextFont = Singleton.Font.GetFont(FontManager.FontType.LUCIDA_CONSOLE);       

            _buttonsContainer = new Frame("DefaultComboBoxTX", DrawPriority.LOWEST, MoveOption.STATIC);
            _buttonsContainer.Initialize();

            _defaultItem = new Button(_defaultTXName, OverlayOption.NORMAL, DrawPriority.LOWEST);
            _defaultItem.Initialize();
           
            _defaultItem.Text = "";

            _footTX = new Button(_footTXName, OverlayOption.NORMAL, DrawPriority.LOWEST);          
            _footTX.Initialize();
            _footTX.Text = "";
            _footTX.Editable = false;

            _buttonsContainer.AddItem(DefaultOffset, _footTX, DrawPriority.LOWEST);

            _buttonsContainer.Active = false;

        }
        public override void Setup()
        {
            base.Setup();
          
            if (_auxiliaryInfo != null)
            {
                _auxiliaryInfo.Position = Position + new Point(2, 3);
                _auxiliaryInfo.Setup();               
            }
            
            _buttonsContainer.Position = new Point(Left, Bottom);
            _buttonSelection.Setup();
            _buttonsContainer.Setup();
            
            if(MouseEvent.IsOnClickNull)
            {
                MouseEvent.onMouseClick += (sender, args) =>
                {
                    Show();
                };
            }
           
        }
        //public UIObject this[int index]
        //{
        //    get { return _buttonsContainer.Slots.ElementAt(index).Item; }
        //}
        public UIObject this[string name]
        {
            get { return _buttonsContainer.Slots.Where(s => s.Item.Text == name).Single().Item; }
        }
       
        public override void AddSpriteRenderer(SpriteBatch batch)
        {
            base.AddSpriteRenderer(batch);         
            _buttonsContainer.AddSpriteRenderer(batch);
            _buttonSelection.AddSpriteRenderer(batch);
           

            if (_auxiliaryInfo != null)
            {
                _auxiliaryInfo.AddSpriteRenderer(batch);
            }
        }
        public override void AddStringRenderer(SpriteBatch batch)
        {
            base.AddStringRenderer(batch);
            _buttonsContainer.AddStringRenderer(batch);
            _buttonSelection.AddStringRenderer(batch);
        }
        public override void AddPropertyRenderer(SpriteBatch batch)
        {
            Property.AddPropertyRenderer(batch);
        }
        public void AddName(string name, Color color, FontContent font)
        {
            Caption = new Label(name);
            Caption.Initialize();
            Caption.TextFont = font;
            Caption.TextColor = color;
            Caption.Position = Position;
        }
        void SelectItem(Button item)
        {
            if (item != null)
            {
                _buttonSelection.Position = item.Position;

                Point rightBarPosition = new Point(item.Right, item.Top);
                Point leftBarPosition = new Point(item.Left, item.Top);
                Point topBarPosition = new Point(item.Left, item.Top);
                Point bottomBarPosition = new Point(item.Left, item.Bottom);

                _buttonSelection.UpdatePosition(rightBarPosition,
                                                              leftBarPosition,
                                                              topBarPosition,
                                                              bottomBarPosition);
                _buttonSelection.UpdateSize(item);
            }

        }
       
        public void AddAuxiliaryInfo()
        {
            _auxiliaryInfo = new BasicSprite("ComboBoxAuxiliaryTX", DrawPriority.LOWEST);
            _auxiliaryInfo.Initialize();          
            _auxiliaryInfo.Active = true;
           
        }
     

        public void AddNewItem(string text, Action buttonClickEvent)       
        {
            Button newButton = new Button(_defaultTXName, OverlayOption.NORMAL, DrawPriority.LOW);            
            newButton.Initialize();
            newButton.Setup();
            newButton.TextColor = Color.SlateGray;
            newButton.Active = true;
            newButton.Name = "ComboBoxButton";
            newButton.Text = text;      

            int bottom = _buttonsContainer.Slots.Where(s => s.Item != _footTX && s.Item != _auxiliaryInfo).Sum(s => s.Item.Height);
           
            newButton.MouseEvent.onMouseClick += (sender, args) => { buttonClickEvent(); };
            newButton.MouseEvent.onMouseOut += (sender, args) => { };
            newButton.MouseEvent.onMouseOver += (sender, args) => { };

            _offset = new Point(DefaultOffset.X, bottom);
            _buttonsContainer.AddItem(_offset, newButton, DrawPriority.LOW);
      
            _buttonsContainer.UpdateSlot(_footTX, new Point(DefaultOffset.X, bottom + newButton.Height));

            _buttonsContainer.AddDefaultRenderers(newButton);

            ResetSize();

            _buttonsContainer.UpdateLayout();

        }

        public void RemoveItem(UIObject item)
        {       
            var slotsArray = _buttonsContainer.Slots.ToArray();
            int delIndex = Array.FindIndex(slotsArray, a => a.Item == item); 

            _buttonsContainer.RemoveSlot(item);
            RearrangeContainer(_buttonsContainer.Length, delIndex);     
        }
        void RearrangeContainer(int end, int start)
        {
            int bottom = _buttonsContainer.Slots.Where(s => s.Item != _footTX).Sum(s => s.Item.Height);
            for (int i = end -1; i >= start; i--)
            {
                var curItem = _buttonsContainer[i].Item;
                int delta = end - i;
                _buttonsContainer.UpdateSlot(curItem, new Point(_offset.X, (bottom - curItem.Height * delta)));
            }
            _buttonsContainer.UpdateSlot(_footTX, new Point(DefaultOffset.X, bottom));
        }

        public override UIObject HitTest(Point mousePosition)
        {
            UIObject result = null;
            if (Active)
            {
                if (result == null)
                {
                    result = _buttonsContainer.HitTest(mousePosition);
                }                
            }
            if (result == null)
            {
                result = base.HitTest(mousePosition);
            }
            return result;
        }
        public override void ResetSize()
        {

            base.ResetSize();
            for (int i = 0; i < _buttonsContainer.Length; i++)
            {
                Slot<UIObject> current = _buttonsContainer[i];
                current.Item.ResetSize();
            }
            RearrangeContainer(_buttonsContainer.Length, 0);

        }
        public override void Resize(Point amount)
        {
            base.Resize(amount);

            for (int i = _buttonsContainer.Length - 1; i > 0; i--)
            {
                var current = _buttonsContainer[i].Item;              
                current.Resize(amount);
            }

            _footTX.Resize(new Point(amount.X, 0));
            RearrangeContainer(_buttonsContainer.Length, 0);  
        }


        public bool Contains(string text)
        {
            for (int i = 0; i < _buttonsContainer.Length; i++)
            {
                Slot<UIObject> slot = _buttonsContainer[i];
                if (slot.Item.Text == text)
                {
                    return true;
                }
            }
            return false;
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Active)
            {

                _buttonsContainer.Position = new Point(Left, Bottom);
                _buttonsContainer.Update(gameTime);

                if (_buttonsContainer.Contains(MouseGUI.Focus))
                {
                    Button btn = MouseGUI.Focus as Button;
                    if (Contains(btn.Text) && Editable)
                    {
                        SelectItem(btn);
                        _buttonSelection.Show();
                    }
                    else
                    {
                        _buttonSelection.Hide();
                    }
                }
            }
        }

        public override void Draw()
        {
            base.Draw();
            if (Active)
            {              

                if (_buttonsContainer.Active)
                {
                    for (int i = _buttonsContainer.Length - 1; i >= 0; i--)
                    {
                        Slot<UIObject> slot = _buttonsContainer[i];
                        slot.Item.Alpha = Alpha;
                        slot.Item.Draw();
                    }
                }

                if (_auxiliaryInfo != null)
                {
                    _auxiliaryInfo.Draw();
                }
                _buttonSelection.Draw();
            }

        }
        public override void Show()
        {
            if (!Editable)
            {
                if (_buttonsContainer.Active)
                {
                    _buttonsContainer.Hide();
                }
                else
                {
                    _buttonsContainer.Show();
                }
            }         
            IsClicked = !IsClicked;
        }
        public override void Hide()
        {
            if (!Editable)
            {
                _buttonsContainer.Hide();
            }
            _buttonSelection.Hide();      
            
        }
    }
}
