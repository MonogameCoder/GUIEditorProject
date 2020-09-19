using _GUIProject.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static _GUIProject.UI.PropertyPanel;
using static _GUIProject.UI.TextBox;
using static _GUIProject.UI.UIObject;

namespace _GUIProject.UI
{
    public class TextProperty
    {
        // _name ->  will be later changed to an editable textbox
     
        public UIObject Owner { get; private set; }
        public PropertyPanel Parent { get; private set; }
        public SortedDictionary<UIObject, Point> Properties { get; private set; }

        private Label _name;
        private ComboMulti _textColor;
        private TextBox _text;

        private Label _nameLb;
        private Label _textLb;
        private Label _txtColorLb;

        public TextProperty(PropertyPanel parent)
        {
            Properties = new SortedDictionary<UIObject, Point>();
            Owner = parent.Owner;
            Parent = parent;
        }

        public void AddProperties(PropertyOwner owner)
        {
            _text = new TextBox("PropertyPanelTextboxTX", "PropertyPanelTextboxPointerTX", TextBoxType.TEXT, DrawPriority.NORMAL);            
            _textColor = new ComboMulti("PropertyPanelCBTX", "PropertyPanelCBBGTX", DrawPriority.LOWEST);
            _name = new Label("");

            _textColor.AddName("Text Color", Color.White, Singleton.Font.GetFont(FontManager.FontType.STANDARD));
            _textColor.AddAuxiliaryInfo();


            _nameLb = new Label("Name:");
            _textLb = new Label("Text:");
            _txtColorLb = new Label("Text Color:");

            _text.Initialize();
            _textColor.Initialize();
            _name.Initialize();
          
            _textLb.Initialize();
            _nameLb.Initialize();
            _txtColorLb.Initialize();

            _textLb.Setup();
            _nameLb.Setup();
            _txtColorLb.Setup();

            _textLb.TextFont = Singleton.Font.GetFont(FontManager.FontType.LUCIDA_CONSOLE);
            _txtColorLb.TextFont = Singleton.Font.GetFont(FontManager.FontType.LUCIDA_CONSOLE);
            _name.TextFont = Singleton.Font.GetFont(FontManager.FontType.ARIAL_BOLD);
            _name.TextColor = Color.Black;



            int lbWidth = (int)_nameLb.TextSize.X;
            Properties.Add(_nameLb, new Point(160 - lbWidth, 369));
            Properties.Add(_name, new Point(171, 368));

            if (owner != PropertyOwner.MULTITEXTBOX && owner != PropertyOwner.SLIDER && owner != PropertyOwner.TOGGLE)
            {
                _text.FieldWidth = 150;
                _text.TextColor = Color.White;

                lbWidth = (int)_textLb.TextSize.X;
                Properties.Add(_textLb, new Point(168 - lbWidth, 271));
                Properties.Add(_text, new Point(171, 271));

                lbWidth = (int)_txtColorLb.TextSize.X;
                Properties.Add(_txtColorLb, new Point(168 - lbWidth, 395));
                Properties.Add(_textColor, new Point(171, 395));
            }
            if (owner == PropertyOwner.MULTITEXTBOX)
            {
                lbWidth = (int)_txtColorLb.TextSize.X;
                Properties.Add(_txtColorLb, new Point(168 - lbWidth, 395));
                Properties.Add(_textColor, new Point(171, 395));

            }
           
            _nameLb.TextFont = Singleton.Font.GetFont(FontManager.FontType.LUCIDA_CONSOLE);

            AddEvents();
        }
        public void AddEvents()
        {
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            _text.MouseEvent.onMouseClick += (sender, args) =>
            {
                _text.Selected = true;
                _text.Clear();
                _text.SimulateInput(Owner.Text);             
            };
            _text.KeyboardEvents.onKeyReleased += (sender, args) =>
            {
                Owner.Text = _text.Text;
            };
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            var colorObj = Reflection.CreateObject(typeof(Color).Name);
            var colors = colorObj.GetType().GetProperties().Where(p => p.PropertyType == typeof(Color)).ToArray();
            foreach (var col in colors)
            {
                Color color = (Color)col.GetValue(col, null);
                _textColor.AddNewItem(color, () =>
                {
                    _textColor.AuxilaryColor = color;
                    Owner.TextColor = _textColor.AuxilaryColor;
                    _textColor.Hide();
                });
            }      
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }
        public void Update(GameTime gameTime)
        {

            if (Properties.Count > 0)
            {
                _textColor.AuxilaryColor = Owner.TextColor;               

                _name.Text = Owner.Name;
                _text.Text = Owner.Text;
            }
        }
    }
}
