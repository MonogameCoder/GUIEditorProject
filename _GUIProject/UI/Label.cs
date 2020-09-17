using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using static _GUIProject.AssetManager;
using static _GUIProject.FontManager;
using ExtensionMethods;
using _GUIProject.Events;

namespace _GUIProject.UI
{ 
    public class Label : UIObject
    {        
        [XmlIgnore]      
        public bool IsEmpty { get { return Text.Length == 0; } }
        
        [XmlAttribute]
        public override string Text { get; set; }       
        public override ColorObject TextColor { get; set; }

        [XmlIgnore]
        public FontContent TextFont { get; set; }
       
        [XmlIgnore]
      
        public override Point Position { get; set; }

        [XmlIgnore]
      
        public Vector2 Scale { get; set; }

        [XmlAttribute]
        public FontType Font
        {
            get { return Singleton.Font.GetType(TextFont); }
            set { TextFont = Singleton.Font.GetFont(value); }
        }       
       
        [XmlIgnore]
        public override Point Size
        {
            get { return Rect.Size ; }
            set
            { 
                Rect = new Rectangle(Position, value); 
            }
        }
        [XmlIgnore]
        public virtual Point TextSize
        {
            get { return Text.Size(TextFont).ToPoint(); }
        }      
      

        [XmlAttribute]
        public int FontSize
        {
            get { return (int)"A".Size(TextFont).Length(); }
            set
            {              
                Scale = Vector2.One;
                Vector2 newScale = new Vector2(value / 100f);
                if (value > FontSize)
                {
                    Scale += newScale;
                }
                else
                {
                    if (value == FontSize)
                    {
                        Scale = Vector2.One;
                    }
                    else
                    {
                        Scale = Vector2.One;
                        newScale = new Vector2(1f / value);
                        Scale -= newScale;
                    }  
                }               
            }
        }
       
        public Label()
        {
            Text = "This is a Sample text";
            LoadAttributes();           
        }
        public Label(string label)
        { 
            Text = label;
            LoadAttributes();
          
        }
        void LoadAttributes()
        {
            MouseEvent = new MouseEvents(this);
            TextFont = Singleton.Font.GetFont(FontType.LUCIDA_CONSOLE);
            TextColor = Color.White;
            Scale = Vector2.One;
            Active = true;
            Singleton.Content.LoadResources();            
        }
        public override void Initialize()
        {            
            XPolicy = SizePolicy.EXPAND;
            YPolicy = SizePolicy.FIXED;
            Priority = DrawPriority.NORMAL;       
        }
        public override void InitPropertyPanel()
        {
            Property = new PropertyPanel(this);
            Property.AddProperties(PropertyPanel.PropertyOwner.LABEL);           
            Property.SetupProperties();           
        }
        public override void Setup()
        {        
            Rect = new Rectangle(Position.X, Position.Y, TextSize.X, TextSize.Y);
            DefaultSize = Rect.Size;
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
            ResetSize();
            float x = amount.X / (float)TextSize.X;
            float y = amount.Y / (float)TextSize.Y;
            Vector2 textSize = new Vector2(x, y);        
            Scale += textSize;

        }
        public override void ResetSize()
        {
            Scale = Vector2.One;
            Rect = new Rectangle(Position, TextSize);
        }

        public override bool Contains(Point position)
        {
            if (Rect.Contains(position.ToPoint()))
            {
                return true;
            }
            return false;
        }
        public override UIObject HitTest(Point mousePosition)
        {
            UIObject result = null;
            if (Property != null && MainWindow.CurrentObject == this)
            {
                result= Property.HitTest(mousePosition);
                if (result != null)
                {
                    return result;
                }
                 
            }
            if (Active)
            {
                if (Contains(mousePosition))
                {                  
                    return this;
                }   
            }

           
            return null;
        }
        public override void Update(GameTime gameTime)
        {
            if (Active)
            {
                Point dim = (TextSize * Scale).ToPoint();
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
                _stringRenderer.DrawString(TextFont, Text, Position, TextColor * Alpha, 0f, Vector2.Zero, Scale, SpriteEffects.None, 1.0f);
              
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
        public override bool ShouldSerializeWidth() { return false; }
        public override bool ShouldSerializeHeight() { return false; }      

    }
}
