using System;
using System.IO;
using System.Text;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.Common
{
    /// <summary>
    /// FileHelper provides functionality related to file manipulation files.
    /// </summary>
    public class FileHelper
    {
        ConfigurationHelper configurationHelper = null;
        EntityFileNameParameterDAL entityFileNameDAL = null;
        public FileHelper(string storeKey)
        {
            //configurationHelper = new ConfigurationHelper(storeKey);
            configurationHelper = new ConfigurationHelper(storeKey);
        }
        #region Public Methods
        /// <summary>
        /// Write writes a string to the specified file.
        /// </summary>
        /// <param name="fileName">The complete file name with path to write to.</param>
        /// <param name="data">The string to write to the file.</param>
        /// <returns>true if operation was successful.</returns>
        public  bool Write(string fileName, string data)
        {
            bool fileCreated = false;

            if (!string.IsNullOrWhiteSpace(fileName))
            {
                using (StreamWriter writer = new StreamWriter(fileName, false, Encoding.UTF8))
                {
                    writer.Write(data);
                    writer.Close();
                }
                fileCreated = true;
            }

            return fileCreated;
        }
        public static string GenerateFileNameByEntity(Entities Entity, int storeId)
        {
            EntityFileNameParameterDAL entityFileNameDAL = null;
            try
            {
                entityFileNameDAL = new EntityFileNameParameterDAL();
                bool updated = entityFileNameDAL.UpdateFileNameParameter(Entity);
                EntityFileNameParameter entityFileName = entityFileNameDAL.GetEntityFileNameAndStore(Entity, storeId);
                string filename = string.Empty;
                if (entityFileName != null)
                {
                    filename = entityFileName.Prefix + entityFileName.Parameters.ToString() + '-' + entityFileName.Postfix;
                }
                return filename;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, 0, string.Empty);
                return string.Empty;
            }
        }
        /// <summary>
        /// Read reads string from the specified file.
        /// </summary>
        /// <param name="fileName">The complete file name with path to read.</param>
        /// <returns></returns>
        public  string Read(string fileName)
        {
            string data;
            using (StreamReader reader = new StreamReader(fileName))
            {
                data = reader.ReadToEnd();
                reader.Close();
            }
            return data;
        }
        #region FilName
        public  string GetProductCSVFileName()
        {
            string fileName = GetFileName(configurationHelper.GetSetting(PRODUCT.Filename_Prefix));
            return GetFullFileName(configurationHelper.GetSetting(PRODUCT.Local_Output_Path), fileName);
        }

        public  string GetProductPriceCSVFileName()
        {
            string fileName = GetFileName(configurationHelper.GetSetting(PRICE.Filename_Prefix));
            return GetFullFileName(configurationHelper.GetSetting(PRICE.local_Output_Path), fileName);
        }

        public  string GetProductInventoryCSVFileName()
        {
            string fileName = GetFileName(configurationHelper.GetSetting(INVENTORY.Filename_Prefix));
            return GetFullFileName(configurationHelper.GetSetting(INVENTORY.Local_Output_Path), fileName);
        }

        public  string GetProductDiscountCSVFileName()
        {
            string fileName = GetFileName(configurationHelper.GetSetting(DISCOUNT.Filename_Prefix));
            return GetFullFileName(configurationHelper.GetSetting(DISCOUNT.Local_Output_Path), fileName);
        }

        public  string GetProductImageCSVFileName()
        {
            string fileName = GetFileName(configurationHelper.GetSetting(PRODUCT.ProductImage_FileName_Prefix));
            return GetFullFileName(configurationHelper.GetSetting(PRODUCT.Image_Local_Output_Path), fileName);
        }

        private  string GetFileName(string fileTag)
        {
            string fileName = fileTag + DateTime.UtcNow.ToString("yyyyMMddHHmm") + ".csv";

            return fileName;
        }

        private  string GetFullFileName(string path, string fileName)
        {
            string fullFileName = Path.Combine(path, fileName);

            return fullFileName;
        }
        #endregion
        /// <summary>
        /// Use this method to move the files from one location to another
        /// </summary>
        /// <param name="file"></param>
        /// <param name="targetFolderName"></param>
        /// <param name="targetDirectoryFullPath">If left Empty, it will consider [AppSetting.ConnectorCustomerCSVPath] path as default path</param>
        /// <returns></returns>
        public  string MoveFileToLocalFolder(string file, string targetFolderName, string targetDirectoryFullPath = "")
        {
            string newFilePath = string.Empty;
            if (CheckFileAvailability(file))
            {
                string localDir = targetDirectoryFullPath;
                if (string.IsNullOrEmpty(localDir))
                    //localDir = ConfigurationHelper.ConnectorCustomerCSVPath;
                    localDir = this.configurationHelper.GetDirectory(configurationHelper.GetSetting(CUSTOMER.Local_Input_Path));
                if (!string.IsNullOrWhiteSpace(localDir))
                {
                    if (!localDir.EndsWith(@"\"))
                        localDir += @"\";
                    if (!string.IsNullOrWhiteSpace(targetFolderName))
                        localDir += targetFolderName + @"\";
                    if (!Directory.Exists(localDir))
                        Directory.CreateDirectory(localDir);
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
                            File.Move(file, newFilePath);
                    }
                    return newFilePath;
                }
            }
            return newFilePath;

        }
        public  bool FileReady(string fullPath)
        {
            int numTries = 0;
            while (true)
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
                    //System.Threading.Thread.Sleep(ConfigurationHelper.FileWaitSleepingTimeinMilliSeconds);
                    System.Threading.Thread.Sleep(configurationHelper.GetSetting(APPLICATION.ThreadSleep_Time).IntValue());
                }
            }
            return true;
        }
        public  string GetSalesOrderStatusFileName()
        {
            throw new NotImplementedException();
        }
        public  bool CheckFileAvailability(string fullPath)
        {
            try
            {
                using (FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None, 100))
                {
                    fs.ReadByte();
                    // If we got this far the file is ready
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

    }
}