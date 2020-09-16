
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

        public static Sprite _mouseScale;      
        public static Sprite MousePointer { get { return _mouseScale; } }

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
        public static int ScrollerValue = _curState.ScrollWheelValue;
        public static bool LeftWasDoubleClicked = false;
        public static bool isScaleMode = false;

        public static void ClearFocus()
        {
            _focus = null;
        }
        public static void Load()
        {
            _mouseScale = new Sprite("MouseScaleCursorSmallTX", DrawPriority.HIGHEST);
            _mouseScale.Initialize();
        }

        public static void Setup()
        {
            _mouseScale.Setup();
        }
        public static void AddSpriteRenderer(SpriteBatch batchRenderer)
        {
            _mouseScale.AddSpriteRenderer(batchRenderer);
        }
        public static void Update()
        {
           
            _prevState = _curState;
            _curState = Mouse.GetState();

            Position = (Point)_curState.Position;
            _GUIProject.Point CenterPosition = new _GUIProject.Point(Position.X - _mouseScale.Width / 2, Position.Y - _mouseScale.Height / 2);
            _mouseScale.Position = CenterPosition;

            LeftIsPressed = _curState.LeftButton == ButtonState.Pressed;
            LeftWasPressed = LeftIsPressed && _prevState.LeftButton == ButtonState.Released;
            LeftWasReleased = !LeftIsPressed && _prevState.LeftButton == ButtonState.Pressed;


            RightIsPressed = _curState.RightButton == ButtonState.Pressed;
            RightWasPressed = RightIsPressed && _prevState.RightButton == ButtonState.Released;
            RightWasReleased = !RightIsPressed && _prevState.RightButton == ButtonState.Pressed;


            ScrollerValue = _curState.ScrollWheelValue;

            if (HitObject != null && Focus == null)
            {
                UIObject sprite = HitObject;
                float radiusX = sprite.Size.X * 0.3f;
                float radiusY = sprite.Size.Y * 0.3f;
                Point cornerPosition = new Point(sprite.Rect.Right, sprite.Rect.Bottom);
                bool xHit = Position.X >= cornerPosition.X - radiusX && Position.X <= cornerPosition.X;
                bool yHit = Position.Y >= cornerPosition.Y - radiusY && Position.Y <= cornerPosition.Y;
                isScaleMode = yHit && xHit;
            }
            if(isScaleMode)
            {
                if (Focus != null && Focus.Locked)
                {
                    Point cornerPosition = new Point(Focus.Rect.Right, Focus.Rect.Bottom);

                    Point changeAmount = (Position - cornerPosition);

                    Point newSize = new Point(changeAmount.X + _mouseScale.Width / 4, changeAmount.Y + _mouseScale.Height / 4);
                    Focus.Resize(newSize);

                }
            }           

        }
        public static void Draw()
        {
            if (HitObject != null)
            {
                if (isScaleMode && HitObject.Locked)
                {
                   
                    _mouseScale.Draw();
                   
                }
                else
                {
                    _mouseScale.Active = false;
                 
                }

            }

        }
        
    }
}
