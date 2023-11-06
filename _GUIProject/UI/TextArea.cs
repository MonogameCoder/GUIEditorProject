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
using Microsoft.VisualBasic;
using _GUIProject.Events;
using static _GUIProject.UI.ScrollBar;
using System.Diagnostics.CodeAnalysis;

namespace _GUIProject.UI
{

    // This class will be fully refactored
    public class TextArea : TextBox, IScrollable
    {     

        private static FontContent _font;
        public class Line : IEquatable<Line>
        {
            public enum SelectionMode
            {
                DISSELECT,
                SELECT
            };

            private TextLines _fullText;

            private readonly List<Character> _characters;
            public static Character operator -(Character ch, Line line)
            {
                return line[ch.Column - 1];
            }
            public static bool operator ==(int row, Line line)
            {
                return row == line.Row;
            }
            public static bool operator !=(int row, Line line)
            {
                return !(row == line);
            }
           
            public static bool operator >(Line l1, Line l2)
            {
                return l1.Row > l2.Row;
            }
            public static bool operator <(Line l1, Line l2)
            {
                return l1.Row < l2.Row;
            }
            public static bool operator >=(Line l1, Line l2)
            {
                return l1.Row >= l2.Row;
            }
            public static bool operator <=(Line l1, Line l2)
            {
                return l1.Row <= l2.Row;
            }


            public string Text
            {
                get { return String.Concat(_characters.Select(c => c.Text).ToList()); }
            }
           
            public Point Size
            {
                get { return Text.Replace("\n","").Size(_font).ToPoint(); }
            }
            public int Width
            {
                get { return _characters.Sum(ch => ch.Width); }
            }
            public int Height
            {
                get { return Size.Y; }
            }
            public Point Position
            {
                get { return new Point(Front.Left, Top); }
            }           
            public Character Front
            {
                get { return this[0]; }
            }
            public Character Back
            {
                get { return this[Text.Replace("\n","").Length -1]; }
            }
            public static Character Space
            {
                get { return new Character(new Rectangle(Point.Zero, " ".Size(_font).ToPoint()), " "); }
            }
            public Character NewLine
            {
                get { return _characters.Where(ch => ch.NewLine).LastOrDefault(); }
            }
            public int Top
            {
                get { return this[0].Top; }
            }
            public int Bottom
            {
                get { return this[0].Bottom; }
            }
            public int Row
            {
                get
                {
                    return _fullText.Lines.IndexOf(this) + 1;
                }
            }
            public bool Fresh
            {
                get { return Text.Equals("\n"); }
            }
            public bool Empty
            {
                get { return Length < 1; }
            }
            public int Length
            {
                get { return Text.Length ; }
            }            
            public int SelectedCount
            {
                get { return _characters.Where(ch => ch.Selected).Count(); }
            }

            public Character this[int index]
            {
                get { return _characters.ElementAtOrDefault(index); }
            }
            public int this[Character ch]
            {
                get { return _characters.IndexOf(ch); }
            }
            public Character FirstSelected
            {
                get { return _characters.Where(ch => ch.Selected).FirstOrDefault(); }
            }
            public Character LastSelected

            {
                get { return _characters.Where(ch => ch.Selected).LastOrDefault(); }
            }
            public List<Character> Characters
            {
                get { return _characters; }
            }
            public Line(TextLines fullText)
            {
                _characters = new List<Character>();
                _fullText = fullText;
            }
            public void LineSelection(Func<Character, bool> predicate, SelectionMode mode)
            {
                foreach (var ch in _characters.Where(predicate))
                {
                    if(!ch.NewLine)
                    {
                        ch.Selected = Convert.ToBoolean(mode);
                    }                   
                }
            }
            public void InsertAt(Character character, int index)
            {

                foreach (Character ch in _characters.Where(ch => ch.Column >= character.Column))
                {
                    ch.Column++;
                }

                _characters.Insert(index, character);
            }
            public void Clear()
            {
                _characters.Clear();
            }
            public void RemoveAt(Character ch)
            {
                _characters.Remove(ch);
            }
            public Line Merge(Line other)
            {
                int start = _characters.Count;
                int end = _characters.Count + other.Characters.Count;

                if (other != this)
                {
                    Line below = other.Below(_fullText.Lines);

                    if (other.Fresh && !below.Fresh)
                    {
                        _fullText.Lines.Remove(other);
                        Merge(below);
                    }
                    else
                    {                       
                        for (int i = start; i < end; i++)
                        {
                            int idx = i - start;
                            Character ch = other[idx];
                            _characters.Insert(i, ch);
                        }

                        below = this.Below(_fullText.Lines);
                        _fullText.Lines.Remove(other);

                        while (below != null && below.Fresh)
                        {
                            _fullText.Lines.Remove(below);
                            below = this.Below(_fullText.Lines);
                        }
                    }
                    _characters.RemoveAll(ch => ch.NewLine && ch != _characters[Length -1]);
                }
                else
                {
                    Line below = this.Below(_fullText.Lines);
                    if (Fresh && !below.Fresh)
                    {
                        Merge(below);
                    }
                    else
                    {

                        if (below != this)
                        {
                            for (int i = Row; i < _fullText.NumberOfLines; i++)
                            {
                                Line ln = _fullText[i];

                                for (int j = 0; j < ln.Length; j++)
                                {
                                    Character ch = ln.Characters[j];
                                    Line bl = ln.Above(_fullText.Lines);
                                    int idx = bl.Characters.IndexOf(bl.Back);
                                    bl.InsertAt(ch, idx + 1);
                                }
                                for (int j = 0; j < ln.Length; j++)
                                {
                                    ln.RemoveAt(ln.Characters[j]);
                                }


                            }
                            for (int i = Row; i < _fullText.NumberOfLines; i++)
                            {
                                _fullText.Lines.RemoveAt(i);
                            }
                        }
                        _characters.RemoveAll(ch => ch.NewLine);
                    }
                }              
                return this;

            }
            public Character HitTest(Point mousePosition)
            {
                return _characters.Where(c => c.Rect.Contains(mousePosition.ToPoint())).FirstOrDefault();
            }
            public bool Contains(Point mousePosition)
            {
                return _characters.AsParallel().Any(ch => ch.Contains(MouseGUI.Position));
            }
            public bool Contains(string ch)
            {
                return Text.Contains(ch);
            }
            public bool Contains(Character ch)
            {
                return _characters.Contains(ch);
            }          
            public void Draw(SpriteBatch batch)
            {
                foreach (Character ch in _characters)
                {
                    ch.Draw(batch);
                }
            }

            public bool Equals([AllowNull] Line other)
            {
                if (Object.ReferenceEquals(this, other))
                {
                    return true;
                }
                return false;
            }
        }
        public class TextLines
        {
            private readonly List<Line> _lines;
            public Line Last
            {
                get
                {
                    return _lines[_lines.Count - 1];
                }
            }
            public Line First
            {
                get
                {
                    return _lines[0];
                }
            }
            public Line TopSelected
            {
                get { return _lines.Where(ln => ln.SelectedCount > 0).FirstOrDefault(); }
            }
            public Line BottomSelected
            {
                get { return _lines.Where(ln => ln.SelectedCount > 0).LastOrDefault(); }
            }
            public int SelectedLinesCount
            {
                get { return _lines.Where(ln => ln.SelectedCount > 0).Count(); }
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
                _lines.Add(new Line(this));
            }

            public Line this[int index]
            {
                get { return _lines.ElementAtOrDefault(index); }
            }

            public int this[Line ln]
            {
                get { return _lines.IndexOf(ln); }
            }
            public void SetPointed(Character ch)
            {
                foreach (Line line in _lines)
                {
                    foreach (Character character in line.Characters.Where(chs => chs != ch))
                    {
                        character.Pointed = false;
                    }
                }
                ch.Pointed = true;
            }
            public void AddLine(Line line)
            {
                _lines.Add(line);
            }
           
            public bool Contains(Character ch)
            {
                foreach (Line line in _lines)
                {
                    return line.Contains(ch);
                }
                return false;
            }
            public Character HitTest(Point mousePosition)
            {
                Character result = null;
                for (int i = 0; i < _lines.Count; i++)
                {
                    Line current = _lines[i];
                    result = current.HitTest(mousePosition);
                }
                return result;
            }
        }

        [XmlIgnore]
        public int MaxNumberOfLines
        {
            get { return (int)Math.Round(Height / (float)Line.Space.Height); }
            set { }
        }
        public int MaxLineWidth
        {
            get { return (Width - _scrollBar.Width) - Line.Space.Width; }
            set { }
        }

        [XmlIgnore]
        public int NumberOfLines
        {
            get { return _fullText.NumberOfLines; }
        }

        [XmlElement]
        public override string Text
        {
            get { return String.Concat(_visibleLines.Select(l => l.Text)); }
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
                return _fullText.Lines.Where(ln => ln.Characters.Where(ch => ch.Selected).Any()).FirstOrDefault();
            }
        }
        public Line BottomSelected
        {
            get
            {
                return _fullText.Lines.Where(ln => ln.Characters.Where(ch => ch.Selected).Any()).LastOrDefault();
            }
        }

        private readonly TextLines _fullText;

        private List<Line> _visibleLines;
        private ScrollBar _scrollBar;
        private Character _firstSelected;
        private Character _currChar;
        private Point _prevPosition;
        private Point _firstPtPosition;
        private ScrollEvents _scrollEvent;

        public TextArea() : base("DefaultMultiTexboxTX", "DefaultTextboxPointerTX", TextBoxType.TEXT, DrawPriority.NORMAL)
        {
            MoveState = MoveOption.DYNAMIC;
            XPolicy = SizePolicy.EXPAND;
            YPolicy = SizePolicy.EXPAND;
            LoadAttributes();
            _fullText = new TextLines();
            _visibleLines = new List<Line>();

        }
        void LoadAttributes()
        {
            base.Setup();
            CharBucket = new CharacterBucket();
            _scrollEvent = new ScrollEvents();
            _scrollBar = new ScrollBar();
            _scrollBar.Parent = this;
            _scrollBar.Initialize();

            _scrollBar.Setup();

            _scrollBar.Position = new Point(Right - _scrollBar.Width, Top);
            Pointer.Active = true;

            TextColor = Color.Black;
            _font = TextFont;
        }
        public override void Initialize()
        {
            base.Initialize();
            MouseEvent.onMouseClick += (sender, args) => Selected = true;
            _scrollEvent.onScroll += _scrollEvent_onScroll;

            Active = true;
        }

        private void _scrollEvent_onScroll(object sender, ScrollEventArgs e)
        {
            var parent = (e.Owner as IScrollable);
            _scrollBar.CurrentScrollValue += e.ScrollValue;
            parent.ApplyScroll();
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
        public override void ResetSize()
        {
            _scrollBar.ResetSize();

            ApplyScroll();
            UpdateText();

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

        }
        public override void OnKeyboardPressed()
        {
            _isShift = _myclsinput.isShiftPressed(Keyboard.GetState().GetPressedKeys());
            _isControl = _myclsinput.isControlPressed(Keyboard.GetState().GetPressedKeys());


            if (Singleton.Input.KeyReleased(Keys.Back))
            {
                Character previous = null;
                Character next = null;

                List<Character> selected = new List<Character>();
                foreach (Line line in _fullText.Lines)
                {
                    selected.AddRange(line.Characters.Where(ch => ch.Selected));
                }

                if (selected.Count > 0)
                {
                    (previous, next) = DeleteMergeText(selected);
                }
                else
                {
                    Character ch = _fullText.At(Pointer.Position);

                    for (int i = 0; i < NumberOfLines; i++)
                    {
                        Line line = _fullText.Lines[i];
                        Line below = line.Below(_fullText.Lines);

                        if (line.Contains(ch))
                        {
                            if (ch.Left < Pointer.Left)
                            {
                                previous = ch.Previous(line);
                                next = previous.Next(line);

                                if (IsMergeable(line, below))
                                {
                                    MergeLinesBelow(line, below);
                                }
                                line.RemoveAt(ch);
                            }
                            break;
                        }
                        else
                        {
                            if (Pointer.Left <= Left + TextOffset.X)
                            {
                                line = Pointer.Position.AtAbove(_fullText);
                                below = line.Below(_fullText.Lines);

                                if (line != null)
                                {
                                    previous = line.Back.Previous(line);
                                    next = line.Back;
                                    if (line.Fresh)
                                    {
                                        previous = below.Front;
                                        next = previous;
                                    }
                                    if (!below.Fresh)
                                    {
                                        line = line.Merge(below);
                                    }

                                    if (below.Fresh && below != _fullText.First)
                                    {
                                        _fullText.Lines.Remove(below);
                                    }
                                }
                                break;
                            }
                        }
                    }
                }


                UpdateLines();
                UpdateVisibleLines();
                PopulateVisibleLines();

                if (next.Exists())
                {
                    _fullText.SetPointed(next);

                    Pointer += new Point(next.Left, next.Top);

                    foreach (Line ln in _fullText.Lines)
                    {
                        if (!ln.Fresh && ln.Contains(next) && next == ln.Back)
                        {
                            Pointer += new Point(next.Right, next.Top);
                            break;
                        }

                    }
                }

                KeyboardEvents.Released();
            }
            else if (Singleton.Input.KeyReleased(Keys.Delete))
            {
                Character previous = null;
                Character next = null;

                List<Character> selected = new List<Character>();
                foreach (Line line in _fullText.Lines)
                {
                    selected.AddRange(line.Characters.Where(ch => ch.Selected));
                }

                if (selected.Count > 0)
                {
                    (previous, next) = DeleteMergeText(selected);
                }
                else
                {
                    Character ch = _fullText.At(Pointer.Position);

                    for (int i = 0; i < NumberOfLines; i++)
                    {
                        Line line = _fullText.Lines[i];
                        Line below = line.Below(_fullText.Lines);

                        if (line.Contains(ch))
                        {
                            if (line.Back.Exists() && Pointer.Right >= line.Back.Right)
                            {
                                Line nextLine = new Point(Pointer.Left, Pointer.Bottom).AtBelow(_fullText);
                                below = nextLine.Below(_fullText.Lines);

                                if (nextLine != null)
                                {
                                    previous = line.Back.Previous(line);
                                    next = line.Back;

                                    if (IsMergeable(line, below))
                                    {
                                        nextLine.RemoveAt(nextLine.Front);
                                        nextLine = nextLine.Merge(below);
                                    }
                                    else
                                    {
                                        if (!below.Fresh && !nextLine.Fresh)
                                        {
                                            line = line.Merge(below);
                                            previous = below.Front;
                                            next = previous;
                                        }
                                    }


                                    if (below.Fresh && below != _fullText.First)
                                    {
                                        _fullText.Lines.Remove(below);
                                    }
                                    if (!below.Fresh && nextLine.Fresh && nextLine != _fullText.First)
                                    {
                                        _fullText.Lines.Remove(nextLine);
                                    }
                                }
                            }
                            else
                            {
                                Character nxt = ch.Next(line);
                                if (ch == line.Front)
                                {
                                    if (ch.Right <= Pointer.Right)
                                    {
                                        if (nxt != line.Front)
                                        {
                                            previous = ch.Previous(line);
                                            next = nxt;
                                            line.RemoveAt(nxt);
                                        }
                                    }
                                    else
                                    {
                                        previous = ch;
                                        next = previous;
                                        line.RemoveAt(ch);
                                    }

                                    if (IsMergeable(line, below))
                                    {
                                        line.RemoveAt(line.Back.Next(line));
                                        MergeLinesBelow(line, below);
                                    }


                                    if (line.Fresh)
                                    {
                                        previous = below.Front;
                                        next = previous;
                                        _fullText.Lines.Remove(line);
                                    }
                                }
                                else
                                {
                                    if (nxt.Right > Pointer.Right)
                                    {
                                        if (ch != line.Front)
                                        {
                                            previous = nxt.Previous(line);
                                            next = previous.Next(line);
                                            line.RemoveAt(nxt);
                                        }
                                    }
                                    if (line.Back.Exists() && line.NewLine.Exists())
                                    {
                                        line.RemoveAt(line.Back.Next(line));
                                    }

                                    MergeLinesBelow(line, below);
                                }

                            }

                            break;
                        }

                    }
                }
                UpdateLines();
                UpdateVisibleLines();
                PopulateVisibleLines();

                if (next.Exists())
                {
                    _fullText.SetPointed(next);
                    Pointer += new Point(selected.Count < 1 ? next.Left : next.Right, next.Top);

                    foreach (Line ln in _fullText.Lines)
                    {
                        if (ln.Back.Exists() && ln.Contains(next) && next == ln.Back)
                        {
                            Pointer += new Point(next.Right, next.Top);
                            break;
                        }
                    }
                }


                KeyboardEvents.Released();
            }

            else if (Singleton.Input.KeyReleased(Keys.Enter))
            {
                _keyboardString += "\n";
                CreateText();
                KeyboardEvents.Released();
            }
            else if (_isControl && Singleton.Input.KeyReleased(Keys.C))
            {
                string text = "";
                foreach (Line line in _fullText.Lines)
                {
                    text = line.Characters.Where(ch => ch.Selected).ToList().Text();
                }
                System.Windows.Forms.Clipboard.SetText(text);
            }
            else if (_isControl && Singleton.Input.KeyReleased(Keys.V))
            {
                string text = System.Windows.Forms.Clipboard.GetText();
              
            }
            else
            {
                base.OnKeyboardPressed();
            }

        }
        public Character CreateCharacter(string character)
        {
            Line line = _fullText.Lines.Where(ln => !ln.Empty && ln.Top == Pointer.Top).FirstOrDefault();
            
            if(line == null)
            {
                line = _fullText.Last;
            }            
           
            int at = line.Characters.Sum(ch => ch.Width);
            int row = !line.Empty  ? line.Row - 1 : NumberOfLines - 1;
           
            if(IsOutOfBounds(line))
            {
                row++;
            }

            Point position = new Point
            {
                X = TextOffset.X + at,
                Y = row * line.Height
            };
            return new Character(new Rectangle(Position + position, character.Size(TextFont).ToPoint()), character);
        }
        public void MergeLinesBelow(Line line, Line other)
        {
            Line current = line.Merge(other);
            while (!IsOutOfBounds(current) && other != current)
            {
                other = current.Below(_fullText.Lines);
                if (other == null)
                {
                    _fullText.AddLine(new Line(_fullText));
                    break;
                }
               
                current = line.Merge(other);
                          
            }
        }
        public Tuple<Character,Character> DeleteMergeText(List<Character> selected)
        {
            Character previous = null;
            Character next = null;

            int top = selected.FirstOrDefault().Row;
            int bottom = selected.LastOrDefault().Row;
            Line topLine = _fullText[top - 1];
            Line bottomLine = _fullText[bottom - 1];

            previous = selected.FirstOrDefault().Previous(topLine);
            next = previous;         

            foreach (Line ln in _fullText.Lines)
            {
                foreach (Character ch in selected)
                {
                    ln.RemoveAt(ch);
                }
            }


            if (bottomLine.Empty && topLine.Empty)
            {
                Line emptyLine = bottomLine;
                while (emptyLine > topLine)
                {
                    _fullText.Lines.Remove(emptyLine);
                    emptyLine = bottomLine.Above(_fullText.Lines);
                }  
                if(!topLine.NewLine.Exists())
                {
                    topLine.InsertAt(CreateCharacter("\n"), 0);
                }
            }
            else
            {
                MergeLinesBelow(topLine, bottomLine);
            }
           

            return new Tuple<Character,Character>(previous, next);
        }
        protected override void CreateText()
        {
            List<Character> selected = new List<Character>();
            Character newChar = CreateCharacter(LastChar);
            Character ch = _fullText.At(Pointer.Position);
            Character next = null;
            if (LastChar == "\n")
            {
                if (ch == null)
                {
                    Line line = _fullText[0];                    
                    line.InsertAt(newChar, 0);

                    _fullText.Lines.Insert(newChar.Row, new Line(_fullText));                  

                    Line below = line.Below(_fullText.Lines);
                   
                    Character newLineChar = CreateCharacter("\n");
                    newLineChar.Row = below.Row;
                    below.InsertAt(newLineChar, 0);
                    next = below[0];
                }
                else
                {

                    Line line = _fullText[ch.Row - 1];
                    int index = line.Characters.IndexOf(ch);

                    _fullText.Lines.Insert(ch.Row, new Line(_fullText));
                    Line below = line.Below(_fullText.Lines);                    
                   
                    foreach (Line ln in _fullText.Lines)
                    {
                        selected.AddRange(ln.Characters.Where(ch => ch.Selected));
                    }
                    if (selected.Count > 0)
                    {                       
                        Line nextLN = _fullText.Lines[selected.LastOrDefault().Row];
                        Line nextLNBelow = nextLN.Below(_fullText.Lines);
                        next = selected.LastOrDefault().Next(nextLN);
                        
                        foreach (Line ln in _fullText.Lines)
                        {
                            foreach (Character character in selected)
                            {
                                ln.RemoveAt(character);
                            }
                        }
                        int idx = next.Exists() ? line.Row - 1 : line.Row;

                        List<Line> lnToDel = _fullText.Lines.Where(ln => (ln.Empty || ln.Fresh) && ln >= line && ln <= nextLN).ToList();
                        for (int i = 0; i < lnToDel.Count; i++)
                        {
                            Line currLine = lnToDel[i];
                            _fullText.Lines.Remove(currLine);
                        }

                        if (nextLN.Empty || nextLN.Fresh)
                        {
                            _fullText.Lines.Insert(idx, new Line(_fullText));

                            Line currentLine = _fullText[idx];
                            Line lineAbove = currentLine.Above(_fullText.Lines);

                            currentLine.InsertAt(CreateCharacter("\n"), 0);
                            if (!lineAbove.NewLine.Exists())
                            {
                                lineAbove.InsertAt(CreateCharacter("\n"), lineAbove.Back.Column);
                            }

                            next = currentLine.Front;
                        }
                        else
                        {
                            MergeLinesBelow(nextLN, nextLNBelow);
                        }                        
                    }
                    else
                    {
                        if (ch != line.Back && ch != line.NewLine && !line.Fresh)
                        {
                            int startIdx = ch.Column;
                            if (ch == line.Front && Pointer.Left <= ch.Left)
                            {
                                startIdx = 0;
                            }
                            List<Character> remainders = line.Characters.Where(chs => chs.Column > startIdx).ToList();

                            if (remainders.Count > 0)
                            {
                                line.InsertAt(newChar, remainders[0].Column - 1);
                                for (int idx = 0; idx < remainders.Count; idx++)
                                {
                                    line.RemoveAt(remainders[idx]);
                                    below.InsertAt(remainders[idx], idx);
                                }
                            }

                            next = remainders[0];
                        }
                        else
                        {
                            if (line.NewLine.Exists() && !line.Fresh)
                            {
                                line.RemoveAt(line.NewLine);
                            }
                            if (line.Back.Exists())
                            {
                                line.InsertAt(newChar, line.Back.Column);
                            }
                            Character newLineChar = CreateCharacter("\n");
                            newLineChar.Row = below.Row;
                            below.InsertAt(newLineChar, 0);
                            next = below[0];
                        }
                    }
                }
            }
            else
            {
                if (ch == null)
                {
                    ch = newChar;
                    ch.Row = NumberOfLines;
                }

                Line line = _fullText[ch.Row - 1];
                int index = line.Characters.IndexOf(ch);

                if (_fullText.NumberOfLines > 0 && IsOutOfBounds(line))
                {
                    Character newLine = CreateCharacter("\n");
                    Line below = line.Below(_fullText.Lines);
                    if (below == line)
                    {
                        _fullText.AddLine(new Line(_fullText));
                    }
                    if (line.NewLine.Exists())
                    {
                        if (line.Back.Right > Pointer.Left)
                        {
                            if (line.Front.Left >= Pointer.Left)
                            {
                                line.InsertAt(newChar, 0);
                            }
                            else
                            {
                                int idx = line.Characters.IndexOf(line.At(Pointer.Position));
                                line.InsertAt(newChar, idx);
                            }
                        }
                        else
                        {
                            line.Below(_fullText.Lines).InsertAt(newChar, 0);
                        }
                    }
                    else
                    {
                        if (line.Back.Right > Pointer.Right)
                        {
                            int idx = line.Characters.IndexOf(line.Back);
                            line.InsertAt(newLine, idx);
                            line.InsertAt(newChar, index == 0 ? 0 : index + 1);
                        }
                        else
                        {
                            line.InsertAt(newLine, index + 1);
                            line.Below(_fullText.Lines).InsertAt(newChar, 0);
                        }
                    }
                   
                }
                else
                {
                    if (Pointer.Left >= ch.Right)
                    {
                        line.InsertAt(newChar, ch.Column);
                    }
                    else
                    {
                        line.InsertAt(newChar, ch.Column - 1);
                    }
                   
                }
                next = newChar;
            }
            PopulateVisibleLines();
            UpdateLines();
         
            _fullText.SetPointed(next);
            Pointer += new Point(newChar.NewLine ? next.Left : next.Right, next.Top);
        }
        bool IsMergeable(Line line, Line below)
        {
            return line.Back.Exists() && line.NewLine.Exists() && below.Front.Exists() &&
                line.Width + below.Front.Width >= MaxLineWidth;
        }
        bool IsOutOfBounds(Line line)
        {
            return line.Width + LastChar.Size(TextFont).X > MaxLineWidth;
        }

       

        public void ApplyScroll()
        {
            if (NumberOfLines > MaxNumberOfLines && MaxNumberOfLines + _scrollBar.CurrentScrollValue <= NumberOfLines)
            {
                DisplayText = "";

                int start = NumberOfLines > MaxNumberOfLines ? _scrollBar.CurrentScrollValue : 0;
                int end = MaxNumberOfLines + _scrollBar.CurrentScrollValue;

                _visibleLines.Clear();
                for (int i = start; i < end; i++)
                {                   
                    _visibleLines.Add(_fullText[i]);                 
                }
                UpdateLines();              
                
                Character chPointed = null;
                foreach (Line line in _fullText.Lines)
                {
                    foreach (Character ch in line.Characters)
                    {
                        if (ch.Pointed)
                        {
                            chPointed = ch;
                            break;
                        }
                    }
                }
                if (!_visibleLines.Contains(chPointed))
                {
                    Pointer.Hide();
                }
                else
                {
                    Pointer += new Point(Pointer.Left, chPointed.Top);
                    Pointer.Show();
                }
            }

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

                    if (dimensions.X > Width - _scrollBar.Width)
                    {
                        tmpStr = tmpStr.Insert(tmpStr.Length - 1, "\n");
                        _keyboardString = _keyboardString.Insert(_keyboardString.Length - 1, "\n");

                    }
                    if (tmpStr.Length > 1 && tmpStr[tmpStr.Length - 1] == '\n')
                    {
                        tmpStr = "";
                    }
                }
                DisplayText = _keyboardString;
            }
            foreach (Line line in _visibleLines)
            {
                UpdateCharacters(line);
            }

        }
        void UpdateScroll()
        {
            if (NumberOfLines > MaxNumberOfLines)
            {
                int scrollAmt = 0;

                Line lastVisible = _visibleLines[_visibleLines.Count - 1];

                if (lastVisible != _fullText.Last)
                {
                    scrollAmt = (_visibleLines[0].Row - _fullText.First.Row);
                    if (Pointer.Bottom >= Bottom)
                    {
                        scrollAmt += 1;
                    }
                }
                else
                {
                    scrollAmt = NumberOfLines - MaxNumberOfLines;
                }
                _scrollBar.CurrentScrollValue = scrollAmt;
            }
        }
        void UpdateVisibleLines()
        {
            _visibleLines.Clear();
            for (int i = 0; i < MaxNumberOfLines; i++)
            {
                _visibleLines.Add(_fullText[Math.Min(i + _scrollBar.CurrentScrollValue, NumberOfLines - 1)]);
            }
        }
        void UpdateLines()
        {
            for (int i = 0; i < NumberOfLines; i++)
            {
                Line line = _fullText.Lines[i];

                
                while (line.Width > Width - _scrollBar.Width - Line.Space.Width)
                {
                    Character newLine = CreateCharacter("\n");
                    var newLines = line.Characters.Where(ch => ch.NewLine);
                    for (int k = 0; k < newLines.Count(); k++)
                    {
                        Character ch = newLines.ElementAt(k);
                        line.RemoveAt(ch);
                    }


                    Line below = line.Below(_fullText.Lines);

                    if (line != below)
                    {
                        below.InsertAt(line.Back, 0);
                        line.RemoveAt(line.Back);
                    }
                    else
                    {
                        _fullText.AddLine(new Line(_fullText));
                        below = line.Below(_fullText.Lines);

                        below.InsertAt(line.Back, 0);
                        line.RemoveAt(line.Back);

                    }

                    line.InsertAt(newLine, line.Characters.IndexOf(line.Back) + 1);
                }


                UpdateRowsColumns();
            }

            foreach (Line ln in _fullText.Lines)
            {
                UpdateCharacters(ln);
            }
        }
        void UpdateRowsColumns()
        {
            for (int j = 0; j < NumberOfLines; j++)
            {
                Line ln = _fullText[j];
                for (int k = 0; k < ln.Characters.Count; k++)
                {
                    Character ch = ln.Characters[k];
                    ch.Column = k + 1;
                    ch.Row = j + 1;
                }
            }
        }
        void UpdateCharacters(Line line)
        {
            for (int i = 0; i < line.Text.Length; i++)
            {
                Character current = line[i];
                if (current != line.NewLine && _visibleLines.Count > 0)
                {
                    int idx = line.Characters.IndexOf(current);

                    string subStr = line.Text.Substring(0, idx + 1);

                    Point dim = subStr.Size(TextFont).ToPoint();

                    int x = dim.X - current.Width + TextOffset.X;
                    int y = dim.Y * line.Row.Subtract((_visibleLines[0].Row - _fullText[0].Row)).Subtract(1);

                    current.Rect = new Rectangle(Position + new Point(x, y), current.Rect.Size);
                }
                if (current == line.NewLine)
                {
                    Line above = line.Above(_fullText.Lines);
                    
                    Point location = !line.Fresh ? new Point(line.Back.Right, line.Back.Top) :
                        new Point(Left + TextOffset.X,line != above ? above.Bottom : line.Top);

                    current.Rect = new Rectangle(location, new Point(Line.Space.Width, Line.Space.Height));
                }

            }
        }
        void PopulateVisibleLines()
        {
            UpdateScroll();

            int start = NumberOfLines > MaxNumberOfLines ? _scrollBar.CurrentScrollValue : 0;
            int end = NumberOfLines > MaxNumberOfLines ? MaxNumberOfLines + _scrollBar.CurrentScrollValue : NumberOfLines;

            _visibleLines.Clear();
            for (int i = start; i < end; i++)
            {
                _visibleLines.Add(_fullText[i]);
            }
            foreach (Line line in _visibleLines)
            {
                UpdateCharacters(line);
            }
        }            
        void ResetSelection()
        {
            foreach (Line line in _fullText.Lines)
            {
                foreach (Character ch in line.Characters)
                {
                    ch.Selected = false;
                }
            }
            _firstSelected = null;
        }
        void CurrentLine(Line line)
        {
            line.LineSelection(new Func<Character, bool>
                                    (ch => ch.Left < MouseGUI.Position.X),
                                    Line.SelectionMode.SELECT);
        }
        void BottomLine(Line line)
        {
            line.LineSelection(new Func<Character, bool>
            (ch => ch.Right <= _firstSelected.Previous(line).Right),
            Line.SelectionMode.SELECT);

            line.LineSelection(new Func<Character, bool>
            (ch => ch.Right > _firstSelected.Right),
            Line.SelectionMode.DISSELECT);
        }
        void TopLine(Line line)
        {
            line.LineSelection(new Func<Character, bool>
            (ch => ch.Right > _firstSelected.Right),
            Line.SelectionMode.SELECT);

            line.LineSelection(new Func<Character, bool>
            (ch => ch.Left < _firstSelected.Left),
            Line.SelectionMode.DISSELECT);
        }
        void CurrentLeft(Line line)
        {            
            line.LineSelection(new Func<Character, bool>(ch =>
                         ch.Left <= _firstSelected.Previous(line).Left && ch.Right > MouseGUI.Position.X),
                         Line.SelectionMode.SELECT);

            if (line.LastSelected != null)
            {
                if (_firstSelected.Right - _firstPtPosition.X > 0 && _firstSelected != line.Back)
                {
                    line.LineSelection(new Func<Character, bool>(ch => ch.Selected &&
                    ch.Right > _firstPtPosition.X && ch.Right >= MouseGUI.Position.X),
                    Line.SelectionMode.DISSELECT);
                }

            }
        }
        void CurrentRight(Line line)
        {
            if (line.FirstSelected != null)
            {
                if (_firstPtPosition.X - line.FirstSelected.Left > 0 )
                {
                    line.LineSelection(new Func<Character, bool>(ch => ch.Selected &&
                    ch.Left <= _firstSelected.Previous(line).Left && ch.Right < MouseGUI.Position.X),
                    Line.SelectionMode.DISSELECT);
                }
            }

            line.LineSelection(new Func<Character, bool>(ch =>
            ch.Right >= _firstSelected.Right && ch.Left < MouseGUI.Position.X),
            Line.SelectionMode.SELECT);
        }
        void SelectUp()
        {
            if (!Contains(MouseGUI.Position) && MouseGUI.Position.Y < Top)
            {
                if (_scrollBar.CurrentScrollValue - 1 >= 0)
                {
                    _scrollEvent.OnScroll(this, ScrollDirection.UP, -1);
                }
            }
            for (int i = 0; i < NumberOfLines; i++)
            {
                Line line = _fullText.Lines[i];
                Line firstSelLine = BottomSelected;
                Line lastSelLine = TopSelected;
               if(line.Fresh)
                {
                    if (MouseGUI.Position.Y <= line.Top)
                    {
                        line.Front.Selected = true;
                        if (_firstSelected.Row < line.Row)
                        {
                            line.Front.Selected = false;
                        }
                    }
                }
                if (!Contains(MouseGUI.Position) && MouseGUI.Position.X > Right)
                {

                    if (firstSelLine != null)
                    {
                        if (line < firstSelLine)
                        {
                            // Lines Below
                            if (MouseGUI.Position.Y <= line.Top)
                            {
                                if (line != _fullText.First)
                                {
                                    CurrentLine(line);

                                    line.LineSelection(new Func<Character, bool>
                                    (ch => ch.Selected && ch.Row > _firstSelected.Row && ch.Left < MouseGUI.Position.X),
                                    Line.SelectionMode.DISSELECT);
                                }
                            }                           
                        }
                        else
                        {
                            // Select Bottom Line
                            if (MouseGUI.Position.Y <= line.Top && line.Row == _firstSelected.Row && line != _fullText.First)
                            {
                                BottomLine(line);

                            }
                            else
                            {
                                // Last Selected Line
                                if (MouseGUI.Position.Y < line.Top)
                                {
                                    line.LineSelection(new Func<Character, bool>
                                    (ch => ch.Selected && ch.Row > _firstSelected.Row),
                                    Line.SelectionMode.DISSELECT);
                                }
                            }
                        }
                    }
                }
                else
                {
                    
                    if (_firstSelected.Row == line.Row && line.SelectedCount < 1)
                    {
                        _firstSelected.Previous(line).Selected = true;
                    }
                    if (firstSelLine != null)
                    {
                        if (_firstSelected.Row < firstSelLine.Row)
                        {

                            if (line.Contains(MouseGUI.Position))
                            {
                                // Disselect Current Line
                                line.LineSelection(new Func<Character, bool>
                               (ch => ch.Selected && ch.Left > MouseGUI.Position.X),
                               Line.SelectionMode.DISSELECT);
                            }
                            else
                            {
                                // Disselect All Else
                                if (line >= firstSelLine)
                                {
                                    line.LineSelection(new Func<Character, bool>(ch => ch.Selected && firstSelLine.Top >= MouseGUI.Position.Y),
                                    Line.SelectionMode.DISSELECT);
                                }
                            }

                        }
                        else
                        {
                            // Select Current Line
                            if (MouseGUI.Position.Y < line.Bottom && line < firstSelLine)
                            {
                                if(_visibleLines.Contains(line))
                                {
                                    line.LineSelection(new Func<Character, bool>
                                    (ch => ch.Right > MouseGUI.Position.X),
                                    Line.SelectionMode.SELECT);
                                }
                                else
                                {
                                    _visibleLines[0].LineSelection(ch => ch.Right > MouseGUI.Position.X, Line.SelectionMode.SELECT);
                                }
                              
                            }
                            if (_fullText.SelectedLinesCount > 1)
                            {
                                // Select Lines In Between
                                if (MouseGUI.Position.Y <= line.Top && line != _fullText.First)
                                {
                                    line.LineSelection(new Func<Character, bool>
                                    (ch => ch.Row < _firstSelected.Row),
                                    Line.SelectionMode.SELECT);
                                }

                                // Select Bottom Line
                                if (MouseGUI.Position.Y <= firstSelLine.Top && line == firstSelLine)
                                {
                                    BottomLine(line);
                                }
                            }
                        }
                    }
                }
            }
        }
        void SelectDown()
        {
            if (!Contains(MouseGUI.Position) && MouseGUI.Position.Y > Bottom)
            {
                if (_scrollBar.CurrentScrollValue  + 1 <= NumberOfLines - MaxNumberOfLines)
                {
                    _scrollEvent.OnScroll(this, ScrollDirection.DOWN, 1);
                }
            }
            for (int i = NumberOfLines - 1; i >= 0; i--)
            {
                Line line = _fullText.Lines[i];
                Line firstSelLine = TopSelected;
                Line lastSelLine = BottomSelected;
                if (line.Fresh)
                {
                    if(MouseGUI.Position.Y >= line.Bottom)
                    {
                        line.Front.Selected = true;
                        if (_firstSelected.Row > line.Row)
                        {
                            line.Front.Selected = false;
                        }
                    }
                   
                }
                if (!Contains(MouseGUI.Position) && MouseGUI.Position.X > Right)
                {

                    if (firstSelLine != null)
                    {
                        if (line > firstSelLine)
                        {
                            // Lines Below
                            if (MouseGUI.Position.Y >= line.Bottom)
                            {
                                CurrentLine(line);

                                line.LineSelection(new Func<Character, bool>
                                (ch => ch.Selected && ch.Row < _firstSelected.Row && ch.Left < MouseGUI.Position.X),
                                Line.SelectionMode.DISSELECT);
                            }
                            else
                            {
                                // Current Line
                                if (MouseGUI.Position.Y >= line.Top)
                                {
                                    CurrentLine(line);
                                }
                            }
                        }
                        else
                        {
                            // Select Top Line
                            if (MouseGUI.Position.Y >= line.Bottom && line.Row == _firstSelected.Row)
                            {
                                TopLine(line);

                            }
                            else
                            {
                                // Last Selected Line
                                if (MouseGUI.Position.Y > line.Bottom)
                                {
                                    line.LineSelection(new Func<Character, bool>
                                    (ch => ch.Selected && ch.Row < _firstSelected.Row),
                                    Line.SelectionMode.DISSELECT);
                                }
                            }
                        }
                    }

                }
                else
                {

                    if (_firstSelected.Row == line.Row && line.SelectedCount < 1)
                    {
                        _firstSelected.Selected = true;
                    }


                    if (firstSelLine != null)
                    {
                        if (_firstSelected.Row > firstSelLine.Row)
                        {
                            if (line.Contains(MouseGUI.Position))
                            {
                                // Disselect Current Line
                                line.LineSelection(new Func<Character, bool>
                                (ch => ch.Selected && ch.Right < MouseGUI.Position.X),
                                Line.SelectionMode.DISSELECT);
                            }
                            else
                            {
                                // Disselect All Else
                                if (line <= firstSelLine)
                                {
                                    line.LineSelection(new Func<Character, bool>(ch => ch.Selected && firstSelLine.Bottom <= MouseGUI.Position.Y),
                                    Line.SelectionMode.DISSELECT);
                                }
                            }
                        }
                        else
                        {

                            // Select Current Line
                            if (MouseGUI.Position.Y >= line.Top && line > firstSelLine)
                            {
                                if(_visibleLines.Contains(line))
                                {
                                    CurrentLine(line);
                                }
                                else
                                {
                                    CurrentLine(_visibleLines[_visibleLines.Count - 1]);                                    
                                }
                            }
                            // Select Lines In Between
                            if (MouseGUI.Position.Y >= line.Bottom && line != _fullText.Last)
                            {
                                line.LineSelection(new Func<Character, bool>
                                (ch => ch.Row > _firstSelected.Row),
                                Line.SelectionMode.SELECT);
                            }

                            // Select Top Line
                            if (MouseGUI.Position.Y > firstSelLine.Bottom && line == firstSelLine && _fullText.SelectedLinesCount > 1)
                            {
                                TopLine(line);
                            }

                        }
                    }
                }
            }
        }
        void SelectRight()
        {
            for (int i = NumberOfLines - 1; i >= 0; i--)
            {
                Line line = _fullText.Lines[i];
                if (_firstSelected == line.Front)
                {
                    _firstSelected.Selected = true;
                }

                Line firstSelLine = TopSelected;
                Line lastSelLine = BottomSelected;
                if (!Contains(MouseGUI.Position))
                {
                    // Going Down
                    if (MouseGUI.Position.Y > Bottom)
                    {
                        // Selection
                        if (line == lastSelLine && lastSelLine.Row == _firstSelected.Row)
                        {
                            CurrentRight(line);
                        }

                    }
                    // Going Up
                    else if (MouseGUI.Position.Y < Top)
                    {
                        // Diselection
                        if (line == firstSelLine && firstSelLine.Row < _firstSelected.Row)
                        {
                            line.LineSelection(new Func<Character, bool>(ch => line.SelectedCount > 1 &&
                            ch.Right >= line.FirstSelected.Right && ch.Right < MouseGUI.Position.X),
                            Line.SelectionMode.DISSELECT);
                        }

                        if (line == firstSelLine && firstSelLine.Row == _firstSelected.Row)
                        {
                            CurrentRight(line);
                        }
                    }
                    else
                    {
                        if (MouseGUI.Position.Y < line.Bottom && line == firstSelLine && firstSelLine.Row == _firstSelected.Row)
                        {
                            CurrentRight(line);
                        }
                    }
                }
                else
                {
                  
                    if (line.FirstSelected != null)
                    {
                        // Select Bottom Line
                        if (MouseGUI.Position.Y > firstSelLine.Bottom && line.Row > _firstSelected.Row)
                        {
                            line.LineSelection(new Func<Character, bool>(ch =>
                                ch.Right > line.FirstSelected.Right && line.FirstSelected.Left < MouseGUI.Position.X && ch.Left < MouseGUI.Position.X),
                                Line.SelectionMode.SELECT);
                        }
                        // Disselect Top Line
                        if (MouseGUI.Position.Y < lastSelLine.Top && MouseGUI.Position.Y >= line.Top
                            && line < lastSelLine)
                        {
                            line.LineSelection(new Func<Character, bool>(ch => ch.Selected &&
                                ch.Right >= line.FirstSelected.Right && MouseGUI.Position.X >= ch.Right),
                                Line.SelectionMode.DISSELECT);
                        }

                    }

                    if (_firstSelected.Top < MouseGUI.Position.Y && line.Row == _firstSelected.Row && _fullText.SelectedLinesCount < 2)
                    {
                        CurrentRight(line);
                    }
                }
            }

        }
        void SelectLeft()
        {
           
            for (int i = 0; i < NumberOfLines; i++)
            {
                Line line = _fullText.Lines[i];
                if (_firstPtPosition.X >= _firstSelected.Right)
                {
                    _firstSelected.Selected = true;
                }
                Line firstSelLine = TopSelected;
                Line lastSelLine = BottomSelected;

                if (!Contains(MouseGUI.Position))
                {
                    // Going Down
                    if (MouseGUI.Position.Y > Bottom)
                    {
                        // Selection
                        if (line == lastSelLine && lastSelLine.Row == _firstSelected.Row)
                        {
                            CurrentLeft(line);
                        }
                        if (line == lastSelLine && lastSelLine.Row > _firstSelected.Row)
                        {
                                line.LineSelection(new Func<Character, bool>(ch => line.SelectedCount > 1 &&
                                ch.Left >= line.LastSelected.Left && ch.Left > MouseGUI.Position.X),
                                Line.SelectionMode.DISSELECT);
                        }
                    }
                    // Going Up
                    else if (MouseGUI.Position.Y < Top)
                    {
                        // Selection
                        if (line == firstSelLine && firstSelLine.Row < _firstSelected.Row)
                        {
                            line.LineSelection(new Func<Character, bool>(ch =>
                            ch.Left < line.LastSelected.Right && ch.Right > MouseGUI.Position.X),
                            Line.SelectionMode.SELECT);
                        }
                        if (line == firstSelLine && firstSelLine.Row == _firstSelected.Row)
                        {
                            CurrentLeft(line);
                        }
                    }
                    else
                    {
                        if (MouseGUI.Position.Y < line.Bottom && line == firstSelLine && firstSelLine.Row == _firstSelected.Row)
                        {
                            CurrentLeft(line);
                        }
                    }
                }
                else
                {
                   
                    if (line.FirstSelected != null)
                    {
                        // Select Top Line
                        if (MouseGUI.Position.Y < lastSelLine.Top && line.Row < _firstSelected.Row)
                        {
                            line.LineSelection(new Func<Character, bool>(ch =>
                                ch.Left < line.LastSelected.Right && line.LastSelected.Right > MouseGUI.Position.X && ch.Right > MouseGUI.Position.X),
                                Line.SelectionMode.SELECT);
                        }
                        // Disselect Bottom Line
                        if (MouseGUI.Position.Y > firstSelLine.Bottom && MouseGUI.Position.Y <= line.Bottom &&
                            line > firstSelLine)
                        {
                            line.LineSelection(new Func<Character, bool>(ch => ch.Selected &&
                            ch.Right <= line.LastSelected.Right && MouseGUI.Position.X <= ch.Left),
                            Line.SelectionMode.DISSELECT);
                        }
                    }
                    if (_firstSelected.Top < MouseGUI.Position.Y && line.Row == _firstSelected.Row && _fullText.SelectedLinesCount < 2)
                    {
                        CurrentLeft(line);
                    }
                }
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
        public override void Update(GameTime gameTime)
        {
            if (Active)
            {
                if (NumberOfLines > MaxNumberOfLines && Contains(MouseGUI.Position))
                {
                    if (MouseGUI.ScrollerValue > 0)
                    {
                        if (_scrollBar.CurrentScrollValue - MouseGUI.ScrollerValue >= 0)
                        {
                            _scrollEvent.OnScroll(this, ScrollDirection.UP, -MouseGUI.ScrollerValue);
                        }
                    }
                    if (MouseGUI.ScrollerValue < 0)
                    {
                        if (_scrollBar.CurrentScrollValue - MouseGUI.ScrollerValue <= NumberOfLines - MaxNumberOfLines)
                        {
                            _scrollEvent.OnScroll(this, ScrollDirection.DOWN, -MouseGUI.ScrollerValue);
                        }
                    }
                }

                if (MouseGUI.LeftWasReleased)
                {
                    _firstSelected = null;
                }
                if (MouseGUI.LeftWasPressed)
                {

                    ResetSelection();
                    if (_prevPosition != (MouseGUI.Position + MouseGUI.DragOffset))
                    {
                        for (int i = 0; i < _visibleLines.Count; i++)
                        {
                            Line line = _visibleLines[i];
                            Point HitPos = MouseGUI.Position;
                            _currChar = line.HitTest(MouseGUI.Position);

                            if (_currChar != null)
                            {
                                _prevPosition = HitPos;
                                _fullText.SetPointed(_currChar);
                                if (_currChar == line.NewLine)
                                {
                                    Pointer += new Point(_currChar.Left, _currChar.Top);
                                }
                                else
                                {
                                    if (HitPos.X >= _currChar.Center.X)
                                    {
                                        Pointer += new Point(_currChar.Right, _currChar.Top);
                                        _firstSelected = _currChar.Next(line);
                                    }
                                    else
                                    {
                                        Pointer += new Point(_currChar.Left, _currChar.Top);
                                        _firstSelected = _currChar;

                                    }
                                }
                                _firstPtPosition = Pointer.Position;

                                break;
                            }
                            else
                            {
                                if(Contains(MouseGUI.Position))
                                {
                                    if(MouseGUI.Position.Y >= line.Top && MouseGUI.Position.Y <= line.Bottom)
                                    {
                                        if(!line.Fresh)
                                        {
                                            if (MouseGUI.Position.X >= line.Back.Right)
                                            {
                                                _currChar = line.Back;
                                                Pointer += new Point(_currChar.Right, _currChar.Top);
                                            }
                                            else
                                            {
                                                _currChar = line.Front;
                                                Pointer += new Point(_currChar.Left, _currChar.Top);
                                            }
                                        }
                                        else
                                        {
                                            _currChar = line.Front;
                                            Pointer += new Point(_currChar.Left, _currChar.Top);

                                        }
                                       
                                        _firstSelected = _currChar;
                                        _fullText.SetPointed(_currChar);
                                        _firstPtPosition = Pointer.Position;
                                        break;

                                    }
                                   
                                    Pointer.Show();
                                }
                            }
                            
                        }
                    }

                }
                UpdateLines();
                if (MouseGUI.LeftIsPressed)
                {
                    Point delta = MouseGUI.Position - _prevPosition;
                    if (_firstSelected != null)
                    {
                        if (delta.Y > 1)
                        {
                            SelectDown();
                        }
                        if (delta.Y < -1)
                        {
                            SelectUp();
                        }
                        if (delta.X > 0)
                        {
                            SelectRight();
                        }
                        if (delta.X < 0)
                        {
                            SelectLeft();
                        }

                        if (TopSelected != null)
                        {
                            if (TopSelected.Bottom <= BottomSelected.Top && _firstSelected.Row == TopSelected[0].Row)
                            {
                               
                                Pointer += new Point(BottomSelected.LastSelected.Right, BottomSelected.LastSelected.Top);
                                _fullText.SetPointed(BottomSelected.LastSelected);
                            }
                            else
                            {
                                if (BottomSelected[0].Row == TopSelected[0].Row)
                                {
                                    if (_firstSelected.Right > TopSelected.LastSelected.Left)
                                    {
                                        Pointer += new Point(TopSelected.FirstSelected.Left, TopSelected.FirstSelected.Top);
                                    }
                                    else
                                    {
                                        Pointer += new Point(TopSelected.LastSelected.Right, TopSelected.FirstSelected.Top);
                                    }

                                }
                                else
                                {
                                    Pointer += new Point(TopSelected.FirstSelected.Left, TopSelected.FirstSelected.Top);
                                }
                                _fullText.SetPointed(TopSelected.LastSelected);
                            }
                           
                        }
                    }
                }
                _prevPosition = MouseGUI.Position;
              

                TextPosition = new Vector2(Left + TextOffset.X, Top);


                if (IsClicked && !Editable)
                {
                    if (_fullText[0].Empty && _fullText.Lines.Count <= 1)
                    {
                        Pointer += new Point(Left + TextOffset.X, Top);
                    }
                    OnKeyboardPressed();
                }
              

             
                if (MouseGUI.LeftIsPressed && MouseGUI.Focus != this)
                {
                    ReceivingInput = false;
                    MouseEvent.Out();
                }
                if (NumberOfLines > MaxNumberOfLines)
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
          
            Pointer.Update(gameTime);
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

                foreach (Line line in _visibleLines)
                {
                    line.Draw(_spriteRenderer);
                }

                if (IsClicked && !Editable && Pointer.Active)
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
