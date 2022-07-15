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
using VSI.EDGEAXConnector.Configuration;
using System.ServiceProcess;
using VSI.EDGEAXConnector.Logging;
using System.Threading;
using System.Timers;
using VSI.EDGEAXConnector.UI.Managers;
using VSI.EDGEAXConnector.Data;

namespace VSI.EDGEAXConnector.UI.UserControlls
{
    /// <summary>
    /// Interaction logic for CommerceLinkServices.xaml
    /// </summary>
    public partial class CommerceLinkServices : UserControl
    {
        private const string SCREEN_NAME = "Services";
        private List<ApplicationSetting> lstApps = new List<ApplicationSetting>();
        public CommerceLinkServices()
        {
            InitializeComponent();
            BindWindowService();

            //var aTimer = new System.Timers.Timer(1000);
            //aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            //aTimer.Interval = 10000;
            //aTimer.Enabled = true;  
        }

         
        //private void OnTimedEvent(object source, ElapsedEventArgs e)
        //{
        //    BindWindowService();
        //}
        
        public void BindWindowService()
        {
            try
            {
                string[] serviceNamesArray = { ConfigurationHelper.GetSetting(APPLICATION.Windows_Service), ConfigurationHelper.GetSetting(APPLICATION.File_Service) };
                List<ServiceClass> lstServices = new List<ServiceClass>();

                ServiceController[] ExistingServices = ServiceController.GetServices();

                ServiceController sc;
                foreach (string data in serviceNamesArray)
                {
                    foreach (ServiceController serviceController in ExistingServices)
                    {
                        if (serviceController.DisplayName == data)
                        {
                            sc = new ServiceController(data);

                            ServiceClass sObj = new ServiceClass();
                            sObj.Name = data;
                            sObj.Status = sc.Status.ToString();

                            if (sc.Status.ToString() == "Stopped")
                            {
                                sObj.IsStart = true;
                                sObj.IsStop = sObj.IsRestart = false;
                            }
                            else if (sc.Status.ToString() == "Running")
                            {
                                sObj.IsStart = false;
                                sObj.IsStop = sObj.IsRestart = true;
                            }

                            lstServices.Add(sObj);
                        }
                    }
                }

                this.dgService.ItemsSource = lstServices;
                this.dgService.Items.Refresh();
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex);
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                var serviceObj = btn.Tag as ServiceClass;
                if (serviceObj != null)
                {
                    ServiceController sc = new ServiceController(serviceObj.Name);
                    sc.Start();
                    Thread.Sleep(2000);
                    sc.Refresh();
                }
                BindWindowService();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Service Error", MessageBoxButton.OK, MessageBoxImage.Error);
                CustomLogger.LogException(ex, "Console");
            }
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                var serviceObj = btn.Tag as ServiceClass;
                if (serviceObj != null)
                {
                    ServiceController sc = new ServiceController(serviceObj.Name);
                    sc.Stop();
                    Thread.Sleep(2000);
                    sc.Refresh();
                }
                BindWindowService();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Service Error", MessageBoxButton.OK, MessageBoxImage.Error);
                CustomLogger.LogException(ex, "Console");
            }
        }

        private void btnRestart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                var serviceObj = btn.Tag as ServiceClass;
                if (serviceObj != null)
                {
                    ServiceController sc = new ServiceController(serviceObj.Name);
                    sc.Refresh();
                    Thread.Sleep(2000);
                    sc.Refresh();
                }
                BindWindowService();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Service Error", MessageBoxButton.OK, MessageBoxImage.Error);
                CustomLogger.LogException(ex, "Console");
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            BindWindowService();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;

            bool result = ApplicationSettingManager.UpdateApplicationSettings(lstApps);

            if (result)
            {
                MessageBox.Show("Service Details have been updated successfully!", "Service Details Update", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("There is a problem please try again!", "Service Details Update", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            this.Cursor = Cursors.Arrow;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;

            lstApps = ApplicationSettingManager.GetApplicationSettingsByScreenName(SCREEN_NAME);

            gdServiceFields.ItemsSource = lstApps;

            this.Cursor = Cursors.Arrow;
        }
    }

    public class ServiceClass
    {
        public string Name { get; set; }
        public string Status { get; set; }

        public bool IsStart { get; set; }
        public bool IsStop { get; set; }
        public bool IsRestart { get; set; }
    }
}
