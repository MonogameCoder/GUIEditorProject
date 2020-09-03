using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace _GUIProject.UI
{

    public class SliderBar : BasicSprite
    {
        enum PalleteMode
        {
            FLOAT,
            INT
        }
        Button _sliderButton;
        ToolTip _toolTip;
        PalleteMode _valueMode;

        public byte PalleteValue { get; set; }
        public float PalleteFloatValue { get; set; }
        public SliderBar() : base("DefaultSliderBarTX", DrawPriority.NORMAL)
        {
        }
        public override void Initialize()
        {
            base.Initialize();
            _sliderButton = new Button("DefaultSliderBarSliderTX", OverlayOption.NORMAL, DrawPriority.LOW);
            _sliderButton.Initialize();

            _sliderButton.Text = "";
            _toolTip = new ToolTip(_sliderButton);
            _toolTip.Initialize();
            _toolTip.Text = "";

            _valueMode = PalleteMode.FLOAT;
            MoveState = MoveOption.DYNAMIC;
            XPolicy = SizePolicy.FIXED;
            YPolicy = SizePolicy.FIXED;

            PalleteValue = 0;
            Active = true;
        }
        public override void InitPropertyPanel()
        {
            Property = new PropertyPanel(this);
            Property.AddProperties(PropertyPanel.PropertyOwner.SLIDER);
            Property.SetupProperties();
        }
        public override void Setup()
        {
            base.Setup();
            _sliderButton.Setup();
            _toolTip.Setup();
            _sliderButton.Position = new Point(Rect.Center.X - _sliderButton.Width / 2, Rect.Center.Y - _sliderButton.Height / 2);

            _sliderButton.Show();
        }
        public override void AddSpriteRenderer(SpriteBatch batch)
        {
            _sliderButton.AddSpriteRenderer(batch);
            _toolTip.AddSpriteRenderer(batch);
            base.AddSpriteRenderer(batch);
        }
        public override void AddStringRenderer(SpriteBatch batch)
        {
            _sliderButton.AddStringRenderer(batch);
            _toolTip.AddStringRenderer(batch);
            base.AddStringRenderer(batch);
        }
        public override void AddPropertyRenderer(SpriteBatch batch)
        {
            Property.AddPropertyRenderer(batch);
        }
        public override UIObject HitTest(Point mousePosition)
        {
            UIObject result = null;
            if (Active)
            {
                result = _sliderButton.HitTest(mousePosition);


                if (result == null)
                {
                    result = base.HitTest(mousePosition);
                }
            }
            if (result == null)
            {
                if (Property != null && MainWindow.CurrentObject == this)
                {
                    result = Property.HitTest(mousePosition);
                }
            }
            return result;
        }
        public bool IsOutOfBounds(int x)
        {
            if (x + _sliderButton.Width > Right || x < Left)
            {
                return true;
            }
            return false;
        }
        public override void Update(GameTime gameTime)
        {
            if (Active)
            {
                if (!Editable && _valueMode == PalleteMode.FLOAT)
                {
                    int pos = _sliderButton.Center.X > Center.X ? _sliderButton.Right - Center.X :
                              _sliderButton.Center.X < Center.X ? Center.X - _sliderButton.Left : 0;
                    PalleteFloatValue = (float)Math.Round(pos / ((float)Width / 2), 2);
                    _toolTip.Text = PalleteFloatValue.ToString();
                }

                _toolTip.Update(gameTime);
                if (Locked)
                {
                    if (MouseGUI.Focus == _sliderButton)
                    {
                        _toolTip.Show();
                        Point newPosition = new Point(MouseGUI.DragOffset.X + MouseGUI.Position.X, _sliderButton.Top);

                        if (!IsOutOfBounds(newPosition.X))
                        {
                            _sliderButton.Position = newPosition;
                        }
                        else
                        {
                            int right = newPosition.X + _sliderButton.Width;
                            if (right > Right)
                            {

                                _sliderButton.Position = new Point(Right - _sliderButton.Width, _sliderButton.Top);
                            }
                            if (_sliderButton.Left < Left)
                            {
                                _sliderButton.Position = new Point(Left, _sliderButton.Top);
                            }
                        }
                    }
                }
                else
                {
                    _toolTip.Hide();

                    _sliderButton.Position = new Point(Center.X - _sliderButton.Width / 2, Center.Y - _sliderButton.Height / 2);
                    _toolTip.Text = "";


                }

                _sliderButton.Update(gameTime);
                _toolTip.Update(gameTime);
                base.Update(gameTime);
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
                _sliderButton.Alpha = Alpha;
                _sliderButton.Draw();
                _toolTip.Draw();
            }
            if (Property != null)
            {
                Property.Draw();
            }
        }
        public override void Show()
        {
            Active = true;
            _sliderButton.Active = true;
        }
        public override void Hide()
        {
            Active = false;
            _sliderButton.Active = false;
        }

    }
}
