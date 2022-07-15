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
    /// Interaction logic for ConfigurableObjects.xaml
    /// </summary>
    public partial class ConfigurableObjects : UserControl
    {
        private List<VSI.EDGEAXConnector.Data.ConfigurableObject> lstConfigObjects = new List<VSI.EDGEAXConnector.Data.ConfigurableObject>();
        public ConfigurableObjects()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;

            lstConfigObjects = ConfigurableObjectManager.GetAllConfigurableObjects();

            dgConfigurableObjects.ItemsSource = lstConfigObjects;

            this.Cursor = Cursors.Arrow;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            var conObj = btn.Tag as VSI.EDGEAXConnector.Data.ConfigurableObject;
            if (conObj != null)
            {
                bool result = ConfigurableObjectManager.UpdateConfigurableObjectById(conObj);
                if (result)
                {
                    MessageBox.Show("Configurable Object has been updated successfully!", "Configurable Object Update", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("There is a problem. Please try again!", "Configurable Object Update Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
