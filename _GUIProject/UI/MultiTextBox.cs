using Microsoft.Xna.Framework;
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
using static _GUIProject.UI.TextBox.CharacterBucket;
using static _GUIProject.AssetManager;

namespace _GUIProject.UI
{

    // This class will be fully refactored
    public class MultiTextBox : TextBox, IScrollable
    {
        enum Selection
        {
            UP,
            DOWN,
            LEFT,
            RIGHT,
            NONE
        }

        private static FontContent _font;
        public class Line
        {
            
            private readonly List<Character> _characters;         
            public string Text 
            {
                get { return String.Concat(_characters.Select(c => c.Text).ToList()); } 
            }
            public Point Size 
            { 
                get { return Text.Size(_font).ToPoint(); } 
            }
            public Character this[int index]
            {
                get { return _characters[index]; }                
            }

            public List<Character> Characters
            {
                get { return _characters; }
            }
            public Line()
            {
                _characters = new List<Character>();          
            }            
            public void InsertCharacter(Character character)
            {
                _characters.Add(character);
            }    
            public void RemoveCharacter(Character character)
            {                
                _characters.Remove(character);                
            }
            public Character HitTest(Point mousePosition)
            {
                return _characters.Where(c => c.Rect.Contains(mousePosition.ToPoint())).FirstOrDefault();
            }
            
            public void Draw(SpriteBatch batch)
            {
                foreach (Character ch in _characters)
                {
                    ch.Draw(batch);
                }
            }


        }
        class TextLines
        {
            private readonly List<Line> _lines;
            public Line LastLine 
            { get
                {
                    return _lines[_lines.Count - 1];
                } 
            }
            public int NumberOfLines
            {
                get { return _lines.Count; }
            }
            public List<Line> Lines 
            { 
                get { return _lines; } 
            }
            public TextLines()
            {
                _lines = new List<Line>();
                _lines.Add(new Line());
            }

            public Line this[int index]
            {
                get { return _lines[index]; }
            }
            public void AddLine(Line line)
            {
                _lines.Add(line);
            }
            public void AddCharacter(Character character)
            {
                LastLine.InsertCharacter(character);
            }

            public Character HitTest(Point mousePosition)
            {
                Character result = null;
                for (int i = 0; i < _lines.Count; i++)
                {
                    Line current = _lines[i];
                    result =  current.HitTest(mousePosition);
                }
                return result;
            }
        }
        
        [XmlIgnore]
        public int MaxLinesLength 
        {
            get { return Height / (Pointer.Height - 3); }
            set { }
        }

       [XmlIgnore]
        public int NumberOfLines
        {
            get { return _textLine.NumberOfLines; }
        }
        
        [XmlElement]
        public override string Text
        {
            get { return String.Concat(_displayLines.Select(l => l.Text)); }
            set 
            {
                foreach (char character in value)
                {
                    _keyboardString += character;
                    CreateText();
                }          
            }
        }
        public Character TopSelect
        {
            get { return _textLine.Lines.AsReadOnly().SelectMany(l => l.Characters.Where(ch => ch.Selected)).FirstOrDefault(); }
        }
        public Character BottomSelect
        {
            get
            {                
                return _textLine.Lines.AsReadOnly().SelectMany(l => l.Characters.Where(ch => ch.Selected)).LastOrDefault();
            }
        }
        private readonly TextLines _textLine;
     
        private List<Line> _displayLines;
        private ScrollBar _scrollBar;
        private Character _topChar;
        private Character _bottomChar;
        private Character _currChar;       
        private Point _prevPosition;
        private Selection _prevSelect;
        public MultiTextBox() : base("DefaultMultiTexboxTX", "DefaultTextboxPointerTX", TextBoxType.TEXT, DrawPriority.NORMAL)
        {            
            MoveState = MoveOption.DYNAMIC;
            XPolicy = SizePolicy.EXPAND;
            YPolicy = SizePolicy.EXPAND;
            LoadAttributes();
            _textLine = new TextLines();
            _displayLines = new List<Line>();
          
        }
        void LoadAttributes()
        {
            base.Setup();
            CharBucket = new CharacterBucket();            
            _scrollBar = new ScrollBar();
            _scrollBar.Parent = this;
            _scrollBar.Initialize();
           
            _scrollBar.Setup();
           
            _scrollBar.Position = new Point(Right - _scrollBar.Width, Top);
            Pointer.Active = true;

            TextColor = Color.Black;
            _font = TextFont;

            _prevSelect = Selection.NONE;
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


            if (Singleton.Input.KeyReleased(Keys.Back))
            {

                if (!string.IsNullOrEmpty(_keyboardString))
                {
                    List<Character> characters = new List<Character>();
                    foreach (Line line in _textLine.Lines)
                    {
                        characters.AddRange(line.Characters.Select(l => l).Where(ch => ch.Selected));
                    }
                    bool isSelected = characters.Count > 0;
                    if (isSelected)
                    {
                        foreach (Line line in _textLine.Lines)
                        {
                            foreach (Character ch in characters)
                            {
                                line.RemoveCharacter(ch);
                            }
                        }
                    }
                    else
                    {
                        bool isNewLine = LastChar == "\n";
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
                            CharBucket.RemoveCharacter();
                            _keyboardString = _keyboardString.Remove(_keyboardString.Length - 1, 1);

                        }
                    }
                }

                UpdatePointerAndCharacters();
                KeyboardEvents.Released();
            }
            else
            {
                base.OnKeyboardPressed();
            }

        }
        public Character CreateCharacter(string character, int row, int column)
        {
            Point size = character.Size(TextFont).ToPoint();
            Point position = new Point
            {
                X = TextOffset.X + _textLine.LastLine.Size.X,
                Y = (_textLine.NumberOfLines -1) * _textLine.LastLine.Size.Y
            };
            return new Character(new Rectangle(Position + position, size), character, row, column);
        }
        protected override void CreateText()
        {
            if (NumberOfLines > (MaxLinesLength + 1) && _scrollBar.SliderButton.Bottom < _scrollBar.Bottom - _scrollBar.SliderButton.Height)
            {
                _scrollBar.CurrentScrollValue = (NumberOfLines - (MaxLinesLength + 1));
            }      

            _textLine.LastLine.InsertCharacter(CreateCharacter(LastChar, _textLine.NumberOfLines, _textLine.LastLine.Text.Length +1));        

            if (_textLine.NumberOfLines > 0  && IsOutOfBounds())
            {            
                _keyboardString += '\n';
                _textLine.AddCharacter(CreateCharacter("\n", _textLine.NumberOfLines, _textLine.LastLine.Text.Length + 1));
                _textLine.AddLine(new Line());              
            }       

            UpdatePointerAndCharacters();

        }
        bool IsOutOfBounds()
        {
            float x = Left + _textLine.LastLine.Size.X + "H|".Size(TextFont).X;
            float y = Top + _textLine.LastLine.Size.Y;
            Vector2 curTextPosition = new Vector2(x, y);
            return curTextPosition.X > _scrollBar.Left;
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
        
        public void ApplyScroll()
        {
          
            _displayLines.Clear();

            if (NumberOfLines > MaxLinesLength && MaxLinesLength + _scrollBar.CurrentScrollValue + 1 <= NumberOfLines)
            {
                DisplayText = "";
                int start, end;

                start = NumberOfLines > MaxLinesLength ? _scrollBar.CurrentScrollValue: 0;
                end = MaxLinesLength + _scrollBar.CurrentScrollValue + 1;
          
                for (int i = start; i < end; i++)
                {
                    _displayLines.Add(_textLine[i]);
                }
            }
        }
       
        public override void ResetSize()
        {
            _scrollBar.ResetSize();          
            
            ApplyScroll();
            UpdateText();
            UpdatePointerAndCharacters();

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
           
            ApplyScroll();
            UpdateText();
            UpdatePointerAndCharacters();

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
            UpdatePointerAndCharacters();
        }
       
       List<Line> GetCurrentLines()
        {
            int start = NumberOfLines > MaxLinesLength ? (NumberOfLines - 1) - MaxLinesLength : 0;
            int end = NumberOfLines;

            List<Line> currentLines = new List<Line>();

            for (int i = start; i < end; i++)
            {                
                currentLines.Add(_textLine[i]);
            }
            return currentLines;
        }
        void UpdatePointerAndCharacters()
        {          
            List<Line> lines = GetCurrentLines();          
            _displayLines = lines;    
           
            for (int i = 0; i < lines.Count; i++)
            {              
                int y = 0;
                int x = 0;
                string copyLines = lines[i].Text;

                for (int j = 0; j < lines[i].Text.Length; j++)
                {
                    string subStr = copyLines.Substring(0, j + 1);
                    string character = lines[i].Text[j].ToString();

                    Vector2 dim = subStr.Replace("\n", "").Size(TextFont);
                    Vector2 charSize = character.Size(TextFont);

                    x = (int)dim.X - (int)charSize.X + TextOffset.X;
                    y = (int)dim.Y * i;

                    lines[i][j].Rect = new Rectangle(Position + new Point(x, y), charSize.ToPoint());                   
                    Pointer += (Point)lines[i][j].Rect.Location + new Point((int)charSize.X, 0);
                    
                }

            }
            if (string.IsNullOrEmpty(lines[0].Text))
            {
                Pointer += new Point(Left, Top);
            }
        }
        void ResetSelection()
        {
            foreach (Line line in _displayLines)
            {
                foreach (Character ch in line.Characters)
                {
                    ch.Selected = false;
                }
               
            }
            _topChar = null;
            _bottomChar = null;
            _prevSelect = Selection.NONE;
        }
      

        void SelectCharacters(Selection selection)
        {
            if (_bottomChar != null && _topChar != null)
            {
                if (selection == Selection.UP)
                {                   
                    if (_topChar.Row < _bottomChar.Row)
                    { 
                        foreach (Character ch in _textLine.Lines[_bottomChar.Row - 1].Characters)
                        {
                            if (ch.Selected && ch.Column >= _bottomChar.Column)
                            {
                                ch.Selected = false;
                            }
                            if (!ch.Selected && ch.Column < _bottomChar.Column)
                            {
                                ch.Selected = true;
                            }
                        }

                        foreach (Character ch in _textLine.Lines[_topChar.Row - 1].Characters)
                        {
                            if (!ch.Selected && ch.Column > _topChar.Column)
                            {
                                ch.Selected = true;
                            }
                        }

                        // Lines in between
                        foreach (Line line in _textLine.Lines)
                        {

                            foreach (Character ch in line.Characters.Where(ch => ch.Row > _topChar.Row && ch.Row < _bottomChar.Row))
                            {
                                if (!ch.Selected)
                                {
                                    ch.Selected = true;
                                }
                            }

                        }
                    }
                    else
                    {
                        Character first = _textLine.Lines[_topChar.Row - 1].Characters.Where(ch => ch.Selected).FirstOrDefault();
                        Character last = _textLine.Lines[_topChar.Row - 1].Characters.Where(ch => ch.Selected).LastOrDefault();
                     
                        foreach (Character ch in _textLine.Lines[_topChar.Row - 1].Characters)
                        {
                            if (ch.Column > first.Column && ch.Column < last.Column)
                            {
                           
                                ch.Selected = true;
                            }
                        }
                    }
                }
                else if (selection == Selection.DOWN)
                {                
                    if (_topChar.Row > _bottomChar.Row)
                    {                      
                        foreach (Character ch in _textLine.Lines[_bottomChar.Row - 1].Characters)
                        {
                            if (ch.Selected && ch.Column <= _bottomChar.Column)
                            {
                                ch.Selected = false;
                            }
                            if (!ch.Selected && ch.Column > _bottomChar.Column)
                            {
                                ch.Selected = true;
                            }
                        }

                        foreach (Character ch in _textLine.Lines[_topChar.Row - 1].Characters)
                        {
                            if (!ch.Selected && ch.Column < _topChar.Column)
                            {
                                ch.Selected = true;
                            }
                        }

                        // Lines in between
                        foreach (Line line in _textLine.Lines)
                        {

                            foreach (Character ch in line.Characters.Where(ch => ch.Row < _topChar.Row && ch.Row > _bottomChar.Row))
                            {
                                if (!ch.Selected)
                                {
                                    ch.Selected = true;
                                }
                            }

                        }
                    }
                    else
                    {
                        Character first = _textLine.Lines[_topChar.Row - 1].Characters.Where(ch => ch.Selected).FirstOrDefault();
                        Character last = _textLine.Lines[_topChar.Row - 1].Characters.Where(ch => ch.Selected).LastOrDefault();

                        foreach (Character ch in _textLine.Lines[_topChar.Row - 1].Characters)
                        {
                            if (ch.Column > first.Column && ch.Column < last.Column)
                            {
                                ch.Selected = true;
                            }
                        }
                    }
                }
                else if (selection == Selection.RIGHT)
                {
                   
                    Character first = _textLine.Lines[_topChar.Row - 1].Characters.Where(ch => ch.Selected).FirstOrDefault();
                    Character last = _textLine.Lines[_topChar.Row - 1].Characters.Where(ch => ch.Selected).LastOrDefault();
                  
                    foreach (Character ch in _textLine.Lines[_topChar.Row -1].Characters)
                    {
                        if(last != null)
                        {
                            if (!ch.Selected && ch.Column > first.Column && ch.Column < last.Column)
                            {
                                ch.Selected = true;
                            }
                            else if (ch.Selected && last.Rect.Left > MouseGUI.Position.X && MouseGUI.Position.X > ch.Rect.Right)
                            {
                                ch.Selected = false;
                            }
                        }
                    
                    }

                }
                else if (selection == Selection.LEFT)
                {
                    Character first = _textLine.Lines[_topChar.Row - 1].Characters.Where(ch => ch.Selected).FirstOrDefault();
                    Character last = _textLine.Lines[_topChar.Row - 1].Characters.Where(ch => ch.Selected).LastOrDefault();

                    foreach (Character ch in _textLine.Lines[_topChar.Row - 1].Characters)
                    {
                        if (!ch.Selected && ch.Column > first.Column && ch.Column < last.Column)
                        {
                            ch.Selected = !ch.Selected;
                        }
                        else if (ch.Selected && first.Rect.Right < MouseGUI.Position.X && MouseGUI.Position.X < ch.Rect.Left)
                        {
                            ch.Selected = false;
                        }                       
                    }
                }
            }
           
        }
        public void SelectRow(int row, bool select = true)
        {
            foreach (Line line in _textLine.Lines)
            {
                var selectables = line.Characters.Where(ch => ch.Row == row);

                foreach (Character ch  in selectables)
                {
                    ch.Selected = select;
                }
            }
        }
        public override void Update(GameTime gameTime)
        {
            if (Active)
            {
                if (MouseGUI.LeftWasPressed)
                {
                    _prevPosition = MouseGUI.Position;
                    ResetSelection();
                }


                if (MouseGUI.LeftIsPressed)
                {
                    for (int i = 0; i < _displayLines.Count; i++)
                    {
                        _currChar = _displayLines[i].HitTest(MouseGUI.Position);
                        if (_currChar != null)
                        {
                            break;
                        }
                    }

                    Point delta = MouseGUI.Position - _prevPosition;

                    if (_currChar != null)
                    {
                        _currChar.Selected = true;  

                        Pointer += new Point(_currChar.Rect.Right, _currChar.Rect.Top);
                    }

                    if (MouseGUI.LeftWasPressed)
                    {
                        _bottomChar = BottomSelect;
                    }

                    if(_bottomChar == null)
                    {
                        _bottomChar = TopSelect;
                    }

                    if (TopSelect != null && BottomSelect != null)
                    {
                        _topChar = TopSelect.Row < _bottomChar.Row ? TopSelect : BottomSelect;
                    }                                  
                  
                    if (_prevPosition != MouseGUI.Position)
                    {
                        if (delta.Y > 0)
                        {
                            SelectCharacters(Selection.DOWN);
                        }
                        else if (delta.Y < 0)
                        {
                            SelectCharacters(Selection.UP);
                        }
                        else if (delta.X > 0)
                        {
                            SelectCharacters(Selection.RIGHT);
                        }
                        else if (delta.X < 0)
                        {
                            SelectCharacters(Selection.LEFT);
                        }

                    }
                }
              

                _prevPosition = MouseGUI.Position;

                Pointer.Update(gameTime);

                TextPosition = new Vector2(Left + TextOffset.X, Top);


                if (IsClicked && !Editable)
                {
                    OnKeyboardPressed();
                }

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



        public override void Draw()
        {
            if (Active)
            {

                _spriteRenderer.Draw(Texture.Texture, Rect, SpriteColor * Alpha);

                foreach (Line line in _displayLines)
                {
                    line.Draw(_spriteRenderer);
                }

                if (IsClicked && !Editable)
                {
                    _spriteRenderer.Draw(Pointer.Texture.Texture, Pointer.Rect, TextColor * Pointer.Alpha);
                }

                _stringRenderer.DrawString(TextFont, Text, TextPosition, TextColor);

                _scrollBar.Draw();

            }
            if (Property != null)
            {
                Property.Draw();
            }
        }
        
    }
}
