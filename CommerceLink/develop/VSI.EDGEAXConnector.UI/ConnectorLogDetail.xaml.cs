using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using VSI.EDGEAXConnector.UI.Helpers;
using VSI.EDGEAXConnector.UI.Managers;

namespace VSI.EDGEAXConnector.UI
{
    /// <summary>
    /// Interaction logic for ConnectorLogDetail.xaml
    /// </summary>
    public partial class ConnectorLogDetail : Window
    {
        private Data.Log log;

        public ConnectorLogDetail()
        {
            InitializeComponent();
        }

        public ConnectorLogDetail(int Id)
        {
            InitializeComponent();

            log = LogsManager.GetLog(Id);

            txtEventDateTime.Text = log.CreatedOn == null ? "" : log.CreatedOn.ToString();
            txtLevel.Text = log.EventLevel == null ? "" : log.EventLevel.ToString();
            txtUserName.Text = log.CreatedBy == null ? "" : log.CreatedBy.ToString();
            txtMachineName.Text = log.MachineName == null ? "" : log.MachineName.ToString();
            txtEventMessage.Text = log.EventMessage == null ? "" : log.EventMessage.ToString();
            txtErrorSource.Text = log.ErrorSource == null ? "" : log.ErrorSource.ToString();
            txtErrorClass.Text = log.ErrorClass == null ? "" : log.ErrorClass.ToString();
            txtErrorMethod.Text = log.ErrorMethod == null ? "" : log.ErrorMethod.ToString();
            txtErrorMessage.Text = log.ErrorMessage == null ? "" : log.ErrorMessage.ToString();
            txtInnerErrorMessage.Text = log.InnerErrorMessage == null ? "" : log.InnerErrorMessage.ToString();
            txtIdentityId.Text = log.IdentityId == null ? "" : log.IdentityId.ToString();
            txtErrorModule.Text = log.ErrorModule == null ? "" : log.ErrorModule.ToString();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnGenerateTextFile_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.Description = "Select Output Folder";
            fbd.ShowDialog();
            string strOutputPath = fbd.SelectedPath;

            FileHelper.CreatingLogTxtFiles("Exception_" + log.LogId.ToString(), strOutputPath, log);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the record?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                LogsManager.DeleteLog(log.LogId);
                MessageBox.Show("Log Deleted");
                this.Close();
            }
        }
    }
}
