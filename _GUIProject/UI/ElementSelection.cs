using Microsoft.Xna.Framework.Graphics;
using static _GUIProject.UI.BasicSprite;
using Microsoft.Xna.Framework;
using System;

namespace _GUIProject.UI
{
    public class ElementSelection : BasicSprite
    {
        private BasicSprite _rightBar;
        private BasicSprite _leftBar;
        private BasicSprite _topBar;
        private BasicSprite _bottomBar;
        public ElementSelection()
        {
           
        }
        public override void Initialize()
        {
            _rightBar = new BasicSprite("ItemSelectionVerticalTX", DrawPriority.LOWEST);
            _rightBar.Initialize();
            _leftBar = new BasicSprite("ItemSelectionVerticalTX", DrawPriority.LOWEST);
            _leftBar.Initialize();
            _topBar = new BasicSprite("ItemSelectionHorizontalTX", DrawPriority.LOWEST);
            _topBar.Initialize();
            _bottomBar = new BasicSprite("ItemSelectionHorizontalTX", DrawPriority.LOWEST);
            _bottomBar.Initialize();
        }
        public override void Setup()
        {
            _rightBar.Setup();
            _leftBar.Setup();
            _topBar.Setup();
            _bottomBar.Setup();

            _rightBar.Hide();
            _leftBar.Hide();
            _topBar.Hide();
            _bottomBar.Hide();
        }
        public override void AddSpriteRenderer(SpriteBatch batch)
        {
            _rightBar.AddSpriteRenderer(batch);
            _leftBar.AddSpriteRenderer(batch);
            _topBar.AddSpriteRenderer(batch);
            _bottomBar.AddSpriteRenderer(batch);
        }
       
        public void UpdatePosition(Point right, Point left, Point top, Point bottom)
        {
            _rightBar.Position = right;
            _leftBar.Position = new Point(left.X - _leftBar.Width, left.Y);
            _topBar.Position = new Point(top.X, top.Y - _topBar.Height);
            _bottomBar.Position = bottom;
        }
        public void UpdateSize(UIObject item)
        {           
            _rightBar.Size = new Point(_rightBar.Width, item.Height);
            _leftBar.Size = new Point(_leftBar.Width, item.Height);
            _topBar.Size = new Point(item.Width, _topBar.Height);
            _bottomBar.Size = new Point(item.Width, _bottomBar.Height);
        }
        public override void AddDefaultRenderers(UIObject item)
        {
            item.AddSpriteRenderer(_spriteRenderer);
            item.AddStringRenderer(_stringRenderer);
        }
     
        public override void ResetSize()
        {
            _rightBar.ResetSize();
            _leftBar.ResetSize();
            _topBar.ResetSize();
            _bottomBar.ResetSize();
        }
        public override void Update(GameTime gameTime)
        {           
            _rightBar.Update(gameTime);
            _leftBar.Update(gameTime);
            _topBar.Update(gameTime);
            _bottomBar.Update(gameTime);
           
        }
        public override void Draw()
        {
            if(Active)
            {
                _rightBar.Draw();
                _leftBar.Draw();
                _topBar.Draw();
                _bottomBar.Draw();
            }
                   
        }
      

        public override void Show()
        {
            Active = true;
            _rightBar.Show();
            _leftBar.Show();
            _topBar.Show();
            _bottomBar.Show();
        }
        public override void Hide()
        {
            Active = false;
            _rightBar.Hide();
            _leftBar.Hide();
            _topBar.Hide();
            _bottomBar.Hide();

        }
    }
}
