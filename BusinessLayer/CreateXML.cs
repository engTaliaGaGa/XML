using DataLayer;
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
    public class CreateXML
    {
        XmlDocument doc = new XmlDocument();
        List<XMLTemplate> _templates = new List<XMLTemplate>();
        List<string> fieldList = new List<string>();
        string ns = "xmlns:cfdi";
        string URL = "http://www.sat.gob.mx/cfd/3";
        public void CreationXML(string[] fields, List<XMLTemplate> templates)
        {
            _templates = templates;

            fieldList.AddRange(fields);
            var doc = new XmlDocument();
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "utf-8", null));
            List<string> ok = templates.Select(x => x.Element).Distinct().ToList();

            XmlElement rootnode = doc.CreateElement("cfdi:Comprobante", null);
            doc.AppendChild(rootnode);

            rootnode.SetAttribute("xmlns:cfdi", "http://www.sat.gob.mx/cfd/3");
            rootnode.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            rootnode.SetAttribute("xsi:schemaLocation", "http://www.sat.gob.mx/cfd/4 http://www.sat.gob.mx/sitio_internet/cfd/4/cfdv40.xsd http://www.sat.gob.mx/sitio_internet/cfd/leyendasFiscales/leyendasFisc.xsd");

            foreach (string pItem in ok)
            {
                XmlNode parent = CreateNODES(doc, pItem);
                rootnode.AppendChild(parent);
            }
                
            doc.Save(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDoc‌​uments), "xml") + Guid.NewGuid().ToString() + ".xml");
        }

        private string AddNodes(string section, List<string> values, int index)
        {
            string nodo = string.Empty;
            XMLProcess xml = new XMLProcess();
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

        public XmlNode CreateNODES(XmlDocument doc, string list)
        {

            XmlNode parent  = doc.CreateNode(XmlNodeType.Element, list, URL);


            CreateChilds(doc, list, parent);

            return parent;

        }
        public void CreateChilds(XmlDocument doc, string list, XmlNode parent)
        {

            List<XMLTemplate> ok = _templates.Where(x => x.Element == list).ToList();
            foreach (XMLTemplate pItem in ok)
            {
                var perAux = fieldList.FindIndex(x => x.Contains(pItem.Section));
                List<string> fieldsNodes = fieldList[perAux + 1].ToString().Split('|').ToList();

                if (pItem.Column != null)
                {
                    if (pItem.ParentElement != "cfdi:Conceptos")
                    {
                     
                        XmlNodeList dataNodes = doc.GetElementsByTagName(pItem.Element);
            
                        XmlAttribute attribute = doc.CreateAttribute(pItem.Attribute);
                        attribute.Value = (pItem.FillWith != null || Convert.ToInt32(pItem.Column) < 0 ? pItem.FillWith : fieldsNodes[Convert.ToInt32(pItem.Column) - 1].ToString());
               
                        parent.Attributes.Append(attribute);

                    }

                }

            }


        }
        public void CreateChildsItems(XmlDocument doc, string section, List<string> fieldsItem, XMLTemplate pItem)
        {

            List<XMLTemplate> ok = _templates.Where(x => x.Section == section && x.Column != null).ToList();
            XmlElement id = doc.CreateElement(pItem.Attribute);
            id.SetAttribute(pItem.Attribute, (pItem.FillWith != null || Convert.ToInt32(pItem.Column) < 0 ? pItem.FillWith : fieldsItem[Convert.ToInt32(pItem.Column) - 1].ToString()));
            id.SetAttribute("passWord", "Tushar");

            XmlElement childOne = doc.CreateElement(pItem.Attribute);
            childOne.InnerText = "This is the first child";
            id.AppendChild(childOne);
        }
    }


}

