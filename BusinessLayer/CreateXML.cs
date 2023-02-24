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
        string URL = "http://www.sat.gob.mx/cfd/3";
        public void CreationXML(string[] fields, List<XMLTemplate> templates)
        {
            try
            {
                _templates = templates;

                fieldList.AddRange(fields);
                var doc = new XmlDocument();
                doc.AppendChild(doc.CreateXmlDeclaration("1.0", "utf-8", null));
                List<string> ok = templates.Select(x => x.Element).Distinct().ToList();
                Dictionary<string, string> att = xml.GetXMLAttributes(templates.Select(x => x.IdMapClient).Distinct().FirstOrDefault());

                XmlElement rootnode = doc.CreateElement(ok.First(), null);
                doc.AppendChild(rootnode);

                foreach (KeyValuePair<string, string> entry in att)
                {
                    rootnode.SetAttribute(entry.Key, entry.Value);
                }
                foreach (string pItem in ok)
                {

                    XmlNode parent = CreateNODES(doc, pItem);
                    rootnode.AppendChild(parent);

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

            XmlNode parent = doc.CreateNode(XmlNodeType.Element, list, URL);

            XmlNodeList elemList = doc.GetElementsByTagName(list);
            if (elemList.Count > 0)
            {
                CreateChildsNuevo(doc, list, elemList[0]);
            }

            CreateChildsNuevo(doc, list, parent);
            return parent;

        }
        private void CreateChilds(XmlDocument doc, string list, XmlNode parent)
        {

            List<XMLTemplate> ok = _templates.Where(x => x.Element == list).ToList();

            foreach (XMLTemplate pItem in ok)
            {
                var perAux = fieldList.FindIndex(x => x.Contains(pItem.Section));
                List<string> fieldsNodes = fieldList[perAux + 1].ToString().Split('|').ToList();

                if (pItem.IdType != (int)Enum.Type.Items)
                {
                    if (pItem.Column != null)
                    {

                        XmlAttribute attribute = doc.CreateAttribute(pItem.Attribute);
                        attribute.Value = (pItem.FillWith != null || Convert.ToInt32(pItem.Column) < 0 ? pItem.FillWith : fieldsNodes[Convert.ToInt32(pItem.Column) - 1].ToString());
                        parent.Attributes.Append(attribute);
                    }
                    else
                    {
                        if (pItem.ParentElement != "root")
                        {
                            XmlNodeList dataNodes = doc.GetElementsByTagName(list);
                            if (dataNodes.Count == 0)
                            {
                                XmlNode newElem = doc.CreateElement("event");
                                XmlAttribute newAttr = doc.CreateAttribute("type");
                                newAttr.Value = "VIZ";
                                newElem.Attributes.Append(newAttr);
                                doc.CreateNode(XmlNodeType.Element, "Event", null);
                            }
                            //;


                            //    XmlAttribute attribute = doc.CreateAttribute(pItem.Attribute);
                            //    attribute.Value = "Attributo";

                            //    parent.Attributes.Append(attribute);
                            //}

                            //else
                            //{
                            //    XmlNode newNode = doc.CreateNode(XmlNodeType.Element, "Database", "");
                            //    parent.AppendChild(newNode);
                            //}
                            //XmlElement newparent = doc.CreateElement(pItem.Element);
                            //doc.AppendChild(newparent);
                            //XmlAttribute attribute = doc.CreateAttribute(pItem.Attribute);
                            //attribute.Value = "Hijo del hijo";

                            //newparent.Attributes.Append(attribute);

                        }
                        //parent = CreateNODES(doc, pItem.Element);
                        //doc.AppendChild(parent);
                    }

                }
                else
                {
                    List<XMLTemplate> newParent = _templates.Where(x => x.Element == list).ToList();
                    List<string> NewfieldsNodes = fieldList[perAux + 2].ToString().Split('|').ToList();
                    foreach (XMLTemplate NewpItem in newParent)
                    {
                        if (NewpItem.Column != null)
                        {

                            XmlAttribute attribute = doc.CreateAttribute(NewpItem.Attribute);
                            attribute.Value = (NewpItem.FillWith != null || Convert.ToInt32(NewpItem.Column) < 0 ? NewpItem.FillWith : NewfieldsNodes[Convert.ToInt32(NewpItem.Column) - 1].ToString());
                            parent.Attributes.Append(attribute);
                        }
                        else
                        {
                            if (NewpItem.ParentElement != "root")
                            {
                                XmlNodeList dataNodes = doc.GetElementsByTagName(list);
                                if (dataNodes.Count == 0)
                                {
                                    XmlNode newElem = doc.CreateElement("event");
                                    XmlAttribute newAttr = doc.CreateAttribute("type");
                                    newAttr.Value = "VIZ";
                                    newElem.Attributes.Append(newAttr);
                                    doc.CreateNode(XmlNodeType.Element, "Event", null);
                                }
                            }

                        }
                    }
                }
            }


        }
        private void CreateChildsItems(XmlDocument doc, string section, List<string> fieldsItem, XMLTemplate pItem)
        {

            List<XMLTemplate> ok = _templates.Where(x => x.Section == section && x.Column != null).ToList();
            XmlElement id = doc.CreateElement(pItem.Attribute);
            id.SetAttribute(pItem.Attribute, (pItem.FillWith != null || Convert.ToInt32(pItem.Column) < 0 ? pItem.FillWith : fieldsItem[Convert.ToInt32(pItem.Column) - 1].ToString()));
            id.SetAttribute("passWord", "Tushar");

            XmlElement childOne = doc.CreateElement(pItem.Attribute);
            childOne.InnerText = "This is the first child";
            id.AppendChild(childOne);
        }



        private void CreateChildsNuevo(XmlDocument doc, string list, XmlNode parent)
        {

            List<XMLTemplate> ok = _templates.Where(x => x.Element == list && x.IdType == null).ToList();
            var xmlAttributes = _templates.Select(x => x.Element == list).ToList();
            List<string> fieldsNodes;
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
                    parent.Attributes.Append(attribute);
                }



            }
        }


    }
}


