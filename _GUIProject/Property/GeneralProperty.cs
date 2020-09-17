using _GUIProject;
using _GUIProject.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using static _GUIProject.UI.PropertyPanel;
using static _GUIProject.UI.Sprite;
using static _GUIProject.UI.UIObject;

namespace _GUIProject
{
    public class GeneralProperty
    {
        private Label _expandLb;
        private Label _scaleLb;
        private Label _posLb;
        private Label _posXLb;
        private Label _posYLb;
        private Label _scaleXLb;
        private Label _scaleYLb;
        private Label _expandXLb;
        private Label _expandYLb;

        private InputInfoArea _scaleX;
        private InputInfoArea _scaleY;
        private InputInfoArea _posX;
        private InputInfoArea _posY;
        private CheckBox _expandX;
        private CheckBox _expandY;

        public UIObject Owner { get; private set; }
        public PropertyPanel Parent { get; private set; }
        public SortedDictionary<UIObject, Point> Properties { get; private set; }

        public GeneralProperty(PropertyPanel parent)
        {
            Properties = new SortedDictionary<UIObject, Point>();
            Owner = parent.Owner;
            Parent = parent;
        }
        public void AddProperties(PropertyOwner owner)
        {
            _posX = new InputInfoArea();
            _posY = new InputInfoArea();
            _scaleX = new InputInfoArea();
            _scaleY = new InputInfoArea();

            _expandX = new CheckBox("PropertyPanelCheckboxTX", OverlayOption.CHECKBOX, DrawPriority.HIGHEST);
            _expandY = new CheckBox("PropertyPanelCheckboxTX", OverlayOption.CHECKBOX, DrawPriority.HIGHEST);

            _scaleLb = new Label("Scale:");
            _posLb = new Label("Position:");
            _expandLb = new Label("Expand:");
            _posXLb = new Label(":x");
            _posYLb = new Label(":y");
            _scaleXLb = new Label(":x");
            _scaleYLb = new Label(":y");
            _expandXLb = new Label(":x");
            _expandYLb = new Label(":y");

            _posXLb.Priority = DrawPriority.HIGH;
            _posYLb.Priority = DrawPriority.HIGH;
            _scaleXLb.Priority = DrawPriority.HIGH;
            _scaleYLb.Priority = DrawPriority.HIGH;
            _expandXLb.Priority = DrawPriority.HIGH;
            _expandYLb.Priority = DrawPriority.HIGH;

            _scaleLb.TextFont = Singleton.Font.GetFont(FontManager.FontType.LUCIDA_CONSOLE);
            _posLb.TextFont = Singleton.Font.GetFont(FontManager.FontType.LUCIDA_CONSOLE);
            _expandLb.TextFont = Singleton.Font.GetFont(FontManager.FontType.LUCIDA_CONSOLE);
            _posXLb.TextFont = Singleton.Font.GetFont(FontManager.FontType.LUCIDA_CONSOLE);
            _posYLb.TextFont = Singleton.Font.GetFont(FontManager.FontType.LUCIDA_CONSOLE);
            _scaleXLb.TextFont = Singleton.Font.GetFont(FontManager.FontType.LUCIDA_CONSOLE);
            _scaleYLb.TextFont = Singleton.Font.GetFont(FontManager.FontType.LUCIDA_CONSOLE);
            _expandXLb.TextFont = Singleton.Font.GetFont(FontManager.FontType.LUCIDA_CONSOLE);
            _expandYLb.TextFont = Singleton.Font.GetFont(FontManager.FontType.LUCIDA_CONSOLE);

            _scaleLb.Initialize();
            _posLb.Initialize();
            _expandLb.Initialize();
            _posXLb.Initialize();
            _posYLb.Initialize();
            _scaleXLb.Initialize();
            _scaleYLb.Initialize();
            _expandXLb.Initialize();
            _expandYLb.Initialize();

            _posX.Initialize();
            _posY.Initialize();
            _scaleX.Initialize();
            _scaleY.Initialize();
            _expandX.Initialize();
            _expandY.Initialize();


            _scaleLb.Setup();
            _posLb.Setup();
            _expandLb.Setup();
            _posXLb.Setup();
            _posYLb.Setup();
            _scaleXLb.Setup();
            _scaleYLb.Setup();
            _expandXLb.Setup();
            _expandYLb.Setup();


            int lbWidth = (int)_posLb.TextSize.X;
            Properties.Add(_posLb, new Point(168 - lbWidth, 214));
            Properties.Add(_posXLb, new Point(224, 215));
            Properties.Add(_posYLb, new Point(299, 215));
            Properties.Add(_posX, new Point(171, 212));
            Properties.Add(_posY, new Point(247, 212));

            lbWidth = (int)_scaleLb.TextSize.X;
            Properties.Add(_scaleLb, new Point(168 - lbWidth, 457));
            Properties.Add(_scaleXLb, new Point(224, 458));
            Properties.Add(_scaleYLb, new Point(299, 458));
            Properties.Add(_scaleX, new Point(171, 455));
            Properties.Add(_scaleY, new Point(246, 455));

            lbWidth = (int)_expandLb.TextSize.X;
            Properties.Add(_expandLb, new Point(168 - lbWidth, 310));

            Properties.Add(_expandXLb, new Point(187, 310));
            Properties.Add(_expandYLb, new Point(266, 310));
            Properties.Add(_expandX, new Point(171, 310));
            Properties.Add(_expandY, new Point(250, 310));

            _posX.TextColor = Color.White;
            _posY.TextColor = Color.White;
            _scaleX.TextColor = Color.White;
            _scaleY.TextColor = Color.White;

            AddEvents();
        }
        public void AddEvents()
        {
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            _expandX.MouseEvent.onMouseClick += (sender, args) =>
            {
                if (!_expandX.Selected)
                {
                    Owner.XPolicy = SizePolicy.EXPAND;
                }
                else
                {
                    Owner.XPolicy = SizePolicy.FIXED;
                }
                MainWindow.RootContainer.UpdateLayout();
            };
            _expandY.MouseEvent.onMouseClick += (sender, args) =>
            {
                if (!_expandY.Selected)
                {
                    Owner.YPolicy = SizePolicy.EXPAND;
                }
                else
                {
                    Owner.YPolicy = SizePolicy.FIXED;
                }
                MainWindow.RootContainer.UpdateLayout();
            };
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            _posX.Up.MouseEvent.onMouseClick += (sender, args) =>
            {
                if (MainWindow.RootContainer is Frame)
                {
                    Point delta = Owner.Position - MainWindow.RootContainer.Position;
                    MainWindow.RootContainer.UpdateSlot(Owner, delta + new Point(4, 0));
                }
            };
            _posY.Down.MouseEvent.onMouseClick += (sender, args) =>
            {
                if (MainWindow.RootContainer is Frame)
                {
                    Point delta = Owner.Position - MainWindow.RootContainer.Position;
                    MainWindow.RootContainer.UpdateSlot(Owner, delta + new Point(0, 4));
                }
            };


            _posX.Down.MouseEvent.onMouseClick += (sender, args) =>
            {
                if (MainWindow.RootContainer is Frame)
                {
                    Point delta = Owner.Position - MainWindow.RootContainer.Position;
                    MainWindow.RootContainer.UpdateSlot(Owner, delta - new Point(4, 0));
                }
            };
            _posY.Up.MouseEvent.onMouseClick += (sender, args) =>
            {
                if (MainWindow.RootContainer is Frame)
                {
                    Point delta = Owner.Position - MainWindow.RootContainer.Position;
                    MainWindow.RootContainer.UpdateSlot(Owner, delta - new Point(0, 4));
                }
            };
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            _scaleX.Up.MouseEvent.onMouseClick += (sender, args) =>
            {
                if (Owner == MainWindow.CurrentObject)
                {
                    Owner.Resize(new Point(1, 0));
                    _scaleX.Text = Owner.Size.X.ToString();
                }
            };
            _scaleX.Down.MouseEvent.onMouseClick += (sender, args) =>
            {
                if (Owner == MainWindow.CurrentObject)
                {
                    Owner.Resize(new Point(-1, 0));
                    _scaleX.Text = Owner.Size.X.ToString();
                }
            };
            _scaleY.Down.MouseEvent.onMouseClick += (sender, args) =>
            {
                if (Owner == MainWindow.CurrentObject)
                {
                    Owner.Resize(new Point(0, -1));
                    _scaleY.Text = Owner.Size.Y.ToString();

                }
            };
            _scaleY.Up.MouseEvent.onMouseClick += (sender, args) =>
            {
                if (Owner == MainWindow.CurrentObject)
                {
                    Owner.Resize(new Point(0, 1));
                    _scaleY.Text = Owner.Size.Y.ToString();
                }
            };
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }
        public void Update(GameTime gameTime)
        {
            _posX.Text = (Owner.Position.X - MainWindow.RootContainer.Rect.Left).ToString();
            _posY.Text = (Owner.Position.Y - MainWindow.RootContainer.Rect.Top).ToString();

            _scaleX.Text = Owner.Size.X.ToString();
            _scaleY.Text = Owner.Size.Y.ToString();


            _expandX.Selected = Convert.ToBoolean(Owner.XPolicy);
            _expandY.Selected = Convert.ToBoolean(Owner.YPolicy);
        }
    }
}
