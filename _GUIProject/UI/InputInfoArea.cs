using Microsoft.Xna.Framework;
using System;
using System.Linq;
using static _GUIProject.UI.Sprite;
using static _GUIProject.UI.TextBox;
using Microsoft.Xna.Framework.Graphics;


namespace _GUIProject.UI
{
    public class InputInfoArea : Sprite
    {     

        public Button Up { get; private set; }
        public Button Down { get; private set; }
        public override string Text
        {
            get { return _textInput.Text; }
            set { _textInput.Text = value; }
        }
        public override ColorObject TextColor
        {
            get { return _textInput.TextColor; }
            set { _textInput.TextColor = value; }
        }

        private TextBox _textInput;
        private Frame _container;
        private readonly string _baseTX;
        public InputInfoArea(string largeTX)
        {
            _baseTX = largeTX;
            LoadAttributes();
        }          
        public InputInfoArea()
        {
            _baseTX = "CombinedArrowsTX";
            LoadAttributes();
           
        }
        void LoadAttributes()
        {
            Up = new Button("CombinedArrowsUpTX", OverlayOption.NORMAL, DrawPriority.NORMAL);
            Down = new Button("CombinedArrowsDownTX", OverlayOption.NORMAL, DrawPriority.NORMAL);
            _container = new Frame(_baseTX, DrawPriority.NORMAL, MoveOption.STATIC);
            _textInput = new TextBox(_baseTX, "DefaultPropertiesPanelTextBoxPointerTextureName", TextBoxType.TEXT, DrawPriority.NORMAL);
        }
        public override void Initialize()
        { 
            TextColor = Color.White;
           
            Up.Text = "";
            Down.Text = "";

            _container.Initialize();    

            _container.Show();
            Active = true;
        }
        public override void Setup()
        {
            _textInput.Setup();
            Up.Setup();
            Down.Setup();

           
            _container.AddItem(new Point(_textInput.Right - Up.Width, _textInput.Top), Up, DrawPriority.HIGH);
            _container.AddItem(new Point(_textInput.Right - Down.Width, _textInput.Height - Down.Height), Down, DrawPriority.HIGH);
            _container.AddItem(Point.Zero, _textInput, DrawPriority.NORMAL);

            Point halfTextSize = (_textInput.TextSize / 2).ToPoint();
            Point halfTexboxSize = new Point(_textInput.Width / 2, _textInput.Height / 2);
            _textInput.TextOffset = halfTexboxSize - halfTextSize;

            _container.Position = Position;

            _container.Show();
        }
      
        public override void Resize(Point newSize)
        {
            _container.Resize(newSize);
        }

        public override void AddSpriteRenderer(SpriteBatch batch)
        {
            _spriteRenderer = batch;
            _container.AddSpriteRenderer(batch);

        }
        public override void AddStringRenderer(SpriteBatch batch)
        {
            _stringRenderer = batch;
            _container.AddStringRenderer(batch);
        }        
        public override UIObject HitTest(Point mousePosition)
        {
            if(Active)
            {
                return _container.HitTest(mousePosition);
            }
            return null;
        }
        public override void Update(GameTime gameTime)
        {
            if (Active)
            {

                Point halfTextSize = (_textInput.TextSize / 2).ToPoint();
                Point halfTexboxSize = new Point(_textInput.Width / 2, _textInput.Height / 2);
                _textInput.TextOffset = halfTexboxSize - halfTextSize;

                _container.Update(gameTime);
            }

        }
        public override void Draw()
        {
            if(Active)
            {
                _container.Draw();
            }            
        }

        public override void Show()
        {
            if (_container.Active)
            {
                Hide();
            }
            else
            {
                for (int i = 0; i < _container.Slots.Count(); i++)
                {
                    Slot<UIObject> slot = _container.Slots.ElementAt(i);

                    if (slot.Item != null)
                    {
                        slot.Item.Show();
                    }

                }
                _container.Active = true;
            }


        }
        public override void Hide()
        {

            for (int i = 0; i < _container.Slots.Count(); i++)
            {
                Slot<UIObject> slot = _container.Slots.ElementAt(i);

                if (slot.Item != null)
                {
                    slot.Item.Hide();
                }

            }
            _container.Active = false;

        }
    }
}
