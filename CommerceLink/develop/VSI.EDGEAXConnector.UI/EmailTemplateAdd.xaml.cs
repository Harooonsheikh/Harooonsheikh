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
using VSI.EDGEAXConnector.UI.Managers;

namespace VSI.EDGEAXConnector.UI
{
    /// <summary>
    /// Interaction logic for EmailTemplateAdd.xaml
    /// </summary>
    public partial class EmailTemplateAdd : Window
    {
        private bool isUpdate = false;
        public EmailTemplateAdd()
        {
            InitializeComponent();
        }

        public EmailTemplateAdd(EmailTemplate templateObj)
        {
            InitializeComponent();

            isUpdate = true;
            btnSave.Content = "Update";

            this.lblId.Content = templateObj.EmailTemplateId;
            this.txtName.Text = templateObj.Name;
            this.txtSubject.Text = templateObj.Subject;
            this.txtBody.Text = templateObj.Body;
            this.txtFooter.Text = templateObj.Footer;
            this.chkIsActive.IsChecked = templateObj.IsActive;
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string strMessage = isUpdate ? "Update" : "Add";

            if (txtName.Text != "" && txtSubject.Text != "" && txtBody.Text != "" && txtFooter.Text != "")
            {
                EmailTemplate templateObj = new EmailTemplate();
                templateObj.EmailTemplateId = Convert.ToInt32(lblId.Content);
                templateObj.Name = this.txtName.Text;
                templateObj.Subject = this.txtSubject.Text;
                templateObj.Body = this.txtBody.Text;
                templateObj.Footer = this.txtFooter.Text;
                templateObj.IsActive = Convert.ToBoolean(this.chkIsActive.IsChecked);


                bool result = false;

                if (isUpdate)
                {
                    result = EmailTemplateManager.UpdateTemplate(templateObj);
                }
                else
                {
                    result = EmailTemplateManager.AddTemplate(templateObj);
                }
                this.Close();


                if (result)
                {
                    MessageBoxResult btnResult = MessageBox.Show("Email Template has been " + (isUpdate ? "updated" : "added") + " successfully!", strMessage + " Email Template", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("There is a problem. Please try again!", strMessage + " Email Template", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please enter Name, Subject, Body & Footer then press" + (isUpdate ? "updated" : "save") + "!", strMessage + " Email Template", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
