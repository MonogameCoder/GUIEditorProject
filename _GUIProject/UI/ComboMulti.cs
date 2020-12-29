using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using static _GUIProject.AssetManager;

namespace _GUIProject.UI
{
    class ComboMulti : Button, IScrollable
    {
        public override int Height
        {
            get { return 5 * _auxiliaryInfo.Height; }
            set { }
        }
      
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
        private Frame container;
      
        public Frame Container
        {
            get { return container; }
            set
            {
                container = value;
                container.Slots = new SortedSet<Slot<UIObject>>(container.Children);
            }
        }


        public int MaxLinesLength { get; set; } = 5;
        public int NumberOfLines
        {
            get { return Container.Length / LINE; }
        }

        private const int LINE = 6;
        private Sprite _auxiliaryInfo;
        private ScrollBar _scrollBar;
       
        private Button _defaultItem;      
        readonly string _defaultTXName;
        private readonly Sprite _bgSprite;

        int start = 0, end = 0;
        public ComboMulti()
        {

        }
        public ComboMulti(string baseTX, string itemTX, DrawPriority priority) : base(baseTX, OverlayOption.NORMAL, priority)
        {
            _bgSprite = new Sprite(itemTX, DrawPriority.NORMAL);                   
            _defaultTXName = itemTX;
            LoadAttributes();
        }
        void LoadAttributes()
        {
         
            Container = new Frame(_defaultTXName, DrawPriority.LOWEST, MoveOption.STATIC);
            Caption.TextFont = Singleton.Font.GetFont(FontManager.FontType.LUCIDA_CONSOLE);
            _defaultItem = new Button(_defaultTXName, OverlayOption.NORMAL, DrawPriority.LOWEST);
            _defaultItem.Text = "";

        }
        public override void Initialize()
        {
            base.Initialize();
            _bgSprite.Initialize();
            Container.Initialize();
            _scrollBar = new ScrollBar();
            _scrollBar.Parent = this;
            _scrollBar.Initialize();      
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
            Container.Setup();

            _bgSprite.Position = Container.Position;
            _bgSprite.Setup();
            _bgSprite.Resize(new Point(0, _bgSprite.Height * 3));
            
            
            _scrollBar.Setup();
            _scrollBar.Hide();

            _scrollBar.Position = new Point(Container.Right - _scrollBar.Width, Container.Top);


            if (MouseEvent.IsOnClickNull)
            {
                MouseEvent.onMouseClick += (sender, args) =>
                {
                    Show();
                };
            }

        }
        public override void AddSpriteRenderer(SpriteBatch batch)
        {
            base.AddSpriteRenderer(batch);
            Container.AddSpriteRenderer(batch);
            _scrollBar.AddSpriteRenderer(batch);
            _bgSprite.AddSpriteRenderer(batch);
            if (_auxiliaryInfo != null)
            {
                _auxiliaryInfo.AddSpriteRenderer(batch);
            }
        }
        public override void AddStringRenderer(SpriteBatch batch)
        {
            base.AddStringRenderer(batch);
            Container.AddStringRenderer(batch);
            _scrollBar.AddStringRenderer(batch);           
        }
      
        public void AddName(string name, Color color, FontContent font)
        {
            Caption = new Label(name);
            Caption.Initialize();
            Caption.TextFont = font;
            Caption.TextColor = color;
            Caption.Position = Position;
        }
        public void AddAuxiliaryInfo()
        {
            _auxiliaryInfo = new Sprite("ComboBoxAuxiliaryTX", DrawPriority.LOWEST);
            _auxiliaryInfo.Initialize();
            _auxiliaryInfo.Active = true;
        }
        public void AddNewItem(string name, Color color, Action buttonClickEvent)
        {
            Button newButton = new Button("ComboBoxAuxiliaryTX", OverlayOption.NORMAL, DrawPriority.LOWEST);
            newButton.Initialize();
            newButton.Setup();
            newButton.SpriteColor = color;
            newButton.SpriteColor.Text = name;
            newButton.Active = true;            
            newButton.Name = name;
            newButton.Text = "";

            int bottom = Container.Slots.Where(s => s.Item != _auxiliaryInfo).Sum(s => s.Item.Height);

            newButton.MouseEvent.onMouseClick += (sender, args) => { buttonClickEvent(); };
            newButton.MouseEvent.onMouseOut += (sender, args) => { };
            newButton.MouseEvent.onMouseOver += (sender, args) => { };

            int line = 0;
            Point position = Point.Zero;
            for (int i = 1; i <= Container.Length; i++)
            {
                int mod = i % LINE;
                if (mod == 0)
                {
                    line++;
                }
                position = new Point(newButton.Width * mod, newButton.Height * line);
            }               
            
            Container.AddItem(position, newButton, DrawPriority.LOW);
            Container.AddDefaultRenderers(newButton);    
            Container.UpdateLayout();
            
        }
        public void RearrangeContainer()
        {
            _scrollBar.ResetScroll();
           
            int line = 0;
            for (int i = 0; i < Container.Length; i++)
            {
                int mod = i  % LINE;
                if (mod == 0)
                {
                    line++;
                }
                Slot<UIObject> slot = Container[i];
                slot.Position = new Point(slot.Item.Width * mod, slot.Item.Height * (line-1));            
            }
        }
        public void ApplyScroll()
        {
            if (NumberOfLines > MaxLinesLength && MaxLinesLength + _scrollBar.CurrentScrollValue <= NumberOfLines)
            {

                start = NumberOfLines > MaxLinesLength ? _scrollBar.CurrentScrollValue : 0;
                end = MaxLinesLength + _scrollBar.CurrentScrollValue;

                int line = 0;
                for (int i = start * LINE; i < end * LINE; i++)
                {
                    if (i % LINE == 0)
                    {
                        line++;
                    }
                    
                    Point newPos = new Point(Container[i].Position.X, (Container[i].Item.Height * line) - Container[i].Item.Height);
                    Container.UpdateSlot(Container[i].Item, newPos);                    
                }

                Container.UpdateLayout();
            }
        }
        public override void Update(GameTime gameTime)
        {
            start = NumberOfLines > MaxLinesLength ? _scrollBar.CurrentScrollValue : 0;
            end = MaxLinesLength + _scrollBar.CurrentScrollValue;

            Container.Update(gameTime);
            
            _scrollBar.Update(gameTime);
            base.Update(gameTime);
        }
        public override UIObject HitTest(Point mousePosition)
        {
            UIObject result = null;
            if (Active)
            {
                result = _scrollBar.HitTest(mousePosition);
                if (result != null)
                {
                    return result;
                }
            }
            if(Container.Active)
            {               
                for (int i = start * LINE; i < end * LINE; i++)
                {
                    result = Container[i].Item.HitTest(mousePosition);
                    if(result != null)
                    {                     
                        return result;
                    }
                }
            }
            
            return base.HitTest(mousePosition);
        }

        public override void Draw()
        {

            base.Draw();

            if (Container.Active)
            {
                _bgSprite.Draw();

                for (int i = start * LINE; i < end * LINE; i++)
                {
                    Container[i].Item.Draw();
                }
            }
            if (_auxiliaryInfo != null)
            {
                _auxiliaryInfo.Draw();
            }
            _scrollBar.Draw();
        }
        public override void Show()
        {
            if (!Editable)
            {
                if (Container.Active)
                {
                    Container.Hide();
                    _scrollBar.Hide();
                }
                else
                {
                    Container.Show();
                    _scrollBar.Show();
                }
            }
            IsClicked = !IsClicked;
        }
        public override void Hide()
        {
            if (!Editable)
            {
                Container.Hide();
                _scrollBar.Hide();
            }          

        }
    }
}
