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
    /// Interaction logic for ERPAdapter.xaml
    /// </summary>
    public partial class ERPAdapter : UserControl
    {
        private const string SCREEN_NAME_GENERAL = "ERPAdapterGeneral";
        private List<ApplicationSetting> lstAppsGeneral = new List<ApplicationSetting>();
        private const string SCREEN_NAME_PRODUCT = "ERPAdapterProduct";
        private List<ApplicationSetting> lstAppsProduct = new List<ApplicationSetting>();
        private const string SCREEN_NAME_CUSTOMER = "ERPAdapterCustomer";
        private List<ApplicationSetting> lstAppsCustomer = new List<ApplicationSetting>();
        private const string SCREEN_NAME_SALESORDER = "ERPAdapterSalesOrder";
        private List<ApplicationSetting> lstAppsSalesOrder = new List<ApplicationSetting>();
        private const string SCREEN_NAME_CUSTOMATTRIBUTES = "ERPAdapterCustomAttributes";
        private List<ApplicationSetting> lstAppsCustomAttributes = new List<ApplicationSetting>();
        private const string SCREEN_NAME_ExternalWebAPI = "ERPAdapterExternalWebAPI";
        private List<ApplicationSetting> lstExternalWebAPI = new List<ApplicationSetting>();

        public ERPAdapter()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BindGeneral();
            BindProduct();
            BindCustomer();
            BindSalesOrder();
            //BindCustomAttributes();
            BindExternalWebAPI();
        }

        void BindGeneral()
        {
            lstAppsGeneral = ApplicationSettingManager.GetApplicationSettingsByScreenName(SCREEN_NAME_GENERAL);

            gdGeneral.ItemsSource = lstAppsGeneral;

        }
        void BindProduct()
        {
            lstAppsProduct = ApplicationSettingManager.GetApplicationSettingsByScreenName(SCREEN_NAME_PRODUCT);

            gdProduct.ItemsSource = lstAppsProduct;
        }
        void BindCustomer()
        {
            lstAppsCustomer = ApplicationSettingManager.GetApplicationSettingsByScreenName(SCREEN_NAME_CUSTOMER);

            gdCustomer.ItemsSource = lstAppsCustomer;
        }
        void BindSalesOrder()
        {
            lstAppsSalesOrder = ApplicationSettingManager.GetApplicationSettingsByScreenName(SCREEN_NAME_SALESORDER);

            gdSalesOrder.ItemsSource = lstAppsSalesOrder;
        }

        //void BindCustomAttributes()
        //{
        //    lstAppsCustomAttributes = ApplicationSettingManager.GetApplicationSettingsByScreenName(SCREEN_NAME_CUSTOMATTRIBUTES);

        //    gdCustomAttributes.ItemsSource = lstAppsCustomAttributes;
        //}
        void BindExternalWebAPI()
        {
            lstExternalWebAPI = ApplicationSettingManager.GetApplicationSettingsByScreenName(SCREEN_NAME_ExternalWebAPI);

            gdExternalWebAPI.ItemsSource = lstExternalWebAPI;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;

            bool result = ApplicationSettingManager.UpdateApplicationSettings(lstAppsGeneral);
            result = ApplicationSettingManager.UpdateApplicationSettings(lstAppsCustomer);
            result = ApplicationSettingManager.UpdateApplicationSettings(lstAppsSalesOrder);
            result = ApplicationSettingManager.UpdateApplicationSettings(lstAppsCustomAttributes);
            result = ApplicationSettingManager.UpdateApplicationSettings(lstExternalWebAPI);

            if (result)
            {
                MessageBox.Show("ERP Adapter configuration have been updated successfully!", "ERP Adapter configuration Update", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("There is a problem please try again!", "ERP Adapter configuration Update", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            this.Cursor = Cursors.Arrow;
        }
    }
}
