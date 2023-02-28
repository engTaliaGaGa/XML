using BusinessLayer.Interfaces;
using DataLayer;
using DataLayer.Interfaces;
using DataLayer.SFTP;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BusinessLayer
{
    public class ProcessTXT : ITXT
    {
        public bool LoadTXT(string fileStream)
        {
            Log log = new Log();
            bool prosecuted = false;
            IXML xml = new CreateXML();
            ISFTP sftp = new SFTP();
            const int client = 1;
            try
            {
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    XMLTemplate file = new XMLTemplate();
                    char[] delims = new[] { '\r', '\n' };

                    string contents = streamReader.ReadToEnd();
                    string[] fields = contents.Split(delims, StringSplitOptions.RemoveEmptyEntries);
                    XMLProcess process = new XMLProcess();

                    List<XMLTemplate> templates = process.GetTemplate(client);
                    prosecuted = xml.CreationXML(fields, templates);

                    if (!prosecuted)
                    {
                        sftp.ErrorFile(fileStream);
                        log.WriteLog("Error", "Process file", fileStream);
                    }
                }
            }
            catch (Exception e)
            {
                log.WriteLog(e.Message, e.StackTrace, fileStream);
            }
            return prosecuted;
        }
    }
}

