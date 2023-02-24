using BusinessLayer;
using System;
using System.Collections.Generic;

namespace Facturas
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcessTXT processTXT = new ProcessTXT();
            List<string> paths = SFTP.SFTP.ConnectionSFTP();
            foreach (string path in paths)
            {
                processTXT.LoadTXT(path);
            }
            Console.WriteLine("Hello World!");
        }
    }
}
