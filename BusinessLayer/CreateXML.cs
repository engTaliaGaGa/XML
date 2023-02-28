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
        Log log = new Log();
        XMLProcess xml = new XMLProcess();
        List<XMLTemplate> _templates = new List<XMLTemplate>();
        List<string> fieldList = new List<string>();
        string URL = Resource.URL;
        string prefix = Resource.prefix;
        bool prosecuted;

        /// <summary>
        /// Creates the XML structure
        /// </summary>
        /// <param name="fields">Fields from TXT file</param>
        /// <param name="templates">Template from DataBase table</param>
        /// <returns></returns>
        public bool CreationXML(string[] fields, List<XMLTemplate> templates)
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
                prosecuted = true;
            }
            catch (Exception e)
            {
                prosecuted = false;
                log.WriteLog(e.Message, e.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString());
            }
            return prosecuted;
        }

        private void SaveDoc(XmlDocument doc)
        {
            string destLocalPath = ConfigurationManager.AppSettings["LocalPathXML"];
            Directory.CreateDirectory(destLocalPath);
            doc.Save(Path.Combine(destLocalPath, "xml") + Guid.NewGuid().ToString() + ".xml");
        }
        /// <summary>
        /// Creates the XmlNode and iterates for get and creates the children
        /// </summary>
        /// <param name="doc">XmlDocument</param>
        /// <param name="ParentNode">Parent name</param>
        /// <param name="IsChilParent">Is Child to root Element?</param>
        /// <returns></returns>
        private XmlNode CreateNodes(XmlDocument doc, string ParentNode, bool IsChilParent)
        {
            XmlNode parent = null;
            List<XMLTemplate> valuesItems = _templates.Where(q => q.ParentElement == ParentNode && q.IdType == (int)Enum.Type.Items).ToList();
            //Section for IdType 2 (items) Elements 
            //Iterate many times elements exist
            if (valuesItems.Count > 0)
            {
                foreach (XMLTemplate ch in valuesItems)
                {
                    var next = fieldList.Where(q => q.Contains("[") == true).Select(q => q).ToList();
                    var indexItem = next.FindIndex(x => x == ("[" + ch.Section + "]"));
                    var NextParent = next[(int)indexItem + 1].ToString();
                    var indexInitial = fieldList.FindIndex(x => x.Contains("[" + ch.Section + "]"));
                    var indexFinal = fieldList.FindIndex(x => x.Contains(NextParent));

                    List<XMLTemplate> ok = _templates.Where(x => x.Element == ch.Element).ToList();
                    parent = doc.CreateNode(XmlNodeType.Element, ParentNode, URL);
                    for (int x = indexInitial; x < indexFinal - 1; x++)
                    {
                        XmlNode child = doc.CreateNode(XmlNodeType.Element, ch.Element, URL);

                        CreateChildsItems(doc, ch.Element, child, x);

                        parent.AppendChild(child);

                        XmlNode parentNode = CreateNodes(doc, ch.Element, true);
                        if (parentNode != null)
                        {
                            parent.AppendChild(parentNode);
                        }
                    }
                }

            }
            //Element with only 1 child
            //Find children and create Append it
            else
            {
                List<XMLTemplate> values = _templates.Where(q => q.ParentElement == ParentNode && q.Column != null).ToList();
                if (values.Count() > 0)
                {
                    foreach (XMLTemplate ch in values)
                    {
                        if (ch.Column != null)
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

                }
            }
            return parent;

        }
        /// <summary>
        /// Creates  the attributes for the XMLNode with child or children in the current Node
        /// </summary>
        /// <param name="doc">XML Document to add</param>
        /// <param name="element">Parent Element Name</param>
        /// <param name="parent">Parent XmlNode</param>
        private void CreateChildsNoParents(XmlDocument doc, string element, XmlNode parent)
        {
            List<XMLTemplate> values = _templates.Where(x => x.Element == element && x.IdType == null).ToList();
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
        /// <summary>
        ///Creates  the attributes for the XMLNode with child or children in the current Node with the number of rows in Fields exist
        /// </summary>
        /// <param name="doc">XML Document to add</param>
        /// <param name="element">Parent Element Name</param>
        /// <param name="parent">Parent XmlNode</param>
        /// <param name="index">Index array in TXT for the element</param>
        private void CreateChildsItems(XmlDocument doc, string element, XmlNode parent, int index)
        {
            List<XMLTemplate> values = _templates.Where(x => x.Element == element && x.IdType == null).ToList();
            List<string> fieldsNodes;
            //Create attributes in Node
            if (values.Count > 0)
            {
                foreach (XMLTemplate pItem in values)
                {

                    fieldsNodes = fieldList[index + 1].ToString().Split('|').ToList();


                    XmlAttribute attribute = doc.CreateAttribute(pItem.Attribute);
                    attribute.Value = (pItem.FillWith != null || Convert.ToInt32(pItem.Column) < 0 ? pItem.FillWith : fieldsNodes[Convert.ToInt32(pItem.Column) - 1].ToString());
                    parent.Attributes.Append(attribute);
                }

            }
        }

    }
}



