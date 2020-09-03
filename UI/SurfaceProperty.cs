using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static _GUIProject.UI.PropertyPanel;
using static _GUIProject.UI.UIObject;

namespace _GUIProject.UI
{
    public class SurfaceProperty
    {
        public UIObject Owner { get; private set; }
        public PropertyPanel Parent { get; private set; }
        public SortedDictionary<UIObject, Point> Properties { get; private set; }

        Label _txPickLb;
        Label _colorLb;
        Label _alphaLb;

        FilePicker _txPicker;
        InputInfoArea _alpha;
        ComboBox _color;

        readonly PngToXnb _converter;

        public SurfaceProperty(PropertyPanel parent)
        {
            Properties = new SortedDictionary<UIObject, Point>();
            _converter = new PngToXnb();
            Owner = parent.Owner;
            Parent = parent;
        }
        public void AddProperties(PropertyOwner owner)
        {
            _color = new ComboBox("PropertyPanelCBTX", "PropertyPanelCBBGTX", "PropertyPanelCBFootTX", DrawPriority.LOW);
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
                _color.AddName(Owner.ColorValue.Text, Color.White, Singleton.Font.GetFont(FontManager.FontType.ARIAL));

                if(owner != PropertyOwner.SLIDER)
                {
                    lbWidth = (int)_txPickLb.TextSize.X;
                    Properties.Add(_txPickLb, new Point(168 - lbWidth, 240));
                    Properties.Add(_txPicker, new Point(171, 240));
                }               
            }
            _alpha.TextColor.Color = Color.White;
            AddEvents();

        }
        void AddEvents()
        {
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            _color.AddNewItem("Black", () =>
            {
                _color.Text = "Black";
                _color.AuxilaryColor.Color = Color.Black;

                if (Owner.Editable)
                {
                    Owner.ColorValue = _color.AuxilaryColor;
                }

                _color.Hide();
            });
            _color.AddNewItem("White", () =>
            {
                _color.Text = "White";
                _color.AuxilaryColor.Color = Color.White;

                if (Owner.Editable)
                {
                    Owner.ColorValue = _color.AuxilaryColor;

                }

                _color.Hide();

            });
            _color.AddNewItem("Green", () =>
            {
                _color.AuxilaryColor.Color = Color.Green;
                _color.Text = "Green";

                if (Owner.Editable)
                {
                    Owner.ColorValue = _color.AuxilaryColor;
                }

                _color.Hide();

            });
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            _txPicker.MouseEvent.onMouseClick += (sender, args) =>
            {
                // TODO: 

                System.Windows.Forms.OpenFileDialog fileDlg = new System.Windows.Forms.OpenFileDialog();
                fileDlg.DefaultExt = ".png";
                fileDlg.Filter = "Images (.png)|*.png";
                try
                {
                    if (fileDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string sourceFile = fileDlg.FileName;
                        string fileName = fileDlg.SafeFileName;
                        string targetPath = "Content";

                        string destFile = System.IO.Path.Combine(targetPath, fileName);

                        File.Copy(sourceFile, destFile, true);
                        _converter.Run(destFile);
                        fileName = fileName.Replace(".png", "");

                        (Owner as BasicSprite).UpdateTexture(fileName);
                        _txPicker.Text = fileName;
                    }
                }
                catch (IOException e)
                {
                    //object p = System.Windows.MessageBox.Show("Image Load Error", "Image could not be loaded: " + e.Message, MessageBoxButton.OK);
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
            _color.AuxilaryColor = Owner.ColorValue;
            _color.Text = Owner.ColorValue.Text;
            _alpha.Text = Owner.Alpha.ToString();
            
            if (!(Owner is Label))
            {
                _txPicker.Text = (Owner as BasicSprite).DefaultSprite.Name;
            }
        }
    }
}
