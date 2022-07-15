using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.UI.Managers;

namespace VSI.EDGEAXConnector.UI.UserControlls
{
    /// <summary>
    /// Interaction logic for FileConfiguration.xaml
    /// </summary>
    public partial class FileConfiguration : UserControl
    {
        private const string SCREEN_NAME_PRODUCT_INVENTORY = "Product/Inventory";
        private List<ApplicationSetting> lstAppsProductInventory = new List<ApplicationSetting>();
        private const string SCREEN_NAME_SALESORDER = "SalesOrder";
        private List<ApplicationSetting> lstAppsSalesOrder = new List<ApplicationSetting>();
        private const string SCREEN_NAME_STORE = "Store";
        private List<ApplicationSetting> lstAppsStore = new List<ApplicationSetting>();
        private const string SCREEN_NAME_PRICE_DISCOUNT = "Price/Discount";
        private List<ApplicationSetting> lstAppsPriceDiscount = new List<ApplicationSetting>();
        private const string SCREEN_NAME_CUSTOMER_ADDRESS = "Customer/Address";
        private List<ApplicationSetting> lstAppsCustomerAddress = new List<ApplicationSetting>();
        private const string SCREEN_NAME_CHANNEL_CONFIGURATION = "ChannelConfiguration";
        private List<ApplicationSetting> lstChannelConfiguration = new List<ApplicationSetting>();
        private const string SCREEN_NAME_OFFER_TYPE_GROUPS = "OfferTypeGroups";
        private List<ApplicationSetting> lstOfferTypeGroups = new List<ApplicationSetting>();

        public FileConfiguration()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BindProductInventory();
            BindSalesOrder();
            //BindStore();
            BindPriceDiscount();
            //BindCustomerAddress();
            BindChannelConfiguration();
            BindOfferTypeGroups();
        }

        void BindProductInventory()
        {
            lstAppsProductInventory = ApplicationSettingManager.GetApplicationSettingsByScreenName(SCREEN_NAME_PRODUCT_INVENTORY);

            gdProductInventory.ItemsSource = lstAppsProductInventory;
        }

        void BindSalesOrder()
        {
            lstAppsSalesOrder = ApplicationSettingManager.GetApplicationSettingsByScreenName(SCREEN_NAME_SALESORDER);

            gdSalesOrder.ItemsSource = lstAppsSalesOrder;
        }

        //void BindStore()
        //{
        //    lstAppsStore = ApplicationSettingManager.GetApplicationSettingsByScreenName(SCREEN_NAME_STORE);

        //    gdStore.ItemsSource = lstAppsStore;
        //}

        void BindPriceDiscount()
        {
            lstAppsPriceDiscount = ApplicationSettingManager.GetApplicationSettingsByScreenName(SCREEN_NAME_PRICE_DISCOUNT);

            gdPriceDiscount.ItemsSource = lstAppsPriceDiscount;
        }

        //void BindCustomerAddress()
        //{
        //    lstAppsCustomerAddress = ApplicationSettingManager.GetApplicationSettingsByScreenName(SCREEN_NAME_CUSTOMER_ADDRESS);

        //    gdCustomerAddress.ItemsSource = lstAppsCustomerAddress;
        //}

        /// <summary>
        /// BindChannelConfiguration gets and binds data for Channel Configiuration
        /// </summary>
        void BindChannelConfiguration()
        {
            this.lstChannelConfiguration = ApplicationSettingManager.GetApplicationSettingsByScreenName(SCREEN_NAME_CHANNEL_CONFIGURATION);

            gdChannelConfiguration.ItemsSource = this.lstChannelConfiguration;
        }

        /// <summary>
        /// BindOfferTypeGroups gets and binds data for offer types.
        /// </summary>
        void BindOfferTypeGroups()
        {
            this.lstOfferTypeGroups = ApplicationSettingManager.GetApplicationSettingsByScreenName(SCREEN_NAME_OFFER_TYPE_GROUPS);

            gdOfferTypeGroups.ItemsSource = this.lstOfferTypeGroups;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;

            bool result = ApplicationSettingManager.UpdateApplicationSettings(lstAppsProductInventory);
            result = ApplicationSettingManager.UpdateApplicationSettings(lstAppsSalesOrder);
            result = ApplicationSettingManager.UpdateApplicationSettings(lstAppsPriceDiscount);
            result = ApplicationSettingManager.UpdateApplicationSettings(lstAppsCustomerAddress);
            result = ApplicationSettingManager.UpdateApplicationSettings(this.lstChannelConfiguration);
            result = ApplicationSettingManager.UpdateApplicationSettings(this.lstOfferTypeGroups);

            if (result)
            {
                MessageBox.Show("CSV/XML Files configuration have been updated successfully!", "CSV/XML Files configuration Update", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("There is a problem please try again!", "CSV/XML Files configuration Update", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            this.Cursor = Cursors.Arrow;
        }
    }
}
