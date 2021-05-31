using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Serialization;

using System.Runtime.Serialization;
using System.Threading.Tasks;

using _GUIProject.Events;
using ExtensionMethods;
using static _GUIProject.FontManager;
using static _GUIProject.AssetManager;
using System.Diagnostics.CodeAnalysis;
using static _GUIProject.UI.TextArea;

namespace _GUIProject.UI
{
   
   
    public class TextBox : Sprite
    {

        public enum TextBoxType
        {
            TEXT,
            PASSWORD
        }
        public class Character : IEquatable<Character>
        {
            private int _column;
            private int _row;
            private readonly Sprite _charSprite;

            public int Column 
            {
                get { return _column; }
                set { _column = value; }
            }
            public int Row
            {
                get { return _row; }
                set { _row = value; }
            }
            public bool Pointed 
            { 
                get;
                set; 
            }
            public Character(Rectangle rect, string text)
            {
                Rect = rect;
                Text = text;
                _charSprite = new Sprite("char", DrawPriority.LOWEST);
                _charSprite.Resize(Rect.Size);
                _row = _column = 1;
            }

            public string Text { get; private set; }
            public Rectangle Rect { get; set; }
            public bool Selected { get; set; }

            //public static implicit operator Tuple<int,int>(Character rhs)
            //{
            //    return new Tuple<int,int>(rhs.Row - 1, rhs.Column - 1);
            //}
            public void Draw(SpriteBatch batch)
            {
                batch.Draw(_charSprite.Texture, Rect, Selected ? Color.CornflowerBlue : Color.Transparent);
            }
           
            public static implicit operator Point(Character character)
            {
                return new Point(character.Rect.Location);
            }
           
            public bool Equals([AllowNull] Character other)
            {
                return GetHashCode().Equals(other.GetHashCode());
            }
            public bool Contains(Point mousePosition)
            {
                return Rect.Contains(mousePosition.ToPoint());
            }
            public bool NewLine 
            {
                get { return Text.Equals("\n"); }
            }
            public int Top 
            {
                get { return Rect.Top; }
            }
            public int Bottom 
            { 
                get { return Rect.Bottom; }
            }
            public int Right
            {
                get { return Rect.Right; }
            }
            public int Left
            {
                get { return Rect.Left; }
            }
            public Point Center
            {
                get { return Rect.Center; }
            }
            
            public int Width
            {
                get { return Rect.Size.X; }
            }
            public int Height
            {
                get { return Rect.Size.Y; }
            }

        }
        public class CharacterBucket
        {
            private Sprite _charSprite;
            readonly Dictionary<int, Character> _bucket;

            private int index = 0;
            private string _fullText;     
           

            public CharacterBucket()
            {
                _bucket = new Dictionary<int, Character>();
                _fullText = "";                
                _charSprite = new Sprite("char", DrawPriority.LOWEST); 

            }
            public void AddCharacter(string character, Point location, Point size)
            {

                _fullText += character;              
             
                _bucket.Add(index++, new Character(new Rectangle(location, size), character));

            }
            public Character HitTest(Point mousePosition)
            {                
                return _bucket.Where(s => s.Value.Rect.Contains(mousePosition.ToPoint())).FirstOrDefault().Value;
            }
            public void Update(int index, Point position, Point size)
            {
                if (index < _bucket.Values.Count)
                {
                    _bucket[index].Rect = new Rectangle(position, size);
                }

            }
            public void InsertAt(int index, string character)
            {
                _fullText.Insert(index, character);
            }

            public void Draw(SpriteFont font, SpriteBatch batch)
            {
                foreach (var item in _bucket)
                {
                    item.Value.Draw(batch);
                }
            }
            public void Clear()
            {
                _fullText = "";
                _bucket.Clear();
                index = 0;
            }
            public void RemoveCharacter()
            {
                if (_fullText.Length > 0)
                {
                    _fullText = _fullText.Remove(index - 1, 1);
                    _bucket.Remove(index--);
                }

            }
         
            public Character Current
            {
                get
                {
                    return this[index - 1];
                }
            }

            public Character this[int index]
            {
                get
                {
                    return _bucket[index];
                }
            }
            public Character this[string character]
            {
                get
                {
                    return _bucket.Values.Where(v => v.Text == character).FirstOrDefault();
                }
            }


            public Dictionary<int, Character> Bucket
            {
                get { return _bucket; }
            }
        }
        public class CustomText
        {
            public string _text;
            public ColorObject _color;

            public CustomText(string word)
            {

            }
           
        }
        protected class clsInput
        {

            public string AKeyState = "none";
            public string BKeyState = "none";
            public string CKeyState = "none";
            public string DKeyState = "none";
            public string EKeyState = "none";
            public string FKeyState = "none";
            public string GKeyState = "none";
            public string HKeyState = "none";
            public string IKeyState = "none";
            public string JKeyState = "none";
            public string KKeyState = "none";
            public string LKeyState = "none";
            public string MKeyState = "none";
            public string NKeyState = "none";
            public string OKeyState = "none";
            public string PKeyState = "none";
            public string QKeyState = "none";
            public string RKeyState = "none";
            public string SKeyState = "none";
            public string TKeyState = "none";
            public string UKeyState = "none";
            public string VKeyState = "none";
            public string WKeyState = "none";
            public string XKeyState = "none";
            public string YKeyState = "none";
            public string ZKeyState = "none";
            public string SpaceKeyState = "none";
            public string BackKeyState = "none";
            public string EnterKeyState = "none";
            /// //////////////////////////////////////////////
            public string OemSemicolonKeyState = "none";
            public string OemQuotesKeyState = "none";
            public string OemCommaKeyState = "none";
            public string OemPeriodKeyState = "none";
            public string OemQuestionKeyState = "none";
            /////////////////////////////////////////////////
            public string OemOpenBracketsKeyState = "none";
            public string OemCloseBracketsKeyState = "none";
            public string OemPipeKeyState = "none";
            //////////////////////////////////////////////////
            public string D1KeyState = "none";
            public string D2KeyState = "none";
            public string D3KeyState = "none";
            public string D4KeyState = "none";
            public string D5KeyState = "none";
            public string D6KeyState = "none";
            public string D7KeyState = "none";
            public string D8KeyState = "none";
            public string D9KeyState = "none";
            public string D0KeyState = "none";
            //////////////////////////////////////////////////
            public string OemTildeKeyState = "none";
            public string OemMinusKeyState = "none";
            public string OemPlusKeyState = "none";
            /// ////////////////////////////////////////////////
            public bool isShiftPressed(Keys[] mykeys)
            {                
                for (int i = 0; i < mykeys.Length; i++)
                {
                    if (mykeys[i] == Keys.LeftShift || mykeys[i] == Keys.RightShift)
                    {
                        return true;
                    }
                }
                return false;
            }
            public bool isControlPressed(Keys[] mykeys)
            {
             
                foreach (Keys key in mykeys)
                {
                    if (key == Keys.LeftControl || key == Keys.RightControl)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
       
        public struct mylist
        {
            public string mystring;
            public int key;

        }
      


        [XmlIgnore]
        public List<CustomText> _textList = new List<CustomText>();
      
        [XmlIgnore]
        public Rectangle CurrItemRect { get; set; }
      
        [XmlIgnore]
        public static Sprite Pointer { get; protected set;}
        
        [XmlIgnore]
        public FontContent TextFont { get; set; }
        
        
        [XmlIgnore]
        public CharacterBucket CharBucket { get; protected set; }

        [XmlAttribute]
        public TextBoxType Category { get; set; }

        [XmlIgnore]
        public Vector2 TextSize
        {
            get
            {
                return _keyboardString.Size(TextFont);
            }
        }
    
      
        [XmlIgnore]
        public Point TextOffset { get; set; }
        
        [XmlIgnore]
        public Vector2 TextPosition { get; set; }
       
        [XmlIgnore]
        public bool StickSampleText { get; set; }
       
        [XmlIgnore]
        public bool ReceivingInput { get; set; } = false;

        [XmlIgnore]
        public string SampleText { get; set; } = "";

        [XmlIgnore]
        public string DisplayText { get; set; } = "";

        private bool _selected;
        [XmlIgnore]
        public bool Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                IsClicked = _selected;
            }
        }

        protected string LastChar
        {
            get { return _keyboardString.Length > 0 ? _keyboardString.Last().ToString() : " "; }
        }
       

       [XmlElement]
        public override string Text
        {
            get { return _keyboardString; }
            set
            {                
                _keyboardString = value;
                DisplayText = _keyboardString;
         
                ApplyTextLines();
            }
        }
        [XmlAttribute]
        public FontType Font
        {
            get { return Singleton.Font.GetType(TextFont); }
            set { TextFont = Singleton.Font.GetFont(value); }
        }

        [XmlAttribute]
        public int FontSize
        {
            get { return (int)"A".Size(TextFont).Length(); }
            set
            {
            }
        }

        [XmlAttribute]
        public int FieldWidth { get; set; }

        protected bool _isShift;
        protected bool _isControl;
        protected Keys[] _mykeys;
        protected clsInput _myclsinput = new clsInput();
        protected string _keyboardString = "";

        public TextBox() : base("DefaultTextboxTX", DrawPriority.NORMAL)
        {           
            Category = TextBoxType.TEXT;
            LoadAttributes();
            Active = true;
        }


        public TextBox(string textureName, string pointerTextureName, TextBoxType category, DrawPriority priority) : base(textureName, priority)
        {           
            Category = category;             
            LoadAttributes();
            Active = true;
        }
        void LoadAttributes()
        {
            XPolicy = SizePolicy.EXPAND;
            YPolicy = SizePolicy.FIXED;
            MoveState = MoveOption.DYNAMIC;
            TextColor = Color.Black;
            TextFont = Singleton.Font.GetFont(FontType.GEORGIA);
            FieldWidth = 0;
            TextOffset = new Point(4, 4);
            Pointer = new Sprite("DefaultTextboxPointerTX", DrawPriority.LOW);
            Pointer.Initialize();
            Pointer.SpriteColor = Color.White;
            Pointer.FadeCapable = true;
         
        }
        public override void Initialize()
        {
            base.Initialize();
            CharBucket = new CharacterBucket();
          
            KeyboardEvents = new KeyboardEvents(this);
            MouseEvent.onMouseOut += (sender, args) => { Selected = false; };
            
            Active = true;
        }
        public override void InitPropertyPanel()
        {
            Property = new PropertyPanel(this);
            Property.AddProperties(PropertyPanel.PropertyOwner.TEXTBOX);
            Property.SetupProperties();
          
        }
        public override void Setup()
        {            
            base.Setup();

            if (FieldWidth != 0)
            {
                Size += new Point(Math.Abs(FieldWidth - Size.X), 0);
            }

            Pointer.Position = new Point(TextOffset.X, (Top + Height / 2) - Pointer.Texture.Height / 2);
            Pointer.Setup();
            if(CharBucket != null)
            {
                CharBucket.AddCharacter("", new Point(Left, Top), "".Size(TextFont).ToPoint());
            }
         


        }
        public void SimulateInput(string textInput)
        {     
            for (int i = 0; i < textInput.Length; i++)
            {
                _keyboardString += textInput[i];
                CreateText();
            }           
        }        
       
        public override void AddSpriteRenderer(SpriteBatch batch)
        {            
            _spriteRenderer = batch;
        }
        public override void AddStringRenderer(SpriteBatch batch)
        {          
            _stringRenderer = batch;
        }


        public void AddTextSpriteBatch(SpriteBatch batch)
        {
            _stringRenderer = batch;
        }
        public override void AddPropertyRenderer(SpriteBatch batch)
        {
            Property.AddPropertyRenderer(batch);
        }

        // Make this more compact and extendable
        public virtual void OnKeyboardPressed()
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
                }
            }
            if (Singleton.Input.KeyReleased(Keys.Back))
            {                

                if (!string.IsNullOrEmpty(_keyboardString))
                {
                   
                    _keyboardString = _keyboardString.Remove(_keyboardString.Length - 1, 1);
                    int startIndex = _keyboardString.Length - DisplayText.Length;

                    if (IsOutOfBounds(TextSize.X) && startIndex >= 0)
                    {
                        string newChar = _keyboardString.Substring(startIndex, 1);
                        DisplayText = DisplayText.Insert(0, newChar);
                        DisplayText = DisplayText.Remove(DisplayText.Length - 1, 1);
                        startIndex--;
                        if (startIndex >= 0)
                        {
                            string extraChar = _keyboardString.Substring(startIndex, 1);
                            string tmp = DisplayText.Insert(0, newChar);
                            float newSize = tmp.Size(TextFont).X - extraChar.Size(TextFont).X;
                            if (!IsOutOfBounds(newSize))
                            {
                                DisplayText = tmp;
                            }
                        }

                        RearrangeBox();
                    }
                    else
                    {
                        DisplayText = DisplayText.Remove(DisplayText.Length - 1, 1);

                    }

                    CharBucket.RemoveCharacter();
                    
                }
                KeyboardEvents.Released();
            }

        }
        protected virtual void CreateText()
        {           
            ApplyTextLines();
            //Point characterDimensions = TextFont.Font.MeasureString(LastChar).ToPoint();
            Point currCharDim = DisplayText.Size(TextFont).ToPoint();
            int x = currCharDim.X;
            int y = currCharDim.Y;

            Point newPosition = new Point(x, y);
            CharBucket.AddCharacter(LastChar, newPosition, currCharDim);
        }
        void RearrangeBox()
        {
            int offset = (_keyboardString.Length - DisplayText.Length);
            int length = _keyboardString.Length - offset;
            int lastCharSize = (int)LastChar.Size(TextFont).X;
            DisplayText = _keyboardString.Substring(offset, _keyboardString.Length - offset);
            float currentDisplayTextSize = DisplayText.Size(TextFont).X - lastCharSize;

            while (IsOutOfBounds(currentDisplayTextSize) && length > 0)
            {
                DisplayText = _keyboardString.Substring(offset, length);
                offset++;
                length = _keyboardString.Length - offset;
                currentDisplayTextSize = DisplayText.Size(TextFont).X - lastCharSize;
            }

        }
        protected virtual void ApplyTextLines()
        {
            float currentDisplayTextSize = DisplayText.Size(TextFont).X;
            

            if (_keyboardString.Length > 0 && IsOutOfBounds(currentDisplayTextSize))
            {
                RearrangeBox();
            }
            else
            {

                if (!IsOutOfBounds(TextSize.X))
                {
                    DisplayText = _keyboardString;
                }
                else
                {
                    DisplayText += LastChar;
                }

            }


        }
        protected virtual void UpdatePointer(GameTime gameTime)
        {
            int x = Left + ((Point)CharBucket.Current).X + Pointer.Width;
            int y = Center.Y - Pointer.Height / 2;

            Pointer += new Point(x, y);
            Pointer.Active = true;
            Pointer.Update(gameTime);
        }
        public virtual void Clear()
        {
            _textList.Clear();
            
            CharBucket.Clear();
            CharBucket.AddCharacter("", Point.Zero, "".Size(TextFont).ToPoint());
            Pointer += Point.Zero;

            _keyboardString = "";
            DisplayText = "";
        }

        protected virtual bool IsOutOfBounds(float size)
        {
            Point lastCharSize = LastChar.Size(TextFont).ToPoint();
            return size + lastCharSize.X >= Width;
        }
        public override UIObject HitTest(Point mousePosition)
        {
            UIObject result = null;
            if (Property != null && MainWindow.CurrentObject == this)
            {
                result = Property.HitTest(mousePosition); 
                if(result != null)
                {
                    return result;
                }
            }
            if (Active)
            {
                return base.HitTest(mousePosition);             
            }           

            return null;
          
        }
        public override void Update(GameTime gameTime)
        {
            if (Active)
            {
                Vector2 textSize = SampleText.Length > 0 ? SampleText.Size(TextFont) : TextSize;
                TextPosition = new Vector2(Left + TextOffset.X, Top + TextOffset.Y);


                if (IsClicked)
                {
                    if (MouseGUI.LeftIsPressed && MouseGUI.Focus != this)
                    {
                        MouseEvent.Out();
                    }
                    OnKeyboardPressed();

                    UpdatePointer(gameTime);

                }
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
                
                if (IsClicked && Pointer.Active)
                {
                    _spriteRenderer.Draw(Pointer.Texture.Texture, Pointer.Rect, TextColor * Pointer.Alpha);
                }
                _stringRenderer.DrawString(TextFont, IsClicked ? DisplayText : (StickSampleText ? SampleText : DisplayText), TextPosition, TextColor);

            }

            if (Property != null)
            {
                Property.Draw();
            }
        }
        
    }
}
