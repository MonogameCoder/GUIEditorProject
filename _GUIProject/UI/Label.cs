using _GUIProject.Events;
using ExtensionMethods;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

using static _GUIProject.AssetManager;
using static _GUIProject.Events.MouseEvents;

namespace _GUIProject.UI
{
   
    public class Label : UIObject
    {        
        public bool IsEmpty 
        { 
            get { return Text.Length == 0; }
        }
        public override string Text { get; set; }           
        public override ColorObject ColorValue { get; set; }        
        public FontContent TextFont { get; set; }      
        public override Point Position { get; set; }
        public override Point Size
        {
            get { return Rect.Size ; }
            set { Rect = new Rectangle(Position, value); }
        }
        public virtual Point TextSize
        {
            get { return Text.Size(TextFont).ToPoint(); }
        }

        private Vector2 _labelPosition;
        private Vector2 _labelScale;
        public Label()
        {
            Text = "This is a Sample text";           
        }
        public Label(string label)
        { 
            Text = label;
        }
        public override void Initialize()
        {
            MouseEvent = new MouseEvents(this);
            ColorValue = new ColorObject();
            TextColor = new ColorObject();
            TextColor.Color = Color.White;
           
            XPolicy = SizePolicy.EXPAND;
            YPolicy = SizePolicy.FIXED;
            Priority = DrawPriority.NORMAL;
           
            TextFont = Singleton.Font.GetFont(FontManager.FontType.LUCIDA_CONSOLE);
            
            _labelPosition = Vector2.Zero;
            _labelScale = Vector2.One;

          
            Active = true;
        }
        public override void InitPropertyPanel()
        {
            Property = new PropertyPanel(this);
            Property.AddProperties(PropertyPanel.PropertyOwner.LABEL);
            Property.SetupProperties();
        }
        public override void Setup()
        {
            Singleton.Content.LoadResources();
         
            Rect = new Rectangle(Position,TextSize);
            DefaultSize = Rect.Size;
        }
        public void AddFontPack()
        {
            Singleton.Content.AddFontContent(TextFont);
        }    
        public void AddPosition(Vector2 position)
        {
            _labelPosition = position;
        }
        public override void AddStringRenderer(SpriteBatch batch)
        {
            _stringRenderer = batch;
        }
        public override void AddSpriteRenderer(SpriteBatch batch)
        {
            _spriteRenderer = batch;
        }
        public override void AddDefaultRenderers(UIObject item)
        {
            item.AddStringRenderer(_stringRenderer);
        }
        public override void AddPropertyRenderer(SpriteBatch batch)
        {
            Property.AddPropertyRenderer(batch);
        }
        public override void Resize(Point amount)
        {            
            float x = amount.X / (float)TextSize.X;
            float y = amount.Y / (float)TextSize.Y;
            Vector2 textSize = new Vector2(x, y);
            Size += amount;
            _labelScale += textSize;                
        }
        public override void ResetSize()
        {
            _labelScale = Vector2.One;
            Rect = new Rectangle(Position, TextSize);
        }

        public override bool Contains(Point position)
        {
            if (Rect.Contains(position))
            {
                return true;
            }
            return false;
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
            if (Active)
            {
                Point dim = (TextSize.ToVector2() * _labelScale).ToPoint();
                Rect = new Rectangle(Position, dim);

            }
            if (Property != null)
            {
                Property.Update(gameTime);
            }
        }
        public override void Draw()
        {
            if (Active)
            {
                _stringRenderer.DrawString(TextFont, Text, Position.ToVector2(), TextColor * Alpha, 0f, Vector2.Zero, _labelScale, SpriteEffects.None, 1.0f);
              
            }
            if (Property != null)
            {
                Property.Draw();
            }
        }
       
        public override void Show()
        {
            Active = true;

        }
        public override void Hide()
        {
            Active = false;
        }



    }
}
