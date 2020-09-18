using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using _GUIProject.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static _GUIProject.UI.UIObject;

namespace _GUIProject.UI
{  
  
    [XmlRoot(ElementName = "Grid")]
    public class Grid : IContainer
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
                if(Children == null)
                {
                    Children = value.ToList();
                }
                _slots = new SortedSet<Slot<UIObject>>(Children);
            }
        }          
        public Point Position { get; set; }
       
        [XmlIgnore]
        public Rectangle Rect 
        {
            get { return FrameBackground.Rect; }           
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
       
        [XmlIgnore]
        public Sprite FrameBackground { get; set; }

        private GridLL ItemList { get; set; }
        public Grid()
        {             
        }
        public Grid(DrawPriority priority = DrawPriority.NORMAL)
        {                   
        }
        public void Initialize()
        {
           
            ItemList = new GridLL();           
            Slots = new SortedSet<Slot<UIObject>>();

            FrameBackground = new Sprite("FrameEditorTX", DrawPriority.NORMAL, MoveOption.STATIC);
            FrameBackground.Initialize();
            FrameBackground.Show();
        }
        public void Setup()
        {
            FrameBackground.Position = Position;
            FrameBackground.Setup();          
            
            ItemList.Rect = FrameBackground.Rect;

            for (int i = 0; i < Length; i++)
            {
                if (this[i].Item != null)
                {
                    this[i].Item.AddSpriteRenderer(MainWindow._mainBatch);
                    this[i].Item.AddStringRenderer(MainWindow._mainBatch);
                }
            }
        }
        public void AddSpriteRenderer(SpriteBatch batch)
        {

            FrameBackground.AddSpriteRenderer(batch);
            for (int i = 0; i < Length; i++)
            {
                UIObject item = this[i].Item;
                if (item != null)
                {
                    item.AddSpriteRenderer(batch);
                }
            }
        }
        public void AddStringRenderer(SpriteBatch batch)
        {

            FrameBackground.AddStringRenderer(batch);
            for (int i = 0; i < Length; i++)
            {
                UIObject item = this[i].Item;
                if (item != null)
                {
                    item.AddStringRenderer(batch);
                }
            }
        }
        public void AddItem(Point position, UIObject item, DrawPriority priority)
        {
            Slot<UIObject> slot = new Slot<UIObject>(position, item, priority);
            Insert(slot);
            Slots.Add(slot);
            Children = Slots.ToList();

            
        }
        public void Insert(Slot<UIObject> slot)
        {
            ItemList.Insert(slot);
        }
        public Point SimulateInsert(Point position, UIObject item, DrawPriority priority)
        {
            Slot<UIObject> _newSlot = new Slot<UIObject>(position, item, priority);            
            return ItemList.SimulateInsert(_newSlot);
        }
        public void RemoveItem(UIObject item)
        {
            for (int i = 0; i < Length; i++)
            {
                Slot<UIObject> slot = this[i];

                if (slot.Item == item)
                {
                    ItemList.DeleteNodebyKey(slot);                    
                    break;
                }
            }
        }
        public void RemoveSlot(UIObject item)
        {
            for (int i = 0; i < Length; i++)
            {
                Slot<UIObject> slot = this[i];

                if (slot.Item == item)
                {                   
                    Slots.Remove(slot);
                    break;
                }
            }
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
        public bool Contains(Point position)
        {
            return FrameBackground.Contains(position);
        }

        public UIObject HitTest(Point mousePosition)
        {
            UIObject result = null;

            if (FrameBackground.Active)
            {
                // Test children first.
                for (int i = Length - 1; i >= 0; i--)
                {
                    Slot<UIObject> slot = this[i];
                    if (slot.Item != null)
                    {
                        result = slot.Item.HitTest(mousePosition);
                        if (result != null)
                        {                           
                            break;
                        }
                    }
                }
            }
            if (result != null)
            {
                return result;
            }

            return FrameBackground.HitTest(mousePosition); 
        }

        
        public void Update(GameTime gameTime)
        {          
            FrameBackground.Update(gameTime);
        
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
            ItemList.UpdateLayout();
        }
        public void UpdateSlot(UIObject item, Point newLocation)
        {
           // Nothing
        }     

        public void Draw()
        {
            FrameBackground.Draw();
            for (int i = 0; i < Length; i++)
            {
                Slot<UIObject> slot = this[i];

                if (slot.Item != null)
                {
                    slot.Item.Draw();
                }
            }
        }
        public void Show()
        {
            for (int i = 0; i < Length; i++)
            {
                Slot<UIObject> slot = this[i];

                if (slot.Item != null)
                {
                    slot.Item.Active = true;
                }

            }
        }
        public void Hide()
        {
            for (int i = 0; i < Length; i++)
            {
                Slot<UIObject> slot = this[i];

                if (slot.Item != null)
                {
                    slot.Item.Hide();
                }

            }
        }

      
    }
}

