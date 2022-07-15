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
using VSI.EDGEAXConnector.UI.Managers;

namespace VSI.EDGEAXConnector.UI.UserControlls
{
    /// <summary>
    /// Interaction logic for PaymentMethod.xaml
    /// </summary>
    public partial class PaymentMethod : UserControl
    {

        private List<VSI.EDGEAXConnector.Data.PaymentMethod> lstPaymentMethods = new List<VSI.EDGEAXConnector.Data.PaymentMethod>();
        public PaymentMethod()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BindPaymentMethods();
        }

        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            var win = new PaymentMethodAdd();
            win.Owner = CommonFunctions.GetParentWindow(this);
            win.ShowDialog();
            BindPaymentMethods();

        }
        private void BindPaymentMethods()
        {
            this.Cursor = Cursors.Wait;

            lstPaymentMethods = PaymentMethodManager.GetAllPaymentObjects();

            dgPaymentMethods.ItemsSource = lstPaymentMethods;

            this.Cursor = Cursors.Arrow;
        }

        private void btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            var paymentMethodObj = btn.Tag as VSI.EDGEAXConnector.Data.PaymentMethod;
            if (paymentMethodObj != null)
            {
                var win = new PaymentMethodAdd(paymentMethodObj);
                win.Owner = CommonFunctions.GetParentWindow(this);
                win.ShowDialog();
                BindPaymentMethods();
            }

        }

        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {

            Button btn = (Button)sender;
            var paymentMethodObj = btn.Tag as VSI.EDGEAXConnector.Data.PaymentMethod;
            if (paymentMethodObj != null)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure to delete this record?", "Payment Method", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    string res = PaymentMethodManager.DeletePaymentMethod(paymentMethodObj);
                    if (res=="true")
                    {
                        MessageBox.Show("Payment Method has been deleted successfully!", "Payment Method", MessageBoxButton.OK, MessageBoxImage.Information);
                        BindPaymentMethods();
                    }
                    else if (res == "false")
                    {
                        MessageBox.Show("There is a problem. Please try again!", "Payment Method", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        MessageBox.Show("This Payment Method has been using as Parent Payment Method, First delete its child records", "Payment Method", MessageBoxButton.OK, MessageBoxImage.Information);

                    }
                }
            }


        }

   
    }
}
