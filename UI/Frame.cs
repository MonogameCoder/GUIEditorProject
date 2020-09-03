using _GUIProject.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _GUIProject.UI
{
    class Frame : BasicSprite, IContainer
    {     
        public SortedSet<Slot<UIObject>> Slots { get; set; }
        public BasicSprite FrameBackground { get; set; }
        public Slot<UIObject> this[UIObject item]
        {
            get { return Slots.Where(s => s.Item == item).Single(); }
        }
        public Frame()
        {

        }
        public Frame(string textureName, DrawPriority priority, MoveOption moveOption = MoveOption.STATIC) : base(textureName, priority, moveOption)
        {           
        }

        public override void Initialize()
        {
            base.Initialize();
            Slots = new SortedSet<Slot<UIObject>>();
        }
        public override void Setup()
        {

            base.Setup();
            for (int i = 0; i < Slots.Count; i++)
            {
                if (Slots.ElementAt(i).Item != null)
                {
                    Slots.ElementAt(i).Item.Position = Position + Slots.ElementAt(i).Position;
                    Slots.ElementAt(i).Item.Setup();
                }

            }

        }
        public override void AddSpriteRenderer(SpriteBatch batch)
        {

            _spriteRenderer = batch;
            for (int i = 0; i < Slots.Count; i++)
            {
                UIObject item = Slots.ElementAt(i).Item;
                if (item != null)
                {
                    item.AddSpriteRenderer(batch);
                }
            }
        }
        public override void AddStringRenderer(SpriteBatch batch)
        {

            _stringRenderer = batch;
            for (int i = 0; i < Slots.Count; i++)
            {
                UIObject item = Slots.ElementAt(i).Item;
                if (item != null)
                {
                    item.AddStringRenderer(batch);
                }
            }
        }

        public void AddItem(Point dropLocation, UIObject item, DrawPriority priority = DrawPriority.LOWEST)
        {
            Slots.Add(new Slot<UIObject>(dropLocation, item, priority));
        }
        public void RemoveSlot(UIObject item)
        {
            for (int i = 0; i < Slots.Count; i++)
            {
                Slot<UIObject> slot = Slots.ElementAt(i);

                if (slot.Item == item || slot.Item == null)
                {
                    Slots.Remove(slot);
                    break;
                }

            }

        }
        public void RemoveItem(UIObject item)
        {

        }
        public override void Resize(Point amount)
        {
            base.Resize(amount);
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
        public override UIObject HitTest(Point mousePosition)
        {
            UIObject result = base.HitTest(mousePosition);
            if (Active)
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
        public Point SimulateInsert(Point dropLocation, UIObject item, DrawPriority priority = DrawPriority.HIGH)
        {
            return dropLocation;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            for (int i = 0; i < Slots.Count; i++)
            {
                Slot<UIObject> slot = Slots.ElementAt(i);

                if (slot.Item != null)
                {
                    slot.Item.Position = Position + slot.Position;
                    slot.Item.Update(gameTime);

                }
            }
        }
        public void UpdateLayout()
        {        
            for (int i = 0; i < Slots.Count; i++)
            {
                Slot<UIObject> slot = Slots.ElementAt(i);

                if (slot.Item != null)
                {
                    slot.Item.Position = Position + slot.Position;                

                }
            }
        }
        public void UpdateSlot(UIObject item, Point newLocation)
        {
            Slot<UIObject> slot = this[item];
            
            Vector2 delta;
            delta.X = newLocation.X - slot.Position.X;
            delta.Y = newLocation.Y - slot.Position.Y;
            float dist = delta.Length();

            if (dist >= 0.0f)
            {
               slot.Position += delta.ToPoint();              
            }
            else
            {               
                slot.Position -= delta.ToPoint();
            }

        }      
        public override void Draw()
        {
            if (Active)
            {
                base.Draw();
                for (int i = Slots.Count - 1; i >= 0; i--)
                {
                    Slot<UIObject> slot = Slots.ElementAt(i);

                    if (slot.Item != null)
                    {
                        slot.Item.Draw();
                    }
                }
            }

        }
        
        public override void Show()
        {
            for (int i = 0; i < Slots.Count; i++)
            {
                Slot<UIObject> slot = Slots.ElementAt(i);
                slot.Item.Active = true;

            }
            Active = true;

        }
        public override void Hide()
        {
            for (int i = 0; i < Slots.Count; i++)
            {
                Slot<UIObject> slot = Slots.ElementAt(i);

                if (slot.Item != null)
                {
                    slot.Item.Hide();
                }

            }
            Active = false;
        }


    }
}
