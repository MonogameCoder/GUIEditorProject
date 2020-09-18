using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using static _GUIProject.AssetManager;
using System;
using static _GUIProject.FontManager;
using _GUIProject.UI;
using System.Diagnostics;

namespace _GUIProject.UI
{    
    public class Button : Sprite
    {   
        [XmlIgnore]      
        public Label Caption { get; set; }      
        public override ColorObject TextColor
        {
            get { return Caption.TextColor; }
            set { Caption.TextColor = value; }
        }

        [XmlAttribute]
        public override string Text
        {
            get { return Caption.Text;}
            set { Caption.Text = value; }
        }
        
        [XmlAttribute]
        public FontType Font
        {
            get { return Singleton.Font.GetType(Caption.TextFont); }
            set { Caption.TextFont = Singleton.Font.GetFont(value); }
        }
        [XmlAttribute]
        public int FontSize
        {
            get { return Caption.FontSize; }
            set { Caption.FontSize = value; }
        }
        
       
        public Button() : base("DefaultButtonTX", DrawPriority.NORMAL, MoveOption.DYNAMIC)
        {
            LoadAttributes();            
        }
        
        public Button(string textureName,  OverlayOption category, DrawPriority priority, MoveOption state = MoveOption.STATIC)
            : base(textureName, priority,category, state)
        {
            LoadAttributes();
        }
        void LoadAttributes()
        {
            Caption = new Label("Button");
            Active = true;
            Caption.Active = true;
            Caption.Scale = Vector2.One;
            TextColor = Color.Black;          
        }
        public override void Initialize()
        {
            base.Initialize();       
            Caption.Initialize();

            Caption.TextFont = Singleton.Font.GetFont(FontType.STANDARD);
            XPolicy = SizePolicy.EXPAND;
            YPolicy = SizePolicy.EXPAND;
            MoveState = MoveOption.DYNAMIC;
          
        }
        public override void InitPropertyPanel()
        {
            Property = new PropertyPanel(this);
            Property.AddProperties(PropertyPanel.PropertyOwner.BUTTON);           
            Property.SetupProperties();
          
        }
        public override void Setup()
        {
            base.Setup();
            Caption.Setup();
            Caption.Active = true;
        }       
       
        public override void AddStringRenderer(SpriteBatch batch)
        {
            Caption.AddStringRenderer(batch);
            
        }
        public override void AddSpriteRenderer(SpriteBatch batch)
        {
            base.AddSpriteRenderer(batch);
            Caption.AddSpriteRenderer(batch);          

        }
        public override void AddPropertyRenderer(SpriteBatch batch)
        {
            Property.AddPropertyRenderer(batch);
        }    
        public override void ResetSize()
        {
            Caption.ResetSize();
            base.ResetSize();          
        }       
      
        public override UIObject HitTest(Point mousePosition)
        {
            UIObject result = null;

            if (Property != null && MainWindow.CurrentObject == this)
            {
                result = Property.HitTest(mousePosition);
              
                if(result != null)
                {                   
                    return result;
                }
            }
            if (Active)
            {
                result = base.HitTest(mousePosition);
                if (result != null)
                {                 
                    IsMouseOver = true;
                    MouseEvent.Over();                   
                }             
            }         

            return result;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Active)
            {              
                if(!Contains(MouseGUI.Position))
                {
                    IsMouseOver = false;
                    MouseEvent.Out();
                }
                Caption.Position = new  Point( Rect.Center.X, Rect.Center.Y);
                Caption.Position -= new Point(Caption.Width/ 2, Caption.Height / 2);
                Caption.Update(gameTime);              
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
                base.Draw();
                if (Caption != null)
                {
                    Caption.Draw();
                }
            }
            if (Property != null)
            {
                Property.Draw();
            }
        }
        
    }   

}   
