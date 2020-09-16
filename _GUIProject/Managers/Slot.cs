using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using static _GUIProject.UI.UIObject;

namespace _GUIProject.UI
{
    public class Slot<T> : IXmlSerializable, IComparable
    {
        [XmlIgnore]
        public Position Position { get; set; }
        
        public T Item { get; set; }
        [XmlIgnore]
        public long Index { get; private set; }
        [XmlIgnore]
        public static long GlobalIndex { get; private set; } = 0;

        [XmlIgnore]
        public DrawPriority Priority { get; set; }
        public Slot()
        {
            
        }
        public Slot(Point position, T item, DrawPriority priority)
        {
            Item = item;
            Position = position;
            Priority = priority;
           
            Index = GlobalIndex;
            GlobalIndex++;
        }

        public XmlSchema GetSchema()
        {
            return null;
        }
        public void ReadXml(XmlReader reader)
        {            
            reader.MoveToAttribute("Position");
            string data = reader.Value;            

            var split = data.Split(':');
            Position = new Point(int.Parse(split[0]), int.Parse(split[1]));
            reader.MoveToElement();         

            if (reader.Read())
            {
                if (reader.IsStartElement())
                {
                  
                    XmlRootAttribute root = new XmlRootAttribute()
                    {
                        ElementName = reader.Name,
                        IsNullable = true
                    };
                    var obj = Reflection.CreateObject(reader.Name);
                    Item = (T)XmlTool.Deserialize(obj.GetType(), reader, root);
                    (Item as UIObject).Initialize();
                    (Item as UIObject).Setup();                  
                    reader.ReadEndElement();
                }
             
               
            }
            
        }

        public void WriteXml(XmlWriter writer)
        {           
            Position.WriteXml(writer);

            XmlTool.Serialize(Item, writer, XmlTool.AddException(typeof(Color), "PackedValue"));

        }
        public int CompareTo(object other)
        {
            int result = Priority.CompareTo((other as Slot<UIObject>).Priority);

            if (result == 0)
            {
                result = (Item as UIObject).Priority.CompareTo((other as Slot<UIObject>).Item.Priority);                    
            }
            if (result == 0)
            {
                result = Index.CompareTo((other as Slot<UIObject>).Index);
            }
            // This should never be true, but its there anyways
            if (result == 0)
            {
                result = GetHashCode().CompareTo(other.GetHashCode());
            }

            return result;
        }

        
    }
}
