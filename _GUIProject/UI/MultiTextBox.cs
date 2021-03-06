﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using ExtensionMethods;
using System.Xml.Serialization;

namespace _GUIProject.UI
{

    // This class will be fully refactored
    public class MultiTextBox : TextBox, IScrollable
    {
        [XmlIgnore]
        public int MaxLinesLength { get; set; }

        [XmlIgnore]
        public int NumberOfLines
        {
            get { return TextLines.Length; }
        }
        [XmlIgnore]
        public string LastLine
        {
            get
            {
                return TextLines[NumberOfLines - 1];
            }
        }
        [XmlIgnore]
        public string[] TextLines
        {
            get { return _keyboardString.Split('\n'); }
        }
        [XmlIgnore]
        public Vector2 LastLineSize
        {
            get { return LastLine.Replace('\n', ' ').Size(TextFont); }
        }
        [XmlElement]
        public override string Text
        {
            get { return DisplayText; }
            set 
            {
                foreach (char character in value)
                {
                    _keyboardString += character;
                    CreateText();
                }          
            }
        }

        private ScrollBar _scrollBar;
        public MultiTextBox() : base("DefaultMultiTexboxTX", "DefaultTextboxPointerTX", TextBoxType.TEXT, DrawPriority.NORMAL)
        {            
            MoveState = MoveOption.DYNAMIC;
            XPolicy = SizePolicy.EXPAND;
            YPolicy = SizePolicy.EXPAND;
            LoadAttributes();
            
        }
        void LoadAttributes()
        {
            base.Setup();
            CharBucket = new CharacterBucket(Left, Top);            
            _scrollBar = new ScrollBar();
            _scrollBar.Parent = this;
            _scrollBar.Initialize();
           
            _scrollBar.Setup();
           
            _scrollBar.Position = new Point(Right - _scrollBar.Width, Top);            

            TextColor = Color.Black;          

        }
        public override void Initialize()
        {
            base.Initialize();
            MouseEvent.onMouseClick += (sender, args) => Selected = true;
            Active = true;

        }
        public override void InitPropertyPanel()
        {
            Property = new PropertyPanel(this);
            Property.AddProperties(PropertyPanel.PropertyOwner.MULTITEXTBOX);         
            Property.SetupProperties();          
        }
        public override void Setup()
        {
            base.Setup();
          
            Pointer += new Point(Left, Top);
            CurrItemRect = Pointer.Rect;
            MouseEvent.onMouseOut += (sender, args) => { Selected = false; };
           
            _scrollBar.Show();
        }
        public override void AddSpriteRenderer(SpriteBatch batch)
        {
            _scrollBar.AddSpriteRenderer(batch);
            base.AddSpriteRenderer(batch);
        }
        public override void AddStringRenderer(SpriteBatch batch)
        {
            _scrollBar.AddStringRenderer(batch);
            base.AddStringRenderer(batch);
        }
        public override void AddPropertyRenderer(SpriteBatch batch)
        {
            Property.AddPropertyRenderer(batch);
        }
        public override void OnKeyboardPressed()
        {
            _isShift = _myclsinput.isShiftPressed(Keyboard.GetState().GetPressedKeys());
            _isControl = _myclsinput.isControlPressed(Keyboard.GetState().GetPressedKeys());
    

            if (Singleton.Input.KeyReleased(Keys.A))
            {
                _keyboardString += _isShift ? "A" : "a";
                CreateText(); KeyboardEvents.Released(); 
            }

            ////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.Space))
            {
                _keyboardString += " ";
                CreateText(); KeyboardEvents.Released(); 
            }


            if (Singleton.Input.KeyReleased(Keys.B))
            {
                _keyboardString += _isShift ? "B" : "b";
                CreateText(); KeyboardEvents.Released(); 
            }

            ///////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.Z))
            {
                _keyboardString += _isShift ? "Z" : "z";
                CreateText(); KeyboardEvents.Released(); 
            }

            ///////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.C))
            {
                _keyboardString += _isShift ? "C" : "c";
                CreateText(); KeyboardEvents.Released(); 
            }

            ////////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.D))
            {
                _keyboardString += _isShift ? "D" : "d";
                CreateText(); KeyboardEvents.Released(); 
            }
            ////////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.E))
            {
                _keyboardString += _isShift ? "E" : "e";
                CreateText(); KeyboardEvents.Released(); 
            }

            ///////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.F))
            {
                _keyboardString += _isShift ? "F" : "f";
                CreateText(); KeyboardEvents.Released(); 
            }

            ///////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.G))
            {
                _keyboardString += _isShift ? "G" : "g";
                CreateText(); KeyboardEvents.Released(); 
            }

            ///////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.H))
            {
                _keyboardString += _isShift ? "H" : "h";
                CreateText(); KeyboardEvents.Released(); 
            }
            ///////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.I))
            {
                _keyboardString += _isShift ? "I" : "i";
                CreateText(); KeyboardEvents.Released(); 
            }
            ///////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.J))
            {
                _keyboardString += _isShift ? "J" : "j";
                CreateText(); KeyboardEvents.Released(); 
            }
            ///////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.K))
            {
                _keyboardString += _isShift ? "K" : "k";
                CreateText(); KeyboardEvents.Released(); 
            }
            if (Singleton.Input.KeyReleased(Keys.L))
            {
                _keyboardString += _isShift ? "L" : "l";
                CreateText(); KeyboardEvents.Released(); 
            }

            ///////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.M))
            {
                _keyboardString += _isShift ? "M" : "m";
                CreateText(); KeyboardEvents.Released(); 
            }
            ///////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.N))
            {
                _keyboardString += _isShift ? "N" : "n";
                CreateText(); KeyboardEvents.Released(); 
            }

            ///////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.O))
            {
                _keyboardString += _isShift ? "O" : "o";
                CreateText(); KeyboardEvents.Released(); 
            }

            ///////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.P))
            {
                _keyboardString += _isShift ? "P" : "p";
                CreateText(); KeyboardEvents.Released(); 
            }

            ///////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.Q))
            {
                _keyboardString += _isShift ? "Q" : "q";
                CreateText(); KeyboardEvents.Released(); 
            }

            ///////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.R))
            {
                _keyboardString += _isShift ? "R" : "r";
                CreateText(); KeyboardEvents.Released(); 
            }

            ///////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.S))
            {
                _keyboardString += _isShift ? "S" : "s";
                CreateText(); KeyboardEvents.Released(); 
            }
            ///////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.T))
            {
                _keyboardString += _isShift ? "T" : "t";
                CreateText(); KeyboardEvents.Released(); 
            }

            ///////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.U))
            {
                _keyboardString += _isShift ? "U" : "u";
                CreateText(); KeyboardEvents.Released(); 
            }

            ///////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.V))
            {
                _keyboardString += _isShift ? "V" : "v";
                CreateText(); KeyboardEvents.Released(); 
            }

            ///////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.W))
            {
                _keyboardString += _isShift ? "W" : "w";
                CreateText(); KeyboardEvents.Released(); 
            }

            ///////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.Y))
            {
                _keyboardString += _isShift ? "Y" : "y";
                CreateText(); KeyboardEvents.Released(); 
            }

            ///////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.X))
            {
                _keyboardString += _isShift ? "X" : "x";
                CreateText(); KeyboardEvents.Released(); 
            }

            /////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.OemSemicolon))
            {
                _keyboardString += _isShift ? ":" : ";";
                CreateText(); KeyboardEvents.Released(); 
            }

            /////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.OemQuotes))
            {
                _keyboardString += _isShift ? "\"" : "'";
                CreateText(); KeyboardEvents.Released(); 
            }

            ///////////////////////////////////////////////////////////

            ///////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.OemComma))
            {
                _keyboardString += _isShift ? "<" : ",";
                CreateText(); KeyboardEvents.Released(); 
            }

            ///////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.OemPeriod))
            {
                _keyboardString += _isShift ? ">" : ".";
                CreateText(); KeyboardEvents.Released(); 
            }

            ///////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.OemQuestion))
            {
                _keyboardString += _isShift ? "?" : "/";
                CreateText(); KeyboardEvents.Released(); 
            }

            ///////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.OemOpenBrackets))
            {
                _keyboardString += _isShift ? "{" : "[";
                CreateText(); KeyboardEvents.Released(); 
            }

            ///////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.OemCloseBrackets))
            {
                _keyboardString += _isShift ? "}" : "]";
                CreateText(); KeyboardEvents.Released(); 
            }

            ///////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.OemPipe))
            {
                _keyboardString += _isShift ? "|" : "\\";
                CreateText(); KeyboardEvents.Released(); 
            }

            ///////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.D1))
            {
                _keyboardString += _isShift ? "!" : "1";
                CreateText(); KeyboardEvents.Released(); 
            }

            ///////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.D2))
            {
                _keyboardString += _isShift ? "@" : "2";
                CreateText(); KeyboardEvents.Released(); 
            }

            ///////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.D3))
            {
                _keyboardString += _isShift ? "#" : "3";
                CreateText(); KeyboardEvents.Released(); 
            }

            ///////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.D4))
            {
                _keyboardString += _isShift ? "$" : "4";
                CreateText(); KeyboardEvents.Released(); 
            }

            ///////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.D5))
            {
                _keyboardString += _isShift ? "%" : "5";
                CreateText(); KeyboardEvents.Released(); 
            }

            ///////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.D6))
            {
                _keyboardString += _isShift ? "^" : "6";
                CreateText(); KeyboardEvents.Released(); 
            }

            ///////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.D7))
            {
                _keyboardString += _isShift ? "&" : "7";
                CreateText(); KeyboardEvents.Released(); 
            }

            ///////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.D8))
            {
                _keyboardString += _isShift ? "*" : "8";
                CreateText(); KeyboardEvents.Released(); 
            }

            ///////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.D9))
            {
                _keyboardString += _isShift ? "(" : "9";
                CreateText(); KeyboardEvents.Released(); 
            }

            ///////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.D0))
            {
                _keyboardString += _isShift ? ")" : "0";
                CreateText(); KeyboardEvents.Released(); 
            }

            ///////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.OemTilde))
            {
                _keyboardString += _isShift ? "~" : "`";
                CreateText(); KeyboardEvents.Released(); 
            }

            ///////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.OemMinus))
            {
                _keyboardString += _isShift ? "_" : "-";
                CreateText(); KeyboardEvents.Released(); 
            }

            ///////////////////////////////////////////////////////////
            if (Singleton.Input.KeyReleased(Keys.OemPlus))
            {
                _keyboardString += _isShift ? "+" : "=";
                CreateText(); KeyboardEvents.Released(); 
            }

            if (Singleton.Input.CurrentKey != Keys.None)
            {

                // ToDo

            }

            if (Singleton.Input.KeyReleased(Keys.Enter))
            {
                if (_myclsinput.EnterKeyState == "down")
                {
                    _myclsinput.EnterKeyState = "none";
                    //DrawTextsOnWindow(_spriteBatch, _chatGlobalList);

                    // Clear();
                    //enter = true;

                }
            }
            if (Singleton.Input.KeyReleased(Keys.Back))
            {
                // textList.Remove(textList.LastOrDefault());

                if (!string.IsNullOrEmpty(_keyboardString))
                {


                    bool isNewLine = LastChar == "\n";
                    MaxLinesLength = Height / (Pointer.Height - 3);

                    if (isNewLine)
                    {
                        CharBucket.RemoveCharacter();
                        CharBucket.RemoveCharacter();
                        _keyboardString = _keyboardString.Remove(_keyboardString.Length - 2, 2);

                        if (NumberOfLines > MaxLinesLength && (NumberOfLines - (MaxLinesLength + 1)) < _scrollBar.CurrentScrollValue / 2)
                        {

                            _scrollBar.CurrentScrollValue = (NumberOfLines - (MaxLinesLength + 1));
                        }


                    }
                    else
                    {
                        CurrItemRect = CharBucket.CurrentItemRectangle;
                        CharBucket.RemoveCharacter();
                        _keyboardString = _keyboardString.Remove(_keyboardString.Length - 1, 1);

                    }
                }
                KeyboardEvents.Released();
            }

        }

        protected override void CreateText()
        {           
            ApplyTextOffset();

            Point textDimensions = DisplayText.Size(TextFont).ToPoint();
            //Point characterDimensions = TextFont.Font.MeasureString(LastChar).ToPoint();

            int x = (int)textDimensions.X;
            int y = (int)textDimensions.Y;
            //y = y <= Top + 2 ? Pointer.Height : y;

            Point newPosition = new Point(x, y);          
            CharBucket.AddCharacter(LastChar, newPosition, textDimensions);
            ProcessString();

        }
        bool IsOutOfBounds()
        {
            float x = Left + LastLineSize.X + "H|".Size(TextFont).X;
            float y = Top + LastLineSize.Y;
            Vector2 curTextPosition = new Vector2(x, y);
            return curTextPosition.X > _scrollBar.Left;
        }
        void ProcessString()
        {

            if (LastLine.Length > 0 && LastLine[LastLine.Length - 1] != '\n' && IsOutOfBounds())
            {
                _keyboardString += '\n';
                CreateText();
            }
        }
        public override UIObject HitTest(Point mousePosition)
        {
            UIObject result = null;
            if (Property != null && MainWindow.CurrentObject == this)
            {
                result = Property.HitTest(mousePosition);
                if (result != null)
                {
                    return result;
                }
            }
            if (Active)
            {
                result = _scrollBar.HitTest(mousePosition);
                if (result != null)
                {
                    return result;
                }                    
            }
            return base.HitTest(mousePosition); 
        }
      
        public void ApplyScrollOffset()
        {
            DisplayText = _keyboardString;
            if(string.IsNullOrEmpty(DisplayText))
            {
                return;
            }
            MaxLinesLength = Height / (Pointer.Height -3);

            if (NumberOfLines > MaxLinesLength && MaxLinesLength + _scrollBar.CurrentScrollValue + 1 <= NumberOfLines)
            {
                DisplayText = "";
                int start, end;

                start = NumberOfLines > MaxLinesLength ? _scrollBar.CurrentScrollValue : 0;
                end = MaxLinesLength + _scrollBar.CurrentScrollValue + 1;
          
                for (int i = start; i < end; i++)
                {
                    DisplayText += TextLines[i] + "\n";
                }
             
            }
        }
        protected override void ApplyTextOffset()
        {           
            DisplayText = _keyboardString;
            if(string.IsNullOrEmpty(DisplayText))
            {
                return;
            }

            MaxLinesLength = Height / (Pointer.Height - 3);
            if (NumberOfLines > 1)
            {
                DisplayText = "";
                int start, end;

                start = NumberOfLines > MaxLinesLength ? (NumberOfLines - 1) - MaxLinesLength : 0;
                end = NumberOfLines;

                for (int i = start; i < end; i++)
                {
                    DisplayText += TextLines[i] + "\n";
                }

                if (end > (MaxLinesLength + 1) && _scrollBar.SliderButton.Bottom < _scrollBar.Bottom - _scrollBar.SliderButton.Height)
                {
                    _scrollBar.CurrentScrollValue = (end - (MaxLinesLength + 1));
                }
            }
        }
        public override void ResetSize()
        {
            _scrollBar.ResetSize();
            UpdateText();
            ApplyTextOffset();
            ApplyScrollOffset();

            base.ResetSize();
        }
        public override void Resize(Point amount)
        {
            _scrollBar.ResetSize();
            base.Resize(amount);
          

            int border = 4;
            int newAmount = (_scrollBar.Position.Y + _scrollBar.Height) - Bottom;
            newAmount += border;
            _scrollBar.Resize(new Point(0, -newAmount));

            UpdateText();
            ApplyTextOffset();
            ApplyScrollOffset();

        }
        void UpdateText()
        {
            string fullText = _keyboardString.Replace("\n", "");
            string tmpStr = "";
            string copyKeyboardString = _keyboardString;
            if (!string.IsNullOrEmpty(fullText))
            {

                _keyboardString = "";
                for (int i = 0; i < fullText.Length; i++)
                {
                    tmpStr += fullText[i];
                    _keyboardString += fullText[i];
                    Vector2 dimensions = tmpStr.Size(TextFont);
                    Vector2 charDim = fullText[i].ToString().Size(TextFont);

                    float x = (float)Math.Ceiling(dimensions.X);

                    if (x + 4 >= Width - _scrollBar.Width)
                    {
                        if (tmpStr.Length >= 2)
                        {
                            tmpStr = tmpStr.Insert(tmpStr.Length - 2, "\n");
                            _keyboardString = _keyboardString.Insert(_keyboardString.Length - 2, "\n");
                        }


                    }
                    if (tmpStr.Length > 1 && tmpStr[tmpStr.Length - 2] == '\n')
                    {
                        tmpStr = "";
                    }
                }
                DisplayText = _keyboardString;
            }
            UpdateCharacters();
        }
        void UpdateCharacters()
        {
            int maxSize = Height / (Pointer.Height - 3);
            int start = NumberOfLines > maxSize ? (NumberOfLines - 1) - maxSize : 0;
            int end = NumberOfLines;

            string displayLocalText = "";

            for (int i = start; i < end; i++)
            {
                displayLocalText += TextLines[i] + (i == end - 1 ? "" : "\n");
            }
            string[] lines = displayLocalText.Split('\n');


            for (int i = 0; i < lines.Length - 1; i++)
            {
                lines[i] = lines[i].Insert(lines[i].Length, "\n");
            }

            int charIndex = (_keyboardString.Length - displayLocalText.Length);
            for (int i = 0; i < lines.Length; i++)
            {
                int height = 0;
                int width = 0;
                string copyLines = lines[i];

                for (int j = 0; j < lines[i].Length; j++)
                {

                    string subStr = copyLines.Substring(0, j + 1);
                    string character = lines[i][j].ToString();

                    Vector2 dim = subStr.Replace("\n", "").Size(TextFont);
                    Vector2 charSize = character.Size(TextFont);

                    height = (int)(dim.Y * (i + 1)) + 2;
                    width = (int)dim.X + 2;

                    Point location = new Point(width, height);
                    CharBucket.Update(charIndex, location, charSize.ToPoint());
                    charIndex++;
                }

            }
        }
        protected override void UpdatePointer(GameTime gameTime)
        {
            ProcessString();
            int x, y;

            int i = _keyboardString.Length - 1;
            if (LastChar == "\n")
            {
                x = Left + (int)TextLines[NumberOfLines - 2].Size(TextFont).X;
                y = Top + CharBucket[i].Y - Pointer.Height;
            }
            else
            {
                x = CharBucket[i] != Point.Zero ? Left + CharBucket[i].X : Left + 2;
                y = CharBucket[i] != Point.Zero ? Top + CharBucket[i].Y - Pointer.Height : Top + 2;
            }

            y = y <= Top ? Top + 2 : y;
            Point newPosition = new Point(x, y);


            Pointer += newPosition;

            Pointer.Active = true;
            Pointer.Update(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (Active)
            {
                UpdateResize();

                MaxLinesLength = Height / (Pointer.Height - 3);


                TextPosition = new Vector2(Left + TextOffset.X, Top);

                if (IsClicked && !Editable)
                {
                    OnKeyboardPressed();
                    ApplyTextOffset();
                }


                //ApplyTextOffset();
                UpdateCharacters();
                UpdatePointer(gameTime);


                if (MouseGUI.LeftIsPressed && MouseGUI.Focus != this)
                {
                    ReceivingInput = false;
                    MouseEvent.Out();
                }
                if (NumberOfLines > MaxLinesLength + 1)
                {
                    _scrollBar.Show();
                }
                else
                {
                    _scrollBar.Hide();
                }


                _scrollBar.Position = new Point(Right - _scrollBar.Width, Top);
                _scrollBar.Update(gameTime);               
            }
            if (Property != null)
            {
                Property.Update(gameTime);
            }
        }
        void UpdateResize()
        {
            if (MouseGUI.Focus == this && !Editable && MouseGUI.isScaleMode)
            {
                Point cornerPosition = new Point(Rect.Right, Rect.Bottom);

                Point changeAmount = (MouseGUI.Position - cornerPosition);

                Point newSize = new Point(changeAmount.X + MouseGUI._mouseScale.Width / 4, changeAmount.Y + MouseGUI._mouseScale.Height / 4);
                Resize(newSize);

            }
        }

        protected override void RenderText()
        {
            _stringRenderer.DrawString(TextFont, DisplayText, TextPosition, TextColor);
        }
        public override void Draw()
        {
            if (Active)
            {
                _spriteRenderer.Draw(Texture.Texture, Rect, SpriteColor * Alpha);
                if (IsClicked && !Editable)
                {
                    _spriteRenderer.Draw(Pointer.Texture.Texture, Pointer.Rect, TextColor * Pointer.Alpha);
                }
                RenderText();
                _scrollBar.Draw();
              
            }
            if (Property != null)
            {
                Property.Draw();
            }
        }
        
    }
}
