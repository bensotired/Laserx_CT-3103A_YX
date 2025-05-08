using MessagePack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace LX_BurnInSolution.Utilities
{
    public static class XmlHelper
    {
        static object syncObj = new object();
        public static XElement GetXElement(this XmlNode node)
        {
            XDocument xDoc = new XDocument();
            using (XmlWriter xmlWriter = xDoc.CreateWriter())
                node.WriteTo(xmlWriter);
            return xDoc.Root;
        }
        public static XmlNode GetXmlNode(this XElement element)
        {
            using (XmlReader xmlReader = element.CreateReader())
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlReader);
                return xmlDoc;
            }
        }
        public static XElement LoadXmlFile(string fileName)
        {
            XElement xmlElement = null;
            int tryCount = 0;
            while (true)
            {
                try
                {
                    xmlElement = XElement.Load(fileName);

                    break;
                }
                catch (Exception ex)
                {
                    if (tryCount > 5)
                    {
                        throw new Exception("Unable to read the xml file: " + fileName, ex);
                    }
                    Thread.Sleep(500);
                    tryCount++;
                }
            }
            return xmlElement;
        }

        public static XAttribute GetAttribute(this XElement element, string attributeName)
        {
            XAttribute attribute = element.Attribute(attributeName);
            if (attribute == null)
            {
                throw new Exception(string.Format("Attribute({0}) can not be found in element({1}).", attributeName, element.ToString()));
            }
            return attribute;
        }

        public static XElement GetElement(this XElement element, string elementName)
        {
            XElement subElement = element.Element(elementName);
            if (subElement == null)
            {
                throw new Exception(string.Format("Element({0}) can not be found in element({1}).", elementName, element.Name));
            }
            return subElement;
        }
        public static object DeserializeXElement(XElement element, Type objectType)
        {
            object result;
            try
            {
                XmlSerializer xe = new XmlSerializer(objectType);
                XmlReader xr = element.CreateReader();
                result = xe.Deserialize(xr);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Can not deserialize XElement to object[{0}]", objectType.FullName), ex);
            }
            return result;
        }
        public static TObject DeserializeXElement<TObject>(XElement element)
        {
            return (TObject)DeserializeXElement(element, typeof(TObject));
        }
        public static TObject DeserializeFile<TObject>(string fileName)
        {
            try
            {
                XmlSerializer se = new XmlSerializer(typeof(TObject));
                using (StreamReader reader = new StreamReader(fileName ))
                {
                    return (TObject)se.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                throw ex;
                return default(TObject);
            }
        }
        public static object DeserializeFile(string fileName, Type objType)
        {
            try
            {
                XmlSerializer se = new XmlSerializer(objType);
                using (StreamReader reader = new StreamReader(fileName ))
                {
                    return se.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static object DeserializeFile(string fileName, Type objType,Type[] extraTypeArray)
        {
            try
            {
                XmlSerializer se = new XmlSerializer(objType, extraTypeArray);
                using (StreamReader reader = new StreamReader(fileName,System.Text.Encoding.UTF8))
                {
                    return se.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void SerializeFile<TObject>(string path, object dataObject , Type[] extraTypeArray)
        {
            try
            {

                XmlSerializer se = new XmlSerializer(typeof(TObject), extraTypeArray);
                using (StreamWriter writer = new StreamWriter(path))
                {
                    se.Serialize(writer, dataObject);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SerializeFile<TObject>(string path, object dataObject)
        {
            try
            {
                XmlSerializer se = new XmlSerializer(typeof(TObject));
                using (StreamWriter writer = new StreamWriter(path, false, System.Text.Encoding.UTF8))
                {
                    se.Serialize(writer, dataObject);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void SerializeFile(string path, object dataObject, Type[] extraTypeArray)
        {
            try
            {

                XmlSerializer se = new XmlSerializer(dataObject.GetType(), extraTypeArray);
                using (StreamWriter writer = new StreamWriter(path ,false, System.Text.Encoding.UTF8))
                {
                    se.Serialize(writer, dataObject);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void SerializeFile (string path, object dataObject)
        {
            try
            {
  
                XmlSerializer se = new XmlSerializer(dataObject.GetType() );
                using (StreamWriter writer = new StreamWriter(path, false, System.Text.Encoding.UTF8))
                {
                    se.Serialize(writer, dataObject);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SerializeFile<TObject>(string path, object dataObject, XmlAttributeOverrides attributeOverrides)
        {
            try
            {
                XmlSerializer se = new XmlSerializer(typeof(TObject), attributeOverrides);
                using (StreamWriter writer = new StreamWriter(path, false, System.Text.Encoding.UTF8))
                {
                    se.Serialize(writer, dataObject);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Create Ignore propties flag by dictionary, true = ignore, false = visible
        /// </summary>
        /// <typeparam name="TAttSource"></typeparam>
        /// <param name="attDict">Prop dictionary, 
        /// if item.value = true,
        /// item.key as prop will be ignore when save to xml file, 
        /// otherwise if item.value =false, 
        /// it.key as prop will be save as usual </param>
        /// <returns></returns>
        public static XmlAttributeOverrides CreateAttributeOverrides<TAttSource>(Dictionary<string, bool> attDict)
        {
            XmlAttributeOverrides overr = new XmlAttributeOverrides();
            Type attSourT = typeof(TAttSource);
            var props = attSourT.GetProperties();
            foreach (var item in attDict)
            {
                bool isPropFound = false;
                foreach (var prop in props)
                {
                    if (prop.Name == item.Key)
                    {
                        isPropFound = true;
                        break;
                    }
                }
                if (isPropFound)
                {
                    XmlAttributes newAtt = new XmlAttributes();
                    newAtt.XmlIgnore = item.Value;
                    overr.Add(attSourT,item.Key ,newAtt);
                }
            }
            return overr;
        }
        public static void AddAttributeOverrides<TAttSource>(ref XmlAttributeOverrides orgAtt, Dictionary<string, bool> attDict)
        {
            //XmlAttributeOverrides overr = new XmlAttributeOverrides();
            Type attSourT = typeof(TAttSource);
            var props = attSourT.GetProperties();
            foreach (var item in attDict)
            {
                bool isPropFound = false;
                foreach (var prop in props)
                {
                    if (prop.Name == item.Key)
                    {
                        isPropFound = true;
                        break;
                    }
                }
                if (isPropFound)
                {
                    XmlAttributes newAtt = new XmlAttributes();
                    newAtt.XmlIgnore = item.Value;
                    orgAtt.Add(attSourT, item.Key, newAtt);
                }
            }
        }
        public static XElement SerializeXElement(object dataObject)
        {
            try
            {
                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add("", "");
            
                XmlSerializer serializer = new XmlSerializer(dataObject.GetType());
                XElement element;
                using (MemoryStream ms = new MemoryStream())
                {
                    serializer.Serialize(ms, dataObject, namespaces);
                    /*after serialization the stream is at the end of stream flow, 
                    we have to make it back to the start of flow.
                    */
                    ms.Seek(0, SeekOrigin.Begin);
                    using (StreamReader sr = new StreamReader(ms))
                    {
                        element = XElement.Load(sr);
                    }
                }
                return element;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
