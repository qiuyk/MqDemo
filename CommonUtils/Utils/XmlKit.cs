using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text.RegularExpressions;

namespace MqSdk.Utils
{
    public class XmlKit
    {
        /// <summary>
        /// XMl序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string XMLSerializer<T>(T obj)
        {
            MemoryStream stream = new MemoryStream();

            XmlSerializer xs = new XmlSerializer(typeof(T));

            XmlSerializerNamespaces xmlNameSpace = new XmlSerializerNamespaces();

            xmlNameSpace.Add("", "");

            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.OmitXmlDeclaration = true;
            
            xmlWriterSettings.Encoding = new UTF8Encoding(false);

            xmlWriterSettings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(stream, xmlWriterSettings))
            {
                xs.Serialize(writer, obj , xmlNameSpace);
            }

            string xml = Encoding.UTF8.GetString(stream.ToArray());

            stream.Close();
            stream.Dispose();

            xml = HandleEmptyNode(xml);

            return xml;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T XMLDeSerializer<T>(string xml)
        {
            T obj = default(T);

            XmlSerializer xs = new XmlSerializer(typeof(T));

            using (StringReader sr = new StringReader(xml))
            {
                obj = (T)xs.Deserialize(sr);
            }

            return obj;
        }

        /// <summary>
        /// 序列化后空单节点转全空节点
        /// </summary>
        /// <param name="xml"></param>
        public static string HandleEmptyNode(string xml)
        {

            MatchCollection matches = Regex.Matches(xml, "<.+?/>");

            if (matches.Count == 0)
            {
                return xml;
            }

            List<string> matchList = new List<string>();
            StringBuilder sb = new StringBuilder(xml);

            foreach (Match match in matches)
            {
                if (matchList.Contains(match.Value))
                {
                    continue;
                }
                else
                {
                    matchList.Add(match.Value);

                    string name = match.Value.Substring(1, match.Value.Length - 3).Trim();

                    sb = sb.Replace(match.Value, "<" + name + "></" + name + ">");

                }
            }

            xml = sb.ToString();

            matchList.Clear();
            matchList = null;
            sb = null;

            return xml;
        }

        /// <summary>
        /// XML标准化
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static string XMLStandardization(string xml)
        {
            XmlDocument xmlDoc = new XmlDocument();

            MemoryStream stream = new MemoryStream();

            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();

            xmlWriterSettings.Encoding = new UTF8Encoding(false);

            xmlWriterSettings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(stream, xmlWriterSettings))
            {
                xmlDoc.LoadXml(xml);

                xmlDoc.WriteTo(writer);
            }

            xml = Encoding.UTF8.GetString(stream.ToArray());

            stream.Close();
            stream.Dispose();

            xml = HandleEmptyNode(xml);

            return xml;

        }

        /// <summary>
        /// 取出XML节点
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public static string RemoveNode(string xml, string nodeName)
        {
            MatchCollection matches = Regex.Matches(xml, "<" + nodeName + ">.+?</" + nodeName + ">");

            foreach (Match match in matches)
            {
                xml = xml.Replace(match.Value, "");
            }

            return xml;
        }

        /// <summary>    
        /// 读取xml中的指定节点的值  
        /// </summary>    
        public static string ReadXmlNode(string filename,string nodeName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(filename);
                //读取Activity节点下的数据。SelectSingleNode匹配第一个Activity节点  
                XmlNode root = xmlDoc.SelectSingleNode("//" + nodeName);//当节点Workflow带有属性是，使用SelectSingleNode无法读取          
                if (root != null)
                {
                    return root.InnerXml;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        } 
        public static T XMLDeSerializer2Object<T>(string filename)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,filename);
            string confXml = File.ReadAllText(path);
            T obj;
            try
            {
                obj = XmlKit.XMLDeSerializer<T>(confXml);
            }
            catch (Exception e)
            {
                throw e;
            }
            return obj;
        }
    }
}
