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
        Label _name;
        ComboBox _textColor;
        TextBox _text;
     
        Label _nameLb;
        Label _textLb;
        Label _txtColorLb;
        public UIObject Owner { get; private set; }
        public PropertyPanel Parent { get; private set; }
        public SortedDictionary<UIObject, Point> Properties { get; private set; }
       
        public TextProperty(PropertyPanel parent)
        {
            Properties = new SortedDictionary<UIObject, Point>();
            Owner = parent.Owner;
            Parent = parent;
        }

        public void AddProperties(PropertyOwner owner)
        {
            _text = new TextBox("PropertyPanelTextboxTX", "PropertyPanelTextboxPointerTX", TextBoxType.TEXT, DrawPriority.NORMAL);            
            _textColor = new ComboBox("PropertyPanelCBTX", "PropertyPanelCBBGTX", "PropertyPanelCBFootTX", DrawPriority.LOW);
            _name = new Label("");

            _textColor.AddName(Owner.TextColor.Text, Color.White, Singleton.Font.GetFont(FontManager.FontType.ARIAL));
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
            _name.TextColor.Color = Color.Black;



            int lbWidth = (int)_nameLb.TextSize.X;
            Properties.Add(_nameLb, new Point(160 - lbWidth, 369));
            Properties.Add(_name, new Point(171, 368));

            if (owner != PropertyOwner.MULTITEXTBOX && owner != PropertyOwner.SLIDER && owner != PropertyOwner.TOGGLE)
            {
                _text.FieldWidth = 150;
                _text.TextColor.Color = Color.White;

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

            PopulateEvents(owner);
        }
        void PopulateEvents(PropertyOwner owner)
        {

            if (owner != PropertyOwner.MULTITEXTBOX && owner != PropertyOwner.SLIDER && owner != PropertyOwner.TOGGLE)
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
                    MainWindow.RootContainer.UpdateLayout();
                };
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            }
            _textColor.AddNewItem("Black", () =>
            {
                _textColor.Text = "Black";
                _textColor.AuxilaryColor.Color = Color.Black;

                if (Owner == MainWindow.CurrentObject && Owner.Editable)
                {
                    Owner.TextColor = _textColor.AuxilaryColor;
                }

                _textColor.Hide();
            });

            _textColor.AddNewItem("White", () =>
            {
                _textColor.Text = "White";
                _textColor.AuxilaryColor.Color = Color.White;

                if (Owner == MainWindow.CurrentObject && Owner.Editable)
                {
                    Owner.TextColor = _textColor.AuxilaryColor;

                }

                _textColor.Hide();

            });
            _textColor.AddNewItem("Green", () =>
            {
                _textColor.AuxilaryColor.Color = Color.Green;
                _textColor.Text = "Green";

                if (Owner.Editable)
                {
                    Owner.TextColor = _textColor.AuxilaryColor;
                }

                _textColor.Hide();

            });
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }
        public void Update(GameTime gameTime)
        {

            if (Properties.Count > 0)
            {
                _textColor.AuxilaryColor = Owner.TextColor;
                _textColor.Text = Owner.TextColor.Text;

                _name.Text = Owner.Name;
                _text.Text = Owner.Text;
            }
        }
    }
}
