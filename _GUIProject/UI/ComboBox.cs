﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;


using Microsoft.Xna.Framework.Graphics;
using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Reflection;

using System.Diagnostics;
using _GUIProject.UI;
using _GUIProject;
using static _GUIProject.AssetManager;

namespace _GUIProject.UI
{
    public class ComboBox : Button
    {       
        public enum GrowDirection
        {
            UP,
            DOWN
        }
        
        [XmlIgnore]
        public Point DefaultOffset { get; set; } = new Point(2, 3);
        
        [XmlIgnore]
        public Point _offset;

        // This will be used in future implementations
        private Sprite _auxiliaryInfo;
        [XmlIgnore]
        public ColorObject AuxilaryColor
        {
            get { return _auxiliaryInfo.SpriteColor; }
            set
            {

                if (_auxiliaryInfo != null)
                {
                    _auxiliaryInfo.SpriteColor = value;

                }
                Caption.Text = Text;
            }
        }
        Frame container;
        [XmlElement]
        public Frame Container 
        {
            get { return container; }
            set
            {
                container = value;
                container.Slots = new SortedSet<Slot<UIObject>>(container.Children);
            }
        }


        private GrowDirection _direction;

        
        private Button _footTX;
        private Button _defaultItem;
        private ElementSelection _buttonSelection;
      
        readonly string _defaultTXName;
        readonly string _footTXName;
      
      
        public ComboBox() : base("DefaultComboBoxTX", OverlayOption.NORMAL, DrawPriority.LOW)
        {
            Parent = this;        

            _defaultTXName = "DefaultComboBoxBGTX";
            _footTXName = "DefaultComboBoxFootTX";

            LoadAttributes();

        }
        public ComboBox(string baseTX, string itemTX, string footTX, DrawPriority priority) : base(baseTX, OverlayOption.NORMAL, priority)
        {
            Parent = this;
      
            _defaultTXName = itemTX;
            _footTXName = footTX;

            LoadAttributes();
          
        }
        void LoadAttributes()
        {

            _buttonSelection = new ElementSelection();
            _buttonSelection.Initialize();
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

            Container = new Frame("DefaultComboBoxTX", DrawPriority.LOWEST, MoveOption.STATIC);
            Container.Initialize();

            _defaultItem = new Button(_defaultTXName, OverlayOption.NORMAL, DrawPriority.LOWEST);
            _defaultItem.Initialize();
            _defaultItem.Text = "";


            Caption.TextFont = Singleton.Font.GetFont(FontManager.FontType.LUCIDA_CONSOLE);            

            _footTX = new Button(_footTXName, OverlayOption.NORMAL, DrawPriority.LOWEST);          
            _footTX.Initialize();
            _footTX.Text = "";
            _footTX.Editable = false;

            Container.AddItem(DefaultOffset, _footTX, DrawPriority.LOWEST);
            Container.Active = false;
            Active = true;
        }
        public override void Setup()
        {
            base.Setup();
          
            if (_auxiliaryInfo != null)
            {
                _auxiliaryInfo.Position = Position + new Point(2, 3);
                _auxiliaryInfo.Setup();               
            }
            
            Container.Position = new Point(Left, Bottom);
            _buttonSelection.Setup();
            Container.Setup();
            
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
            get { return Container.Slots.Where(s => s.Item.Text == name).Single().Item; }
        }
       
        public override void AddSpriteRenderer(SpriteBatch batch)
        {
            base.AddSpriteRenderer(batch);         
            Container.AddSpriteRenderer(batch);
            _buttonSelection.AddSpriteRenderer(batch);
           

            if (_auxiliaryInfo != null)
            {
                _auxiliaryInfo.AddSpriteRenderer(batch);
            }
        }
        public override void AddStringRenderer(SpriteBatch batch)
        {
            base.AddStringRenderer(batch);
            Container.AddStringRenderer(batch);
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
            _auxiliaryInfo = new Sprite("ComboBoxAuxiliaryTX", DrawPriority.LOWEST);
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

            int bottom = Container.Slots.Where(s => s.Item != _footTX && s.Item != _auxiliaryInfo).Sum(s => s.Item.Height);
           
            newButton.MouseEvent.onMouseClick += (sender, args) => { buttonClickEvent(); };
            newButton.MouseEvent.onMouseOut += (sender, args) => { };
            newButton.MouseEvent.onMouseOver += (sender, args) => { };

            _offset = new Point(DefaultOffset.X, bottom);
            Container.AddItem(_offset, newButton, DrawPriority.LOW);
      
            Container.UpdateSlot(_footTX, new Point(DefaultOffset.X, bottom + newButton.Height));

            Container.AddDefaultRenderers(newButton);

            ResetSize();

            Container.UpdateLayout();

        }

        public void RemoveItem(UIObject item)
        {       
            var slotsArray = Container.Slots.ToArray();
            int delIndex = Array.FindIndex(slotsArray, a => a.Item == item); 

            Container.RemoveSlot(item);
            RearrangeContainer(Container.Length, delIndex);     
        }
        void RearrangeContainer(int end, int start)
        {
            int bottom = Container.Slots.Where(s => s.Item != _footTX).Sum(s => s.Item.Height);
            for (int i = end -1; i >= start; i--)
            {
                var curItem = Container[i].Item;
                int delta = end - i;
                Container.UpdateSlot(curItem, new Point(_offset.X, (bottom - curItem.Height * delta)));
            }
            Container.UpdateSlot(_footTX, new Point(DefaultOffset.X, bottom));
        }

        public override UIObject HitTest(Point mousePosition)
        {
            UIObject result = null;
            if (Active)
            {
                if (result == null)
                {
                    result = Container.HitTest(mousePosition);
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
            for (int i = 0; i < Container.Length; i++)
            {
                Slot<UIObject> current = Container[i];
                current.Item.ResetSize();
            }
            RearrangeContainer(Container.Length, 0);

        }
        public override void Resize(Point amount)
        {
            base.Resize(amount);

            for (int i = Container.Length - 1; i > 0; i--)
            {
                var current = Container[i].Item;              
                current.Resize(amount);
            }

            _footTX.Resize(new Point(amount.X, 0));
            RearrangeContainer(Container.Length, 0);  
        }


        public bool Contains(string text)
        {
            for (int i = 0; i < Container.Length; i++)
            {
                Slot<UIObject> slot = Container[i];
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

                Container.Position = new Point(Left, Bottom);
                Container.Update(gameTime);

                if (Container.Contains(MouseGUI.Focus))
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

                if (Container.Active)
                {
                    for (int i = Container.Length - 1; i >= 0; i--)
                    {
                        Slot<UIObject> slot = Container[i];
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
                if (Container.Active)
                {
                    Container.Hide();
                }
                else
                {
                    Container.Show();
                }
            }         
            IsClicked = !IsClicked;
        }
        public override void Hide()
        {
            if (!Editable)
            {
                Container.Hide();
            }
            _buttonSelection.Hide();      
            
        }
    }
}
