using _GUIProject.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using static _GUIProject.UI.BasicSprite;
using static _GUIProject.UI.TextBox;
using static _GUIProject.UI.UIObject;
using System.IO;
using Microsoft.Win32;
using System.Windows;
using System.Threading.Tasks;
using System.Diagnostics;
using _GUIProject.Events;
using Microsoft.Xna.Framework.Content;
using ExtensionMethods;

namespace _GUIProject
{
    // TODO:
    // Fix UIOBject manual scaling and add preffered Size Policy  
    // Implement export and save, by exporting all to XML using Serialization
    public class MainWindow : Game
    {
        public static MainWindow MainInstance;

        // Basic frame elements       
        public static IContainer RootContainer { get; set; }
        public static UIObject CurrentObject { get; set; }

        public static SpriteBatch _mainBatch;
        public static ElementSelection Selection { get; private set; }
        private SpriteBatch _spriteBatch;

        private static List<IObject> _guiList;

        // Save and Load elements
        private ComboBox _fileMenu;

        // Actual window elements       
        private Label _mainFrameCaption;

        // Toolbox elements
        private Frame UIToolShelf;
        private TextBox _toolShelfTextbox;
        private Label _toolShelfLabel;
        private Button _toolShelfButton;
        private CheckBox _toolShelfCheckbox;
        private ComboBox _toolShelfComboBox;
        private ToggleButton _toolShelfToggle;
        private SliderBar _toolShelfSlider;
        private MultiTextBox _toolShelfMultilineTextbox;

        private readonly GraphicsDeviceManager _graphics;

        public static void AddContainer()
        {
            _guiList.Add(RootContainer);
        }
        public static void RemoveContainer()
        {
            _guiList.Remove(RootContainer);
        }

        public MainWindow()
        {

            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";



            Window.AllowUserResizing = true;
            IsMouseVisible = true;

        }
        public void SetMouseVisibility(bool isVisible)
        {
            IsMouseVisible = isVisible;
        }


        protected override void Initialize()
        {

            _guiList = new List<IObject>();

            MainInstance = this;

            // This is necessary for performance, it caches the resources before first use
            var caching = new ComboBox();
            caching.Initialize();
            caching.InitPropertyPanel();

            MouseGUI.Load();

            _fileMenu = new ComboBox();
            _fileMenu.Position = new Point(32, 32);
            _fileMenu.Initialize();
            _fileMenu.Editable = false;

            _fileMenu.AddName("File", Color.Black, Singleton.Font.GetFont(FontManager.FontType.LUCIDA_CONSOLE));


            _fileMenu.AddNewItem("Save", () =>
            {
                //TODO:
            });
            _fileMenu.AddNewItem("Load", () =>
            {

                _fileMenu.Hide();
                // TODO: 

            });


            _fileMenu.AddNewItem("Export", () =>
            {
                // TODO: 

                _fileMenu.Hide();
            });
            _fileMenu.AddNewItem("Close", () =>
            {
                Exit();
            });


            // Toolbox elements
            UIToolShelf = new Frame("UItoolboxTX", DrawPriority.NORMAL, MoveOption.STATIC);
            UIToolShelf.Position = new Point(32, 100);
            UIToolShelf.Initialize();


            _toolShelfComboBox = new ComboBox();
            _toolShelfComboBox.Initialize();
            _toolShelfComboBox.Editable = true; ;
            _toolShelfComboBox.DefaultOffset = new Point(8, 43);

            _toolShelfCheckbox = new CheckBox();
            _toolShelfCheckbox.Initialize();
            _toolShelfCheckbox.Text = "CheckBox";
            _toolShelfCheckbox.Editable = true; ;

            _toolShelfLabel = new Label();
            _toolShelfLabel.Initialize();
            _toolShelfLabel.TextColor = Color.White;
            _toolShelfLabel.Editable = true; ;

            _toolShelfTextbox = new TextBox();
            _toolShelfTextbox.Initialize();
            _toolShelfTextbox.Selected = false;
            _toolShelfTextbox.Editable = true; ;

            _toolShelfButton = new Button();
            _toolShelfButton.Initialize();
            _toolShelfButton.Editable = true; ;
            _toolShelfButton.TextColor = Color.Black;

            _toolShelfToggle = new ToggleButton();
            _toolShelfToggle.Initialize();
            _toolShelfToggle.Editable = true; ;
            _toolShelfToggle.Toggle = false;

            _toolShelfSlider = new SliderBar();
            _toolShelfSlider.Initialize();
            _toolShelfSlider.Editable = true; ;

            _toolShelfMultilineTextbox = new MultiTextBox();
            _toolShelfMultilineTextbox.Initialize();
            _toolShelfMultilineTextbox.Editable = true; ;

            UIToolShelf.AddItem(new Point(32, 32), _toolShelfTextbox);
            UIToolShelf.AddItem(new Point(32, 96), _toolShelfLabel);
            UIToolShelf.AddItem(new Point(32, 160), _toolShelfButton);
            UIToolShelf.AddItem(new Point(32, 224), _toolShelfComboBox);
            UIToolShelf.AddItem(new Point(32, 288), _toolShelfCheckbox);
            UIToolShelf.AddItem(new Point(32, 352), _toolShelfToggle);
            UIToolShelf.AddItem(new Point(16, 416), _toolShelfSlider);
            UIToolShelf.AddItem(new Point(16, 480), _toolShelfMultilineTextbox);


            Selection = new ElementSelection();
            Selection.Initialize();
            Selection.Editable = false;
            Selection.Position = Point.Zero;

            UIToolShelf.Show();

            _mainFrameCaption = new Label("This is the Actual Window");
            _mainFrameCaption.Initialize();
            _mainFrameCaption.TextFont = Singleton.Font.GetFont(FontManager.FontType.GEORGIA);


            // Main frame layout test
            RootContainer = new Grid();
            RootContainer.Initialize();
            RootContainer.Position = new Point(568, 100);

            RootContainer.Show();


            _guiList.Add(Selection);
            _guiList.Add(_fileMenu);
            _guiList.Add(UIToolShelf);
            _guiList.Add(RootContainer);


            base.Initialize();
        }


        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            MouseGUI.Setup();

            for (int i = 0; i < _guiList.Count; i++)
            {
                _guiList[i].Setup();
            }

            Vector2 textSize = _mainFrameCaption.Text.Size(_mainFrameCaption.TextFont);
            Vector2 textPosition = new Vector2(RootContainer.Rect.Center.X - textSize.X / 2, RootContainer.Rect.Top - textSize.Y);
            _mainFrameCaption.AddPosition(textPosition);

            UIToolShelf.Size += new Point(0, 256);

            MouseGUI.AddSpriteRenderer(_spriteBatch);

            for (int i = 0; i < _guiList.Count; i++)
            {
                _guiList[i].AddSpriteRenderer(_spriteBatch);
                _guiList[i].AddStringRenderer(_spriteBatch);
            }

            _mainBatch = _spriteBatch;

        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        IContainer _fromContainer;
        protected override void Update(GameTime gameTime)
        {
            if (Singleton.Input.KeyReleased(Keys.Escape))
            {
                Exit();
            }
            if (Singleton.Input.KeyReleased(Keys.Delete))
            {
                RootContainer.RemoveItem(CurrentObject);
                RootContainer.RemoveSlot(CurrentObject);
                RootContainer.Update(gameTime);
                RootContainer.UpdateLayout();
            }
            MouseGUI.Update();



            for (int i = _guiList.Count - 1; i >= 0; i--)
            {
                _guiList[i].Update(gameTime);

            }
            _fileMenu.Update(gameTime);

            Singleton.Input.Update();

            MouseGUI.HitObject = null;

            for (int i = 0; i < _guiList.Count; i++)
            {
                MouseGUI.HitObject = _guiList[i].HitTest(MouseGUI.Position);

                if (MouseGUI.HitObject != null)
                {
                    break;
                }
            }

            if (MouseGUI.HitObject != null)
            {

                if (MouseGUI.LeftWasPressed)
                {
                    MouseGUI.Focus = MouseGUI.HitObject;
                    MouseGUI.DragOffset = MouseGUI.Focus.Position - MouseGUI.Position;

                    _fromContainer = RootContainer.Contains(MouseGUI.Focus) ? RootContainer : UIToolShelf;


                    if (MouseGUI.Focus.Editable && RootContainer.Contains(MouseGUI.Focus))
                    {
                        RootContainer.RemoveItem(MouseGUI.Focus);
                    }
                }
                else
                {
                    if (MouseGUI.Focus != null)
                    {
                        if (MouseGUI.Focus.Editable && MouseGUI.Focus.MoveState == MoveOption.DYNAMIC)
                        {

                            MouseGUI.Focus.Position = MouseGUI.Position + MouseGUI.DragOffset;
                            MouseGUI.Focus.Update(gameTime);

                            // TODO: Make sure this is inside the MainWindowFrame bounds
                            if (MouseGUI.LeftIsPressed && RootContainer.Contains(MouseGUI.Position))
                            {
                                Point newItemPosition = RootContainer.SimulateInsert(MouseGUI.Focus.Position - RootContainer.Position, MouseGUI.Focus);
                                newItemPosition += RootContainer.Position;

                                Point rightBarPosition = new Point(newItemPosition.X + MouseGUI.Focus.Width, newItemPosition.Y);
                                Point leftBarPosition = new Point(newItemPosition.X, newItemPosition.Y);
                                Point topBarPosition = new Point(newItemPosition.X, newItemPosition.Y);
                                Point bottomBarPosition = new Point(newItemPosition.X, newItemPosition.Y + MouseGUI.Focus.Height);

                                Selection.UpdatePosition(rightBarPosition, leftBarPosition, topBarPosition, bottomBarPosition);
                                Selection.UpdateSize(MouseGUI.Focus);

                                Selection.Show();
                            }
                        }


                        if (MouseGUI.LeftWasReleased)
                        {

                            Selection.Hide();
                            if (MouseGUI.Focus.Editable && RootContainer.Contains(MouseGUI.Focus) && _fromContainer != UIToolShelf)
                            {
                                CurrentObject = MouseGUI.Focus;
                                RootContainer.RemoveSlot(MouseGUI.Focus);
                                RootContainer.AddItem(MouseGUI.Focus.Position - RootContainer.Position, MouseGUI.Focus);
                                RootContainer.Update(gameTime);
                                RootContainer.UpdateLayout();

                            }

                            if (MouseGUI.Focus is UIObject && !MouseGUI.Focus.Editable)
                            {

                                MouseGUI.Focus.MouseEvent.Click();
                            }


                            if (MouseGUI.Focus.Editable && RootContainer.Contains(MouseGUI.Focus.Position) && _fromContainer == UIToolShelf)
                            {
                                CurrentObject = UIFactory<UIObject, IContainer>.CreateObject(MouseGUI.Focus.Position, MouseGUI.Focus, RootContainer);
                                CurrentObject.InitPropertyPanel();
                                CurrentObject.AddSpriteRenderer(_mainBatch);
                                CurrentObject.AddStringRenderer(_mainBatch);
                                CurrentObject.AddPropertyRenderer(_mainBatch);
                                RootContainer.Update(gameTime);
                                RootContainer.UpdateLayout();

                            }
                            if (MouseGUI.Focus.Locked)
                            {
                                CurrentObject = MouseGUI.Focus;
                            }

                            MouseGUI.ClearFocus();
                        }
                    }
                }

            }
            if (MouseGUI.Focus != null && !MouseGUI.Focus.Locked)
            {
                if (MouseGUI.LeftIsPressed && _fromContainer != UIToolShelf)
                {

                    if (MouseGUI.Position.X <= RootContainer.Rect.Left)
                    {
                        MouseGUI.Focus.Position =
                            new Point(RootContainer.Rect.Left, MouseGUI.Position.Y);
                    }
                    if (MouseGUI.Position.X + MouseGUI.Focus.Width >= RootContainer.Rect.Right)
                    {
                        MouseGUI.Focus.Position =
                            new Point(RootContainer.Rect.Right - MouseGUI.Focus.Width, MouseGUI.Position.Y);
                    }
                    if (MouseGUI.Position.Y + MouseGUI.Focus.Height >= RootContainer.Rect.Bottom)
                    {
                        MouseGUI.Focus.Position =
                           new Point(MouseGUI.Position.X, RootContainer.Rect.Bottom - MouseGUI.Focus.Height);
                    }
                    if (MouseGUI.Position.Y - MouseGUI.Focus.Height <= RootContainer.Rect.Top)
                    {
                        MouseGUI.Focus.Position =
                          new Point(MouseGUI.Position.X, RootContainer.Rect.Top);

                    }

                    MouseGUI.Focus.Update(gameTime);
                }
            }

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.Gray);

            _spriteBatch.Begin(SpriteSortMode.Deferred);

            for (int i = _guiList.Count - 1; i >= 0; i--)
            {
                _guiList[i].Draw();
            }
            MouseGUI.Draw();

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }

}
