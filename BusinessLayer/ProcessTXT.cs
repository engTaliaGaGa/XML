using DataLayer;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BusinessLayer
{
    public static class ProcessTXT
    {
        public static void LoadTXT(string fileStream)
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
    }
}

