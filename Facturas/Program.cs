using BusinessLayer;
using System;
using System.Collections.Generic;

namespace Facturas
{
    class Program
    {
        static void Main(string[] args)
        {
           List<string> paths= SFTP.SFTP.ConnectionSFTP();
            foreach (string path in paths)
            {
                ProcessTXT.LoadTXT(path);
            }
            Console.WriteLine("Hello World!");
        }
    }
}
