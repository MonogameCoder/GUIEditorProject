using _GUIProject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExtensionMethods
{
    public static class MyExtensions
    {
        public static Vector2 Size(this String str, AssetManager.FontContent font)
        {
            return ((SpriteFont)font).MeasureString(str);
        }
    }
}
