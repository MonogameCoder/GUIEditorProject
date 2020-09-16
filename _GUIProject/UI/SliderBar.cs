using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.Serialization;
using System.Diagnostics;
using System.Xml.Serialization;

namespace _GUIProject.UI
{

    public class SliderBar : Sprite
    {
        public enum PalleteMode
        {
            FLOAT,
            INT
        }     

        [XmlIgnore]
        public byte PalleteValue { get; set; }
        [XmlIgnore]
        public float PalleteFloatValue { get; set; }

        [XmlAttribute]
        public PalleteMode Mode { get; set; }

        private Button _slider;
        private ToolTip _toolTip;

        public SliderBar() : base("DefaultSliderBarTX", DrawPriority.NORMAL)
        {
            LoadAttributes();   
        }
        void LoadAttributes()
        {
            _slider = new Button("DefaultSliderBarSliderTX", OverlayOption.NORMAL, DrawPriority.LOW);            
            _toolTip = new ToolTip(_slider);           
            _slider.Text = "";
            Mode = PalleteMode.FLOAT;
        }
        public override void Initialize()
        {
            base.Initialize();
            _toolTip.Initialize();
            _slider.Initialize();
         
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
            _slider.Setup();
            _toolTip.Setup();
            _slider.Position = new Point(Rect.Center.X - _slider.Width / 2, Rect.Center.Y - _slider.Height / 2);

            _slider.Show();
        }
        public override void AddSpriteRenderer(SpriteBatch batch)
        {
            _slider.AddSpriteRenderer(batch);
            _toolTip.AddSpriteRenderer(batch);
            base.AddSpriteRenderer(batch);
        }
        public override void AddStringRenderer(SpriteBatch batch)
        {
            _slider.AddStringRenderer(batch);
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
                result = _slider.HitTest(mousePosition);


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
            if (x + _slider.Width > Right || x < Left)
            {
                return true;
            }
            return false;
        }
        public override void Update(GameTime gameTime)
        {
            if (Active)
            {
                if (!Editable && Mode == PalleteMode.FLOAT)
                {
                    int pos = _slider.Center.X > Center.X ? _slider.Right - Center.X :
                              _slider.Center.X < Center.X ? Center.X - _slider.Left : 0;
                    PalleteFloatValue = (float)Math.Round(pos / ((float)Width / 2), 2);
                    _toolTip.Text = PalleteFloatValue.ToString();
                }

                _toolTip.Update(gameTime);
                if (Locked)
                {
                    if (MouseGUI.Focus == _slider)
                    {
                        _toolTip.Show();
                        Point newPosition = new Point(MouseGUI.DragOffset.X + MouseGUI.Position.X, _slider.Top);

                        if (!IsOutOfBounds(newPosition.X))
                        {
                            _slider.Position = newPosition;
                        }
                        else
                        {
                            int right = newPosition.X + _slider.Width;
                            if (right > Right)
                            {

                                _slider.Position = new Point(Right - _slider.Width, _slider.Top);
                            }
                            if (_slider.Left < Left)
                            {
                                _slider.Position = new Point(Left, _slider.Top);
                            }
                        }
                    }
                }
                else
                {
                    _toolTip.Hide();

                    _slider.Position = new Point(Center.X - _slider.Width / 2, Center.Y - _slider.Height / 2);
                    _toolTip.Text = "";


                }

                _slider.Update(gameTime);
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
                _slider.Alpha = Alpha;
                _slider.Draw();
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
            _slider.Active = true;
        }
        public override void Hide()
        {
            Active = false;
            _slider.Active = false;
        }

        public override bool ShouldSerializeTextColor() { return false; }

    }
}
