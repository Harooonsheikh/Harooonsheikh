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
using VSI.EDGEAXConnector.UI.Managers;

namespace VSI.EDGEAXConnector.UI.UserControlls
{
    /// <summary>
    /// Interaction logic for JobsDetail.xaml
    /// </summary>
    public partial class JobsDetail : UserControl
    {
        private List<VSI.EDGEAXConnector.Data.Job> lstJobs = new List<VSI.EDGEAXConnector.Data.Job>();

        public JobsDetail()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;

            lstJobs = JobsManager.GetAllJobsByType(false);

            dgJobs.ItemsSource = lstJobs;

            this.Cursor = Cursors.Arrow;
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            var jobObj = btn.Tag as VSI.EDGEAXConnector.Data.Job;
            if (jobObj != null)
            {
                bool result = JobsManager.UpdateJobById(jobObj);
                if(result)
                {
                    MessageBox.Show(jobObj.JobName + " job has been updated successfully! Please restart service now.", "Job Update", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("There is a problem. Please try again!", "Job Update Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
