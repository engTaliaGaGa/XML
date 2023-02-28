using BusinessLayer;
using BusinessLayer.Interfaces;
using DataLayer;
using EntityLayer;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.SFTP
{
    public class SFTP : ISFTP
    {
        static string host = ConfigurationManager.AppSettings["SFTPhost"];
        static string username = ConfigurationManager.AppSettings["SFTPusername"];
        static string password = ConfigurationManager.AppSettings["SFTPpassword"];
        static string source = ConfigurationManager.AppSettings["SFTPsource"];
        static string destLocalPath = ConfigurationManager.AppSettings["LocalPath"];
        static string destLocalPathError = ConfigurationManager.AppSettings["LocalPathError"];
        static int port = Convert.ToInt32(ConfigurationManager.AppSettings["SFTPport"]);

        public List<string> ConnectionSFTP()
        {
            List<string> paths = new List<string>();
            Log log = new Log();
            try
            {
                //SFTP

                using (SftpClient sftp = new SftpClient(host, port, username, password))
                {
                    sftp.Connect();
                    paths = DownloadDirectory(sftp, source, destLocalPath);
                    sftp.Disconnect();
                };

            }
            catch (Exception e)
            {
                log.WriteLog(e.Message, e.StackTrace, paths.Select(x => x).ToString());
                Console.WriteLine("An exception has been caught " + e.ToString());
            }
            return paths;
        }


        public void ErrorFile(string filename)
        {
            filename = Path.GetFileName(filename);
            string paths = string.Empty;
            Log log = new Log();
            try
            {
                //SFTP

                using (SftpClient sftp = new SftpClient(host, port, username, password))
                {
                    sftp.Connect();
                    paths = DownloadDirectoryError(sftp, source, destLocalPathError, filename);
                    sftp.Disconnect();
                };

            }
            catch (Exception e)
            {
                log.WriteLog(e.Message, e.StackTrace, filename);
                Console.WriteLine("An exception has been caught " + e.ToString());
            }
        }
        private List<string> DownloadDirectory(
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

        private string DownloadDirectoryError(
        SftpClient sftpClient, string sourceRemotePath, string destLocalPath, string fileName)
        {
            string sourceFilePath = string.Empty;
            string destFilePath = string.Empty;
            List<string> paths = new List<string>();
            Directory.CreateDirectory(destLocalPath);
            IEnumerable<SftpFile> files = sftpClient.ListDirectory(sourceRemotePath);
            foreach (SftpFile file in files)
            {
                if ((file.Name != ".") && (file.Name != ".."))
                {
                    if (file.Name == fileName)
                    {
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

                    }

                }
            }

            return destFilePath;
        }
    }
}
