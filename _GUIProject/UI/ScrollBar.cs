using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using _GUIProject.Events;

namespace _GUIProject.UI
{


    public class ScrollBar : Sprite
    {        
        public enum ScrollDirection
        {
            UP,
            DOWN,
            NONE
        }        

        public Button SliderButton { get; private set; }
        public Button UpButton { get; private set; }
        public Button DownButton { get; private set; }      

        public ScrollDirection ScrollerDirection
        { 
            get { return direction; } 
        }

        public int CurrentScrollValue { get; set; } = 0;

        private Frame _itemsContainer;
        private ScrollEvents _scrollEvent;
        private ScrollDirection direction;

        public ScrollBar() : base("DefaultScrollbarTX", DrawPriority.LOW)
        {
            XPolicy = SizePolicy.EXPAND;
            YPolicy = SizePolicy.FIXED;      
        }

       
        public override void Initialize()
        {
            base.Initialize();
            _itemsContainer = new Frame("DefaultScrollbarTX", DrawPriority.NORMAL);          

            UpButton = new Button("DefaultScrollBarUpTX", OverlayOption.NORMAL, DrawPriority.LOW); 
            UpButton.Text = "";

            DownButton = new Button("DefaultScrollBarDownTX", OverlayOption.NORMAL, DrawPriority.LOW);  
            DownButton.Text = "";

            SliderButton = new Button("DefaultScrollBarBARTX", OverlayOption.NORMAL, DrawPriority.LOW);  
            SliderButton.Text = "";
            _scrollEvent = new ScrollEvents();
         

            _itemsContainer.Initialize();

        }
   
        public override void Setup()
        {
            base.Setup();
            Size = new Point(Size.X, Parent.Size.Y);
            _itemsContainer.Position = Position;
            _itemsContainer.Setup();

            UpButton.Setup();
            DownButton.Setup();
            SliderButton.Setup();

            UpButton.Position = new Point(Left, Top);
            DownButton.Position = new Point(Left, Parent.Height - DownButton.Height);
            SliderButton.Position = new Point(Left, UpButton.Bottom + CurrentScrollValue);

            _itemsContainer.AddItem(UpButton.Position, UpButton);
            _itemsContainer.AddItem(DownButton.Position, DownButton);
            _itemsContainer.AddItem(SliderButton.Position, SliderButton);

            _scrollEvent.onScroll += ScrollBar_onScrollEvent;

            UpButton.MouseEvent.onMouseClick += (sender, args) =>
            {
                
                    var slider = _itemsContainer[SliderButton].Position;
                    _itemsContainer.UpdateSlot(SliderButton, new Point(slider.X, slider.Y - 1));
                    _scrollEvent.OnScroll(Parent, ScrollDirection.UP , -1);
              
            };
            DownButton.MouseEvent.onMouseClick += (sender, args) =>
            {
                               
                    var slider = _itemsContainer[SliderButton].Position;
                    _itemsContainer.UpdateSlot(SliderButton, new Point(slider.X, slider.Y + 1));
                    _scrollEvent.OnScroll(Parent, ScrollDirection.DOWN, 1);
               
            };
           
          
        }
        private void ScrollBar_onScrollEvent(object sender, ScrollEventArgs e)
        {
            direction = e.Direction;
            var parent = (e.Owner as IScrollable);

            if (direction == ScrollDirection.DOWN)
            {
                if (GetBounds(e.ScrollValue) == 0)
                {
                    if (CurrentScrollValue + e.ScrollValue < parent.NumberOfLines - parent.MaxLinesLength)
                    {
                        CurrentScrollValue+= e.ScrollValue;
                        parent.ApplyScroll();
                    }
                   
                }
                else
                {
                    var down = _itemsContainer[DownButton].Position;
                    _itemsContainer.UpdateSlot(SliderButton, new Point(down.X, down.Y - SliderButton.Height));

                }
            }
            else
            {
                if (GetBounds(e.ScrollValue) == 0)
                {

                    if(CurrentScrollValue + e.ScrollValue >= 0)
                    {
                        CurrentScrollValue+= e.ScrollValue;
                        parent.ApplyScroll();
                    }                 
                   
                }
                else
                {
                    var up = _itemsContainer[UpButton].Position;
                    _itemsContainer.UpdateSlot(SliderButton, new Point(up.X, up.Y + UpButton.Height));                  

                }
            }
        }
        public void ResetScroll()
        {
            CurrentScrollValue = 0;
            var up = _itemsContainer[UpButton].Position;
            _itemsContainer.UpdateSlot(SliderButton, new Point(up.X, up.Y + UpButton.Height));
        }
        public override void AddSpriteRenderer(SpriteBatch batch)
        {
            _itemsContainer.AddSpriteRenderer(batch);
            base.AddSpriteRenderer(batch);
        }
        public override void AddStringRenderer(SpriteBatch batch)
        {
            _itemsContainer.AddStringRenderer(batch);
            base.AddStringRenderer(batch);
        }
        public override void ResetSize()
        {
            _itemsContainer.UpdateSlot(DownButton, new Point(0, Height - DownButton.Height));
            base.ResetSize();
        }
        public override void Resize(Point amount)
        {
            base.Resize(new Point(0, amount.Y));
            _itemsContainer.UpdateSlot(DownButton, new Point(0, Height - DownButton.Height));
        }
        public override UIObject HitTest(Point mousePosition)
        {
            UIObject result = null;
            if (Active)
            {
                result = _itemsContainer.HitTest(mousePosition);

                if (result == null)
                {
                    return base.HitTest(mousePosition);
                }           
            }
            return result;
        }
        public int GetBounds(int y)
        {
            if (SliderButton.Bottom + y > DownButton.Top)
            {
                return 1;
            }
            else if (SliderButton.Top + y < UpButton.Bottom)
            {
                return -1;
            }
            return 0;
        }

        public override void Update(GameTime gameTime)
        {

            _itemsContainer.Position = new Point(Left, Top);         
            //MultiTextBox mtb = Parent as MultiTextBox;       

            Point lastPosition = SliderButton.Position;
            Point lastMousePosition = MouseGUI.Position;
            _itemsContainer.Update(gameTime);
            if (MouseGUI.Focus == SliderButton)
            {
                Point delta = MouseGUI.Position - SliderButton.Center;

                direction = delta.Y > 0 ? ScrollDirection.DOWN : delta.Y < 0 ? ScrollDirection.UP : ScrollDirection.NONE;

                if (direction == ScrollDirection.DOWN)
                {
                    if (GetBounds(delta.Y) == 0)
                    {
                        var slider = _itemsContainer[SliderButton].Position;
                        _itemsContainer.UpdateSlot(SliderButton, new Point(slider.X, slider.Y + delta.Y));

                        _scrollEvent.OnScroll(Parent, ScrollDirection.DOWN, delta.Y);
                    }
                    else
                    {
                        var down = _itemsContainer[DownButton].Position;
                        _itemsContainer.UpdateSlot(SliderButton, new Point(down.X, down.Y - SliderButton.Height));

                    }
                }
                else
                {
                    if (direction == ScrollDirection.UP)
                    {
                        if (GetBounds(delta.Y) == 0)
                        {
                            var slider = _itemsContainer[SliderButton].Position;
                            _itemsContainer.UpdateSlot(SliderButton, new Point(slider.X, slider.Y + delta.Y));

                            _scrollEvent.OnScroll(Parent, ScrollDirection.UP, delta.Y);
                        }
                        else
                        {
                            var up = _itemsContainer[UpButton].Position;
                            _itemsContainer.UpdateSlot(SliderButton, new Point(up.X, up.Y + UpButton.Height));
                        }

                    }
                }
            }
            else
            {
                IScrollable parent = (Parent as IScrollable);
                if (parent.NumberOfLines > parent.MaxLinesLength)
                {
                    var slot = _itemsContainer[SliderButton];
                    int delta = (slot.Position.Y + CurrentScrollValue) - slot.Position.Y;

                    if (delta > 0)
                    {
                        _itemsContainer.UpdateSlot(SliderButton, new Point(slot.Position.X, DownButton.Height + CurrentScrollValue));
                    }
                }

            }       
          
            base.Update(gameTime);          

        }

        public override void Draw()
        {
            base.Draw();

            _itemsContainer.Draw();
          
        }
        public override void Show()
        {
            base.Show();
            _itemsContainer.Show();
        }
        public override void Hide()
        {
            base.Hide();
            _itemsContainer.Hide();
        }

    }
}
