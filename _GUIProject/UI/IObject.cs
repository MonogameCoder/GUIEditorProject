using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace _GUIProject.UI
{
    public interface IObject
    {
        void Initialize();
        void Setup();
        void AddSpriteRenderer(SpriteBatch batch);
        void AddStringRenderer(SpriteBatch batch);
        UIObject HitTest(Point mousePosition); 
        void Update(GameTime gameTime);
        void Draw();        
    }
}