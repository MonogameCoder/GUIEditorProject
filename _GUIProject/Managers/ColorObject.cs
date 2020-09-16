using Microsoft.Xna.Framework;
using System.Xml.Serialization;
namespace _GUIProject.UI
{
    public class ColorObject
    {
        
        [XmlIgnore]
        public Color Color { get; set; }

        [XmlAttribute]
        public byte R 
        {
            get { return Color.R; }
            set { Color = new Color(value,Color.G, Color.B); }
        }
        [XmlAttribute]
        public byte G
        {
            get { return Color.G; }
            set { Color = new Color(Color.R, value, Color.B); }
        }
        [XmlAttribute]
        public byte B
        {
            get { return Color.B; }
            set { Color = new Color(Color.R, Color.G, value); }
        }
        [XmlAttribute]
        public byte A
        {
            get { return Color.A; }
            set { Color = new Color(Color.R, Color.G, Color.B, value); }
        }

        public ColorObject()
        {

        }
        public string Text
        {
            get
            {
                if (Color == Color.Green)
                {
                    return "Green";
                }
                if (Color == Color.Black)
                {
                    return "Black";
                }
                if (Color == Color.White)
                {
                    return "White";

                }
                else
                {
                    return "";
                }
            }

        }
      
        public static implicit operator Color(ColorObject rhs)
        {
            return rhs.Color;
        }
        public static implicit operator ColorObject(Color rhs)
        {
            ColorObject color = new ColorObject
            {
                Color = rhs
            };
            return color;
        }
        public static Color operator *(ColorObject color, float rhs)
        {
            return rhs * color.Color;
        }
    }
}
