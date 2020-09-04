using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Xml.Serialization;


using static _GUIProject.UI.BasicSprite;


using System.Reflection;
using System.Reflection.Emit;

using System.CodeDom.Compiler;

using System.Text;
using _GUIProject.Events;
using System.Linq;
using System.Dynamic;
using static _GUIProject.InputManager;
using static _GUIProject.AssetManager;
using static _GUIProject.Events.MouseEvents;

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

        public Point DefaultSize
        {
            get;
            protected set;
        }       
       
        public float Alpha { get; set; } = 1.0f;
        public bool AlphaIncrease { get; private set; } = false;
        public PropertyPanel Property { get; set; }
        public virtual string Text { get; set; } = "";
        public string Name { get; set; }
        public virtual ColorObject ColorValue { get; set; }
        public virtual ColorObject TextColor { get; set; }   
        public MouseEvents MouseEvent { get; set; }
        public KeyboardEvents KeyboardEvents { get; set; }        
        public UIObject Parent { get; set; }  
        public bool Active { get; set; }           
        public bool Editable { get; set; }
        public bool Locked { get; set; }
        public bool IsClicked { get; set; } 
        public bool IsMouseOver { get; set; }
        public bool FadeCapable { get; set; }              
        public OverlayOption Overlay { get; set; }        
        public MoveOption MoveState { get; set; }        
        public DrawPriority Priority { get; set; }
        public SizePolicy XPolicy { get; set; }
        public SizePolicy YPolicy { get; set; }
     
        public abstract Point Position { get; set; }
        public abstract Point Size { get; set; }
        public Rectangle Rect { get; set; }
        public int Height
        {
            get { return Rect.Height; }
        }
        public int Width
        {
            get { return Rect.Width; }
        }
        public int Top
        {
            get { return Rect.Top; }
        }
        public int Right
        {
            get { return Rect.Right; }
        }
        public int Bottom
        {
            get { return Rect.Bottom; }
        }
        public int Left
        {
            get { return Rect.Left; }
        }

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

        protected SpriteBatch _spriteRenderer;
        protected SpriteBatch _stringRenderer;
        private const float FADE_SPEED = 2.0f;
        private const float MAX_ALPHA = 1.0f;
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
