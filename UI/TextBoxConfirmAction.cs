using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using static _GUIProject.Events.MouseEvents;
using _GUIProject.Events;

namespace _GUIProject.UI
{
    public class TextBoxConfirmAction : BasicSprite
    {
        List<Button> _buttonList;    
        TextBox _textBox; 
     
        public override string Text 
        {
            get { return _textBox.Text; }
            set { _textBox.SimulateInput(value); }
        }

        public bool Selected
        {
            get { return _textBox.Selected; }
            set { _textBox.Selected = value; }
        }

        public TextBoxConfirmAction() : base("TextBoxConfirmPickerTX", DrawPriority.HIGH)
        {
            
        }
        public override void Initialize()
        {
            base.Initialize();
            _textBox = new TextBox();
            _textBox.Initialize();
            
            _buttonList = new List<Button>();
         
            _textBox.FieldWidth = 200;
            _textBox.TextOffset = new Point(4, 4);
            MouseEvent = new MouseEvents(this);

            Active = true;
        }
        public override void Setup()
        {           
            base.Setup();          
            _textBox.Setup();
            _textBox.Position = new Point(Position.X, Center.Y - _textBox.Height / 2);
            
            Point position = new Point(_textBox.Right, _textBox.Center.Y);
            for (int i = 0; i < _buttonList.Count; i++)
            {
                _buttonList[i].Setup();
                int x = position.X + _buttonList[i].Width * i;
                int y = position.Y - _buttonList[i].Height / 2;
                _buttonList[i].Position = new Point(x,y);               
              
            }

            int sum = _buttonList.Sum(s => s.Width);
            Size = new Point(_textBox.Width + sum, Size.Y);

            _textBox.MouseEvent.onMouseClick += (sender, args) =>
            {
                _textBox.Selected = true;
            };
           
        }
        public override void AddSpriteRenderer(SpriteBatch batch)
        {
            _textBox.AddSpriteRenderer(batch);
            for (int i = 0; i < _buttonList.Count; i++)
            {
                _buttonList[i].AddSpriteRenderer(batch);
            }
            base.AddSpriteRenderer(batch);
        }
        public override void AddStringRenderer(SpriteBatch batch)
        {
            _textBox.AddStringRenderer(batch);
            for (int i = 0; i < _buttonList.Count; i++)
            {
                _buttonList[i].AddStringRenderer(batch);
            }
            base.AddStringRenderer(batch);
        }
        public void AddButton(Button button, int offset = 8)
        {
            button.Initialize();
            button.Text = "";       
            _buttonList.Add(button);          
          
        }
        public virtual void Clear()
        {
            _textBox.Clear();
        }

        public override UIObject HitTest(Point mousePosition)
        {
            UIObject result = null;
            if (Active)
            {
                for (int i = 0; i < _buttonList.Count; i++)
                {
                    result = _buttonList[i].HitTest(mousePosition);
                    if (result != null)
                        return result;

                }

                return _textBox.HitTest(mousePosition);
            }
            return result;
        }
        public override void Update(GameTime gameTime)
        {
            if (Active)
            {               
                _textBox.Update(gameTime);  
                base.Update(gameTime);
            }
        }
       
        public override void Draw()
        {
            if (Active)
            {
                base.Draw();

                _textBox.Draw();
                for (int i = 0; i < _buttonList.Count; i++)
                {
                    _buttonList[i].Draw();
                }
                
            }
        }
        public override void Show()
        {
            Active = true;
            _textBox.Active = true;
         
            for (int i = 0; i < _buttonList.Count; i++)
            {
                _buttonList[i].Active = true;
            }
        }
        public override void Hide()
        {          
            Active = false;

            _textBox.Active = false;

            for (int i = 0; i < _buttonList.Count; i++)
            {
                _buttonList[i].Active = false;
            }

        }


    }
}
