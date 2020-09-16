using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace _GUIProject
{
    public static class XmlTool
    {
        static XmlSerializer serializer;
        static private FileStream OpenFile(string path)
        {
            return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);          
        }
        static public TextReader OpenTextReader(string path)
        {
            return new StreamReader(OpenFile(path), Encoding.UTF8);
        }
        static public XmlReader OpenXmlReader(string path)
        {
            return XmlReader.Create(OpenFile(path), AddReaderSettings());
        }
        static public XmlWriter OpenXmlWriter(string path)
        {
            return XmlWriter.Create(path, AddWriterSettings());
        }
        static public XmlWriterSettings AddWriterSettings()
        {
            var settings = new XmlWriterSettings();
            settings.Indent = true;         
            settings.WriteEndDocumentOnClose = true;
            settings.OmitXmlDeclaration = false;
            settings.NamespaceHandling = NamespaceHandling.OmitDuplicates;

            return settings;
        }
        static public XmlReaderSettings AddReaderSettings()
        {
            var settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;

            return settings;
        }
        static public XmlAttributeOverrides AddException(Type type, string attr)
        {
            XmlAttributeOverrides overrides = new XmlAttributeOverrides();
            XmlAttributes attribs = new XmlAttributes();
            attribs.XmlIgnore = true;

            attribs.XmlElements.Add(new XmlElementAttribute(attr));
            overrides.Add(type, attr, attribs);
            return overrides;
        }
        static public void Serialize(object obj, XmlWriter writer, XmlAttributeOverrides overrides)
        {
            serializer = new XmlSerializer(obj.GetType(), overrides);
            serializer.Serialize(writer, obj, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
        }
        static public object Deserialize(Type type, XmlReader reader, XmlRootAttribute root = null )
        {
            serializer = new XmlSerializer(type);
            return serializer.Deserialize(reader);
        }
            
    }
}
