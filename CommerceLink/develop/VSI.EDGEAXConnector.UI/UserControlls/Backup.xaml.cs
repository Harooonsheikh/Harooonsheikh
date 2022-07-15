using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.UI.Helpers;

namespace VSI.EDGEAXConnector.UI.UserControlls
{
    /// <summary>
    /// Interaction logic for Backup.xaml
    /// </summary>
    public partial class Backup : UserControl
    {
        public Backup()
        {
            InitializeComponent();
        }

        private void btnSetBackupOutputPath_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
                fbd.Description = "Select Backup Output Folder";
                fbd.ShowDialog();
                string Source = fbd.SelectedPath;

                txtBackupOutputPath.Text = Source;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, "Console");
            }
        }

        private void btnOpenOutputFolder_Click(object sender, RoutedEventArgs e)
        {
            if (txtBackupOutputPath.Text != "")
            {
                Process.Start(txtBackupOutputPath.Text);
            }
        }

        private void btnCreateBackup_Click(object sender, RoutedEventArgs e)
        {
            if (txtBackupOutputPath.Text != "")
            {
                CreateBackup();
            }
            else
            {
                MessageBox.Show("Please select backup output path.", "Create Backup", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void CreateBackup()
        {
            /// AQ: Commented to get UI working
            try
            {
                string strBackupFolder = DateTime.UtcNow.ToString("dd-MM-yyyy_hh-mm-ss") + "_backup";
                var folderPath = System.IO.Path.Combine(txtBackupOutputPath.Text, strBackupFolder);
                if (!Directory.Exists(folderPath))
                {
                    System.IO.Directory.CreateDirectory(folderPath);
                }

                //string strBackupFolderMaps = "Maps";
                //var folderPathMaps = System.IO.Path.Combine(folderPath, strBackupFolderMaps);
                //if (!Directory.Exists(folderPathMaps))
                //{
                //    System.IO.Directory.CreateDirectory(folderPathMaps);
                //}

                //string strBackupFolderResolvers = "Resolvers";
                //var folderPathResolvers = System.IO.Path.Combine(folderPath, strBackupFolderResolvers);
                //if (!Directory.Exists(folderPathResolvers))
                //{
                //    System.IO.Directory.CreateDirectory(folderPathResolvers);
                //}

                string strBackupFolderXMLs = "XMLs";
                var folderPathXMLs = System.IO.Path.Combine(folderPath, strBackupFolderXMLs);
                if (!Directory.Exists(folderPathXMLs))
                {
                    System.IO.Directory.CreateDirectory(folderPathXMLs);
                }

                //var pathMaps = VSI.EDGEAXConnector.Configuration.ConfigurationHelper.MapsXmlPath;
                //var filesMaps = new DirectoryInfo(pathMaps).GetFiles("*.xml");
                //foreach (var file in filesMaps)
                //{
                //    FileHelper.CopyFileToLocalFolder(file.Name, pathMaps, folderPathMaps);
                //}

                //var pathResolvers = VSI.EDGEAXConnector.Configuration.ConfigurationHelper.ResolversXmlPath;
                //var filesResolvers = new DirectoryInfo(pathResolvers).GetFiles("*.xml");
                //foreach (var file in filesResolvers)
                //{
                //    FileHelper.CopyFileToLocalFolder(file.Name, pathResolvers, folderPathResolvers);
                //}

                // var pathXMLs = VSI.EDGEAXConnector.Configuration.ConfigurationHelper.XmlPath;
                var pathXMLs = ConfigurationHelper.GetSetting(APPLICATION.XML_Base_Path);
                var filesXMLs = new DirectoryInfo(pathXMLs).GetFiles("*.xml");
                foreach (var file in filesXMLs)
                {
                    FileHelper.CopyFileToLocalFolder(file.Name, pathXMLs, folderPathXMLs);
                }

                MessageBoxResult result = MessageBox.Show("Backup has been created successfully! Do you want to open the backup directory?", "Create Backup", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    Process.Start(txtBackupOutputPath.Text);
                }
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex);
                MessageBox.Show("There is any problem, Please try again.", "Create Backup", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

    }
}
