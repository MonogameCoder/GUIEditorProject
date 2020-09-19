using _GUIProject.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace _GUIProject.UI
{   
    [XmlRoot(ElementName = "Frame")]
    public class Frame : Sprite, IContainer
    {
        [XmlElement("Child")]
        public List<Slot<UIObject>> Children { get; set; }

        private SortedSet<Slot<UIObject>> _slots;
        [XmlIgnore]
        public SortedSet<Slot<UIObject>> Slots
        {
            get { return _slots; }
            set
            {
                if (Children == null)
                {
                    Children = value.ToList();
                }
                _slots = new SortedSet<Slot<UIObject>>(Children);
            }
        }

        [XmlIgnore]
        public Sprite FrameBackground { get; set; }

        [XmlIgnore]
        public Slot<UIObject> this[UIObject item]
        {
            get { return Slots.Where(s => s.Item == item).Single(); }
        }
        [XmlIgnore]
        public Slot<UIObject> this[int index]
        {
            get { return Slots.ElementAt(index); }
        }
        [XmlIgnore]
        public int Length
        {
            get { return Slots.Count; }
        }
        public Frame()
        {
            Children = new List<Slot<UIObject>>();
            Slots = new SortedSet<Slot<UIObject>>();
          
        }
        public Frame(string textureName, DrawPriority priority, MoveOption moveOption = MoveOption.STATIC) : base(textureName, priority, moveOption)
        {
            Slots = new SortedSet<Slot<UIObject>>();
            Children = new List<Slot<UIObject>>();
        }

        public override void Initialize()
        {
            base.Initialize();          
            Slots = new SortedSet<Slot<UIObject>>();
            foreach (Slot<UIObject> slot in Slots)
            {
                slot.Item.Initialize();
            }
        }
        public override void Setup()
        {
            base.Setup();
            for (int i = 0; i < Length; i++)
            {
                if (this[i].Item != null)
                {
                    this[i].Item.Position = Position + this[i].Position;
                    this[i].Item.Setup();
                }
            }
        }
        public override void AddSpriteRenderer(SpriteBatch batch)
        {

            _spriteRenderer = batch;
            for (int i = 0; i < Length; i++)
            {
                UIObject item = this[i].Item;
                if (item != null)
                {
                    item.AddSpriteRenderer(batch);
                }
            }
        }
        public override void AddStringRenderer(SpriteBatch batch)
        {

            _stringRenderer = batch;
            for (int i = 0; i < Length; i++)
            {
                UIObject item = this[i].Item;
                if (item != null)
                {
                    item.AddStringRenderer(batch);
                }
            }
        }

        public void AddItem(Point dropLocation, UIObject item, DrawPriority priority = DrawPriority.LOWEST)
        {
            Slots.Add(new Slot<UIObject>(dropLocation, item, priority));
            Children = Slots.ToList();
        }
        public void Insert(Slot<UIObject> slot)
        {
            // Nothing
        }
        public void RemoveSlot(UIObject item)
        {
            for (int i = 0; i < Length; i++)
            {
                Slot<UIObject> slot = this[i];

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
            for (int i = 0; i < Length; i++)
            {
                if (this[i].Item == item)
                {
                    return true;
                }
            }
            return false;
        }
        public override UIObject HitTest(Point mousePosition)
        {
            UIObject result = null;
            
            if (Active)
            {
                // Test children first.
                for (int i = Length -1; i >= 0; i--)
                {
                    Slot<UIObject> slot = this[i];
                    if (slot.Item != null)
                    {
                        result = slot.Item.HitTest(mousePosition);   
                        if(result != null)
                        {
                            //Debug.WriteLine("Index: {0} Object: {1} Type: {2}", i, result.Text, result);
                            break;
                        }
                    }
                }
            }
            if (result != null)
            {               
                return result;
            }
            return base.HitTest(mousePosition);
        }
        public Point SimulateInsert(Point dropLocation, UIObject item, DrawPriority priority = DrawPriority.HIGH)
        {
            return dropLocation;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            for (int i = 0; i < Length; i++)
            {
                Slot<UIObject> slot = this[i];

                if (slot.Item != null)
                {
                    slot.Item.Position = Position + slot.Position;
                    slot.Item.Update(gameTime);

                }
            }
        }
        public void UpdateLayout()
        {        
            for (int i = 0; i < Length; i++)
            {
                Slot<UIObject> slot = this[i];

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
                for (int i = 0; i < Length; i++)
                {
                    Slot<UIObject> slot = this[i];

                    if (slot.Item != null)
                    {
                        slot.Item.Draw();
                    }
                }
            }
        }
        
        public override void Show()
        {
            for (int i = 0; i < Length; i++)
            {
                Slot<UIObject> slot = this[i];
                slot.Item.Active = true;

            }
            Active = true;

        }
        public override void Hide()
        {
            for (int i = 0; i < Length; i++)
            {
                Slot<UIObject> slot = this[i];

                if (slot.Item != null)
                {
                    slot.Item.Active = false;
                }

            }
            Active = false;
        }

        public override bool ShouldSerializeTexture() { return false; }
        public override bool ShouldSerializeSpriteColor() { return false; }
        public override bool ShouldSerializeTextColor() { return false; }
        
    }
}
