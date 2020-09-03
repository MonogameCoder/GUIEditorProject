using _GUIProject.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _GUIProject
{
    public class GridLL
    {
        public class Node
        {
            public Node _right;
            public Node _left;
            public Node _down;
            public Node _up;
            public Slot<UIObject> _data;
            public int Width;
            public int Height;
            public Node(Slot<UIObject> d)
            {
                _data = d;
                _right = null;
                _left = null;
                _down = null;
                _up = null;

                if (d.Item != null)
                {
                    Width = d.Item.Width;
                    Height = d.Item.Height;
                }

            }
        }

        public Rectangle Rect { get; set; }
        Node _head;

        const int MIN_SPACE = 7;
        const float ONE_PER = 0.01f;
        const float TWO_PER = 0.02f;


        public GridLL()
        {
            _head = null;
        }

        public int GetXMax(Node current)
        {
            Node temp = current;
            temp = RewindRows(temp);
            Node _max = null;

            while (temp != null)
            {
                if (temp._data.Item != null)
                {
                    if (_max == null)
                    {
                        _max = temp;
                    }
                    if (temp._data.Item.DefaultSize.X > _max._data.Item.DefaultSize.X)
                    {
                        _max = temp;
                    }
                }
                temp = temp._down;
            }
            if (_max == null)
            {
                temp = RewindRows(current);
                if (temp._up == null && temp._right != null)
                {
                    _head = temp._right;
                }
                DeleteColumn(temp);
                return 0;
            }
            return _max.Width;
        }
        public int GetXMaxExcept(Node current)
        {
            Node temp = current;
            temp = RewindRows(temp);
            Node _max = null;

            while (temp != null)
            {
                if (temp._data.Item != null)
                {
                    if (temp != current)
                    {
                        if (_max == null)
                        {
                            _max = temp;
                        }
                        if (temp._data.Item.DefaultSize.X > _max._data.Item.DefaultSize.X)
                        {
                            _max = temp;
                        }
                    }

                }
                temp = temp._down;
            }
            if (_max == null)
            {
                _max = current;
            }
            return _max._data.Item.DefaultSize.X;
        }
        public int GetYMax(Node current)
        {
            Node temp = current;
            temp = RewindColumns(temp);
            Node _max = null;


            while (temp != null)
            {
                if (temp._data.Item != null)
                {
                    if (_max == null)
                    {
                        _max = temp;
                    }
                    if (temp._data.Item.DefaultSize.Y > _max._data.Item.DefaultSize.Y)
                    {
                        _max = temp;
                    }
                }
                temp = temp._right;
            }
            if (_max == null)
            {
                temp = RewindColumns(current);
                if (temp._up == null && temp._down != null)
                {
                    _head = temp._down;
                }
                DeleteRow(temp);
                return 0;
            }
            return _max.Height;
        }
        public int GetYMaxExcept(Node current)
        {
            Node temp = current;
            temp = RewindColumns(temp);
            Node _max = null;

            while (temp != null)
            {
                if (temp._data.Item != null)
                {
                    if (temp != current)
                    {
                        if (_max == null)
                        {
                            _max = temp;
                        }
                        if (temp._data.Item.DefaultSize.Y > _max._data.Item.DefaultSize.Y)
                        {
                            _max = temp;
                        }
                    }

                }
                temp = temp._right;
            }
            if (_max == null)
            {
                _max = current;
            }
            return _max._data.Item.DefaultSize.Y;
        }

        public Point SimulateInsert(Slot<UIObject> data)
        {
            Node _newNode = new Node(data);
            Node temp = _head;
            if (_head == null)
            {
                return new Point(Rect.Width / 2 - _newNode.Width / 2, Rect.Height / 2 - _newNode.Height / 2);
            }
            else
            {
                while (temp != null)
                {
                    if (data.Position.X < temp._data.Position.X)
                    {
                        while (temp._down != null && data.Position.Y > temp._data.Position.Y + temp.Height)
                        {
                            temp = temp._down;
                        }
                        return new Point(temp._data.Position.X - _newNode.Width, temp._data.Position.Y);

                    }
                    else if (temp._right == null && data.Position.X >= temp._data.Position.X + temp.Width - MIN_SPACE)
                    {
                        while (temp._down != null && data.Position.Y > temp._data.Position.Y + temp.Height)
                        {
                            temp = temp._down;
                        }

                        return new Point(temp._data.Position.X + temp.Width, temp._data.Position.Y);

                    }
                    else
                    {
                        if (data.Position.X >= temp._data.Position.X - MIN_SPACE && data.Position.X <= temp._data.Position.X + temp.Width - MIN_SPACE)
                        {
                            while (temp._right != null && data.Position.X > temp._data.Position.X + temp.Width)
                            {
                                temp = temp._right;
                            }

                            if (data.Position.Y + data.Item.Height <= temp._data.Position.Y)
                            {

                                return new Point(temp._data.Position.X, temp._data.Position.Y - _newNode.Height);
                            }
                            else
                            {

                                while (temp._down != null && data.Position.Y > temp._data.Position.Y + temp.Height + MIN_SPACE)
                                {
                                    temp = temp._down;
                                }


                                if (data.Position.Y <= temp._data.Position.Y + temp.Height && temp._data.Item == null)
                                {

                                    return temp._data.Position;
                                }
                                else
                                {
                                    return new Point(temp._data.Position.X, temp._data.Position.Y + temp.Height);

                                }

                            }
                        }
                    }
                    temp = temp._right;
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

                while (temp != null)
                {
                    if (data.Position.X < temp._data.Position.X)
                    {
                        while (temp._down != null && data.Position.Y > temp._data.Position.Y + temp.Height)
                        {
                            temp = temp._down;
                        }
                        _newNode._right = temp;
                        _newNode._left = temp._left;

                        if (temp._left != null)
                        {
                            temp._left._right = _newNode;
                        }

                        temp._left = _newNode;

                        Node _current = RewindRows(temp);
                        _current = CreateColumnBefore(_current, _newNode);
                        _current = RewindRows(temp);


                        while (_current._down != null)
                        {
                            _current._left._down = _current._down._left;
                            _current._left._down._up = _current._left;

                            _current = _current._down;
                        }

                        _current = RewindRows(temp);
                        _current = RewindColumns(_current);

                        _head = _current;
                        break;


                    }
                    else if (temp._right == null && data.Position.X >= temp._data.Position.X + temp.Width - MIN_SPACE)
                    {
                        while (temp._down != null && data.Position.Y > temp._data.Position.Y + temp.Height)
                        {
                            temp = temp._down;
                        }

                        _newNode._left = temp;
                        _newNode._right = temp._right;
                        temp._right = _newNode;


                        Node _current = RewindRows(temp);
                        _current = CreateColumnAfter(_current, _newNode);

                        _current = RewindRows(temp);
                        while (_current._down != null)
                        {
                            _current._right._down = _current._down._right;
                            _current._down._right._up = _current._right;
                            _current = _current._down;
                        }
                        break;
                    }
                    else
                    {
                        if (data.Position.X >= temp._data.Position.X - MIN_SPACE && data.Position.X <= temp._data.Position.X + temp.Width - MIN_SPACE)
                        {
                            while (temp._right != null && data.Position.X > temp._data.Position.X + temp.Width)
                            {
                                temp = temp._right;
                            }

                            if (data.Position.Y + data.Item.Height <= temp._data.Position.Y)
                            {
                                temp._up = _newNode;
                                _newNode._down = temp;

                                Node _current = RewindColumns(temp);
                                _current = CreateRowAbove(_current, _newNode);

                                _current = RewindColumns(temp);

                                while (_current._right != null)
                                {
                                    _current._up._right = _current._right._up;
                                    _current._right._up._left = _current._up;
                                    _current = _current._right;
                                }
                                _current = RewindRows(_current);
                                _current = RewindColumns(_current);
                                _head = _current;
                                break;

                            }
                            else
                            {

                                while (temp._down != null && data.Position.Y > temp._data.Position.Y + temp.Height + MIN_SPACE)
                                {
                                    temp = temp._down;
                                }


                                if (data.Position.Y <= temp._data.Position.Y + temp.Height && temp._data.Item == null)
                                {

                                    //Node colBefore = temp.prev_column;
                                    //Node colAfter = temp.next_column;
                                    //Node rowBefore = temp.prev_row;
                                    //Node rowAfter = temp.next_row;

                                    _newNode._down = temp._down;
                                    _newNode._up = temp._up;
                                    _newNode._left = temp._left;
                                    _newNode._right = temp._right;

                                    if (temp._down != null)
                                    {
                                        temp._down._up = _newNode;
                                    }
                                    if (temp._up != null)
                                    {
                                        temp._up._down = _newNode;
                                    }
                                    if (temp._left != null)
                                    {
                                        temp._left._right = _newNode;
                                    }
                                    if (temp._right != null)
                                    {
                                        temp._right._left = _newNode;
                                    }


                                    if (temp == _head)
                                    {
                                        _head = _newNode;
                                    }


                                    break;
                                }
                                else
                                {
                                    _newNode._down = temp._down;
                                    _newNode._up = temp;

                                    if (temp._down != null)
                                    {
                                        temp._down._up = _newNode;
                                    }
                                    temp._down = _newNode;

                                    Node _current = temp;

                                    _current = RewindColumns(_current);
                                    _current = CreateRowBelow(_current, _newNode);
                                }

                                temp = _head;
                                Node column = temp;
                                while (column != null)
                                {

                                    temp = column;
                                    Node _colAfter = temp._right;

                                    while (temp != null && _colAfter != null)
                                    {

                                        temp._right = _colAfter;
                                        _colAfter._left = temp;

                                        temp = temp._down;
                                        _colAfter = _colAfter._down;
                                    }

                                    column = column._right;
                                }
                                break;
                            }
                        }
                    }
                    temp = temp._right;
                }

            }  
            FixToBounds();
            SetRowsColsDim();
            RearrangeList();

        }
        Node CreateDummyNode(Node current)
        {
            Slot<UIObject> _dummySlot = new Slot<UIObject>(Point.Zero, null, UIObject.DrawPriority.NORMAL);
            Node _dummyNode = new Node(_dummySlot);

            _dummyNode.Width = current.Width;
            _dummyNode.Height = current.Height;

            return _dummyNode;
        }

        Node CreateColumnAfter(Node current, Node newNode)
        {
            while (current != null)
            {
                if (current._right != newNode)
                {
                    Node dummyNode = CreateDummyNode(current);
                    dummyNode._right = current._right;
                    dummyNode._left = current;

                    current._right = dummyNode;

                }
                current = current._down;
            }
            return current;
        }
        Node CreateColumnBefore(Node current, Node newNode)
        {
            while (current != null)
            {
                if (current._left != newNode)
                {
                    Node dummyNode = CreateDummyNode(current);

                    dummyNode._right = current;
                    dummyNode._left = current._left;


                    if (current._left != null)
                    {
                        current._left._right = dummyNode;
                    }

                    current._left = dummyNode;

                }
                current = current._down;
            }
            return current;
        }
        Node CreateRowAbove(Node current, Node newNode)
        {
            while (current != null)
            {
                if (current._up != newNode)
                {

                    Node _dummyNode = CreateDummyNode(current);
                    _dummyNode.Width = current.Width;

                    _dummyNode._down = current;
                    _dummyNode._up = current._up;
                    current._up = _dummyNode;

                }
                current = current._right;
            }
            return current;
        }
        Node CreateRowBelow(Node current, Node newNode)
        {
            while (current != null)
            {

                if (current._down != newNode)
                {

                    Node _dummyNode = CreateDummyNode(current);
                    _dummyNode.Width = current.Width;

                    _dummyNode._up = current;
                    _dummyNode._down = current._down;

                    if (current._down != null)
                    {
                        current._down._up = _dummyNode;
                    }

                    current._down = _dummyNode;


                }
                current = current._right;
            }
            return current;
        }
        Node RewindRows(Node row)
        {
            while (row._up != null)
            {
                row = row._up;
            }
            return row;
        }
        Node RewindColumns(Node column)
        {
            while (column._left != null)
            {
                column = column._left;
            }
            return column;
        }

        void SetRowsColsDim()
        {
            Node temp = _head;
            while (temp != null)
            {

                Node tmp = temp;
                while (tmp != null)
                {
                    tmp.Height = GetYMax(tmp);
                    tmp.Width = GetXMax(tmp);

                    tmp = tmp._down;
                }
                temp = temp._right;
            }

        }
        void ResetSizes()
        {
            Node temp = _head;
            while (temp != null)
            {
                Node tmp = temp;
                while (tmp != null)
                {
                    if (tmp._data.Item != null)
                    {
                        tmp.Height = tmp._data.Item.DefaultSize.Y;
                        tmp.Width = tmp._data.Item.DefaultSize.X;
                        tmp._data.Item.ResetSize();
                    }

                    tmp = tmp._down;
                }
                temp = temp._right;
            }
        }
        void SetExpandPolicies()
        {
            Node temp = _head;
            while (temp != null)
            {
                Node tmp = temp;
                while (tmp != null)
                {
                    if (tmp._data.Item != null)
                    {
                       // tmp._data.Item.ResetSize();
                        if (tmp._data.Item.XPolicy == UIObject.SizePolicy.EXPAND)
                        {
                            int _delta = GetXMax(tmp) - tmp._data.Item.Width;

                            if (_delta > 0)
                            {  
                                tmp._data.Item.Resize(new Point(_delta, 0));
                            }

                        }
                        if (tmp._data.Item.YPolicy == UIObject.SizePolicy.EXPAND)
                        {
                            int _delta = GetYMax(tmp) - tmp._data.Item.Height;

                            if (_delta > 0)
                            {  
                                tmp._data.Item.Resize(new Point(0, _delta));
                            }
                        }

                    }
                    tmp = tmp._down;
                }
                temp = temp._right;
            }
        }
        public void FixToBounds()
        {            
            ResetSizes();

           
            int _totalXSize = GetColumnsTotalSize(GetWidestRow());
            while (_totalXSize > Rect.Size.X - MIN_SPACE * 2)
            {
                ResizeHorizontal();
                _totalXSize = GetColumnsTotalSize(GetWidestRow());
            }
         
            int totalYSize = GetRowsTotalSize(GetHeighestColumn());
            while (totalYSize > Rect.Size.Y - MIN_SPACE * 2)
            {
                ResizeVertical();
                totalYSize = GetRowsTotalSize(GetHeighestColumn());
            }
            
             SetExpandPolicies();
        }
        int GetCountX()
        {
            Node temp = _head;
            int _count = 0;
            while (temp != null)
            {
                temp = temp._right;
                _count++;
            }

            return _count;
        }
        int GetCountY()
        {
            Node temp = _head;
            int _count = 0;
            while (temp != null)
            {
                temp = temp._down;
                _count++;
            }

            return _count;
        }
        int GetColumnsTotalSize(Node current)
        {
            int totalWidth = 0;
            int spacesSum = 0;

            if (current != null)
            {
                Node temp = RewindColumns(current);

                while (temp != null)
                {
                    totalWidth += GetXMax(temp);
                    temp = temp._right;

                }

                spacesSum = MIN_SPACE * (GetCountX() + 1);
            }
            return totalWidth + spacesSum;
        }
        int GetRowsTotalSize(Node current)
        {
            int totalHeight = 0;
            int spacesSum = 0;

            if (current != null)
            {
                Node temp = RewindRows(current);

                while (temp != null)
                {
                    totalHeight += GetYMax(temp);
                    temp = temp._down;

                }

                spacesSum = MIN_SPACE * (GetCountY() + 1);
            }

            return totalHeight + spacesSum;
        }
        Node GetWidestRow()
        {
            Node temp = _head;
            Node _max = null;
            while (temp != null)
            {
                if (_max == null)
                {
                    _max = temp;
                }
                if (GetColumnsTotalSize(temp) >= GetColumnsTotalSize(_max))
                {
                    _max = temp;
                }
                temp = temp._down;
            }
            return _max;
        }
        Node GetHeighestColumn()
        {
            Node temp = _head;
            Node _max = null;
            while (temp != null)
            {
                if (_max == null)
                {
                    _max = temp;
                }
                if (GetRowsTotalSize(temp) >= GetRowsTotalSize(_max))
                {
                    _max = temp;
                }
                temp = temp._right;
            }
            return _max;
        }
        void ResizeHorizontal()
        {

            Node temp = _head;

            while (temp != null)
            {
              
                Node tmp = temp;
                while (tmp != null)
                {
                    int _newSizeX = GetXMax(tmp);
                    if (GetWidestRow().Width > tmp.Width)
                    {
                        float ratio = tmp.Width / (float)GetWidestRow().Width;
                        _newSizeX = (int)(tmp.Width * (ONE_PER * ratio));
                    }
                    else
                    {
                        _newSizeX = (int)Math.Ceiling(_newSizeX * ONE_PER);
                    }
                    if (tmp._data.Item != null)
                    {
                        //tmp._data.Item.ResetSize();   

                        if(_newSizeX > 0)
                        {
                            tmp._data.Item.Resize(new Point(-_newSizeX, 0));
                            tmp.Width = tmp._data.Item.Width;
                        }                       
                    }
                    else
                    {
                        if (_newSizeX > 0)
                        {
                            tmp.Width += -_newSizeX;
                        }
                    }

                    tmp = tmp._right;

                }
                temp = temp._down;
            }
        }
        void ResizeVertical()
        {
            Node temp = _head;

            while (temp != null)
            {
                
                Node tmp = temp;
                while (tmp != null)
                {

                    int _newSizeY = GetYMax(tmp);
                    if (GetHeighestColumn().Height > tmp.Height)
                    {
                        float ratio = tmp.Height / (float)GetHeighestColumn().Height;
                        _newSizeY = (int)(tmp.Height * (ONE_PER * ratio));
                    }
                    else
                    {
                        _newSizeY = (int)Math.Ceiling(_newSizeY * ONE_PER);
                    }
                    if (tmp._data.Item != null)
                    {
                        //tmp._data.Item.ResetSize();                    

                        if(_newSizeY > 0)
                        {
                            tmp._data.Item.Resize(new Point(0, -_newSizeY));
                            tmp.Height = tmp._data.Item.Height;
                        } 
                    }
                    else
                    {
                        if (_newSizeY > 0)
                        {
                            tmp.Height += -_newSizeY;
                        }
                    }
                    tmp = tmp._down;

                }
                temp = temp._right;
            }
        }
        void RearrangeList()
        {
            Node temp = _head;
            if (temp != null)
            {
                temp._data.Position = new Point(Rect.Width / 2 - temp.Width / 2, Rect.Height / 2 - temp.Height / 2);
                temp._data.Position = new Point(temp._data.Position.X - GetTotalXSizeExcept(temp) / 2, Rect.Height / 2 - temp.Height / 2);

                while (temp._right != null)
                {

                    temp._right._data.Position = new Point(temp._data.Position.X + temp.Width + MIN_SPACE, Rect.Height / 2 - temp.Height / 2);
                    temp = temp._right;
                }

            }

            temp = _head;
            while (temp != null)
            {
                Node tmp = temp;

                tmp = RewindRows(tmp);

                if (tmp._down != null)
                {
                    tmp._data.Position = new Point(tmp._data.Position.X, Rect.Height / 2 - tmp.Height / 2);

                    tmp._data.Position = new Point(tmp._data.Position.X, tmp._data.Position.Y - GetTotalYSizeExcept(tmp) / 2);

                    while (tmp._down != null)
                    {
                        tmp._down._data.Position = new Point(tmp._data.Position.X, tmp._data.Position.Y + tmp.Height + MIN_SPACE);
                        tmp = tmp._down;
                    }
                }
                temp = temp._right;
            }
        }
        public void UpdateLayout()
        {    
            FixToBounds();
            SetRowsColsDim();
            RearrangeList();
        }
        int GetTotalXSizeExcept(Node current)
        {
            Node temp = _head;
            int totalWidth = 0;
            int spacesSum = 0;
            while (temp != null)
            {
                if (temp != current)
                {
                    totalWidth += GetXMax(temp);
                }
                temp = temp._right;
            }

            spacesSum = MIN_SPACE * GetCountX();

            return totalWidth + spacesSum;
        }

        int GetTotalYSizeExcept(Node current)
        {
            Node temp = current;
            int totalHeight = 0;
            int spacesSum = 0;
            while (temp != null)
            {
                if (temp != current)
                {
                    totalHeight += GetYMax(temp);

                }
                temp = temp._down;
            }

            spacesSum = MIN_SPACE * GetCountY(current);

            return totalHeight + spacesSum;
        }
        int GetCountY(Node current)
        {
            Node temp = current;
            int _count = 0;
            while (temp != null)
            {
                temp = temp._down;
                _count++;
            }

            return _count;
        }
       
        public bool RowHasItems(Node row)
        {
            Node current = row;
            row = RewindColumns(row);
            while (row != null)
            {
                if (row != current && row._data.Item != null)
                {
                    return true;
                }
                row = row._right;
            }
            return false;
        }
        public bool ColumnHasItems(Node column)
        {
            Node current = column;
            column = RewindRows(column);
            while (column != null)
            {
                if (column != current && column._data.Item != null)
                {
                    return true;
                }
                column = column._down;
            }
            return false;
        }

        public void DeleteRow(Node row)
        {
            row = RewindColumns(row);
            while (row != null)
            {
                if (row._up != null)
                {
                    row._up._down = row._down;
                }
                if (row._down != null)
                {
                    row._down._up = row._up;
                }
                row = row._right;
            }
        }

        public void DeleteColumn(Node column)
        {
            column = RewindRows(column);
            while (column != null)
            {

                if (column._right != null)
                {
                    column._right._left = column._left;
                }
                if (column._left != null)
                {
                    column._left._right = column._right;
                }

                column = column._down;
            }
        }
        public void DeleteNodebyKey(Slot<UIObject> key)
        {
            Node temp = _head;

            if (MouseGUI.Focus == null)
            {
                key.Item.ResetSize();
            }
            if (temp != null && temp._data == key)
            {

                Node _dummyNode = CreateDummyNode(temp);
                _dummyNode._right = temp._right;
                _dummyNode._left = temp._left;
                _dummyNode._down = temp._down;
                _dummyNode._up = temp._up;
                _dummyNode.Width = GetXMaxExcept(temp);
                _dummyNode.Height = GetYMaxExcept(temp);
                _dummyNode._data.Position = temp._data.Position;

                if (temp._down != null)
                {
                    temp._down._up = _dummyNode;
                }
                if (temp._right != null)
                {
                    temp._right._left = _dummyNode;
                }

                _head = _dummyNode;

                Node _nextColumn = _head._right;
                Node _nextRow = _head._down;

                if (!RowHasItems(_head))
                {
                    DeleteRow(_head);
                    _head = _nextRow;
                }

                else if (!ColumnHasItems(_head))
                {
                    DeleteColumn(_head);
                    _head = _nextColumn;
                }

            }
            else
            {
                while (temp != null)
                {
                    Node tmp = temp;
                    while (tmp != null)
                    {
                        if (tmp._data == key)
                        {
                            Node _dummyNode = CreateDummyNode(tmp);
                            _dummyNode._left = tmp._left;
                            _dummyNode._right = tmp._right;
                            _dummyNode._up = tmp._up;
                            _dummyNode._down = tmp._down;
                            _dummyNode._data.Position = tmp._data.Position;

                            _dummyNode.Width = GetXMaxExcept(tmp);
                            _dummyNode.Height = GetYMaxExcept(tmp);

                            if (tmp._left != null)
                            {
                                tmp._left._right = _dummyNode;
                            }
                            if (tmp._right != null)
                            {
                                tmp._right._left = _dummyNode;
                            }
                            if (tmp._up != null)
                            {
                                tmp._up._down = _dummyNode;
                            }
                            if (tmp._down != null)
                            {
                                tmp._down._up = _dummyNode;
                            }

                            if (!RowHasItems(tmp))
                            {
                                Node _row = RewindColumns(tmp);

                                DeleteRow(tmp);
                                if (_row._up == null)
                                {
                                    _head = _row._down;
                                }

                            }

                            else if (!ColumnHasItems(tmp))
                            {
                                Node _col = RewindRows(tmp);
                                DeleteColumn(tmp);
                                if (_col._left == null)
                                {
                                    _head = _col._right;
                                }
                            }

                            break;

                        }
                        tmp = tmp._down;
                    }
                    temp = temp._right;
                }
            }


        }

    }
}
