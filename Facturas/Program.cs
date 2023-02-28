using BusinessLayer;
using DataLayer;
using System;
using System.Collections.Generic;

namespace Facturas
{
    class Program
    {

        static void Main(string[] args)
        {
            Log log = new Log();

            ProcessTXT processTXT = new ProcessTXT();
            List<string> paths = SFTP.SFTP.ConnectionSFTP();
            foreach (string path in paths)
            {
                try
                {
                    processTXT.LoadTXT(path);
                }
                catch (Exception e)
                {
                    log.WriteLog(e, path);
                }
            }

        }
    }
}
