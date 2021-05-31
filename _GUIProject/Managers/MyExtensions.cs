using _GUIProject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static _GUIProject.UI.TextArea;
using static _GUIProject.UI.TextBox;

namespace ExtensionMethods
{
    public static class MyExtensions
    {
        
        public static Character At(this TextLines lines, _GUIProject.Point pt)
        {
            Character ch = null;
            for (int i = 0; i < lines.Lines.Count; i++)
            {
                Line line = lines.Lines[i];
                for (int j = 0; j < line.Characters.Count; j++)
                {
                    ch = line.Characters[j];

                    if (pt.Y == ch.Top)
                    {
                        Character chnext = ch.Next(line);

                        if (pt.X == ch.Right || pt.X == ch.Left)
                        {
                            return ch;
                        }
                    }
                }
            }
            return null;
        }
        public static Character At(this Line line, _GUIProject.Point pt)
        {
            Character ch = null;

            for (int i = 0; i < line.Characters.Count; i++)
            {
                ch = line.Characters[i];

                if (pt.Y == ch.Top)
                {
                    if (pt.X < ch.Right)
                    {

                        return ch;
                    }
                }
            }
            return null;
        }
        public static Line AtAbove(this _GUIProject.Point pt, TextLines lines)
        {    
            for (int i = 0; i < lines.Lines.Count; i++)
            {
                Line line = lines.Lines[i];
                if(!line.Empty)
                {
                    if (pt.Y <= line.Bottom && pt.Y > line.Top)
                    {
                        return line;
                    }
                }
             
            }
            return null;
        }
        public static Line AtBelow(this _GUIProject.Point pt, TextLines lines)
        {
            for (int i = 0; i < lines.Lines.Count; i++)
            {
                Line line = lines.Lines[i];
                if(!line.Empty)
                {
                    if (pt.Y >= line.Top && pt.Y < line.Bottom)
                    {
                        return line;
                    }
                }
              
            }
            return null;
        }
        
        public static void Move<T> (this List<T> list , int currIdx, int newIndex)
        {          
            var item = list[currIdx];

            list.RemoveAt(currIdx);

            list.Insert(newIndex, item);

        }
        public static bool Contains(this List<Line> lines, Character ch)
        {
            foreach (Line line in lines)
            {
                if(line.Characters.Contains(ch))
                {
                    return true;
                }               
            }
            return false;
        }
        public static bool Exists (this Character ch)
        {
            return ch != null;
        }
        public static int Sum(this int lhs, int rhs)
        {
            return lhs + rhs;
        }
        public static int Subtract(this int lhs, int rhs)
        {
            return lhs - rhs;
        }
        public static Vector2 Size(this String str, AssetManager.FontContent font)
        {
            return ((SpriteFont)font).MeasureString(str);
        }
        public static Character Previous(this Character ch, Line ln)
        {
            return ln[Math.Max(ln.Characters.IndexOf(ch) - 1, 0)];
        }
        public static Character Next(this Character ch, Line ln)
        {

            return ln[Math.Min(ln.Characters.IndexOf(ch) + 1, ln.Characters.Count - 1)];
        }

        public static Line Below(this Line ln, List<Line> lns)
        {
            int idx = Math.Min(lns.IndexOf(ln) + 1, lns.Count - 1);
            if (lns.Count > 0)
            {
                return lns[idx];
            }
            return null;
        }
        public static Line Above(this Line ln, List<Line> lns)
        {
            int idx = Math.Max(lns.IndexOf(ln) - 1, 0);
            if (lns.Count > 0)
            {
                return lns[idx];
            }
            return null;
        }
        public static string Text(this List<Character> chars)
        {
            string text = "";
            foreach (Character ch in chars)
            {
                text += ch.Text;
            }
            return text;
        }
    }
}
