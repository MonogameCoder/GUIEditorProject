
using Microsoft.Xna.Framework;

namespace _GUIProject.UI
{

    public class ColorObject
    {
        private Color _color;
        public ColorObject()
        {

        }
        public string Text
        {
            get
            {
                if(_color == Color.Green)
                {
                   return "Green";
                }
                if(_color == Color.Black )
                {
                    return "Black";
                }
                if(_color == Color.White)
                {
                    return "White";

                }             
                else
                {
                    return "";
                }         
            }

        }
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
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
        public static Color operator* (ColorObject color, float rhs)
        {
            return rhs * color.Color ;
        }
    }
}
