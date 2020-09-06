
using System;

using Microsoft.Xna.Framework;


using System.Xml.Serialization;
using static _GUIProject.UI.MouseGUI;
using System.Runtime.Serialization;
using static _GUIProject.AssetManager;
using Microsoft.Xna.Framework.Graphics;
using static _GUIProject.Events.MouseEvents;
using _GUIProject.Events;
using System.Diagnostics;

namespace _GUIProject.UI
{
   
    
    public class BasicSprite : UIObject
    {
        public enum OverlayOption
        {
            NESTED,
            NORMAL,
            CHECKBOX,
            TOGGLE
        }
        public enum ScaleOrigin
        {
            CENTER,
            TOP_LEFT,
            TOP_RIGHT,
        }    

        public TextureContent DefaultSprite { get; set; }      
        public TextureContent SpriteClicked { get; set; }      
        public TextureContent SpriteOver { get; set; }
        public TextureContent DisabledSprite { get; set; }
        public override Point Size
        {
            get { return Rect.Size; }
            set { Rect = new Rectangle(Position, value); }
        }
        protected Point _position;
        public override Point Position
        {
            get { return _position; }
            set
            {
                if (_position != value)
                {                    
                    _position = value;
                    Rect = new Rectangle(_position, Rect.Size);
                }
            }
        }      
      
        public BasicSprite()
        {
            MoveState = MoveOption.DYNAMIC;
            Priority = DrawPriority.NORMAL;          
        }
        
        public BasicSprite(string baseTX, DrawPriority priority, MoveOption moveState = MoveOption.STATIC) 
        {           
            DefaultSprite = Singleton.Content.AddTexture(baseTX);
            SpriteClicked = Singleton.Content.AddTexture(baseTX + "Clicked");
            SpriteOver = Singleton.Content.AddTexture(baseTX + "Over");
            DisabledSprite = Singleton.Content.AddTexture(baseTX + "Disabled");

            Priority = priority;
            MoveState = moveState;           
        }

        public BasicSprite(string baseTX, DrawPriority priority, OverlayOption overlay, MoveOption state = MoveOption.STATIC)
        {

            DefaultSprite = Singleton.Content.AddTexture(baseTX);
            SpriteClicked = Singleton.Content.AddTexture(baseTX + "Clicked");
            SpriteOver = Singleton.Content.AddTexture(baseTX + "Over");
            DisabledSprite = Singleton.Content.AddTexture(baseTX + "Disabled");            

            Priority = priority;
            MoveState = state;
            Overlay = overlay;      

        }
        public override void Initialize()
        {
            ColorValue = new ColorObject();
            TextColor = new ColorObject();
            ColorValue.Color = Color.White;
            TextColor.Color = Color.White;
            MouseEvent = new MouseEvents(this);

            Active = true;
        }
       
        public override void Setup()
        {
            Singleton.Content.LoadResources();
            if (SpriteOver != null && SpriteOver.Texture == null)
                SpriteOver = DefaultSprite;

            if (SpriteClicked != null && SpriteClicked.Texture == null)
                SpriteClicked = DefaultSprite;

            if (DisabledSprite != null && DisabledSprite.Texture == null)
                DisabledSprite = DefaultSprite;

            Rect = new Rectangle(Position.X, Position.Y, DefaultSprite.Width, DefaultSprite.Height);
            DefaultSize = Rect.Size;

        }
        public void AddTexture(string txName)
        {
            DefaultSprite = Singleton.Content.AddTexture(txName);
        }
        public void AddTextureOver(string txName)
        {
            SpriteOver = Singleton.Content.AddTexture(txName);
        }
        public void AddTextureClicked(string txName)
        {
            SpriteClicked = Singleton.Content.AddTexture(txName);
        }

        public void AddTextureDisabled(string textureName)
        {
            DisabledSprite = Singleton.Content.AddTexture(textureName);
        }
        public override void AddStringRenderer(SpriteBatch batch)
        {
            _stringRenderer = batch;
        }
        public void AddTexturesPack()
        {
            Singleton.Content.AddTextureContent(DefaultSprite);
            Singleton.Content.AddTextureContent(SpriteClicked);
            Singleton.Content.AddTextureContent(SpriteOver);
            Singleton.Content.AddTextureContent(DisabledSprite);
        }
        public override void AddSpriteRenderer(SpriteBatch batch)
        {
            _spriteRenderer = batch;
        }
        public override void AddDefaultRenderers(UIObject item)
        {
            item.AddSpriteRenderer(_spriteRenderer);
            item.AddStringRenderer(_stringRenderer);
        }

        public override void Resize(Point amount)
        {
            Size += amount;
        }
        public override void ResetSize()
        {
            Size = DefaultSize;
        }
        public static BasicSprite operator -(BasicSprite a, Point b)
        {
            a.Position = b;
            return a;
        }
        public static BasicSprite operator -(BasicSprite a, Rectangle b)
        {
            a.Rect = b;
            return a;
        }
        public static BasicSprite operator +(BasicSprite a, Point b)
        {
            a.Position = b;
            return a;
        }
        public static BasicSprite operator +(BasicSprite a, Rectangle b)
        {
         
            a.Rect = b;       
            return a;
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

            return null;
        }
        public override void Update(GameTime gameTime)
        {

            if (Active)
            {
                base.Update(gameTime);
            }
        }       
        public void UpdateTexture(string textureName)
        {
            DefaultSprite = Singleton.Content.AddTexture(textureName);
            SpriteClicked = Singleton.Content.AddTexture(textureName + "Clicked");
            SpriteOver = Singleton.Content.AddTexture(textureName + "Over");
            DisabledSprite = Singleton.Content.AddTexture(textureName + "Disabled");
            Singleton.Content.LoadResources();

            if (SpriteOver!= null && SpriteOver.Texture == null)
                SpriteOver = DefaultSprite;

            if (SpriteClicked != null && SpriteClicked.Texture == null)
                SpriteClicked = DefaultSprite;

            if (DisabledSprite != null && DisabledSprite.Texture == null)
                DisabledSprite = DefaultSprite;
          
        }
        public override void Draw()
        {
            if (Active)
            {
                if (DefaultSprite != null)
                {

                    if (Focus == this || IsClicked)
                    {
                        _spriteRenderer.Draw(SpriteClicked, Rect, ColorValue.Color * Alpha);

                    }
                    else
                    {
                        if (IsMouseOver)
                        {
                            if (Overlay == OverlayOption.NESTED)
                            {
                                if (IsClicked)
                                {
                                    _spriteRenderer.Draw(SpriteClicked, Rect, ColorValue.Color * Alpha);
                                    _spriteRenderer.Draw(SpriteOver, Rect, ColorValue.Color * Alpha);
                                }
                                else
                                {
                                    _spriteRenderer.Draw(SpriteOver, Rect, ColorValue.Color * Alpha);
                                }
                            }
                            else
                            {

                                _spriteRenderer.Draw(SpriteOver, Rect, ColorValue.Color * Alpha);
                            }
                        }
                        else
                        {
                            _spriteRenderer.Draw(DefaultSprite, Rect, ColorValue.Color * Alpha);
                        }
                    }
                }
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
