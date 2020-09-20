using _GUIProject;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static _GUIProject.UI.PropertyPanel;
using static _GUIProject.UI.UIObject;

namespace _GUIProject.UI
{
    public class SurfaceProperty
    {
        public UIObject Owner { get; private set; }
        public PropertyPanel Parent { get; private set; }
        public SortedDictionary<UIObject, Point> Properties { get; private set; }

        private Label _txPickLb;
        private Label _colorLb;
        private Label _alphaLb;

        private FilePicker _txPicker;
        private InputInfoArea _alpha;
        //private ComboBox _color;
        private ComboMulti _color;

        private readonly PngToXnb _converter;

        public SurfaceProperty(PropertyPanel parent)
        {
            Properties = new SortedDictionary<UIObject, Point>();
            _converter = new PngToXnb();
            Owner = parent.Owner;
            Parent = parent;
        }
        public void AddProperties(PropertyOwner owner)
        {
            _color = new ComboMulti("PropertyPanelCBTX", "PropertyPanelCBBGTX", DrawPriority.LOWEST);
            _txPicker = new FilePicker();
            _alpha = new InputInfoArea("PropertyPanelCombinedArrowsTX");

            _txPickLb = new Label("Texture:");
            _colorLb = new Label("Color:");
            _alphaLb = new Label("Alpha:");
           
            _color.Initialize();         
            _txPicker.Initialize();
            _alpha.Initialize();          

            _txPickLb.Initialize();
            _colorLb.Initialize();
            _alphaLb.Initialize();
          
            _color.AddAuxiliaryInfo();     
           
            _color.AddName("Sprite Color", Color.White, Singleton.Font.GetFont(FontManager.FontType.STANDARD));

            _txPickLb.TextFont = Singleton.Font.GetFont(FontManager.FontType.LUCIDA_CONSOLE);
            _colorLb.TextFont = Singleton.Font.GetFont(FontManager.FontType.LUCIDA_CONSOLE);
            _alphaLb.TextFont = Singleton.Font.GetFont(FontManager.FontType.LUCIDA_CONSOLE);         


            int lbWidth = (int)_alphaLb.TextSize.X;
            Properties.Add(_alphaLb, new Point(168 - lbWidth, 514));
            Properties.Add(_alpha, new Point(171, 514));

            if (owner != PropertyOwner.LABEL)
            {
                lbWidth = (int)_colorLb.TextSize.X;
                Properties.Add(_colorLb, new Point(168 - lbWidth, 181));
                Properties.Add(_color, new Point(171, 181));                

                if(owner != PropertyOwner.SLIDER)
                {
                    lbWidth = (int)_txPickLb.TextSize.X;
                    Properties.Add(_txPickLb, new Point(168 - lbWidth, 240));
                    Properties.Add(_txPicker, new Point(171, 240));
                }               
            }
            _alpha.TextColor = Color.White;            
          

            AddEvents();

        }
        public void AddEvents()
        {
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            var colorObj = Reflection.CreateObject(typeof(Color).Name);
            var colors = colorObj.GetType().GetProperties().Where(p => p.PropertyType == typeof(Color)).ToArray();
            foreach (var col in colors)
            {
                Color color = (Color)col.GetValue(col, null);
                _color.AddNewItem(col.Name, color, () =>
                {
                    _color.Text = col.Name;
                    _color.AuxilaryColor = color;                    
                    Owner.SpriteColor = _color.AuxilaryColor;
                    _color.Hide();                    
                });
               
            }         
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            _txPicker.MouseEvent.onMouseClick += (sender, args) =>
            {
                if (_txPicker.IsSuccess)
                {
                    try
                    {
                        string destination = Path.Combine("Content", _txPicker.FileName);
                        File.Copy(_txPicker.FilePath, destination, true);
                        _converter.Run(destination);
                        (Owner as Sprite).UpdateTexture(_txPicker.FileName.Replace(".png", ""));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Image could not be loaded: " + ex.Message, "Image Load Error", MessageBoxButtons.OK);
                    }
                }

            };
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            _alpha.Up.MouseEvent.onMouseClick += (sender, args) =>
            {
                if (Owner == MainWindow.CurrentObject)
                {
                    if (Owner.Alpha < 1.0f)
                    {
                        Owner.Alpha += 0.25f;
                    }
                }
            };
            _alpha.Down.MouseEvent.onMouseClick += (sender, args) =>
            {
                if (Owner == MainWindow.CurrentObject)
                {
                    if (Owner.Alpha > 0.0f)
                    {
                        Owner.Alpha -= 0.25f;
                    }
                }
            };
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }

        public void Update(GameTime gameTime)
        {
            _color.AuxilaryColor = Owner.SpriteColor;            
            _alpha.Text = Owner.Alpha.ToString();
            
            if (!(Owner is Label))
            {
                _txPicker.Text = (Owner as Sprite).Texture.Name;
            }
        }
    }
}
