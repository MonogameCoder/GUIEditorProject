using _GUIProject.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace _GUIProject
{

    public class VerticalStackLL
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
        private Node _head;
        private const int MIN_SPACE = 7;
       
        public VerticalStackLL()
        {
            _head = null;

        }

        public Point SimulateInsert(Slot<UIObject> data)
        {
            Node _newNode = new Node(data);

            if (_head == null)
            {
                return new Point(Rect.Width / 2 - _newNode._data.Item.Width / 2, Rect.Height / 2 - _newNode._data.Item.Height / 2);
            }
            else
            {
                Node temp = _head;
                if (temp._data.Position.Y + temp._data.Item.Height / 2 > data.Position.Y)
                {
                    return new Point(temp._data.Position.X, temp._data.Position.Y - _newNode._data.Item.Height);

                }
                else
                {
                    while (temp != null)
                    {
                        if (data.Position.Y < temp._data.Position.Y + temp._data.Item.Height / 2)
                        {
                            return new Point(temp._data.Position.X, temp._data.Position.Y - _newNode._data.Item.Height);
                        }


                        if (data.Position.Y > temp._data.Position.Y + temp._data.Item.Height / 2)
                        {
                            while (temp._next != null && data.Position.Y > temp._next._data.Position.Y + temp._next._data.Item.Height / 2)
                            {
                                temp = temp._next;
                            }
                            return new Point(temp._data.Position.X, temp._data.Position.Y + temp._data.Item.Height);
                        }

                        temp = temp._next;

                    }
                }
            }

            return Point.Zero;
        }
        public void Insert(Slot<UIObject> data)
        {
            Node _newNode = new Node(data);

            if (_head == null)
            {
                _head = _newNode;
            }
            else
            {
                Node temp = _head;
                if (temp._data.Position.Y + temp._data.Item.Height / 2 > data.Position.Y)
                {
                    _newNode._next = temp;
                    temp._prev = _newNode;
                    _head = _newNode;

                }
                else
                {
                    while (temp != null)
                    {
                        if (data.Position.Y < temp._data.Position.Y + temp._data.Item.Height / 2)
                        {
                            _newNode._next = temp;
                            _newNode._prev = temp._prev;
                            temp._prev._next = _newNode;
                            temp._prev = _newNode;
                            break;
                        }


                        if (data.Position.Y > temp._data.Position.Y + temp._data.Item.Height / 2)
                        {
                            while (temp._next != null && data.Position.Y > temp._next._data.Position.Y + temp._next._data.Item.Height / 2)
                            {
                                temp = temp._next;
                            }
                            _newNode._prev = temp;
                            _newNode._next = temp._next;
                            temp._next = _newNode;
                            break;
                        }

                        temp = temp._next;

                    }
                }
            }


            SetExpandPolicies();
            FixToBounds();
            RearrangeList();
        }
        public void UpdateLayout()
        {
            SetExpandPolicies();
            FixToBounds();
            RearrangeList();
        }
        void RearrangeList()
        {
            Node temp = _head;
            if (temp != null)
            {
                temp._data.Position = new Point(Rect.Width / 2 - GetXMax() / 2, Rect.Height / 2 - temp._data.Item.Height / 2);

                if (temp._next != null)
                {                 
                    temp._data.Position = new Point(Rect.Width / 2 - GetXMax() / 2, temp._data.Position.Y - GetTotalSizeExcept(temp) / 2);
                    while (temp._next != null)
                    {
                        temp._next._data.Position = new Point(Rect.Width / 2 - GetXMax() / 2, temp._data.Position.Y + temp._data.Item.Height + MIN_SPACE);
                        temp = temp._next;
                    }
                }
            }
        }

        int GetTotalSizeExcept(Node current)
        {
            Node temp = _head;
            int totalHeight = 0;
            int spacesSum = 0;
            while (temp != null)
            {
                if (temp == current)
                    temp = temp._next;
                if (temp != null)
                {
                    totalHeight += temp._data.Item.Height;
                    temp = temp._next;
                }
            }

            spacesSum = MIN_SPACE * GetCount();

            return totalHeight + spacesSum;
        }
        public void FixToBounds()
        {
            ResetSizes();
            SetExpandPolicies();
            int _totalSize = GetTotalSize();
            while (_totalSize > Rect.Size.Y - MIN_SPACE * 2)
            {
                Resize();
                _totalSize = GetTotalSize();
                int _start = Rect.Size.Y - GetTotalSize();
                _head._data.Position = new Point(Rect.Size.X / 2 - GetXMax() / 2, Math.Min(MIN_SPACE, _start));
            }
        }
        void SetExpandPolicies()
        {
          
            Node temp = _head;
            while (temp != null)
            {

                if (temp._data.Item.XPolicy == UIObject.SizePolicy.EXPAND)
                {                   
                    int delta = GetXMax() - temp._data.Item.Width;
                    //temp._data.Item.ResetSize();
                    if (delta > 0)
                    {
                        temp._data.Item.Resize(new Point(delta, 0));
                    }
                    
                }
                temp = temp._next;
            }                  

            
        }
        void ResetSizes()
        {
            
                Node temp = _head;
                while (temp != null)
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
                subSize = (int)Math.Ceiling(tmp._data.Item.DefaultSize.Y * 0.01f);
                tmp._data.Item.Resize(new Point(0, -subSize));
                tmp = tmp._next;

            }
        }
        int GetCount()
        {
            Node temp = _head;
            int _count = 0;
            while (temp != null)
            {
                temp = temp._next;
                _count++;
            }

            return _count;
        }
        public int GetXMax()
        {
            Node temp = _head;
            Node max = _head;

            while (temp != null)
            {
                if (temp._data.Item.Width >= max._data.Item.Width)
                {
                    max = temp;
                }
                temp = temp._next;
            }


            return max._data.Item.Width;
        }
        int GetTotalSize()
        {
            Node temp = _head;
            int totalHeight = 0;
            int spacesSum = 0;
            while (temp != null)
            {
                totalHeight += temp._data.Item.Height;
                temp = temp._next;

            }

            spacesSum = MIN_SPACE * (GetCount() + 1);

            return totalHeight + spacesSum;
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
