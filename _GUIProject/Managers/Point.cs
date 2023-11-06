using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace _GUIProject
{
    [Serializable]
    public struct Point : IEquatable<Point>
    {
        [XmlAttribute("X")]
        public int X;
        [XmlAttribute("Y")]
        public int Y;

        public Point(int value)
        {
            X = value;
            Y = value;
        }
        public Point(Microsoft.Xna.Framework.Point value)
        {
            X = value.X;
            Y = value.Y;
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
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
        public static bool operator >(Point value1, Point value2)
        {
            return (value1.X >= value2.X && value1.Y > value2.Y || value1.Y >= value2.Y && value1.X > value2.X);
        }
        public static bool operator <(Point value1, Point value2)
        {
            return (value1.X <= value2.X && value1.Y < value2.Y || value1.Y <= value2.Y && value1.X < value2.X);
        }
        public static Point operator +(Point value1, Point value2)
        {
            return new Point(value1.X + value2.X, value1.Y + value2.Y);
        }

        public static Point operator -(Point value1, Point value2)
        {
            return new Point(value1.X - value2.X, value1.Y - value2.Y);
        }
        public static Point operator *(Point value1, float value2)
        {
            return new Point((int)(value1.X * value2), (int)(value1.Y * value2));
        }
        public static Point operator *(Point value1, Point value2)
        {
            return new Point(value1.X * value2.X, value1.Y * value2.Y);
        }
        public static Point operator /(Point source, Point divisor)
        {
            if(divisor.X == 0 || divisor.Y == 0)
            {
                return Point.Zero;
            }
            return new Point(source.X / divisor.X, source.Y / divisor.Y);
        }
        public static Point operator /(Point source, int divisor)
        {
            if (divisor == 0)
            {
                return Point.Zero;
            }
            return new Point(source.X / divisor, source.Y / divisor);
        }
        public static Point operator /(Point source, float divisor)
        {
            if (divisor == 0)
            {
                return Point.Zero;
            }
            return new Point((int)(source.X / divisor), (int)(source.Y / divisor));
        }
        public static implicit operator Vector2(Point rhs)
        {
            return new Vector2(rhs.X, rhs.Y);
        }
        public static implicit operator Microsoft.Xna.Framework.Point(Point rhs)
        {
            return new Microsoft.Xna.Framework.Point(rhs.X, rhs.Y);
        }
        public static implicit operator Point(Microsoft.Xna.Framework.Point rhs)
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
