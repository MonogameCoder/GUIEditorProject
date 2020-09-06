using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Xml.Serialization;

namespace GUIProject_XML
{
    // This custom Point struct will be used for serialization, and therefore will replace the XNA version
    public struct Point : IEquatable<Point>
    {

        public int X;
        public int Y;

        public Point(int value)
        {
            X = value;
            Y = value;
        }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Point Zero { get { return new Point(0, 0); } }

        public bool Equals(Point other)
        {
            return this == other;
        }

        public override string ToString()
        {
            return "{x: " + X.ToString() + ",y: " + Y.ToString() + ")";
        }

        public Vector2 ToVector2()
        {
            return new Vector2(X, Y);
        }
        public Microsoft.Xna.Framework.Point ToPoint()
        {
            return new Microsoft.Xna.Framework.Point(X, Y);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
       
        public static Point operator +(Point value1, Point value2)
        {
            return new Point(value1.X + value2.X, value1.Y + value2.Y);
        }

        public static Point operator -(Point value1, Point value2)
        {
            return new Point(value1.X - value2.X, value1.Y - value2.Y);
        }
        public static Point operator *(Point value1, Point value2)
        {
            return new Point(value1.X * value2.X, value1.Y * value2.Y);
        }
        public static Point operator /(Point source, Point divisor)
        {
            return new Point(source.X / divisor.X, source.Y / divisor.Y);
        }
        public static implicit operator Microsoft.Xna.Framework.Point(Point rhs)
        {
            return new Microsoft.Xna.Framework.Point(rhs.X, rhs.Y);
        }
        public static explicit operator Point(Microsoft.Xna.Framework.Point rhs)
        {
            return new Point(rhs.X, rhs.Y);
        }
        public static bool operator ==(Point a, Point b)
        {
            return a.X == b.X && a.Y == b.Y;
        }
        public static bool operator !=(Point a, Point b)
        {
            return !(a == b);
        }

    }
}
