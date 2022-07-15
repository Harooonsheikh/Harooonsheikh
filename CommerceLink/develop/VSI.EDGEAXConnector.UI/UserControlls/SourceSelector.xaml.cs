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
using VSI.EDGEAXConnector.UI.Classes;
using VSI.EDGEAXConnector.UI.ViewModel;

namespace VSI.EDGEAXConnector.UI.UserControlls
{
    /// <summary>
    /// Interaction logic for SourceSelector.xaml
    /// </summary>
    public partial class SourceSelector : UserControl
    {
        public SourceSelector()
        {
            InitializeComponent();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as MapsViewModel;
            if ((cboxDest.SelectedItem as ComboBoxItem).Content.ToString() == "ECommerce (DemandWare)" && (cboxSrc.SelectedItem as ComboBoxItem).Content.ToString() == "ERP (Microsoft Dynamics AX)")
            {
                viewModel.Direction = Common.MapDirection.ErpToEcom;
            }
            else if ((cboxDest.SelectedItem as ComboBoxItem).Content.ToString() == "ERP (Microsoft Dynamics AX)" && (cboxSrc.SelectedItem as ComboBoxItem).Content.ToString() == "ECommerce (DemandWare)")
            {
                viewModel.Direction = Common.MapDirection.EcomToErp;
            }
            viewModel.ConfigureMap();

            (this.Parent as Grid).Visibility = System.Windows.Visibility.Collapsed;
            var parentControl = ((this.Parent as Grid).Parent as Grid);
            object obj = parentControl.FindName("grdMapping");
            System.Windows.Controls.Grid grdMapping = (System.Windows.Controls.Grid)obj;
            grdMapping.Visibility = System.Windows.Visibility.Visible;

            //this.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void cboxDest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboxDest.SelectedValue != null)
            {
                if (cboxDest.SelectedValue.Equals("ERP (Microsoft Dynamics AX)"))
                {
                    cboxSrc.SelectedIndex = 1;
                }
                else
                {
                    cboxSrc.SelectedIndex = 0;
                }
            }
        }

        private void cboxSrc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboxSrc.SelectedValue != null)
            {
                if (cboxSrc.SelectedValue.Equals("ERP (Microsoft Dynamics AX)"))
                {
                    cboxDest.SelectedIndex = 1;
                }
                else
                {
                    cboxDest.SelectedIndex = 0;
                }
            }
        }
    }
}
