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
    public class TextArea : TextBox, IScrollable
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
            public enum SelectionMode
            {
                DISSELECT,
                SELECT
            };
            
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
            public void LineSelection(Func<Character, bool> predicate, SelectionMode selection)
            {
                foreach(Character ch in _characters.Where(predicate))
                {
                    ch.Selected = Convert.ToBoolean(selection);
                }
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
            public bool Contains(Point mousePosition)
            {               
                return _characters.AsParallel().Any(ch => ch.Contains(MouseGUI.Position));
            }
            public void Draw(SpriteBatch batch)
            {
                foreach (Character ch in _characters)
                {
                    ch.Draw(batch);
                }
            }
            public static bool operator ==(Line l1, Line l2)
            {
                return l1.Text.Equals(l2.Text);
            }
            public static bool operator !=(Line l1, Line l2)
            {
                return !(l1 == l2);
            }
        }
        class TextLines
        {
            private readonly List<Line> _lines;
            public Line Last
            { 
                get
                {
                    return _lines.LastOrDefault();
                } 
            }
            public Line First
            {
                get
                {
                    return _lines.FirstOrDefault();
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
                Last.InsertCharacter(character);
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
        public Line TopSelected
        {
            get
            {              
                return _textLine.Lines.First(l => l.Characters.Exists(ch => ch.Selected));
            }
        }
        public Line BottomSelected
        {
            get
            {
                return _textLine.Lines.Last(l => l.Characters.Exists(ch => ch.Selected));
            }
        }


        private readonly TextLines _textLine;
     
        private List<Line> _displayLines;
        private ScrollBar _scrollBar;   
        private Character _firstSelected;
        private Character _currChar;       
        private Point _prevPosition;
        private Selection _prevSelect;
        public TextArea() : base("DefaultMultiTexboxTX", "DefaultTextboxPointerTX", TextBoxType.TEXT, DrawPriority.NORMAL)
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
                X = TextOffset.X + _textLine.Last.Size.X,
                Y = (_textLine.NumberOfLines -1) * _textLine.Last.Size.Y
            };
            return new Character(new Rectangle(Position + position, size), character, row, column);
        }
        protected override void CreateText()
        {
            if (NumberOfLines > (MaxLinesLength + 1) && _scrollBar.SliderButton.Bottom < _scrollBar.Bottom - _scrollBar.SliderButton.Height)
            {
                _scrollBar.CurrentScrollValue = (NumberOfLines - (MaxLinesLength + 1));
            }      

            _textLine.Last.InsertCharacter(CreateCharacter(LastChar, _textLine.NumberOfLines, _textLine.Last.Text.Length +1));        

            if (_textLine.NumberOfLines > 0  && IsOutOfBounds())
            {            
                _keyboardString += '\n';
                _textLine.AddCharacter(CreateCharacter("\n", _textLine.NumberOfLines, _textLine.Last.Text.Length + 1));
                _textLine.AddLine(new Line());              
            }       

            UpdatePointerAndCharacters();

        }
        bool IsOutOfBounds()
        {
            float x = Left + _textLine.Last.Size.X + "H|".Size(TextFont).X;
            float y = Top + _textLine.Last.Size.Y;
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
            _firstSelected = null;
            _prevSelect = Selection.NONE;
        }
      

        void SelectCharacters(Selection selection)
        {
            if (_firstSelected != null)
            {
                if (selection == Selection.UP)
                {
                    Line firstLine = BottomSelected;

                    foreach (Line line in _textLine.Lines)
                    {
                        if (line.Contains(MouseGUI.Position))
                        {
                            if (line != firstLine)
                            {
                                line.LineSelection(new Func<Character, bool>
                                    (ch => !ch.Selected && ch.Left >= MouseGUI.Position.X && ch.Row < _firstSelected.Row),
                                    Line.SelectionMode.SELECT);
                            }
                        }
                        else
                        {
                            if (line != firstLine)
                            {
                                line.LineSelection(new Func<Character, bool>
                                    (ch => !ch.Selected && ch.Row < _firstSelected.Row && ch.Top > MouseGUI.Position.Y),
                                    Line.SelectionMode.SELECT);
                            }
                            else
                            {
                                line.LineSelection(new Func<Character, bool>
                                    (ch => !ch.Selected && ch.Column < _firstSelected.Column && ch.Top > MouseGUI.Position.Y),
                                    Line.SelectionMode.SELECT);
                            }
                        }
                    }

                }
                else if (selection == Selection.DOWN)
                {                   
                    Line firstLine = TopSelected;

                    Character firstSelected = firstLine.Characters.Where(ch => ch.Selected).FirstOrDefault();
                    foreach (Line line in _textLine.Lines)
                    {
                        if (line.Contains(MouseGUI.Position))
                        {
                            if (line != firstLine)
                            {
                                line.LineSelection(new Func<Character, bool>
                                    (ch => !ch.Selected && ch.Left <= MouseGUI.Position.X),
                                    Line.SelectionMode.SELECT);                                
                           
                            }                          
                        }
                        else
                        {
                            if (line != firstLine)
                            {
                                line.LineSelection(new Func<Character, bool>
                                    (ch => !ch.Selected && ch.Row >= firstSelected.Row &&
                                     _firstSelected == firstSelected && ch.Bottom < MouseGUI.Position.Y),
                                    Line.SelectionMode.SELECT);

                                line.LineSelection(new Func<Character, bool>
                                    (ch => ch.Selected && ch.Row >= firstSelected.Row &&
                                    _firstSelected != firstSelected && ch.Bottom <= MouseGUI.Position.Y),
                                    Line.SelectionMode.DISSELECT);
                            }
                            else
                            {
                                line.LineSelection(new Func<Character, bool>
                                    (ch => !ch.Selected &&  ch.Column > firstSelected.Column &&
                                     _firstSelected == firstSelected && ch.Bottom < MouseGUI.Position.Y),
                                    Line.SelectionMode.SELECT);

                                line.LineSelection(new Func<Character, bool>
                                   (ch => ch.Selected && ch.Column >= firstSelected.Column &&
                                     _firstSelected != firstSelected && ch.Bottom < MouseGUI.Position.Y),
                                   Line.SelectionMode.DISSELECT);
                            }
                        }
                    }
                }
                else if (selection == Selection.RIGHT)
                {
                    //Character lastChar = BottomSelected[BottomSelected.Characters.Count - 1];
                    //foreach (Line line in _textLine.Lines)
                    //{
                    //    line.LineSelection(new Func<Character, bool>
                    //   (ch => ch.Row == lastChar.Row && ch != _firstSelected 
                    //   && ch.Left >= _firstSelected.Left && MouseGUI.Position.X >= ch.Left),
                    //   Line.SelectionMode.SELECT);
                    //}


                }
                else if (selection == Selection.LEFT)
                {
                    //Character firstChar = TopSelected[0];
                    //foreach (Line line in _textLine.Lines)
                    //{
                    //    line.LineSelection(new Func<Character, bool>
                    //    (ch => ch.Row == firstChar.Row && ch != _firstSelected 
                    //    && ch.Right <= _firstSelected.Left && MouseGUI.Position.X <= ch.Right),
                    //    Line.SelectionMode.SELECT);
                    //}
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
                    if (_prevPosition != (MouseGUI.Position + MouseGUI.DragOffset))
                    {
                        for (int i = 0; i < _displayLines.Count; i++)
                        {
                            _currChar = _displayLines[i].HitTest(MouseGUI.Position);
                            if (_currChar != null)
                            {
                                Pointer += new Point(_currChar.Rect.Right, _currChar.Rect.Top);
                                _firstSelected = _currChar;
                                _currChar.Selected = true;
                                break;
                            }
                        }
                    }
                       
                }

                if (MouseGUI.LeftIsPressed)
                {                    
                    
                    Point delta = MouseGUI.Position - _prevPosition;

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
