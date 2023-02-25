using DataLayer;
using DataLayer.Interfaces;
using EntityLayer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        const string URL = "http://www.sat.gob.mx/cfd/3";
        const string prefix = "cfdi";
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

                #endregion

                foreach (string pItem in ok)
                {
                    //Don't repeat root node
                    string rootName = doc.SelectSingleNode("/*").Name;
                    if (rootName != pItem)
                    {
                        //Create                      
                        XmlNode parent = CreateNODES(doc, pItem);
                        rootnode.AppendChild(parent);

                    }

                }

                doc.Save(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDoc‌​uments), "xml") + Guid.NewGuid().ToString() + ".xml");
            }
            catch (Exception e)
            {
                log.WriteLog(e, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString());
            }
        }

        private string AddNodes(string section, List<string> values, int index)
        {
            string nodo = string.Empty;

            List<string> fields = values[index + 1].ToString().Split('|').ToList();
            List<XMLTemplate> nodes = xml.GetElementsBySection(section);

            foreach (XMLTemplate node in nodes)
            {
                if (node.IdType == (int?)Enum.Type.Items)
                {
                    List<string> fieldsItem = values[index + 2].ToString().Split('|').ToList();
                    nodo += node.Attribute + "= \"" + (node.Column != null ? fieldsItem[Convert.ToInt32(node.Column) - 1].ToString() : "") + "\"";

                }
                else
                {
                    if (node.Column != null && fields.Count > 1)
                    {
                        nodo += node.Attribute + "= \"" + (node.FillWith != null || Convert.ToInt32(node.Column) < 0 ? node.FillWith : fields[Convert.ToInt32(node.Column) - 1].ToString()) + "\"";
                    }
                }
            }
            return nodo;
        }

        private XmlNode CreateNODES(XmlDocument doc, string list)
        {
            //Node without childs
            XmlNode parent = doc.CreateNode(XmlNodeType.Element, list, URL);

            //Create Childs to Node 
            CreateChildsNuevo(doc, list, parent);
            return parent;

        }
        private void CreateChilds(XmlDocument doc, XMLTemplate list, XmlNode parent)
        {
            XMLTemplate pItem = _templates.Where(x => x.ParentElement == list.Element && x.Column == null).FirstOrDefault();

            if (pItem != null)
            {
                XmlNode xmlElement = doc.CreateNode(XmlNodeType.Element, pItem.Element, URL);
                parent.AppendChild(xmlElement);
                CreateChildsNuevo(doc, pItem.Element, xmlElement);
            }
        }
        private void CreateChildsNuevo(XmlDocument doc, string list, XmlNode parent)
        {
            List<XMLTemplate> ok = _templates.Where(x => x.Element == list && x.IdType == null).ToList();
            List<string> fieldsNodes;
            //Create attributes in Node
            if (ok.Count > 0)
            {
                foreach (XMLTemplate pItem in ok)
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
                        parent.Attributes.Append(attribute); ;
                    }

                }
            }
            else
            {
                XMLTemplate temp = _templates.Where(x => x.Element == list && x.Row == null).FirstOrDefault();
                XmlNode xmlElement = doc.CreateNode(XmlNodeType.Element, temp.Element, URL);
                parent.AppendChild(xmlElement);
                CreateChilds(doc, temp, xmlElement);
            }

        }
       
    }
}


