using BusinessLayer;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturas.SFTP
{
    public static class SFTP
    {
        public static List<string> ConnectionSFTP()
        {
            List<string> paths = new List<string>();
            string path = string.Empty;
            try
            {
                //SFTP
                string host = ConfigurationManager.AppSettings["SFTPhost"];
                string username = ConfigurationManager.AppSettings["SFTPusername"];
                string password = ConfigurationManager.AppSettings["SFTPpassword"];
                string source = ConfigurationManager.AppSettings["SFTPsource"];
                int port = Convert.ToInt32(ConfigurationManager.AppSettings["SFTPport"]);
                string destLocalPath = ConfigurationManager.AppSettings["LocalPath"];
                using (SftpClient sftp = new SftpClient(host, port, username, password))
                {
                    sftp.Connect();
                    paths = DownloadDirectory(sftp, source, destLocalPath);
                    sftp.Disconnect();
                };

            }
            catch (Exception e)
            {
                Console.WriteLine("An exception has been caught " + e.ToString());
            }
            return paths;
        }


        private static List<string> DownloadDirectory(
        SftpClient sftpClient, string sourceRemotePath, string destLocalPath)
        {
            string sourceFilePath = string.Empty;
           
            List<string> paths = new List<string>();
            Directory.CreateDirectory(destLocalPath);
            IEnumerable<SftpFile> files = sftpClient.ListDirectory(sourceRemotePath);
            foreach (SftpFile file in files)
            {
                if ((file.Name != ".") && (file.Name != ".."))
                {
                    string destFilePath = string.Empty;
                    sourceFilePath = sourceRemotePath + "/" + file.Name;
                    destFilePath = Path.Combine(destLocalPath, file.Name);
                    if (file.IsDirectory)
                    {
                        DownloadDirectory(sftpClient, sourceFilePath, destFilePath);
                    }
                    else
                    {
                        using (Stream fileStream = File.Create(destFilePath))
                        {
                            sftpClient.DownloadFile(sourceFilePath, fileStream);
                        }
                    }
                    paths.Add(destFilePath);
                }
            }
            return paths;
        }
    }
}
