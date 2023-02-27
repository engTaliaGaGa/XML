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
                CreateChildsNoParents(doc, doc.SelectSingleNode("/*").Name, rootnode);
                #endregion

                List<string> padres = _templates.Where(x => x.ParentElement == doc.SelectSingleNode("/*").Name).Select(x => x.Element).Distinct().ToList();
                List<string> hijos = _templates.Select(x => x.Element).ToList();

                for (int x = 0; x < padres.Count(); x++)
                {
                    XmlNode parent = null;

                    IEnumerable<XMLTemplate> enumerable = _templates.Where(q => q.ParentElement == padres[x]).ToList();
                    if (enumerable.Count() > 0)
                    {

                        parent = CreateNODES(doc, padres[x], false);

                    }
                    else
                    {
                        parent = doc.CreateNode(XmlNodeType.Element, padres[x].ToString(), URL);
                        CreateChildsNoParents(doc, padres[x].ToString(), parent);
                    }
                    if (parent != null)
                    {
                        rootnode.AppendChild(parent);
                    }


                }
                doc.Save(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDoc‌​uments), "xml") + Guid.NewGuid().ToString() + ".xml");
            }
            catch (Exception e)
            {
                log.WriteLog(e, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString());
            }
        }

        private XmlNode CreateNODES(XmlDocument doc, string padres, bool IsChilParent)
        {
            XmlNode parent = null;
            IEnumerable<XMLTemplate> enumerable = _templates.Where(q => q.ParentElement == padres && q.Column != null).ToList();
            if (enumerable.Count() > 0)
            {
                foreach (XMLTemplate ch in enumerable)
                {
                    if (!IsChilParent)
                    {
                        parent = doc.CreateNode(XmlNodeType.Element, padres, URL);
                        XmlNode child = doc.CreateNode(XmlNodeType.Element, ch.Element, URL);

                        CreateChildsNoParents(doc, ch.Element, child);

                        parent.AppendChild(child);

                        XmlNode parentOK = CreateNODES(doc, ch.Element, true);
                        if (parentOK != null)
                        {
                            parent.AppendChild(parentOK);
                        }
                    }
                    else
                    {
                        parent = doc.CreateNode(XmlNodeType.Element, ch.Element, URL);

                        CreateChildsNoParents(doc, ch.Element, parent);
                        XmlNode parentOK = CreateNODES(doc, ch.Element, true);
                        if (parentOK != null)
                        {
                            parent.AppendChild(parentOK);
                        }
                    }
                }

            }
            return parent;

        }

        private void CreateChildsNoParents(XmlDocument doc, string list, XmlNode parent)
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
                        parent.Attributes.Append(attribute);
                    }

                }
            }

        }

    }
}


