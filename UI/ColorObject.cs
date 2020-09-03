
using Microsoft.Xna.Framework;

namespace _GUIProject.UI
{

    public class ColorObject
    {
        
        Color _color;
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
    }
}
