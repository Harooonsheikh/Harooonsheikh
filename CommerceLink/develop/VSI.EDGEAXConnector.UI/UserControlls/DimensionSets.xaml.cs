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
    /// Interaction logic for DimensionSets.xaml
    /// </summary>
    public partial class DimensionSets : UserControl
    {
        private List<VSI.EDGEAXConnector.Data.DimensionSet> lstDimensionSetObjects = new List<VSI.EDGEAXConnector.Data.DimensionSet>();
        public DimensionSets()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;

            lstDimensionSetObjects = DimensionSetManager.GetAllDimensionSets();

            dgDimensionSets.ItemsSource = lstDimensionSetObjects;

            this.Cursor = Cursors.Arrow;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            var dimObj = btn.Tag as VSI.EDGEAXConnector.Data.DimensionSet;
            if (dimObj != null)
            {
                bool result = DimensionSetManager.UpdateDimensionSetById(dimObj);
                if (result)
                {
                    MessageBox.Show("Dimension Set has been updated successfully!", "Dimension Set Update", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("There is a problem. Please try again!", "Dimension Set Update Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
