using _GUIProject.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// <summary>
///  TODO
/// </summary>
namespace _GUIProject
{
   
    public class HorizontalStackLL
    {

        public class Node
        {
            public Slot<UIObject> _data;
            public Node _next;
            public Node _prev;
            
            public Node(Slot<UIObject> d)
            {
                _data = d;
                _next = null;
                _prev = null;

            }
        }

        public Rectangle Rect { get; set; }
        Node _head;
        const int MIN_SPACE = 7;
        public int GetYMax()
        {
            Node temp = _head;
            Node max = _head;           

            while (temp != null)
            {
                if (temp._data.Item.Height > max._data.Item.Height)
                {
                    max = temp;
                }
                temp = temp._next;
            }
            return max._data.Item.Height;
        }
        public HorizontalStackLL()
        {
            _head = null;
          
        }

        public Point SimulateInsert(Slot<UIObject> data)
        {
            Node newNode = new Node(data);

            if (_head == null)
            {
                return new Point(Rect.Width / 2 - newNode._data.Item.Width / 2, Rect.Height / 2 - newNode._data.Item.Height / 2);
            }
            else
            {
                Node temp = _head;
                if (temp._data.Position.X + temp._data.Item.Width / 2 > data.Position.X)
                {
                    return new Point(temp._data.Position.X - data.Item.Width, temp._data.Position.Y);

                }
                else
                {
                    while (temp != null)
                    {
                        if (data.Position.X < temp._data.Position.X + temp._data.Item.Width / 2)
                        {
                            return new Point(temp._data.Position.X - data.Item.Width, temp._data.Position.Y);
                        }


                        if (data.Position.X > temp._data.Position.X + temp._data.Item.Width / 2)
                        {
                            while (temp._next != null && data.Position.X > temp._next._data.Position.X + temp._next._data.Item.Width / 2)
                            {
                                temp = temp._next;
                            }
                            return new Point(temp._data.Position.X + temp._data.Item.Width, temp._data.Position.Y);
                        }

                        temp = temp._next;

                    }
                }
            }

            return Point.Zero;

        }
        public void Insert(Slot<UIObject> data)
        {          
            Node newNode = new Node(data);

            if (_head == null)
            {             
                _head = newNode;               
            }
            else
            {
                Node temp = _head;
                if (temp._data.Position.X + temp._data.Item.Width / 2 > data.Position.X)
                {
                    newNode._next = temp;
                    temp._prev = newNode;              
                    _head = newNode;
               
                }
                else
                {
                    while (temp != null)
                    {
                        if (data.Position.X < temp._data.Position.X + temp._data.Item.Width / 2)
                        {
                            newNode._next = temp;
                            newNode._prev = temp._prev;
                            temp._prev._next = newNode;
                            temp._prev = newNode;
                            break;
                        }

                      
                        if (data.Position.X > temp._data.Position.X + temp._data.Item.Width / 2)
                        {          
                            while(temp._next != null && data.Position.X > temp._next._data.Position.X + temp._next._data.Item.Width / 2)
                            {
                                temp = temp._next;
                            }
                            newNode._prev = temp;
                            newNode._next = temp._next;
                            temp._next = newNode;
                            break;
                        }

                        temp = temp._next;

                    }     
                }
            }

            //SetExpandPolicies();
            FixToBounds();
            RearrangeList();
        }
        public void UpdateLayout()
        {
            //SetExpandPolicies();
            FixToBounds();
            RearrangeList();
        }
        void RearrangeList()
        {
            Node temp = _head;
            if(temp != null)
            {
                temp._data.Position = new Point(Rect.Width / 2 - temp._data.Item.Width /2, Rect.Height / 2 - GetYMax() / 2);
               
                if (temp._next != null)
                {                 
                    temp._data.Position = new Point(temp._data.Position.X - GetTotalSizeExcept(temp) / 2, Rect.Height / 2 - GetYMax() / 2);
                    while (temp._next != null)
                    {                        
                        temp._next._data.Position = new Point(temp._data.Position.X + temp._data.Item.Width + MIN_SPACE, Rect.Height / 2 - GetYMax() / 2);                     
                        temp = temp._next;                        
                    }
                }
            }
        }
       
        int GetTotalSizeExcept(Node current)
        {
            Node temp = _head;
            int totalWidth = 0;
            int spacesSum = 0;
            while (temp != null)
            {
                if (temp == current)
                    temp = temp._next;
                if(temp != null)
                {
                    totalWidth += temp._data.Item.Width;
                    temp = temp._next;
                }  
            }

            spacesSum = MIN_SPACE * GetCount();

            return totalWidth + spacesSum;
        }
        public void FixToBounds()
        {
            ResetSizes();
            SetExpandPolicies();
            int _totalSize = GetTotalSize();
            while (_totalSize > Rect.Width - MIN_SPACE * 2)
            {
                Resize();               
                int _start = _totalSize - Rect.Width;
                _head._data.Position = new Point(Math.Min(MIN_SPACE, _start), Rect.Size.Y / 2 - GetYMax() / 2);
              
                _totalSize = GetTotalSize();
            }                     
        }
        void SetExpandPolicies()
        {            
            Node temp = _head;
            while (temp != null)
            {               
                if (temp._data.Item.YPolicy == UIObject.SizePolicy.EXPAND)
                {                    
                    
                    int delta = GetYMax() - temp._data.Item.Height;
                    //temp._data.Item.ResetSize();
                    
                    if (delta > 0)
                    {
                        temp._data.Item.Resize(new Point(0, delta));
                    }
                  
                }
                temp = temp._next;
            }
           

        }
        void ResetSizes()
        {            
            Node temp = _head;
            while(temp != null)
            {
                temp._data.Item.Size = temp._data.Item.DefaultSize;
                temp = temp._next;
            }
        }
        void Resize()
        {
            Node tmp = _head;

            int subSize = 0;
            while (tmp != null)
            {
                subSize = (int)Math.Ceiling(tmp._data.Item.DefaultSize.X * 0.01f);
                tmp._data.Item.Size -= new Point(subSize,0);
                tmp = tmp._next;

            }
        }
        int GetCount()
        {
            Node temp = _head;
            int count = 0;
            while (temp != null)
            {                
                temp = temp._next;
                count++;
            }

            return count;
        }

        int GetTotalSize()
        {
            Node temp = _head;          
            int totalWidth = 0;
            int spacesSum = 0;
            while (temp != null)
            {
                totalWidth += temp._data.Item.Width;
                temp = temp._next;
                
            }

            spacesSum = MIN_SPACE * (GetCount() + 1);

            return totalWidth + spacesSum;
        }
       
       
   
        public void DeleteNodebyKey(Slot<UIObject> key)
        {
            Node temp = _head;
            Node _prev = null;

            if (MouseGUI.Focus == null)
            {
                key.Item.ResetSize();
            }
            if (temp != null && temp._data == key)
            {
                temp = temp._next;
                _prev = temp;               
                _head = temp;       
                return;
            }

            while (temp != null && temp._data != key)
            {
                _prev = temp;
                temp = temp._next;
            }
            if (temp == null)
            {
                return;
            }
            
            _prev._next = temp._next;                    
           
        }
     

    }
}
