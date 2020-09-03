using _GUIProject.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using static _GUIProject.UI.TextBox;

namespace _GUIProject.UI
{
    // This class will be extended in future implementations
    public class FilePicker : BasicSprite
    {
        public Button PickButton { get; private set; }
        public TextBox TextureField { get; private set; }        
        public override string Text
        {
            get { return TextureField.Text; }
            set { TextureField.Text = value; }
        }
        public FilePicker()
        {
            Active = true;
        }     
        public override void Initialize()
        {          
            PickButton = new Button("PropertyPanelFilePickerTX", OverlayOption.NORMAL, DrawPriority.NORMAL);
            PickButton.Initialize();
            PickButton.Text = "";
       

            TextureField = new TextBox("PropertyPanelTextboxTX", "PropertyPanelTextboxPointerTX", TextBoxType.TEXT, DrawPriority.HIGHEST);
            TextureField.Initialize();
            TextureField.FieldWidth = 150;
            TextureField.TextColor.Color = Color.White;       

            MouseEvent = PickButton.MouseEvent;
            TextureField.Active = true;
            PickButton.Active = true;
        }
        public override void Setup()
        {  
            TextureField.Setup();
            PickButton.Setup();

            TextureField.Position = Position;
            PickButton.Position = new Point(TextureField.Right - PickButton.Width, TextureField.Center.Y - PickButton.Height /2);            
        }
        public override void AddSpriteRenderer(SpriteBatch batch)
        {
            TextureField.AddSpriteRenderer(batch);
            PickButton.AddSpriteRenderer(batch);
        }
        public override void AddStringRenderer(SpriteBatch batch)
        {
            TextureField.AddStringRenderer(batch);
            PickButton.AddStringRenderer(batch);
        }
        public override UIObject HitTest(Point mousePosition)
        {
            return PickButton.HitTest(mousePosition);
        }
        public override void Update(GameTime gameTime)
        {            
            TextureField.Update(gameTime);
            PickButton.Update(gameTime);
        }
        public override void Draw()
        {          
            TextureField.Draw();
            PickButton.Draw();
        }
       
    }
}
