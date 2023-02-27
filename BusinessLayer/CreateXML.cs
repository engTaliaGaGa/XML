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
                //List<string> ok = (from up in _templates
                //                   where _templates.Any(ut => ut.ParentElement == up.Element)
                //                   select up.ParentElement).Distinct().ToList();
                var rootnode = doc.CreateElement(prefix, rootSection, URL);
                doc.AppendChild(rootnode);

                foreach (KeyValuePair<string, string> entry in att)
                {
                    rootnode.SetAttribute(entry.Key, entry.Value);
                }

                #endregion

                //foreach (string pItem in ok)
                //{
                //Don't repeat root node


                List<string> padres = _templates.Where(x => x.ParentElement == doc.SelectSingleNode("/*").Name).Select(x=> x.Element).Distinct().ToList();
                List<string> hijos = _templates.Select(x => x.Element).ToList();



                for (int x = 0; x < padres.Count(); x++)
                {
                    XmlNode parent = null;
                    //string rootName = doc.SelectSingleNode("/*").Name;
                    if (x > 0)
                    {                  //Create      
                                       //XmlNode parent = doc.CreateNode(XmlNodeType.Element, padres[x].ToString(), URL);
                                       //CreateChildsNoParents(doc, padres[x].ToString(), parent);


                        //rootnode.AppendChild(parent);

                        IEnumerable<XMLTemplate> enumerable = _templates.Where(q => q.ParentElement == padres[x]).ToList();
                        if (enumerable.Count() > 0)
                        {
                            foreach (XMLTemplate ch in enumerable)
                            {

                                parent = doc.CreateNode(XmlNodeType.Element, padres[x].ToString(), URL);

                                XmlNode child = doc.CreateNode(XmlNodeType.Element, ch.Element, URL);
                                CreateChildsNoParents(doc, ch.Element, child);
                                parent.AppendChild(child);
                            }


                        }
                        else
                        {
                            parent = doc.CreateNode(XmlNodeType.Element, padres[x].ToString(), URL);
                            CreateChildsNoParents(doc, padres[x].ToString(), parent);
                        }
                        rootnode.AppendChild(parent);

                    }
                }
                //XmlNode parent = CreateNODES(doc, pItem, rootName);
                //if (parent != null)
                //{
                //    rootnode.AppendChild(parent);
                //}

                //}

                // }

                doc.Save(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDoc‌​uments), "xml") + Guid.NewGuid().ToString() + ".xml");
            }
            catch (Exception e)
            {
                log.WriteLog(e, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString());
            }
        }

        private XmlNode CreateNODES(XmlDocument doc, string list, string rootName)
        {
            XmlNode parent = null;

            List<string> padres = _templates.Select(x => x.ParentElement).Distinct().ToList();
            List<string> hijos = _templates.Select(x => x.Element).ToList();



            for (int x = 0; x < padres.Count(); x++)
            {
                if (rootName != padres[x].ToString() || padres[x].ToString() != "root")
                {
                    parent = doc.CreateNode(XmlNodeType.Element, padres[x].ToString(), URL);
                    // CreateChildsNoParents(doc, padres[x].ToString(), parent);
                    for (int y = 0; y < _templates.Count(); y++)
                    {

                        if (_templates[y].ParentElement == padres[x].ToString())
                        {
                            XmlNode parentCicle = doc.CreateNode(XmlNodeType.Element, _templates[y].Element, URL);
                            CreateChildsNoParents(doc, list, parent);
                            parent.AppendChild(parentCicle);
                            Console.WriteLine("padre" + padres[x] + "hijo" + _templates[y].Element);
                        }
                    }
                }
            }


            //XmlNode parent = null;
            //List<string> ok = (from up in _templates
            //                   where _templates.Any(ut => ut.ParentElement == up.Element && up.Element == list)
            //                   select up.ParentElement).Distinct().ToList();

            //int IsParent = _templates.Where(x => x.ParentElement == list).Count();
            //if (ok.Count == 0)
            //{
            //    //Node without childs
            //    parent = doc.CreateNode(XmlNodeType.Element, list, URL);

            //    CreateChildsNoParents(doc, list, parent);
            //    XMLTemplate pItem = _templates.Where(x => x.ParentElement == list && x.IdType == null).FirstOrDefault();
            //    if (pItem != null)
            //    {
            //        XmlNode parentCicle = CreateNODES(doc, pItem.Element, rootName);
            //        //Create Childs to Node 
            //        CreateChildsNoParents(doc, list, parentCicle);
            //        parent.AppendChild(parentCicle);
            //    }
            //    else
            //    {
            //        CreateChildsNoParents(doc, list, parent);
            //    }
            //}
            //else
            //{
            //    parent = doc.CreateNode(XmlNodeType.Element, list, URL);

            //    //Create Childs to Node 
            //    CreateChildsNoParents(doc, list, parent);
            //    XMLTemplate pItem = _templates.Where(x => x.ParentElement == list && x.IdType == null).FirstOrDefault();
            //    XmlNode parentCicle = CreateNODES(doc, pItem.Element, rootName);
            //    CreateChildsParent(doc, list, parentCicle);
            //    parent.AppendChild(parentCicle);
            //}
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

        private void CreateChildsParent(XmlDocument doc, string list, XmlNode parent)
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
            XMLTemplate temp2 = _templates.Where(x => x.ParentElement == list && x.Row == null).FirstOrDefault();
            if (temp2 != null)
            {
                XmlNode xmlElement = doc.CreateNode(XmlNodeType.Element, temp2.Element, URL);
            }

        }
        private XmlAttribute GenerateAttributes(string list, XmlNode parent)
        {
            XmlAttribute attribute = null;
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


                        attribute = doc.CreateAttribute(pItem.Attribute);
                        attribute.Value = (pItem.FillWith != null || Convert.ToInt32(pItem.Column) < 0 ? pItem.FillWith : fieldsNodes[Convert.ToInt32(pItem.Column) - 1].ToString());
                        //parent.Attributes.Append(attribute);
                    }

                }
            }
            return attribute;
        }

    }
}


