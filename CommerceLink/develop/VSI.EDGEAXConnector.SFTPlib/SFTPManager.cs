using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Enums;

namespace VSI.EDGEAXConnector.SFTPlib
{
    public class SFTPManager
    {

        public FileHelper fileHelper = null;

        ConfigurationHelper configurationHelper;
        public SFTPManager(string storeKey)
        {
            configurationHelper = new ConfigurationHelper(storeKey);
            fileHelper = new FileHelper(storeKey);
        }
        /// <summary>
        ///This function downloads all files(filtered by Extensions) from the source directory in SFTP to target directory in local file system. 
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="targetPath"></param>
        public void DownloadDirectory(string sourcePath, string targetPath)
        {
            String extension = configurationHelper.GetSetting(ECOM.Remote_SFTP_Extenstions);
            var con = GetFTPConnection();
            using (var client = new SftpClient(con))
            {
                client.Connect();
                var files = client.ListDirectory(sourcePath);
                foreach (var file in files)
                {
                    if (file != null)
                    {
                        if (Regex.IsMatch(file.Name, @"^.*\.(" + extension + ")$"))
                        {
                            if (!sourcePath.EndsWith(@"/"))
                                sourcePath = sourcePath + @"/";
                            if (!targetPath.EndsWith(@"\"))
                                targetPath = targetPath + @"\";
                            using (var fs = new FileStream(targetPath + file.Name, FileMode.Create)) //create file on local
                            {
                                string processedFileName = sourcePath + @"processed/" + file.Name;
                                client.DownloadFile(file.FullName, fs);
                                if (client.Exists(processedFileName))
                                {
                                    client.Delete(processedFileName); // if we dont delete file which aleady exists in the processed folder then it gives exception 
                                }

                                client.RenameFile(file.FullName, processedFileName); // Move to processed folder on FTP                                
                                //client.DeleteFile(file.FullName);
                                fs.Close();
                            }
                        }
                    }
                }
                client.Disconnect();
            }
        }
        // This function uploads all files(filtered by Extensions) from the source directory path in local file system to target directory path in sftp.
        public void UploadDirectory(string sourcePath, string targetPath)
        {
            String extension = configurationHelper.GetSetting(ECOM.Remote_SFTP_Extenstions);
            var con = GetFTPConnection();
            using (var client = new SftpClient(con))
            {
                client.Connect();
                client.ChangeDirectory(targetPath);
                string[] fileEntries = Directory.GetFiles(sourcePath);
                foreach (string file in fileEntries)
                {
                    if (string.IsNullOrWhiteSpace(file) == false)
                    {
                        string fileNameText = Path.GetFileName(file);
                        if (!string.IsNullOrEmpty(fileNameText))
                        {

                            if (Regex.IsMatch(file, @"^.*\.(" + extension + ")$"))
                            {
                                using (var fileStream = new FileStream(sourcePath + fileNameText, FileMode.Open))
                                {
                                    client.BufferSize = 4 * 1024; // bypass Payload error large files
                                    client.UploadFile(fileStream, Path.GetFileName(sourcePath + fileNameText));
                                }
                            }
                        }
                    }
                }
                client.Disconnect();
            }
        }
        // This function moves all files(filtered by Extensions) from the source directory in SFTP to target directory in SFTP.
        public void MoveDirectory(string sourcePath, string targetPath)
        {
            String extension = configurationHelper.GetSetting(ECOM.Remote_SFTP_Extenstions);
            var con = GetFTPConnection();
            using (var client = new SftpClient(con))
            {
                client.Connect();
                var files = client.ListDirectory(sourcePath);
                foreach (var file in files)
                {
                    if (file != null)
                    {
                        if (Regex.IsMatch(file.Name, @"^.*\.(" + extension + ")$"))
                        {
                            client.RenameFile(sourcePath + "/" + file.Name, targetPath + "/" + file.Name);
                        }
                    }

                }
                client.Disconnect();
            }
        }
        // This function downloads single file from the source file path in SFTP to target file path in local file system path.
        //fileSourcePath should include name of the file.
        //fileTargetPath should include name of the file.
        public void DownloadFile(string fileSourcePath, string fileTargetPath)
        {
            var con = GetFTPConnection();
            using (var client = new SftpClient(con))
            {
                client.Connect();
                using (var fs = new FileStream(fileTargetPath, FileMode.Create))
                {
                    client.DownloadFile(fileSourcePath, fs);
                    fs.Close();
                }
                client.Disconnect();
            }
        }
        // This function uploads single file from the source file path in local file system to target file path in SFTP path.
        //fileSourcePath should include name of the file.
        //fileTargetPath should include name of the file.
        public void UploadFile(string fileSourcePath, string targetDirectoryPath)
        {
            var con = GetFTPConnection();
            using (var client = new SftpClient(con))
            {
                client.Connect();
                client.ChangeDirectory(targetDirectoryPath);
                string fileNameText = Path.GetFileName(fileSourcePath);
                using (var fileStream = new FileStream(fileSourcePath, FileMode.Open))
                {
                    client.BufferSize = 4 * 1024; // bypass Payload error large files
                    client.UploadFile(fileStream, Path.GetFileName(fileSourcePath));
                }
                client.Disconnect();
            }
        }
        // This function uploads multiple file from the source file path in local file system to target file path in SFTP path.
        //fileSourcePath should include name of the file.
        //fileTargetPath should include name of the file.
        public void UploadFiles(string[] sourceFiles, string targetDirectoryPath, out List<string> failedFiles, bool isCatalog = false)
        {
            var con = GetFTPConnection();
            List<string> fFiles = new List<string>();
            using (var client = new SftpClient(con))
            {
                client.Connect();
                client.ChangeDirectory(targetDirectoryPath);
                foreach (var file in sourceFiles)
                {
                    if (fileHelper.CheckFileAvailability(file))
                    {
                        string fileNameText = Path.GetFileName(file);
                        using (var fileStream = new FileStream(file, FileMode.Open))
                        {
                            client.BufferSize = 4 * 1024; // bypass Payload error large files
                            try
                            {
                                string strFileName = Path.GetFileName(file);
                                if (configurationHelper.GetSetting(ECOM.Constant_Filename_For_SFTP).BoolValue())
                                {
                                    string[] fileName = Path.GetFileNameWithoutExtension(file).Split('-');
                                    string fileExtension = Path.GetExtension(file);

                                    strFileName = fileName[0];

                                    if (bool.Parse(configurationHelper.GetSetting(PRODUCT.Enable_Region_Catalog))
                                    && isCatalog)
                                    {
                                        var regionName = fileName[fileName.Length - 1];
                                        strFileName = $"{strFileName}-{regionName}";

                                    }

                                    strFileName = strFileName + fileExtension;
                                }
                                client.UploadFile(fileStream, strFileName);
                            }
                            catch (Exception)
                            {
                                fFiles.Add(file);
                            }
                        }
                    }
                }

                client.Disconnect();
            }
            failedFiles = fFiles;
        }
        public void CreateServerDirectoryIfItDoesntExist(List<string> serverDestinationPathlst)
        {
            var connection = GetFTPConnection();
            try
            {
                using (var client = new SftpClient(connection))
                {
                    client.Connect();

                    foreach (string path in serverDestinationPathlst)
                    {
                        if (!string.IsNullOrWhiteSpace(path))
                        {
                            string destinationPath = path;

                            destinationPath = destinationPath[0] == '/' ? destinationPath.Substring(1) : destinationPath;

                            destinationPath = destinationPath.Replace("//", "/");

                            string[] directories = destinationPath.Split('/');

                            for (int i = 0; i < directories.Length; i++)
                            {
                                string dirName = string.Join("/", directories, 0, i + 1);

                                if (!string.IsNullOrWhiteSpace(dirName))
                                {
                                    dirName = string.Concat("/", dirName);
                                    if (!client.Exists(dirName))
                                        client.CreateDirectory(dirName);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        // This function uploads move file from the source file path in SFTP to target file path in SFTP.
        //sourcePath should include name of the file.
        //targetPath should include name of the file.
        public void MoveFile(string sourcePath, string targetPath)
        {
            var con = GetFTPConnection();
            using (var client = new SftpClient(con))
            {
                client.Connect();
                client.RenameFile(sourcePath, targetPath);
                client.Disconnect();
            }
        }
        private ConnectionInfo GetFTPConnection()
        {
            String host = configurationHelper.GetSetting(ECOM.Remote_SFTP_Host);
            String username = configurationHelper.GetSetting(ECOM.Remote_SFTP_UserName);
            String password = configurationHelper.GetSetting(ECOM.Remote_SFTP_Password);
            int port = Convert.ToInt32(configurationHelper.GetSetting(ECOM.Remote_SFTP_Port));
            int timeOutDuration = Convert.ToInt32(configurationHelper.GetSetting(ECOM.Remote_SFTP_Time_Out));

            var methods = new List<AuthenticationMethod>();
            methods.Add(new PasswordAuthenticationMethod(username, password));
            ConnectionInfo connectionInfo = new ConnectionInfo(host, port, username, methods.ToArray());
            connectionInfo.Timeout = TimeSpan.FromSeconds(timeOutDuration);
            return connectionInfo;
        }
    }
}
