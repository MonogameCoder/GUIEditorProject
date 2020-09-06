using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace _GUIProject.UI
{
    /// <summary>
    /// This ToolTip class will eventualy be extended, but right now it is just a button
    /// </summary>
    public class ToolTip : Button
    {


        private UIObject _parent;
        public ToolTip(UIObject parent) :base("DefaultSliderBarTooltipTX", OverlayOption.NORMAL, DrawPriority.LOWEST)
        {
            _parent = parent;
        }
        public ToolTip(UIObject parent, string baseTX, OverlayOption overlay, DrawPriority priority) : base(baseTX, overlay, priority)
        {
            _parent = parent;
        }
        public override void Update(GameTime gameTime)
        {
            Position = new Point(_parent.Rect.Center.X - Width / 2, _parent.Top - Height - 4);
           
            base.Update(gameTime);          
           
        }
        public override void Initialize()
        {
            base.Initialize();
            TextColor = Color.Black;                 
            Hide();
          
        }
        public override void Setup()
        {
            base.Setup();
            Resize(Caption.TextSize);
        }
    }
}
