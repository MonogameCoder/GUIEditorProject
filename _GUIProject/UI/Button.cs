
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


using System.Xml.Serialization;

using System.Runtime.Serialization;
using static _GUIProject.AssetManager;
using System;
using _GUIProject.Events;
using static _GUIProject.UI.TextBox;

namespace _GUIProject.UI
{

    
    public class Button : BasicSprite
    {   
        public Label Caption { get; set; }
        ColorObject _textColor;      
        public override ColorObject TextColor
        {
            get
            {
                if (Caption != null)
                {
                    return Caption.TextColor;
                }
                return _textColor;
            }
            set
            {
                _textColor = value;
                if (Caption != null)
                {
                    Caption.TextColor = value;
                }
            }
        }

        public override string Text
        {
            get { return Caption.Text;}
            set { Caption.Text = value; }
        }

        
        public Button() : base("DefaultButtonTX", DrawPriority.NORMAL, MoveOption.STATIC)
        {           
          
        }
        
       
        public Button(string textureName,  OverlayOption category, DrawPriority priority, MoveOption state = MoveOption.STATIC)
            : base(textureName, priority,category, state)
        {
         
        }



        public override void Initialize()
        {
            base.Initialize();
            Caption = new Label("Button");
            Caption.Initialize();

            Caption.TextFont = Singleton.Font.GetFont(FontManager.FontType.STANDARD);
            XPolicy = SizePolicy.EXPAND;
            YPolicy = SizePolicy.EXPAND;
            MoveState = MoveOption.DYNAMIC;

            Active = true;

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
        //public override void Resize(Point newSize)
        //{
        //    // Scale Currently is not fully supported 
        //    // Due to the fact that the font loses too much quality,
        //    // so it will remain unchanged for the time being... as in other GUI editors
        //    // TODO: Make only the text that fits in the button boundaries visible

        //    //float labelXRatio = Caption.TextSize.X / (float)DefaultSize.X;
        //    //float labelYRatio = Caption.TextSize.Y / (float)DefaultSize.Y;

        //    //Point newLabelSize = new Point((int)Math.Round(newSize.X * labelXRatio), (int)Math.Round(newSize.Y * labelYRatio));
        //    //Caption.Resize(newLabelSize);


        //    base.Resize(newSize);
        //}
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
            }
            if (Active)
            {               
                if (result == null)
                {
                    result = base.HitTest(mousePosition);

                    if (result == null)
                    {
                        if (IsMouseOver)
                        {
                            MouseEvent.Out();
                        }
                        IsMouseOver = false;
                    }
                    else
                    {
                        IsMouseOver = true;
                        MouseEvent.Over();

                    }
                }
            }
            return result;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Active)
            {              

                Caption.Position = Rect.Center;
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
