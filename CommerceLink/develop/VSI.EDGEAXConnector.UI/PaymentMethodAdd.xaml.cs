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
using System.Windows.Shapes;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.UI.Classes;
using VSI.EDGEAXConnector.UI.Managers;

namespace VSI.EDGEAXConnector.UI
{
    /// <summary>
    /// Interaction logic for PaymentMethodAdd.xaml
    /// </summary>
    public partial class PaymentMethodAdd : Window
    {
        private bool isUpdate = false;
        public PaymentMethodAdd()
        {
            InitializeComponent();

            LoadCombo();

   

        }

        public PaymentMethodAdd(PaymentMethod paymentObj)
        {
            InitializeComponent();

            isUpdate = true;
            btnSave.Content = "Update";

            this.lblId.Content = paymentObj.PaymentMethodId;
            this.chkHasSubMethod.IsChecked = paymentObj.HasSubMethod;
            this.txtErpCode.Text = paymentObj.ErpCode;
            this.txtECommerceValue.Text = paymentObj.ECommerceValue;
            this.txtErpValue.Text = paymentObj.ErpValue;
            this.chkIsPrepayment.IsChecked = paymentObj.IsPrepayment;
            this.chkIsCreditCard.IsChecked = paymentObj.IsCreditCard;
            this.chkUsePaymentConnector.IsChecked = paymentObj.UsePaymentConnector;
            this.txtServiceAccountId.Text = paymentObj.ServiceAccountId;
            LoadCombo();
            this.cmbParentPaymentMethod.SelectedValue = paymentObj.ParentPaymentMethodId;
      

        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            string strMessage = isUpdate ? "Update" : "Add";

            if (  txtECommerceValue.Text != "" && txtErpValue.Text != "" && txtErpCode.Text != "")
            {
                PaymentMethod paymentMethodObj = new PaymentMethod();
                paymentMethodObj.PaymentMethodId = Convert.ToInt32(lblId.Content);
                if (cmbParentPaymentMethod.SelectedItem !=null)
                {
                paymentMethodObj.ParentPaymentMethodId = Convert.ToInt32(cmbParentPaymentMethod.SelectedItem.ToString());
                }
                paymentMethodObj.ECommerceValue = this.txtECommerceValue.Text;
                paymentMethodObj.ErpValue = this.txtErpValue.Text;
                paymentMethodObj.HasSubMethod = Convert.ToBoolean(this.chkHasSubMethod.IsChecked);
                paymentMethodObj.ErpCode = this.txtErpCode.Text;
                paymentMethodObj.IsPrepayment = Convert.ToBoolean(this.chkIsPrepayment.IsChecked);
                paymentMethodObj.IsCreditCard = Convert.ToBoolean(this.chkIsCreditCard.IsChecked);
                paymentMethodObj.UsePaymentConnector = Convert.ToBoolean(this.chkUsePaymentConnector.IsChecked);
                paymentMethodObj.ServiceAccountId = this.txtServiceAccountId.Text;

                bool result = false;

                if (isUpdate)
                {
                    result = PaymentMethodManager.UpdatePaymentMethod(paymentMethodObj);

                }
                else
                {
                    result = PaymentMethodManager.AddPaymentMethod(paymentMethodObj);
                }
                this.Close();


                if (result)
                {
                    MessageBoxResult btnResult = MessageBox.Show("Payment Method has been " + (isUpdate ? "updated" : "added") + " successfully!", strMessage + "Payment Method", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("There is a problem. Please try again!", strMessage + "Payment Method", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please enter ECommerce Value,Erp Value,Erp Code then press" + (isUpdate ? "updated" : "save") + "!", strMessage + "Payment Method", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void LoadCombo()
        {

            List<int> lstPaymentMethods = PaymentMethodManager.GetAllPaymentObjects().Select(i => i.PaymentMethodId).ToList();

            if (lstPaymentMethods.Count>0)
            {
                 cmbParentPaymentMethod.ItemsSource = lstPaymentMethods;
                 cmbParentPaymentMethod.SelectedIndex = 0;

            }
           
          

        }
     
    }
}
