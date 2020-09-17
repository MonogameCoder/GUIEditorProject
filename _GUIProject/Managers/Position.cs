using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace _GUIProject
{
    public class Position 
    {
        [XmlAttribute("Position")]
        public Point Location { get; set; }

        public int X
        {
            get { return Location.X; }
        }
        public int Y
        {
            get { return Location.Y; }
        }
        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(XmlReader reader)
        {
            reader.MoveToAttribute("Position");
            string data = reader.Value;
            var split = data.Split(':');
            Location = new Point(int.Parse(split[0]), int.Parse(split[1]));
            reader.MoveToElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("Position", Location.X + ":" + Location.Y);          
        }
        public static Point operator +(Position value1, Point value2)
        {
            return new Point(value1.X + value2.X, value1.Y + value2.Y);
        }

        public static Point operator -(Position value1, Point value2)
        {
            return new Point(value1.X - value2.X, value1.Y - value2.Y);
        }
        public static implicit operator Point(Position rhs)
        {
            return rhs.Location;
        }
        public static implicit operator Position(Point rhs)
        {
            var posi = new Position()
            {
                Location = rhs
            };

            return posi;
        }     
    }
}
