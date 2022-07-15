using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.UI.Helpers
{
    public class FileHelper
    {
        #region Helper Methods

        /// <summary>
        /// Move file from failed folder to processing folder
        /// </summary>
        /// <param name="file"> file to move </param>
        /// <param name="targetFolderName"> destination folder </param>
        /// <param name="targetDirectoryFullPath"> destination Full path </param>
        /// <returns> new file path </returns>
        public static string MoveFileToLocalFolder(string file, string targetFolderName, string targetDirectoryFullPath)
        {
            string newFilePath = string.Empty;
            if (FileReady(file))
            {
                string localDir = targetDirectoryFullPath;
                if (string.IsNullOrEmpty(localDir))
                {
                    System.Windows.Forms.MessageBox.Show("target location has not been specified");
                }
                if (!string.IsNullOrWhiteSpace(localDir))
                {
                    if (!localDir.EndsWith(@"\"))
                    {
                        localDir += @"\";
                    }
                    if (!string.IsNullOrWhiteSpace(targetFolderName))
                    {
                        localDir += targetFolderName + @"\";
                    }
                    if (!Directory.Exists(localDir))
                    {
                        Directory.CreateDirectory(localDir);
                    }
                    string fileName = Path.GetFileName(file);
                    newFilePath = localDir + fileName;
                    if (File.Exists(file) && !File.Exists(newFilePath))
                    {
                        File.Move(file, newFilePath);
                    }
                    else
                    {
                        string name = Path.GetFileNameWithoutExtension(file);
                        string ext = Path.GetExtension(file);
                        int cnt = 1;
                        newFilePath = localDir + name + "(" + cnt.ToString() + ")" + ext;
                        while (File.Exists(newFilePath))
                        {
                            cnt++;
                            newFilePath = localDir + name + "(" + cnt.ToString() + ")" + ext;
                        }
                        if (File.Exists(file))
                        {
                            File.Move(file, newFilePath);
                        }
                    }
                    return newFilePath;
                }
            }
            else
            {
                MessageBox.Show(file + " is in locked state.Close it and then try again");
            }

            return newFilePath;

        }

        /// <summary>
        /// Check whether have read write access or not
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns>true</returns>
        private static bool FileReady(string fullPath)
        {

            int numTries = 0;
            while (numTries < 6)
            {
                ++numTries;
                try
                {
                    // Attempt to open the file exclusively.
                    using (FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None, 100))
                    {
                        fs.ReadByte();
                        // If we got this far the file is ready
                        break;
                    }
                }
                catch (Exception)
                {

                    // Wait for the lock to be released
                    System.Threading.Thread.Sleep(1000);
                }
            }
            if (numTries < 5)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static void CopyFileToLocalFolder(string file, string targetFolderName, string targetDirectoryFullPath)
        {
            FileStream inf = new FileStream(System.IO.Path.Combine(targetFolderName, file), FileMode.Open, FileAccess.Read, FileShare.Read);
            FileStream outf = new FileStream(System.IO.Path.Combine(targetDirectoryFullPath, file), FileMode.Create);
            int a;
            while ((a = inf.ReadByte()) != -1)
            {
                outf.WriteByte((byte)a);
            }
            inf.Close();
            inf.Dispose();
            outf.Close();
            outf.Dispose();
        }


        public static void CreatingLogCsvFiles(string file, string path, Log logObj)
        {
            string filePath = System.IO.Path.Combine(path, file + ".csv");
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }

            string delimiter = ",";
            string[][] header = new string[][]{
                new string[]{"Id"
                           ,"EventDateTime"
                           ,"EventLevel"
                           ,"UserName"
                           ,"MachineName"
                           ,"EventMessage"
                           ,"ErrorSource"
                           ,"ErrorClass"
                           ,"ErrorMethod"
                           ,"ErrorMessage"
                           ,"InnerErrorMessage"
                           ,"IdentityId"
                           ,"ErrorModule"} /*add the values that you want inside a csv file. Mostly this function can be used in a foreach loop.*/
            };
            int headerLength = header.GetLength(0);
            StringBuilder sb = new StringBuilder();
            for (int index = 0; index < headerLength; index++)
            {
                sb.AppendLine(string.Join(delimiter, header[index]));
            }
            File.AppendAllText(filePath, sb.ToString());
            sb = new StringBuilder();
            //delimiter = ",";
            string[][] output = new string[][]{
                new string[]{logObj.LogId.ToString()
                           ,logObj.CreatedOn.ToString()
                           ,logObj.EventLevel
                           ,logObj.CreatedBy
                           ,logObj.MachineName
                           ,logObj.EventMessage
                           ,logObj.ErrorSource
                           ,logObj.ErrorClass
                           ,logObj.ErrorMethod
                           ,logObj.ErrorMessage
                           ,logObj.InnerErrorMessage
                           ,logObj.IdentityId
                           ,logObj.ErrorModule} /*add the values that you want inside a csv file. Mostly this function can be used in a foreach loop.*/
            };
            int outputLength = output.GetLength(0);
            for (int index = 0; index < outputLength; index++)
            {
                sb.AppendLine(string.Join(delimiter, output[index]));
            }
            File.AppendAllText(filePath, sb.ToString());
        }

        public static void CreatingLogTxtFiles(string file, string path, Log logObj)
        {
            string filePath = System.IO.Path.Combine(path, file + ".txt");
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }

            string delimiter = ",";
            string[][] header = new string[][]{
                new string[]{"Id : " + logObj.IdentityId + Environment.NewLine + Environment.NewLine +
                           "EventDateTime : " + logObj.CreatedOn + Environment.NewLine + Environment.NewLine +
                           "EventLevel :"+ logObj.EventLevel + Environment.NewLine + Environment.NewLine +
                           "UserName : "+ logObj.CreatedBy + Environment.NewLine + Environment.NewLine +
                           "MachineName : "+ logObj.MachineName + Environment.NewLine + Environment.NewLine +
                           "EventMessage : "+ logObj.EventMessage + Environment.NewLine + Environment.NewLine +
                           "ErrorSource : "+ logObj.ErrorSource + Environment.NewLine + Environment.NewLine +
                           "ErrorClass : "+ logObj.ErrorClass + Environment.NewLine + Environment.NewLine +
                           "ErrorMethod : "+ logObj.ErrorMethod + Environment.NewLine + Environment.NewLine +
                           "ErrorMessage : "+ logObj.ErrorMessage + Environment.NewLine + Environment.NewLine +
                           "InnerErrorMessage : "+ logObj.InnerErrorMessage + Environment.NewLine + Environment.NewLine +
                           "IdentityId : "+ logObj.IdentityId + Environment.NewLine + Environment.NewLine +
                           "ErrorModule : "+ logObj.ErrorModule + Environment.NewLine} /*add the values that you want inside a csv file. Mostly this function can be used in a foreach loop.*/
            };
            int headerLength = header.GetLength(0);
            StringBuilder sb = new StringBuilder();
            for (int index = 0; index < headerLength; index++)
            {
                sb.AppendLine(string.Join(delimiter, header[index]));
            }
            File.WriteAllText(filePath, sb.ToString());
        }
        
        #endregion
    }
}
