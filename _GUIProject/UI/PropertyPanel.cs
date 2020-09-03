using _GUIProject.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static _GUIProject.UI.BasicSprite;
using static _GUIProject.UI.TextBox;
using static _GUIProject.UI.UIObject;

namespace _GUIProject.UI
{
    public class PropertyPanel
    {

        public enum PropertyOwner
        {
            LABEL,
            BUTTON,
            COMBOBOX,
            SLIDER,
            TEXTBOX,
            MULTITEXTBOX,
            CHECKBOX,
            TOGGLE,
        }         
     
        CheckBox _showhide;
        TextBox _search;
        CheckBox _locker;
      
        CheckBox _grid;
        CheckBox _free;
        CheckBox _vertical;
        CheckBox _horizontal;
        
     
        Label _showhideLb;
        Label _lockLb;   

        ComboBox _events;
        TextBoxConfirmAction _onClickConfirm;
        TextBoxConfirmAction _onOverConfirm;
        TextBoxConfirmAction _onOutConfirm;
       
        Button _okClickConfirm;        
        Button _cancelClickConfirm;

        Button _okOverConfirm;
        Button _cancelOverConfirm;

        Button _okOutConfirm;
        Button _cancelOutConfirm;

        ToolTip _lockerTooltip;       
    
        public BasicSprite BGImage { get; set; }        
        public UIObject Owner { get; private set; }

        readonly CheckBoxGroup _layoutCBG;
        readonly TextProperty _textProperties;
        readonly SurfaceProperty _surfaceProperties;
        readonly GeneralProperty _generalProperties;
        readonly AdvancedProperty _advancedProperties;

        readonly SortedDictionary<UIObject, Point> _properties;
        public PropertyPanel(UIObject owner)
        {
            Owner = owner;                
            BGImage = new BasicSprite("PropertyPanelTX", UIObject.DrawPriority.NORMAL);
            _properties = new SortedDictionary<UIObject, Point>();
            _layoutCBG = new CheckBoxGroup();
            _textProperties = new TextProperty(this);
            _surfaceProperties = new SurfaceProperty(this);
            _generalProperties = new GeneralProperty(this);
            _advancedProperties = new AdvancedProperty(this);
        }

        public void SetupProperties()
        {
            BGImage.Setup();
           
            foreach (var item in _properties)
            {
                item.Key.Position = BGImage.Position + item.Value;
                item.Key.Setup();
            }
            _lockerTooltip.Setup();
        }       

        void Initialize()
        {            
            foreach (var item in _properties)
            {
                item.Key.Initialize();
                item.Key.Active = true;
                item.Key.Editable = false;
            }
            
        }
        public void AddProperties(PropertyOwner owner)
        {
            BGImage.Position = new Point(1590, 100);
            BGImage.Initialize();

            _textProperties.AddProperties(owner);
            _surfaceProperties.AddProperties(owner);
            _generalProperties.AddProperties(owner);
            _advancedProperties.AddProperties(owner);

            AddPanelProperties();
        
         
            _search.FieldWidth = 320;
            _search.SampleText = "Search for an item here...";
            _search.StickSampleText = true;          
   
         
            _search.TextColor.Color = Color.White;


            foreach (var item in _textProperties.Properties)
            {
                _properties.Add(item.Key, item.Value);
            }
            foreach (var item in _surfaceProperties.Properties)
            {
                _properties.Add(item.Key, item.Value);
            }
            foreach (var item in _generalProperties.Properties)
            {
                _properties.Add(item.Key, item.Value);
            }
            foreach (var item in _advancedProperties.Properties)
            {
                _properties.Add(item.Key, item.Value);
            }
            
            AddEvents();
        }
        void AddPanelProperties()
        {
            /////////////////////////////////////  Default Panel Properties  ////////////////////////////////////////////////////
            ////////////////////////////////////////////// Start ////////////////////////////////////////////////////////////////
            _grid = new CheckBox("PropertyPanelCheckboxTX", OverlayOption.CHECKBOX, DrawPriority.LOW);
            _free = new CheckBox("PropertyPanelCheckboxTX", OverlayOption.CHECKBOX, DrawPriority.LOW);
            _vertical = new CheckBox("PropertyPanelCheckboxTX", OverlayOption.CHECKBOX, DrawPriority.LOW);
            _horizontal = new CheckBox("PropertyPanelCheckboxTX", OverlayOption.CHECKBOX, DrawPriority.LOW);

            _search = new TextBox("PropertyPanelTextboxTX", "PropertyPanelTextboxPointerTX", TextBoxType.TEXT, DrawPriority.HIGHEST);       
            _events = new ComboBox("PropertyPanelEvtCBTX", "PropertyPanelEvtCBBGTX", "PropertyPanelEvtCBFootTX", DrawPriority.LOWEST);        

            _showhide = new CheckBox("PropertyPanelCheckboxTX", OverlayOption.CHECKBOX, DrawPriority.HIGHEST);
            _locker = new CheckBox("PropertyPanelCheckboxTX", OverlayOption.CHECKBOX, DrawPriority.HIGHEST);       

            _onClickConfirm = new TextBoxConfirmAction();          
            _onOverConfirm = new TextBoxConfirmAction();            
            _onOutConfirm = new TextBoxConfirmAction();
            _onClickConfirm.Priority = DrawPriority.LOWEST;
            _onOverConfirm.Priority = DrawPriority.LOWEST;
            _onOutConfirm.Priority = DrawPriority.LOWEST;

            _onOverConfirm.Initialize();
            _onClickConfirm.Initialize();
            _onOutConfirm.Initialize(); 

            _okClickConfirm = new Button("TextboxPickerConfirmTX", OverlayOption.NORMAL, DrawPriority.LOWEST);
            _cancelClickConfirm = new Button("TextboxPickerCancelTX", OverlayOption.NORMAL, DrawPriority.LOWEST);
            _okOverConfirm = new Button("TextboxPickerConfirmTX", OverlayOption.NORMAL, DrawPriority.LOWEST);
            _cancelOverConfirm = new Button("TextboxPickerCancelTX", OverlayOption.NORMAL, DrawPriority.LOWEST);
            _okOutConfirm = new Button("TextboxPickerConfirmTX", OverlayOption.NORMAL, DrawPriority.LOWEST);
            _cancelOutConfirm = new Button("TextboxPickerCancelTX", OverlayOption.NORMAL, DrawPriority.LOWEST);

            _showhideLb = new Label("Show/Hide:");
            _lockLb = new Label("Lock:");

            _showhideLb.TextFont = Singleton.Font.GetFont(FontManager.FontType.LUCIDA_CONSOLE);
            _lockLb.TextFont = Singleton.Font.GetFont(FontManager.FontType.LUCIDA_CONSOLE);          
          
            _lockerTooltip = new ToolTip(_locker);           
            _lockerTooltip.Initialize();
            _lockerTooltip.Text = "Tick this option\nTo lock the Item in place,\nAnd enable live actions\n(scale, show/hide etc)!";

            _properties.Add(_grid, new Point(141, 38));
            _properties.Add(_free, new Point(182, 38));
            _properties.Add(_vertical, new Point(226, 38));
            _properties.Add(_horizontal, new Point(275, 38));
            _properties.Add(_search, new Point(4, 55));

            _showhideLb.Setup();
            _lockLb.Setup();

            _properties.Add(_onClickConfirm, new Point(-100, 140));
            _properties.Add(_onOverConfirm, new Point(-100, 170));
            _properties.Add(_onOutConfirm, new Point(-100, 195));

            _properties.Add(_events, new Point(171, 118));

            int lbWidth = (int)_showhideLb.TextSize.X;
            _properties.Add(_showhideLb, new Point(168 - lbWidth, 488));
            _properties.Add(_showhide, new Point(171, 488));

            lbWidth = (int)_lockLb.TextSize.X;
            _properties.Add(_lockLb, new Point(168 - lbWidth, 339));
            _properties.Add(_locker, new Point(171, 339));

            _layoutCBG.AddRange(_grid, _free, _vertical, _horizontal);
            ////////////////////////////////////////////// End ////////////////////////////////////////////////////////////////

            Initialize();
       
            _events.AddName("Mouse Events", Color.White, Singleton.Font.GetFont(FontManager.FontType.GEORGIA));

            _okClickConfirm.Initialize();
            _cancelClickConfirm.Initialize();
            _okOverConfirm.Initialize();
            _cancelOverConfirm.Initialize();
            _okOutConfirm.Initialize();
            _cancelOutConfirm.Initialize();

            _okClickConfirm.Setup();
            _cancelClickConfirm.Setup();
            _okOverConfirm.Setup();
            _cancelOverConfirm.Setup();
            _okOutConfirm.Setup();
            _cancelOutConfirm.Setup();

            _onClickConfirm.AddButton(_okClickConfirm);
            _onClickConfirm.AddButton(_cancelClickConfirm);
            _onOverConfirm.AddButton(_okOverConfirm);
            _onOverConfirm.AddButton(_cancelOverConfirm);
            _onOutConfirm.AddButton(_okOutConfirm);
            _onOutConfirm.AddButton(_cancelOutConfirm);

            _onClickConfirm.Hide();
            _onOverConfirm.Hide();
            _onOutConfirm.Hide();
        }
        void AddEvents()
        {
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            _locker.MouseEvent.onMouseOver += (sender, args) =>
            {
                _lockerTooltip.Show();
            };
            _locker.MouseEvent.onMouseOut += (sender, args) =>
            {
                _lockerTooltip.Hide();
            };
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////           
            _locker.MouseEvent.onMouseClick += (sender, args) =>
            {
                _locker.Selected = !_locker.Selected;
                Owner.Locked = _locker.Selected;
                Owner.Editable = !Owner.Locked;
            };
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            _layoutCBG.SetSelected(_grid);
            _grid.MouseEvent.onMouseClick += (sender, args) =>
            {
                _layoutCBG.SetSelected(_grid);
                MainWindow.RemoveContainer();
                MainWindow.RootContainer = UIFactory<UIObject, IContainer>.CreateContainer(MainWindow.RootContainer, new Grid(), MainWindow._mainBatch);
                MainWindow.AddContainer();
                MainWindow.RootContainer.Show();

            };
            _free.MouseEvent.onMouseClick += (sender, args) =>
            {
                _layoutCBG.SetSelected(_free);

                MainWindow.RemoveContainer();
                MainWindow.RootContainer = UIFactory<UIObject, IContainer>.CreateContainer(MainWindow.RootContainer, new Frame("FrameEditorTX", DrawPriority.NORMAL, MoveOption.STATIC), MainWindow._mainBatch);
                MainWindow.AddContainer();
                MainWindow.RootContainer.Show();

            };
            _vertical.MouseEvent.onMouseClick += (sender, args) =>
            {
                _layoutCBG.SetSelected(_vertical);
                MainWindow.RemoveContainer();
                MainWindow.RootContainer = UIFactory<UIObject, IContainer>.CreateContainer(MainWindow.RootContainer, new VerticalStack(), MainWindow._mainBatch);
                MainWindow.AddContainer();
                MainWindow.RootContainer.Show();
            };
            _horizontal.MouseEvent.onMouseClick += (sender, args) =>
            {
                _layoutCBG.SetSelected(_horizontal);
                MainWindow.RemoveContainer();
                MainWindow.RootContainer = UIFactory<UIObject, IContainer>.CreateContainer(MainWindow.RootContainer, new HorizontalStack(), MainWindow._mainBatch);
                MainWindow.AddContainer();
                MainWindow.RootContainer.Show();
            };
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            _onClickConfirm.MouseEvent.onMouseClick += (sender, args) =>
            {
                _onClickConfirm.Selected = true;
            };           

            _onOverConfirm.MouseEvent.onMouseClick += (sender, args) =>
            {
                _onOverConfirm.Selected = true;
            };

            _onOutConfirm.MouseEvent.onMouseClick += (sender, args) =>
            {
                _onOutConfirm.Selected = true;
            };

            _okClickConfirm.MouseEvent.onMouseClick += (sender, args) =>
            {
                // TODO: 
                _onClickConfirm.Hide();
                _events.Hide();
            };
            _cancelClickConfirm.MouseEvent.onMouseClick += (sender, args) =>
            {
                _onClickConfirm.Clear();
                _onClickConfirm.Hide();
                _events.Hide();
            };

            _okOverConfirm.MouseEvent.onMouseClick += (sender, args) =>
            {
                // TODO: 
                _onOverConfirm.Hide();
                _events.Hide();
            };
            _cancelOverConfirm.MouseEvent.onMouseClick += (sender, args) =>
            {
                _onOverConfirm.Clear();
                _onOverConfirm.Hide();
                _events.Hide();
            };

            _okOutConfirm.MouseEvent.onMouseClick += (sender, args) =>
            {
                // TODO: 
                _onOutConfirm.Hide();
                _events.Hide();
            };
            _cancelOutConfirm.MouseEvent.onMouseClick += (sender, args) =>
            {
                _onOutConfirm.Clear();
                _onOutConfirm.Hide();
                _events.Hide();
            };

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            _events.MouseEvent.onMouseClick += (sender, args) =>
            {
                _events.Show();
                _onClickConfirm.Hide();
                _onOverConfirm.Hide();
                _onOutConfirm.Hide();
            };

            _events.AddNewItem("OnClick", () =>
            {
                UIObject item = _events["OnClick"];             

                _onClickConfirm.Show();
                _onOverConfirm.Hide();
                _onOutConfirm.Hide();
            });

            _events.AddNewItem("MouseOver", () =>
            {
                UIObject item = _events["MouseOver"];
                      

                _onOverConfirm.Show();
                _onClickConfirm.Hide();
                _onOutConfirm.Hide();
            });

            _events.AddNewItem("MouseOut", () =>
            {
                UIObject item = _events["MouseOut"];           

                _onOutConfirm.Show();
                _onClickConfirm.Hide();
                _onOverConfirm.Hide();
            });
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            _showhide.MouseEvent.onMouseClick += (sender, args) =>
            {
                if (_showhide.Selected)
                {
                    Owner.Active = false;
                }
                else
                {
                    Owner.Active = true;

                }
            };
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            _search.MouseEvent.onMouseClick += (sender, args) =>
            {
                _search.Selected = true;

            };

            _search.KeyboardEvents.onKeyReleased += (sender, args) =>
            {


                if (Singleton.Input.KeyReleased(Keys.Back))
                {
                    MainWindow.Selection.Hide();
                }
                else
                {
                    if (Singleton.Input.KeyReleased(Singleton.Input.CurrentKey))
                    {

                        if (_search.Text.Length >= 3)
                        {
                            string pattern = @".*(?=" + _search.Text + ")+";
                            Regex reg = new Regex(pattern, RegexOptions.IgnoreCase);

                            var objectsFound = MainWindow.RootContainer.Slots.Where(t => reg.Match(t.Item.Name).Success).ToList();

                            if (objectsFound.Count > 0)
                            {
                                var owner = objectsFound.Last().Item;
                                MainWindow.Selection.Position = owner.Position;

                                Point rightBarPosition = new Point(owner.Right, owner.Top);
                                Point leftBarPosition = new Point(owner.Left, owner.Top);
                                Point topBarPosition = new Point(owner.Left, owner.Top);
                                Point bottomBarPosition = new Point(owner.Left, owner.Bottom);

                                MainWindow.Selection.UpdatePosition(rightBarPosition,
                                                                              leftBarPosition,
                                                                              topBarPosition,
                                                                              bottomBarPosition);

                                MainWindow.Selection.UpdateSize(owner);

                                _search.Clear();
                                _search.SimulateInput(owner.Name);
                                MainWindow.Selection.Show();
                            }
                        }
                    }
                }
            };
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }

        public void AddPropertyRenderer(SpriteBatch batch)
        {
            BGImage.AddSpriteRenderer(batch);
            _lockerTooltip.AddSpriteRenderer(batch);
            _lockerTooltip.AddStringRenderer(batch);

            foreach (var item in _properties)
            {
                item.Key.AddSpriteRenderer(batch);
                item.Key.AddStringRenderer(batch);
            }
            BGImage.Active = true;
        }


        public UIObject HitTest(Point mousePosition)
        {
            UIObject result = null;

            if (MainWindow.CurrentObject == Owner)
            {
                result = BGImage.HitTest(mousePosition);
                if (BGImage.Active)
                {
                    for (int i = _properties.Count - 1; i >= 0; i--)
                    {
                        var item = _properties.ElementAt(i).Key;

                        if (item != null)
                        {
                            if (item.HitTest(mousePosition) != null)
                            {
                                result = item.HitTest(mousePosition);
                                continue;
                            }
                        }
                    }

                }
            }       
         
            return result;
        }
        public void Update(GameTime gameTime)
        {
            if (MainWindow.CurrentObject == Owner)
            {
                BGImage.Update(gameTime);

                for (int i = _properties.Count - 1; i >= 0; i--)
                {
                    var item = _properties.ElementAt(i);

                    item.Key.Position = BGImage.Position + item.Value;
                    item.Key.Update(gameTime);
                }
                if (_properties.Count > 0)
                {
                    _textProperties.Update(gameTime);
                    _surfaceProperties.Update(gameTime);
                    _generalProperties.Update(gameTime);
                    _advancedProperties.Update(gameTime);

                    _showhide.Selected = Owner.Active;              
                    _locker.Selected = Owner.Locked;           
                   
                    if(MainWindow.RootContainer is Grid)
                    {
                        _layoutCBG.SetSelected(_grid);
                    }
                    else if (MainWindow.RootContainer is Frame)
                    {
                        _layoutCBG.SetSelected(_free);
                    }
                    else if (MainWindow.RootContainer is VerticalStack)
                    {
                        _layoutCBG.SetSelected(_vertical);
                    }
                    else
                    {
                        _layoutCBG.SetSelected(_horizontal);
                    }
                }
                _lockerTooltip.Update(gameTime);
            }
            
        }
        public void Draw()
        {
            if (MainWindow.CurrentObject == Owner)
            {
                BGImage.Draw();

                for (int i = _properties.Count - 1; i >= 0; i--)
                {
                    _properties.ElementAt(i).Key.Draw();
                }
                // Temporary
                _lockerTooltip.Draw();
            }
        }
    }
}
