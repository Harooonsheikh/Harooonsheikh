using System;
using System.Collections.Generic;
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
using VSI.EDGEAXConnector.UI.Classes;
using VSI.EDGEAXConnector.UI.Helpers;

namespace VSI.EDGEAXConnector.UI.UserControlls
{
    /// <summary>
    /// Interaction logic for CommerceLinkFiles.xaml
    /// </summary>
    public partial class CommerceLinkFiles : UserControl
    {
        string source = string.Empty;
        string destination = string.Empty;
        string[] sourceFileArray;
        string[] destinationFileArray;
        List<FileClass> sourceFileData = new List<FileClass>();
        List<FileClass> destinationFileData = new List<FileClass>();
        public CommerceLinkFiles()
        {
            InitializeComponent();
            BindFile();
        }

        private void btnMoveSelected_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder strData = new StringBuilder();
            try
            {                
                foreach (var item in dgFailed.SelectedItems)
                {
                    string response = FileHelper.MoveFileToLocalFolder(source + (item as FileClass).Name, "", destination);
                    if (response != "")
                    {
                        strData.Append(DateTime.UtcNow + " : " + (item as FileClass).Name + " moved to " + destination + Environment.NewLine);
                    }
                    //GlobalUI.MainWindowUI.txtConnectorTracking.AppendText(strData.ToString());
                }
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex);
            }
            BindFile();
        }

        private void btnMoveAll_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder strData = new StringBuilder();
            try
            {
                foreach (var item in dgFailed.Items)
                {
                    string response = FileHelper.MoveFileToLocalFolder(source + (item as FileClass).Name, "", destination);
                    if (response != "")
                    {
                        strData.Append(DateTime.UtcNow + " : " + (item as FileClass).Name + " moved to " + destination + Environment.NewLine);
                    }
                    //GlobalUI.MainWindowUI.txtConnectorTracking.AppendText(strData.ToString());
                }
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex);
            }
            BindFile();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            BindFile();
        }

        private void cmbEntity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /// AQ: ConnectorCustomerCSVPath updated with SALESORDER.Singlefile_Input_Path
            if (cmbEntity.SelectedIndex == 0)
            {
                source = ConfigurationHelper.GetSetting(SALESORDER.Singlefile_Input_Path) + "\\Failed\\";
                destination = ConfigurationHelper.GetSetting(SALESORDER.Singlefile_Input_Path);
                BindFile();
            }

            /// AQ: ConnectorCustomerCSVPath updated with CUSTOMER.Local_Input_Path
            if (cmbEntity.SelectedIndex == 1)
            {
                source = ConfigurationHelper.GetSetting(CUSTOMER.Local_Input_Path) + "\\Failed\\";
                destination = ConfigurationHelper.GetSetting(CUSTOMER.Local_Input_Path);
                BindFile();
            }
        }

        private void BindFile()
        {
            //Checking if the source and destination folder exist or not.
            if (Directory.Exists(source) | Directory.Exists(destination))
            {
                //getting file names
                sourceFileArray = Directory.GetFiles(source);
                destinationFileArray = Directory.GetFiles(destination);

                sourceFileData.Clear();
                destinationFileData.Clear();

                //Adding source file names and their dates into the dictionary
                foreach (string files in sourceFileArray)
                {
                    FileClass fObj = new FileClass();
                    fObj.Name = System.IO.Path.GetFileName(files);
                    fObj.DateTime = File.GetLastWriteTime(files).ToString();
                    sourceFileData.Add(fObj);
                }

                if (dgFailed != null)
                {
                    this.dgFailed.ItemsSource = sourceFileData;
                    //this.dgFailed.UpdateLayout();
                    this.dgFailed.Items.Refresh();
                }
                //Adding Destination file names and their dates into the dictionary to display on the listView
                foreach (string files in destinationFileArray)
                {
                    FileClass fObj = new FileClass();
                    fObj.Name = System.IO.Path.GetFileName(files);
                    fObj.DateTime = File.GetLastWriteTime(files).ToString();
                    destinationFileData.Add(fObj);
                }
                if (dgProcessed != null)
                {
                    this.dgProcessed.ItemsSource = destinationFileData;
                    //this.dgProcessed.UpdateLayout();
                    this.dgProcessed.Items.Refresh();
                }
            }
            else
            {
                //MessageBox.Show("EdgeAX CommerceLink Source and Destination Paths are not configured! Please set from Configuration >> Data Files >> SalesOrder & Customer Section. Thanks!", "Edge AX CommerceLink Service files path warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                MessageBox.Show("EdgeAX CommerceLink Source and Destination Paths are not configured! Please set from Configuration >> Data Files >> SalesOrder section. Thanks!", "Edge AX CommerceLink Service files path warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnProcessed_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    public class FileClass
    {
        public string Name { get; set; }
        public string DateTime { get; set; }
    }
}
