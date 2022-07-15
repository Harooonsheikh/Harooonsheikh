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
using System.Windows.Navigation;
using System.Windows.Shapes;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.UI.Helpers;
using VSI.EDGEAXConnector.UI.Managers;

namespace VSI.EDGEAXConnector.UI.UserControlls
{
    /// <summary>
    /// Interaction logic for ConnectorLogs.xaml
    /// </summary>
    public partial class ConnectorLogs : UserControl
    {
        private List<VSI.EDGEAXConnector.Data.Log> lstLogs = new List<VSI.EDGEAXConnector.Data.Log>();

        public ConnectorLogs()
        {
            InitializeComponent();
            dpkFromDate.Text = DateTime.UtcNow.AddDays(-1).ToString();
            dpkToDate.Text = DateTime.UtcNow.ToString();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //this.Cursor = Cursors.Wait;

            //lstLogs = LogsManager.GetLogsByType("Fatal");

            //dgErrorLogs.ItemsSource = lstLogs;

            //this.Cursor = Cursors.Arrow;
        }

        private void cboEventLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //this.Cursor = Cursors.Wait;
            //var type = cmbErrorLevel.SelectedValue.ToString();

            //if (type != null)
            //{
            //    lstLogs = LogsManager.GetLogsByType(type);
            //    dgErrorLogs.ItemsSource = lstLogs;
            //}

            //this.Cursor = Cursors.Arrow;
        }

        private void dgErrorLogs_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;
            if (dep.GetType().Name.Equals("TextBlock"))
            {
                string Content = (dep as TextBlock).Text;

                MessageBox.Show(Content, "Error Log");
            }
        }

        private void GetAndBindData()
        {
            this.Cursor = Cursors.Wait;
            string type = cmbErrorLevel.SelectedValue.ToString();

            if (type != null)
            {
                DateTime fromDate = Convert.ToDateTime(dpkFromDate.Text);
                DateTime toDate = Convert.ToDateTime(dpkToDate.Text);

                lstLogs = LogsManager.GetLogs(type, fromDate, toDate);
                dgErrorLogs.ItemsSource = lstLogs;
            }

            this.Cursor = Cursors.Arrow;
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            GetAndBindData();
        }

        private void btnText_Click(object sender, RoutedEventArgs e)
        {
            if (dgErrorLogs.SelectedIndex != -1)
            {
                List<Log> selectedLog = new List<Log>();

                foreach (Log log in dgErrorLogs.SelectedItems)
                {
                    selectedLog.Add(log);
                }

                // = dgErrorLogs.SelectedItem as Log;

                System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
                fbd.Description = "Select Output Folder";
                fbd.ShowDialog();
                string strOutputPath = fbd.SelectedPath;

                foreach (Log log in dgErrorLogs.SelectedItems)
                {
                    FileHelper.CreatingLogTxtFiles("Exception_" + log.LogId.ToString(), strOutputPath, log);
                }

            }
            else
            {
                MessageBox.Show("Please select any row and then Generate Text File.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {


            this.Cursor = Cursors.Wait;
            MessageBoxResult result = MessageBox.Show("Are you sure to delete the Logs?", "EdgeAX CommerceLink Logs", MessageBoxButton.YesNo, MessageBoxImage.Question);
             if (result == MessageBoxResult.Yes)
             {
                 string type = cmbErrorLevel.SelectedValue.ToString();

                 if (type != null)
                 {
                     DateTime fromDate = Convert.ToDateTime(dpkFromDate.Text);
                     DateTime toDate = Convert.ToDateTime(dpkToDate.Text);

                     bool res = LogsManager.DeleteLogs(type, fromDate, toDate);
                 

                     if (res == true)
                     {
                         MessageBox.Show("EdgeAX CommerceLink Logs has been deleted successfully!", "EdgeAX CommerceLink Logs", MessageBoxButton.OK, MessageBoxImage.Information);
                        lstLogs = LogsManager.GetLogs(type, fromDate, toDate);
                        dgErrorLogs.ItemsSource = lstLogs;
                     }
                     else
                     {
                         MessageBox.Show("There is a problem. Please try again!", "EdgeAX CommerceLink Logs", MessageBoxButton.OK, MessageBoxImage.Error);
                     }

                 }
             }

            this.Cursor = Cursors.Arrow;

        }

        private void dgErrorLogs_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int Id = 0;

            // Get the Log Id
            Log log = (Log)dgErrorLogs.SelectedItem;
            Id = log.LogId;

            // Display the Log Detail
            ConnectorLogDetail connectorLogDetail = new ConnectorLogDetail(Id);
            connectorLogDetail.ShowDialog();

            // Check if Log was deleted to refresh the list
            Log logAfterDisplay = LogsManager.GetLog(Id);
            if (logAfterDisplay.LogId == null || logAfterDisplay.LogId == 0)
            {
                GetAndBindData();
            }
        }
    }
}
