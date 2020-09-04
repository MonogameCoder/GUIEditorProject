namespace _GUIProject
{
    class Singleton
    {      
        
        private static InputManager _input;
        public static InputManager Input
        {
            get
            {
                if (_input == null)
                {
                    _input = new InputManager();
                }
                return _input;
            }
        }
        private static FontManager _font;
        public static FontManager Font
        {
            get
            {
                if (_font == null)
                {
                    _font = new FontManager();
                }
                return _font;
            }
        }
        private static AssetManager _content;
        public static AssetManager Content
        {
            get
            {
                if (_content == null)
                {
                    _content = new AssetManager();
                }
                return _content;
            }
        }
        // In a Future implementation sound feature will be supported.
        // It will support things like object click sounds and so forth
        //static SoundMgr sound;
        //public static SoundMgr Sound
        //{
        //    get
        //    {
        //        if (sound == null)
        //        {
        //            sound = new SoundMgr();
        //        }
        //        return sound;
        //    }
        //}
    }
}
