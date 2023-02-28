using DataLayer;
using DataLayer.Interfaces;
using EntityLayer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Enum = EntityLayer.Enum;

namespace BusinessLayer
{
    public class CreateXML : IXML
    {

        XmlDocument doc = new XmlDocument();
        Log log = new Log();
        XMLProcess xml = new XMLProcess();
        List<XMLTemplate> _templates = new List<XMLTemplate>();
        List<string> fieldList = new List<string>();
        string URL = Resource.URL;
        string prefix = Resource.prefix;
        public void CreationXML(string[] fields, List<XMLTemplate> templates)
        {
            try
            {
                _templates = templates;

                fieldList.AddRange(fields);
                var doc = new XmlDocument();
                doc.AppendChild(doc.CreateXmlDeclaration("1.0", "utf-8", null));
                List<string> ok = templates.Select(x => x.Element).Distinct().ToList();

                #region Root Node
                Dictionary<string, string> att = xml.GetXMLAttributes(templates.Select(x => x.IdMapClient).Distinct().FirstOrDefault());

                string rootSection = templates.Select(x => x.Section).FirstOrDefault();
                var rootnode = doc.CreateElement(prefix, rootSection, URL);

                doc.AppendChild(rootnode);

                foreach (KeyValuePair<string, string> entry in att)
                {
                    rootnode.SetAttribute(entry.Key, entry.Value);
                }
                CreateChildsNoParents(doc, doc.SelectSingleNode("/*").Name, rootnode);
                #endregion

                List<string> ParentNode = _templates.Where(x => x.ParentElement == doc.SelectSingleNode("/*").Name).Select(x => x.Element).Distinct().ToList();
                List<string> ChildNode = _templates.Select(x => x.Element).ToList();

                for (int x = 0; x < ParentNode.Count(); x++)
                {
                    XmlNode parent = null;

                    IEnumerable<XMLTemplate> enumerable = _templates.Where(q => q.ParentElement == ParentNode[x]).ToList();
                    if (enumerable.Count() > 0)
                    {

                        parent = CreateNodes(doc, ParentNode[x], false);

                    }
                    else
                    {
                        parent = doc.CreateNode(XmlNodeType.Element, ParentNode[x].ToString(), URL);
                        CreateChildsNoParents(doc, ParentNode[x].ToString(), parent);
                    }
                    if (parent != null)
                    {
                        rootnode.AppendChild(parent);
                    }


                }

                SaveDoc(doc);
            }
            catch (Exception e)
            {
                log.WriteLog(e, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString());
            }
        }
        private void SaveDoc(XmlDocument doc)
        {
            string destLocalPath = ConfigurationManager.AppSettings["LocalPathXML"];
            Directory.CreateDirectory(destLocalPath);
            doc.Save(Path.Combine(destLocalPath, "xml") + Guid.NewGuid().ToString() + ".xml");
        }
        private XmlNode CreateNodes(XmlDocument doc, string ParentNode, bool IsChilParent)
        {
            XmlNode parent = null;
            List<XMLTemplate> values = _templates.Where(q => q.ParentElement == ParentNode && q.Column != null).ToList();
            if (values.Count() > 0)
            {
                foreach (XMLTemplate ch in values)
                {
                    if (!IsChilParent)
                    {
                        parent = doc.CreateNode(XmlNodeType.Element, ParentNode, URL);
                        XmlNode child = doc.CreateNode(XmlNodeType.Element, ch.Element, URL);

                        CreateChildsNoParents(doc, ch.Element, child);

                        parent.AppendChild(child);

                        XmlNode parentNode = CreateNodes(doc, ch.Element, true);
                        if (parentNode != null)
                        {
                            parent.AppendChild(parentNode);
                        }
                    }
                    else
                    {
                        parent = doc.CreateNode(XmlNodeType.Element, ch.Element, URL);

                        CreateChildsNoParents(doc, ch.Element, parent);
                        XmlNode parentNode = CreateNodes(doc, ch.Element, true);
                        if (parentNode != null)
                        {
                            parent.AppendChild(parentNode);
                        }
                    }
                }

            }
            return parent;

        }

        private void CreateChildsNoParents(XmlDocument doc, string list, XmlNode parent)
        {
            List<XMLTemplate> values = _templates.Where(x => x.Element == list && x.IdType == null).ToList();
            List<string> fieldsNodes;
            //Create attributes in Node
            if (values.Count > 0)
            {
                foreach (XMLTemplate pItem in values)
                {
                    if (pItem.Column != null)
                    {
                        var perAux = fieldList.FindIndex(x => x.Contains(pItem.Section));
                        if (fieldList[perAux + 1].ToString().Contains('|'))
                        {
                            fieldsNodes = fieldList[perAux + 1].ToString().Split('|').ToList();
                        }
                        else
                        {
                            fieldsNodes = fieldList[perAux + 2].ToString().Split('|').ToList();
                        }

                        XmlAttribute attribute = doc.CreateAttribute(pItem.Attribute);
                        attribute.Value = (pItem.FillWith != null || Convert.ToInt32(pItem.Column) < 0 ? pItem.FillWith : fieldsNodes[Convert.ToInt32(pItem.Column) - 1].ToString());
                        parent.Attributes.Append(attribute);
                    }

                }
            }

        }

    }
}


