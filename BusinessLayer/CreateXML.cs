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

           /// XmlNamespaceManager xnm = new XmlNamespaceManager(doc.NameTable);
            //xnm.AddNamespace("schemaLocation", "loc");
            //xnm.AddNamespace("payload", "loc2");
            //xnm.AddNamespace("a", "http://www.w3.org/2001/XMLSchema-instance");
            //xnm.AddNamespace("x", doc.DocumentElement.NamespaceURI);

           // xnm.AddNamespace("xmlns:cfdi", "http://www.sat.gob.mx/cfd/3");
            rootnode.SetAttribute("xmlns:cfdi", "http://www.sat.gob.mx/cfd/3");
            rootnode.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            rootnode.SetAttribute("xsi:schemaLocation", "http://www.sat.gob.mx/cfd/4 http://www.sat.gob.mx/sitio_internet/cfd/4/cfdv40.xsd http://www.sat.gob.mx/sitio_internet/cfd/leyendasFiscales/leyendasFisc.xsd");
            // rootnode.SetAttribute("xmlns:JOB", "http://www.example.com");





            foreach (string pItem in ok)
            {


                //var perAux = fieldList.Find(x => x.Contains(pItem.Section)).Distinct().ToList();

                ////if (perAux > 0)
                ////{
                XmlNode parent = CreateNODES(doc, pItem);
                rootnode.AppendChild(parent);
                //doc.AppendChild(nodes);
            }

            // doc.AppendChild(rootnode);



            //string xmlData = "<book xmlns:bk='urn:samples'></book>";

            //doc.Load(new StringReader(xmlData));

            //// Create a new element and add it to the document.
            //XmlElement elem = doc.CreateElement("bk", "genre", "urn:samples");
            //elem.InnerText = "fantasy";
            //doc.DocumentElement.AppendChild(elem);


            //string nodo = string.Empty;
            //List<string> fieldList = new List<string>();
            //fieldList.AddRange(fields);
            //foreach (var pItem in templates)
            //{
            //    XmlElement LiftsMainNode = doc.CreateElement(pItem.Element);
            //    LiftsMainNode.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");

            //    XmlElement child = doc.CreateElement(pItem.Element);
            //    //child.SetAttribute(pItem.Section, "Att");
            //    //child.SetAttribute("CustomerName", cust.CustomerName);
            //    //child.SetAttribute("PhoneNumber", cust.PhoneNumber);
            //    //child.SetAttribute("Email", cust.Email);

            //    //foreach (XmlElement ndNode in child.SelectNodes(pItem.ParentElement, nsmgr))
            //    //{
            //    //    XmlElement newelem = doc.CreateElement(pItem.Element);
            //    //    newelem.InnerText = "test";
            //    //    ndNode.InsertBefore(newelem, ndNode.FirstChild);
            //    //}
            //    doc.AppendChild(child);
            //    var perAux = fieldList.FindIndex(x => x.Contains(pItem.Section));

            //    if (perAux > 0)
            //    {
            //       	//var policies = doc.AppendChild(doc.CreateElement(pItem.Element,ns));

            //        //string nodo = AddNodes(pItem.Section, fieldList, perAux);
            //        XMLProcess xml = new XMLProcess();
            //        List<string> fieldsNodes = fieldList[perAux + 1].ToString().Split('|').ToList();
            //        List<XMLTemplate> nodes = xml.GetElementsBySection(pItem.Section);

            //        foreach (XMLTemplate node in nodes)
            //        {
            //            if (node.IdType == (int?)Enum.Type.Items)
            //            {
            //                List<string> fieldsItem = fieldList[perAux + 2].ToString().Split('|').ToList();
            //                nodo += node.Attribute + "= \"" + (node.Column != null ? fieldsItem[Convert.ToInt32(node.Column) - 1].ToString() : "") + "\"";
            //                //XmlText tex = doc.CreateTextNode(nodo);
            //                // doc.DocumentElement.LastChild.AppendChild(tex);
            //                //AddNodes(section, string values)

            //            }
            //            else
            //            {
            //                if (node.Column != null && fieldsNodes.Count > 1)
            //                {
            //                    nodo += node.Attribute + "= \"" + (node.FillWith != null || Convert.ToInt32(node.Column) < 0 ? node.FillWith : fieldsNodes[Convert.ToInt32(node.Column) - 1].ToString()) + "\"";
            //                  //  XmlText tex = doc.CreateTextNode(nodo);
            //                    //doc.DocumentElement.LastChild.AppendChild(tex);
            //                }
            //            }
            //        }




            //    }
            //};

            //// doc.LoadXml(stringwriter.ToString());
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
                    //AddNodes(section, string values)

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
            //XMLTemplate templates = new XMLTemplate();
            //XmlNodeList countries = doc.DocumentElement.ChildNodes;
            //XmlElement country = null;


            //foreach (XmlElement temp in countries)
            //{
            //    if (temp.Attributes["Element"].Value == "**" + templates.Element + "**")
            //    {
            //        country = temp;
            //        break;
            //    }
            //}

            //if (country == null)
            //{
            //    country = doc.CreateElement("Country", "http://tempuri.org/format.xsd");
            //    country.SetAttribute("id", "**" + templates.Element + "**");
            //}

            //XmlElement employee = doc.CreateElement("Emp", "http://tempuri.org/format.xsd");
            //employee.SetAttribute("Name", templates.Attribute);
            //employee.SetAttribute("address", templates.Attribute);
            //employee.SetAttribute("id", templates.Attribute);

            //country.AppendChild(employee);
            //doc.DocumentElement.AppendChild(country);

            //XmlElement parent = doc.CreateElement(list);

            XmlNode parent  = doc.CreateNode(XmlNodeType.Element, list, URL);


            CreateChilds(doc, list, parent);
            //parent.AppendChild(childOne);
            //XmlElement childTwo = doc.CreateElement("childTwo");
            //childOne.InnerText = "This is the second child";
            //parent.AppendChild(childTwo);

            //XmlElement childThree = doc.CreateElement("childThree");
            //childOne.InnerText = "This is the third child";
            //parent.AppendChild(childThree);

            return parent;





        }
        public void CreateChilds(XmlDocument doc, string list, XmlNode parent)
        {

            List<XMLTemplate> ok = _templates.Where(x => x.Element == list).ToList();
            foreach (XMLTemplate pItem in ok)
            {
                var perAux = fieldList.FindIndex(x => x.Contains(pItem.Section));
                List<string> fieldsNodes = fieldList[perAux + 1].ToString().Split('|').ToList();


                //if (pItem.IdType == (int?)Enum.Type.Items)
                //{
                //    //List<string> fieldsItem = fieldList[perAux + 2].ToString().Split('|').ToList();
                //    //XmlElement id = doc.CreateElement(pItem.Attribute);
                //    //id.SetAttribute(pItem.Attribute, (pItem.Column != null ? fieldsItem[Convert.ToInt32(pItem.Column) - 1].ToString() : ""));
                //    //id.SetAttribute("passWord", "Tushar");

                //    ////XmlElement XEle = doc.CreateElement("Child");
                //    //XmlNode TestChild = doc.CreateNode(XmlNodeType.Attribute, "TestChild", null);
                //    //parent.SetAttribute("Name","OK");
                //    //TestChild.AppendChild(parent);
                //    //doc.AppendChild(parent);

                //    //CreateChilds(doc, pItem.Attribute);
                //    CreateChildsItems(doc, pItem.Section, fieldList, pItem);
                //    //nodo += node.Attribute + "= \"" + (node.Column != null ? fieldsItem[Convert.ToInt32(node.Column) - 1].ToString() : "") + "\"";
                //    //XmlText tex = doc.CreateTextNode(nodo);
                //    // doc.DocumentElement.LastChild.AppendChild(tex);
                //    //AddNodes(section, string values)

                //}
                //else
                //{
                if (pItem.Column != null)
                {
                    if (pItem.ParentElement != "cfdi:Conceptos")
                    {
                        //XmlElement id = doc.CreateElement(pItem.Element);

                        //parent.SetAttribute(pItem.Attribute, (pItem.FillWith != null || Convert.ToInt32(pItem.Column) < 0 ? pItem.FillWith : fieldsNodes[Convert.ToInt32(pItem.Column) - 1].ToString()));

                        XmlNodeList dataNodes = doc.GetElementsByTagName(pItem.Element);
                        //if (dataNodes != null && dataNodes.Count > 0)
                        //{
                        XmlAttribute attribute = doc.CreateAttribute(pItem.Attribute);
                        attribute.Value = (pItem.FillWith != null || Convert.ToInt32(pItem.Column) < 0 ? pItem.FillWith : fieldsNodes[Convert.ToInt32(pItem.Column) - 1].ToString());
                        // }
                        parent.Attributes.Append(attribute);


                        //parent.AppendChild(id);
                        //doc.AppendChild(id);
                    }

                }

                //  }

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

