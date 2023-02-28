using BusinessLayer;
using BusinessLayer.Interfaces;
using DataLayer;
using DataLayer.Interfaces;
using DataLayer.SFTP;
using System;
using System.Collections.Generic;

namespace Facturas
{
    class Program
    {

        static void Main(string[] args)
        {
            Log log = new Log();
            bool prosecuted;
            ITXT processTXT = new ProcessTXT();
            ISFTP sftp = new SFTP();
            List<string> paths = sftp.ConnectionSFTP();
            foreach (string path in paths)
            {
                try
                {
                    prosecuted = processTXT.LoadTXT(path);
                    if (!prosecuted)
                    {
                        sftp.ErrorFile(path);
                    }
                }
                catch (Exception e)
                {
                    log.WriteLog(e.Message, e.StackTrace, path);
                }
            }

        }
    }
}
