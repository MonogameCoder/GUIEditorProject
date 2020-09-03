using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using static _GUIProject.AssetManager;

namespace _GUIProject
{
    public class FontManager
    {
      
        
        public enum FontType
        {            
            STANDARD,           
            ARIAL,
            ARIAL_BOLD,
            LUCIDA_CONSOLE,
            GEORGIA
        }
        Dictionary<FontType, FontContent> _fonts;
    


        public FontManager()
        {
			_fonts = new Dictionary<FontType, FontContent>();
			_fonts.Add(FontType.STANDARD, Singleton.Content.AddFont("Fonts/StandardFont"));
			_fonts.Add(FontType.ARIAL, Singleton.Content.AddFont("Fonts/Arial"));
			_fonts.Add(FontType.ARIAL_BOLD, Singleton.Content.AddFont("Fonts/ArialBold"));
			_fonts.Add(FontType.LUCIDA_CONSOLE, Singleton.Content.AddFont("Fonts/Lucida Console"));
			_fonts.Add(FontType.GEORGIA, Singleton.Content.AddFont("Fonts/Georgia"));		
        }
       
        public void Setup()
        {
           
        }
        public FontContent GetFont(FontType type)
        {
            return _fonts[type];
        }      
       
    }
}
