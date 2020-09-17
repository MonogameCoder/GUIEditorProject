using System;
using Microsoft.Xna.Framework;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using static _GUIProject.AssetManager;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using _GUIProject.Events;

namespace _GUIProject.UI
{  
  
    public class Sprite : UIObject
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
        private TextureContent texture;        
        private TextureContent textureClicked;
        private TextureContent textureOver;
        private TextureContent textureDisabled;
        public TextureContent Texture 
        {
            get { return texture; }
            set
            {
                texture = Singleton.Content.AddTexture(value.Name);
            }
        }
        [XmlIgnore]
        public TextureContent TextureClicked
        {
            get { return textureClicked; }
            set
            {
                textureClicked = Singleton.Content.AddTexture(value.Name);
            }
        }
        [XmlIgnore]
        public TextureContent TextureOver
        {
            get { return textureOver; }
            set
            {
                textureOver = Singleton.Content.AddTexture(value.Name);
            }
        }
        [XmlIgnore]
        public TextureContent TextureDisabled
        {
            get { return textureDisabled; }
            set
            {
                textureDisabled = Singleton.Content.AddTexture(value.Name);
            }
        }      
       
        [XmlIgnore]      
        public override Point Size
        {
            get { return (Point)Rect.Size; }
            set { Rect = new Rectangle(Position, value); }
        }
        protected Point _position;
       
        [XmlIgnore]
        public override Point Position
        {
            get { return _position; }
            set
            {
                if (_position != value)
                {                    
                    _position = value;
                    Rect = new Rectangle(value, Rect.Size);
                }
            }
        }      
      
        public Sprite()
        {          
         
            MoveState = MoveOption.DYNAMIC;
            Priority = DrawPriority.NORMAL;
            SpriteColor = Color.White;
            MouseEvent = new MouseEvents(this);
        }
        
        public Sprite(string baseTX, DrawPriority priority, MoveOption moveState = MoveOption.STATIC) 
        {           
            Texture = Singleton.Content.AddTexture(baseTX);
            TextureClicked = Singleton.Content.AddTexture(baseTX + "Clicked");
            TextureOver = Singleton.Content.AddTexture(baseTX + "Over");
            TextureDisabled = Singleton.Content.AddTexture(baseTX + "Disabled");

            Priority = priority;
            MoveState = moveState;
            SpriteColor = Color.White;
            MouseEvent = new MouseEvents(this);
        }

        public Sprite(string baseTX, DrawPriority priority, OverlayOption overlay, MoveOption state = MoveOption.STATIC)
        {

            Texture = Singleton.Content.AddTexture(baseTX);
            TextureClicked = Singleton.Content.AddTexture(baseTX + "Clicked");
            TextureOver = Singleton.Content.AddTexture(baseTX + "Over");
            TextureDisabled = Singleton.Content.AddTexture(baseTX + "Disabled");            

            Priority = priority;
            MoveState = state;
            Overlay = overlay;
            SpriteColor = Color.White;
            MouseEvent = new MouseEvents(this);
        }
        public override void Initialize()
        { 
            Active = true;
        }
       
        public override void Setup()
        {            

            Singleton.Content.LoadResources();
           
            if (TextureOver != null && TextureOver.Texture == null)
                TextureOver = Texture;

            if (TextureClicked != null && TextureClicked.Texture == null)
                TextureClicked = Texture;

            if (TextureDisabled != null && TextureDisabled.Texture == null)
                TextureDisabled = Texture;

            Rect = new Rectangle(Position.X, Position.Y, Texture.Width, Texture.Height);
            DefaultSize = Rect.Size;

        }
        public void AddTexture(string txName)
        {
            Texture = Singleton.Content.AddTexture(txName);
        }
        public void AddTextureOver(string txName)
        {
            TextureOver = Singleton.Content.AddTexture(txName);
        }
        public void AddTextureClicked(string txName)
        {
            TextureClicked = Singleton.Content.AddTexture(txName);
        }

        public void AddTextureDisabled(string textureName)
        {
            TextureDisabled = Singleton.Content.AddTexture(textureName);
        }
        public override void AddStringRenderer(SpriteBatch batch)
        {
            _stringRenderer = batch;
        }
        public void AddTexturesPack()
        {
            Singleton.Content.AddTextureContent(Texture);
            Singleton.Content.AddTextureContent(TextureClicked);
            Singleton.Content.AddTextureContent(TextureOver);
            Singleton.Content.AddTextureContent(TextureDisabled);
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
        public static Sprite operator -(Sprite a, _GUIProject.Point b)
        {
            a.Position = b;
            return a;
        }
        public static Sprite operator -(Sprite a, Rectangle b)
        {
            a.Rect = b;
            return a;
        }
        public static Sprite operator +(Sprite a, _GUIProject.Point b)
        {
            a.Position = b;
            return a;
        }
        public static Sprite operator +(Sprite a, Rectangle b)
        {
         
            a.Rect = b;       
            return a;
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
            Texture = Singleton.Content.AddTexture(textureName);
            TextureClicked = Singleton.Content.AddTexture(textureName + "Clicked");
            TextureOver = Singleton.Content.AddTexture(textureName + "Over");
            TextureDisabled = Singleton.Content.AddTexture(textureName + "Disabled");
            Singleton.Content.LoadResources();

            if (TextureOver!= null && TextureOver.Texture == null)
                TextureOver = Texture;

            if (TextureClicked != null && TextureClicked.Texture == null)
                TextureClicked = Texture;

            if (TextureDisabled != null && TextureDisabled.Texture == null)
                TextureDisabled = Texture;
          
        }
        public override void Draw()
        {
            if (Active)
            {
                if (Texture != null && Texture.Texture != null)
                {

                    if (MouseGUI.Focus == this || IsClicked)
                    {
                        _spriteRenderer.Draw(TextureClicked, Rect, SpriteColor * Alpha);

                    }
                    else
                    {
                        if (IsMouseOver)
                        {
                            if (Overlay == OverlayOption.NESTED)
                            {
                                if (IsClicked)
                                {
                                    _spriteRenderer.Draw(TextureClicked, Rect, SpriteColor * Alpha);
                                    _spriteRenderer.Draw(TextureOver, Rect, SpriteColor * Alpha);
                                }
                                else
                                {
                                    _spriteRenderer.Draw(TextureOver, Rect, SpriteColor * Alpha);
                                }
                            }
                            else
                            {

                                _spriteRenderer.Draw(TextureOver, Rect, SpriteColor * Alpha);
                            }
                        }
                        else
                        {
                            _spriteRenderer.Draw(Texture, Rect, SpriteColor * Alpha);
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
