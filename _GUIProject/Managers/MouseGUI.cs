
using _GUIProject.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using static _GUIProject.UI.Sprite;
using static _GUIProject.UI.UIObject;

namespace _GUIProject.UI
{
   

    public static class MouseGUI
    {

        public enum PointerType
        {
            MOUSE_ARROW,
            MOUSE_HAND,
            MOUSE_HAND_CLOSED
        }
        // Declare these outside the Update() function
        static int deltaScrollWheelValue = 0;
        static int currentScrollWheelValue = 0;
        const int SCROLL_DIVISOR = 120;
        public static int ScrollerValue = 0;

        public static Sprite _mouseEdit;
        public static Sprite _mouseScale;      
        public static Sprite MousePointer { get { return _mouseScale; } }
        public static Sprite MouseEdit 
        { 
            get { return _mouseEdit; }            
        }

        public static MouseState _prevState = Mouse.GetState();
        public static MouseState _curState = Mouse.GetState();
        public static Point Position = (Point)_curState.Position;
       

        static UIObject _hitObject;       
        public static UIObject HitObject
        {
            get { return _hitObject; }
            set { _hitObject = value; }
        }

        static UIObject _focus;     
                                    
        public static UIObject Focus
        {
            get { return _focus; }
            set { _focus = value; }
        }

        public static Point DragOffset;            
                                                    
        public static PointerType mouseMode = PointerType.MOUSE_ARROW;
      
        public static bool LeftIsPressed = false;       
        public static bool LeftWasPressed = false;      
        public static bool LeftWasReleased = false;     

        public static bool RightIsPressed = false;      
        public static bool RightWasPressed = false;     
        public static bool RightWasReleased = false;   
        public static bool LeftWasDoubleClicked = false;
        public static bool isScaleMode = false;

        public static void ClearFocus()
        {
            _focus = null;
        }
        public static void Load()
        {           
            _mouseScale = new Sprite("MouseScaleCursorSmallTX", DrawPriority.HIGHEST);
            _mouseEdit = TextBox.Pointer;
            _mouseEdit.SpriteColor = new ColorObject()
            {
                Color = Color.Black
            };

            _mouseScale.Initialize();
          
        }

        public static void Setup()
        {
            _mouseScale.Setup();          
            _mouseEdit.Active = false;
            
            _mouseEdit.AddSpriteRenderer(MainWindow._mainBatch);
            _mouseEdit.AddStringRenderer(MainWindow._mainBatch);
        }
        public static void AddSpriteRenderer(SpriteBatch batchRenderer)
        {
            _mouseScale.AddSpriteRenderer(batchRenderer);
        }
        public static void Update()
        {
            _prevState = _curState;
            _curState = Mouse.GetState();

            deltaScrollWheelValue = _curState.ScrollWheelValue - currentScrollWheelValue;
            currentScrollWheelValue += deltaScrollWheelValue;
            ScrollerValue = deltaScrollWheelValue / SCROLL_DIVISOR;

          
            Position = _curState.Position;
            Point CenterPosition = new Point(Position.X - _mouseScale.Width / 2, Position.Y - _mouseScale.Height / 2);
            _mouseScale.Position = CenterPosition;

            LeftIsPressed = _curState.LeftButton == ButtonState.Pressed;
            LeftWasPressed = LeftIsPressed && _prevState.LeftButton == ButtonState.Released;
            LeftWasReleased = !LeftIsPressed && _prevState.LeftButton == ButtonState.Pressed;


            RightIsPressed = _curState.RightButton == ButtonState.Pressed;
            RightWasPressed = RightIsPressed && _prevState.RightButton == ButtonState.Released;
            RightWasReleased = !RightIsPressed && _prevState.RightButton == ButtonState.Pressed;


    

            if (HitObject != null && Focus == null)
            {
                Point pt = new Point(HitObject.Right, HitObject.Bottom);
                Point length = (HitObject.Size * 0.1f);
                Rectangle corner = new Rectangle(pt.X - length.X, pt.Y - length.Y, length.X, length.Y);
                isScaleMode = corner.Contains(Position.ToPoint());
            }
            if (isScaleMode)
            {
                if (Focus != null && Focus.Locked)
                {
                    Point corner = new Point(Focus.Right, Focus.Bottom);

                    Point scaleAmt = (Position - corner);

                    Point newSize = new Point(scaleAmt.X + _mouseScale.Width / 4, scaleAmt.Y + _mouseScale.Height / 4);
                    Focus.Resize(newSize);

                }
            }
            if (HitObject is TextArea)
            {
                TextArea ta = HitObject as TextArea;
                if (!ta.Editable && !isScaleMode)
                {
                    MainWindow.MainInstance.HideMouse();
                    MouseEdit.Show();
                }
            }
            else
            {
                MouseEdit.Hide();
                MainWindow.MainInstance.ShowMouse();
            }
          
            _mouseEdit.Position = new Point(Position.X - _mouseEdit.Width /2, Position.Y - _mouseEdit.Height /2);
        }
        public static void Draw()
        {
            if (HitObject != null)
            {
                if (isScaleMode && HitObject.Locked)
                {
                    _mouseScale.Draw();
                }
                if(_mouseEdit.Active)
                {
                    _mouseEdit.Draw();
                }
           
            }

        }
        
    }
}
