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
using VSI.EDGEAXConnector.UI.Managers;

namespace VSI.EDGEAXConnector.UI.UserControlls
{
    /// <summary>
    /// Interaction logic for EComAdapter.xaml
    /// </summary>
    public partial class EComAdapter : UserControl
    {
        private const string SCREEN_NAME_GENERAL = "EComAdapterGeneral";
        private List<ApplicationSetting> lstAppsGeneral = new List<ApplicationSetting>();
        private const string SCREEN_NAME_WEBAPI = "EComAdapterWebAPI";
        private List<ApplicationSetting> lstAppsWebAPI = new List<ApplicationSetting>();
        public EComAdapter()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BindGeneral();
            BindWebAPI();
        }

        void BindGeneral()
        {
            lstAppsGeneral = ApplicationSettingManager.GetApplicationSettingsByScreenName(SCREEN_NAME_GENERAL);

            gdGeneral.ItemsSource = lstAppsGeneral;

        }

        void BindWebAPI()
        {
            lstAppsWebAPI = ApplicationSettingManager.GetApplicationSettingsByScreenName(SCREEN_NAME_WEBAPI);

            gdWebAPI.ItemsSource = lstAppsWebAPI;

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;

            bool result = ApplicationSettingManager.UpdateApplicationSettings(lstAppsGeneral);
            result = ApplicationSettingManager.UpdateApplicationSettings(lstAppsWebAPI);

            if (result)
            {
                MessageBox.Show("ECOM Adapter configuration have been updated successfully!", "ECOM Adapter configuration Update", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("There is a problem please try again!", "ECOM Adapter configuration Update", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            this.Cursor = Cursors.Arrow;
        }
    }
}
