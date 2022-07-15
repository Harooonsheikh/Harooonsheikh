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
    /// Interaction logic for EmailSetting.xaml
    /// </summary>
    public partial class EmailSetting : UserControl
    {
        private const string SCREEN_NAME = "EmailSetting";
        private List<ApplicationSetting> lstApps = new List<ApplicationSetting>();
        public EmailSetting()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;

            lstApps = ApplicationSettingManager.GetApplicationSettingsByScreenName(SCREEN_NAME);

            gdEmailSetting.ItemsSource = lstApps;

            this.Cursor = Cursors.Arrow;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;

            bool result = ApplicationSettingManager.UpdateApplicationSettings(lstApps);

            if(result)
            {
                MessageBox.Show("Email settings have been updated successfully!", "Email Settings Update", MessageBoxButton.OK, MessageBoxImage.Information);            
            }
            else
            {
                MessageBox.Show("There is a problem please try again!", "Email Settings Update", MessageBoxButton.OK, MessageBoxImage.Error);            
            }
            
            this.Cursor = Cursors.Arrow;
        }
    }
}
