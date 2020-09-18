using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Xml.Serialization;


using static _GUIProject.UI.Sprite;


using System.Reflection;
using System.Reflection.Emit;

using System.CodeDom.Compiler;

using System.Text;

using System.Linq;
using System.Dynamic;

using static _GUIProject.AssetManager;
using _GUIProject.Events;

namespace _GUIProject.UI
{  
    public abstract class UIObject : IObject, IComparable
    {
        public enum DrawPriority
        {
            LOWEST,
            LOW,
            NORMAL,
            HIGH,
            HIGHEST
        }
        public enum SizePolicy
        {
            FIXED,
            EXPAND
        }
        public enum MoveOption
        {
            DYNAMIC,
            STATIC,
            FLEXIBLE
        }

        [XmlIgnore]
        public virtual Point DefaultSize { get; set; }

        [XmlIgnore]
        public float Alpha { get; set; } = 1.0f;

        [XmlIgnore]
        public bool AlphaIncrease { get; private set; } = false;

        [XmlIgnore]
        public virtual string Text { get; set; } = "";

        [XmlAttribute]
        public string Name { get; set; }
        public virtual ColorObject SpriteColor { get; set; }
        public virtual ColorObject TextColor { get; set; }
        [XmlIgnore]
        public MouseEvents MouseEvent { get; set; }
        [XmlIgnore]
        public KeyboardEvents KeyboardEvents { get; set; }
        [XmlIgnore]
        public PropertyPanel Property { get; set; }

        [XmlIgnore]
        public UIObject Parent { get; set; }

        [XmlIgnore]
        public bool Active { get; set; }
        [XmlIgnore]
        public bool Editable { get; set; }
        [XmlIgnore]
        public bool Locked { get; set; }
        [XmlIgnore]
        public bool IsClicked { get; set; }
        [XmlIgnore]
        public bool IsMouseOver { get; set; }
        [XmlIgnore]
        public bool FadeCapable { get; set; }
        [XmlIgnore]

        public OverlayOption Overlay { get; set; }
        [XmlIgnore]

        public MoveOption MoveState { get; set; }

        [XmlAttribute]
        public DrawPriority Priority { get; set; }
        [XmlAttribute]
        public SizePolicy XPolicy { get; set; }
        [XmlAttribute]
        public SizePolicy YPolicy { get; set; }

        [XmlIgnore]

        public abstract Point Position { get; set; }
        [XmlIgnore]

        public abstract Point Size { get; set; }
        [XmlIgnore]

        public Rectangle Rect { get; set; }

        [XmlAttribute]
        public virtual int Height
        {
            get { return Rect.Height; }
            set { Rect = new Rectangle(Position, new Point(Size.X, value)); }
        }      

        [XmlAttribute]        
        public virtual int Width
        {
            get { return Rect.Width; }
            set { Rect = new Rectangle(Position, new Point(value, Size.Y)); }
        }       
        [XmlIgnore]
        public int Top
        {
            get { return Rect.Top; }
        }
        [XmlIgnore]
        public int Right
        {
            get { return Rect.Right; }
        }
        [XmlIgnore]
        public int Bottom
        {
            get { return Rect.Bottom; }
        }
        [XmlIgnore]
        public int Left
        {
            get { return Rect.Left; }
        }
        [XmlIgnore]
        public Point Center
        {
            get { return Rect.Center; }
        }
       

        public abstract void Initialize();
        public virtual void InitPropertyPanel()
        {

        }
        public abstract void Setup();     
        public abstract void AddDefaultRenderers(UIObject item);
        public abstract void AddSpriteRenderer(SpriteBatch batch);
        public abstract void AddStringRenderer(SpriteBatch batch);         
        public virtual void AddPropertyRenderer(SpriteBatch batch)
        {

        }
        public abstract void Resize(Point newSize);
        public abstract void ResetSize();
        public abstract bool Contains(Point position);
        public abstract UIObject HitTest(Point mousePosition);
        [XmlIgnore]
        protected SpriteBatch _spriteRenderer;
        [XmlIgnore]
        protected SpriteBatch _stringRenderer;
        [XmlIgnore]
        private const float FADE_SPEED = 2.0f;
        [XmlIgnore]
        private const float MAX_ALPHA = 1.0f;
        [XmlIgnore]
        private const float MIN_ALPHA = 0.0f;
        public virtual void Update(GameTime gameTime)
        {
            if (FadeCapable && Active)
            {
                if (!AlphaIncrease)
                    Alpha -= FADE_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
                else
                    Alpha += FADE_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (Alpha <= MIN_ALPHA)
                {
                    AlphaIncrease = true;
                    Alpha = MIN_ALPHA;
                }
                else if (Alpha >= MAX_ALPHA)
                {
                    AlphaIncrease = false;
                    Alpha = MAX_ALPHA;
                }
            }
        }     
        public abstract void Draw();       
        public abstract void Show();
        public abstract void Hide();
        public virtual bool ShouldSerializeHeight() { return true; }
        public virtual bool ShouldSerializeTextColor() { return true; }
        public virtual bool ShouldSerializeWidth() { return true; }
        public virtual bool ShouldSerializeTexture() { return true; }
        public virtual bool ShouldSerializeSpriteColor() { return true; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public int CompareTo(object obj)
        {
            int result = Priority.CompareTo((obj as UIObject).Priority);
            if(result == 0)
            {
                result  = GetHashCode().CompareTo(obj.GetHashCode());
            }
            return result;
        }
    }
}
