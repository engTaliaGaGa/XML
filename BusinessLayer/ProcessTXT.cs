using DataLayer;
using DataLayer.Interfaces;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BusinessLayer
{
    public class ProcessTXT: ITXT
    {
        public void LoadTXT(string fileStream)
        {
            Log log = new Log();
            try
            {
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    XMLTemplate file = new XMLTemplate();
                    char[] delims = new[] { '\r', '\n' };

                    string contents = streamReader.ReadToEnd();
                    string[] fields = contents.Split(delims, StringSplitOptions.RemoveEmptyEntries);
                    XMLProcess process = new XMLProcess();

                    List<XMLTemplate> templates = process.GetTemplate(1);
                    CreateXML createXML = new CreateXML();
                    createXML.CreationXML(fields, templates);
                }
            }
            catch (Exception e)
            {
                log.WriteLog(e, fileStream);
            }
        }
    }
}

