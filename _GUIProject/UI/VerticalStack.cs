using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static _GUIProject.UI.UIObject;

namespace _GUIProject.UI
{
    public class VerticalStack : IContainer
    {
        VerticalStackLL ItemList { get; set; }
        public SortedSet<Slot<UIObject>> Slots { get; set; }
        public Point Position { get; set; }
        public Rectangle Rect
        {
            get { return FrameBackground.Rect; }
        }
        public BasicSprite FrameBackground { get; set; }
        public VerticalStack()
        {         
        }
        public void Initialize()
        {
            Slots = new SortedSet<Slot<UIObject>>();
            ItemList = new VerticalStackLL();
            FrameBackground = new BasicSprite("FrameEditorTX", DrawPriority.NORMAL, MoveOption.STATIC);
            FrameBackground.Initialize();
            FrameBackground.Show();
        }
        public void Setup()
        {
            FrameBackground.Position = Position;
            FrameBackground.Setup();

            ItemList.Rect = FrameBackground.Rect;

            for (int i = 0; i < Slots.Count; i++)
            {
                Slots.ElementAt(i).Item.AddSpriteRenderer(MainWindow._mainBatch);
                Slots.ElementAt(i).Item.AddStringRenderer(MainWindow._mainBatch);
            }
            ///_itemsDLL.PrintList();

        }
        public void AddSpriteRenderer(SpriteBatch batch)
        {

            FrameBackground.AddSpriteRenderer(batch);
            for (int i = 0; i < Slots.Count; i++)
            {
                UIObject item = Slots.ElementAt(i).Item;
                if (item != null)
                {
                    item.AddSpriteRenderer(batch);
                }
            }
        }
        public void AddStringRenderer(SpriteBatch batch)
        {

            FrameBackground.AddStringRenderer(batch);
            for (int i = 0; i < Slots.Count; i++)
            {
                UIObject item = Slots.ElementAt(i).Item;
                if (item != null)
                {
                    item.AddStringRenderer(batch);
                }
            }
        }
        public void AddItem(Point position, UIObject item, DrawPriority priority)
        {
            Slot<UIObject> _newSlot = new Slot<UIObject>(position, item, priority);
            item.Active = true;

            ItemList.Insert(_newSlot);
            Slots.Add(_newSlot);
        }
        public void RemoveItem(UIObject item)
        {
            for (int i = 0; i < Slots.Count; i++)
            {
                Slot<UIObject> slot = Slots.ElementAt(i);

                if (slot.Item == item)
                {
                    ItemList.DeleteNodebyKey(slot);
                    break;
                }
            }
        }
        public void RemoveSlot(UIObject item)
        {
            for (int i = 0; i < Slots.Count; i++)
            {
                Slot<UIObject> slot = Slots.ElementAt(i);

                if (slot.Item == item)
                {
                    Slots.Remove(slot);
                    break;
                }
            }
        }
        public bool Contains(UIObject item)
        {
            for (int i = 0; i < Slots.Count; i++)
            {
                if (Slots.ElementAt(i).Item == item)
                {
                    return true;
                }
            }
            return false;
        }
        public bool Contains(Point position)
        {
            return Rect.Contains(position);
        }
        public UIObject HitTest(Point mousePosition)
        {
            UIObject result = FrameBackground.HitTest(mousePosition); 
            if (FrameBackground.Active)
            {
                // Test children first.
                for (int i = Slots.Count - 1; i >= 0; i--)
                {
                    Slot<UIObject> slot = Slots.ElementAt(i);

                    if (slot.Item != null)
                    {
                        if (slot.Item.HitTest(mousePosition) != null)
                        {
                            result = slot.Item.HitTest(mousePosition);
                            continue;
                        }
                    }
                }             
            }

            return result;
        }

        public void Update(GameTime gameTime)
        {
            FrameBackground.Update(gameTime);         

            for (int i = 0; i < Slots.Count; i++)
            {
                Slot<UIObject> slot = Slots.ElementAt(i);

                if (slot.Item != null)
                {
                    slot.Item.Position = Position + slot.Position;

                    slot.Item.Update(gameTime);

                }
            }
            // ItemList.UpdateLayout();
        }
        public void UpdateLayout()
        {
            ItemList.UpdateLayout();
        }
        public void UpdateSlot(UIObject item, Point newLocation)
        {

        }
        public void Draw()
        {
            FrameBackground.Draw();
            for (int i = Slots.Count - 1; i >= 0; i--)
            {
                Slot<UIObject> slot = Slots.ElementAt(i);

                if (slot.Item != null)
                {
                    slot.Item.Draw();
                }
            }
        }


        public Point SimulateInsert(Point position, UIObject item, DrawPriority priority = DrawPriority.HIGH)
        {
            Slot<UIObject> _newSlot = new Slot<UIObject>(position, item, priority);
            item.Active = true;
            return ItemList.SimulateInsert(_newSlot);
        }

        public void Show()
        {
            for (int i = 0; i < Slots.Count; i++)
            {
                Slot<UIObject> slot = Slots.ElementAt(i);

                if (slot.Item != null)
                {
                    slot.Item.Active = true;
                }

            }
        }
        public void Hide()
        {
            for (int i = 0; i < Slots.Count; i++)
            {
                Slot<UIObject> slot = Slots.ElementAt(i);

                if (slot.Item != null)
                {
                    slot.Item.Hide();
                }

            }
        }


    }
}
