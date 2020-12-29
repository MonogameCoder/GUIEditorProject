using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;
using _GUIProject;


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
namespace _GUIProject
{
    
    public class AssetManager
    {

        enum ResourceCategory
        {
            MAP,
            ENTITY,
            UI
        }

        static int currentImageCount = 0;
        static int currentFontCount = 0;
        static int currentSoundCount = 0;
        static int currentSoundBGCount = 0;


        [DataContractAttribute]
        public class TextureContent : IEquatable<TextureContent>
        {
           
           Texture2D _texture;
            string _name;         
            public TextureContent()
            {

            }
            public TextureContent(string name)
            {              
                _name = name;              
            }       
            [XmlIgnore]            
            public Texture2D Texture
            {
                get { return _texture; }
                set { _texture = value; }
            }
           
            [XmlAttribute]
            public string Name
            {
                get { return _name; }     
                set { _name = value; }           
            }          
            public int Height
            {
                get { return _texture.Height; }
            }
            public int Width
            {
                get { return _texture.Width; }
            }
            public Vector2 Size
            {
                get { return new Vector2(Width, Height); }
            }
            public static implicit operator Texture2D(TextureContent rhs)
            {
                return rhs.Texture;
            }

            public bool Equals([AllowNull] TextureContent other)
            {
                return Name.Equals(other.Name);
            }           
           
        }
        [DataContractAttribute]
        public class FontContent : IEquatable<FontContent>
        {
            SpriteFont _font;
            string _name;
            public FontContent()
            {

            }
            public FontContent(string name)
            {
                _name = name;
            }
            [XmlIgnore]
            public SpriteFont Font
            {
                get { return _font; }
                set { _font = value; }
            }
            [XmlIgnore]
            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }
            public static implicit operator SpriteFont(FontContent rhs)
            {
                return rhs.Font;
            }

            public bool Equals([AllowNull] FontContent other)
            {
                return Name.Equals(other.Name);
            }
        }
        public class SoundContent : IEquatable<SoundContent>
        {
            SoundEffect _sound;
            string _name;    
            public SoundContent()
            {

            }
            public SoundContent(string name)
            {             
                _name = name;
              
            }
          
            public SoundEffect Sound
            {
                get { return _sound; }
                set { _sound = value; }
            }           
            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }          
       
            public bool Equals([AllowNull] SoundContent other)
            {
                return this.Name.Equals(other.Name);
            }
        }
      
        public class SoundBGContent :IEquatable<SoundBGContent>
        {
            Song _sound;
            string _name;           
            public SoundBGContent()
            {

            }
            public SoundBGContent(string name)
            {               
                _name = name;             
            }
            public Song Sound
            {
                get { return _sound; }
                set { _sound = value; }
            }        
            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }                    
        
            public bool Equals([AllowNull] SoundBGContent other)
            {
                return this.Name.Equals(other.Name);
            }
        }

        private readonly List<TextureContent> _textures;
        private readonly List<FontContent> _fonts;
        private readonly List<SoundContent> _sounds;
        private readonly List<SoundBGContent> _bgSounds;

   
        public AssetManager()
        {
            _textures = new List<TextureContent>();
            _fonts = new List<FontContent>();
            _sounds = new List<SoundContent>();
            _bgSounds = new List<SoundBGContent>();
        }
        public void AddTextureContent(TextureContent texturePack)
        {
            _textures.Add(texturePack);
        }
        public void AddFontContent(FontContent fontPack)
        {
            _fonts.Add(fontPack);
        }
        public void AddSoundContent(SoundContent soundPack)
        {
            _sounds.Add(soundPack);
        }
        public void AddBGSongContent(SoundBGContent bgSoundPack)
        {
            _bgSounds.Add(bgSoundPack);
        }
        public TextureContent AddTexture(string name)
        {
            TextureContent tex = new TextureContent(name);
            if(!_textures.Contains(tex))
            {
                _textures.Add(tex);
                return tex;
                   
            }
            int index = _textures.IndexOf(tex);
            return _textures[index];
        }
        public FontContent AddFont(string name)
        {
            FontContent font = new FontContent(name);
            if(!_fonts.Contains(font))
            {
                _fonts.Add(font);
                return font;
            }
            _fonts.Add(font);

            int index = _fonts.IndexOf(font);
            return _fonts[index];
        }
        public SoundContent AddSoundEffect(string name)
        {
            SoundContent sound = new SoundContent(name);
            if (!_sounds.Contains(sound))
            {
                _sounds.Add(sound);
                return sound;
            }
            _sounds.Add(sound);

            int index = _sounds.IndexOf(sound);
            return _sounds[index];
        }
        public SoundBGContent AddBGSound(string name)
        {
            SoundBGContent sound = new SoundBGContent(name);
            if (!_bgSounds.Contains(sound))
            {
                _bgSounds.Add(sound);
                return sound;
            }
            _bgSounds.Add(sound);

            int index = _bgSounds.IndexOf(sound);
            return _bgSounds[index];          
        }

        public void LoadResources()
        {
            LoadTextures(MainWindow.MainInstance.Content);
            LoadFonts(MainWindow.MainInstance.Content);
        }

        void LoadTextures(ContentManager content)
        {

            for (int i = currentImageCount; i < _textures.Count; i++)
            {

                if (_textures[i] != null )
                {
                    
                    try
                    {                      
                        _textures[i].Texture = content.Load<Texture2D>(_textures[i].Name);


                    }
                    catch (ContentLoadException e)
                    {
                        
                        //Debug.WriteLine("Texture: " + textures[i].Name + " could not be loaded.\n" + e.Message);
                    }
                  
                   
                }
                currentImageCount++;
            }

        }
        void LoadFonts(ContentManager content)
        {
            for (int i = currentFontCount; i < _fonts.Count; i++)
            {

                if (_fonts[i] != null)
                {
                    try
                    {
                        _fonts[i].Font = content.Load<SpriteFont>(_fonts[i].Name);
                    }
                    catch (Exception e)
                    {

                    }
                }
                currentFontCount++;

            }
        }
        void LoadSoundEffects(ContentManager content)
        {
            for (int i = currentSoundCount; i < _sounds.Count; i++)
            {

                if (_sounds[i] != null)
                {
                    SoundContent sound = _sounds[i];
                    try
                    {
                        sound.Sound = content.Load<SoundEffect>(sound.Name);
                    }
                    catch (Exception e)
                    {

                    }

                }
                currentSoundCount++;
            }

        }

        void LoadBGSounds(ContentManager content)
        {
            for (int i = currentSoundBGCount; i < _bgSounds.Count; i++)
            {

                if (_bgSounds[i] != null)
                {
                    SoundBGContent sound = _bgSounds[i];
                    try
                    {
                        sound.Sound = content.Load<Song>(sound.Name);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Song could not be loaded: " + e.Message);
                    }


                }
                currentSoundBGCount++;
            }

        }

        public void Setup()
        {
           
        }

        public void ReleaseResources()
        {
            ReleaseTextures();
            ReleaseFonts();
            ReleaseSounds();           
        }
        void ReleaseTextures()
        {
            //for (int i = 0; i < _textures.Count; i++)
            //{
            //    if(_textures[i].Texture != null && _textures[i].Loaded)
            //    {
            //        _textures[i].Texture.Dispose();
            //        _textures[i].Texture = null;
            //        _textures[i].Loaded = false;
            //    }              
            //    _textures[i] = null;
            //}
        }
        void ReleaseFonts()
        {
            //for (int i = 0; i < _fonts.Count; i++)
            //{
            //    if(_fonts[i].Font != null && _fonts[i].Loaded)
            //    {
            //        _fonts[i].Font.Texture.Dispose();
            //        _fonts[i].Font = null;
            //        _fonts[i].Loaded = false;
            //    }
            //    _fonts[i] = null;
            //}
        }
        void ReleaseSounds()
        {

            
        }
    

    }
}
