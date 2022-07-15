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
using VSI.EDGEAXConnector.UI.Classes;
using VSI.EDGEAXConnector.UI.Managers;

namespace VSI.EDGEAXConnector.UI.UserControlls
{
    /// <summary>
    /// Interaction logic for EmailTemplates.xaml
    /// </summary>
    public partial class EmailTemplates : UserControl
    {
        private List<VSI.EDGEAXConnector.Data.EmailTemplate> lstTemplates = new List<VSI.EDGEAXConnector.Data.EmailTemplate>();

        public EmailTemplates()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BindEmailTempaltes();
        }

        private void BindEmailTempaltes()
        {
            this.Cursor = Cursors.Wait;

            lstTemplates = EmailTemplateManager.GetAllTemplates();

            dgEmailTemplates.ItemsSource = lstTemplates;

            this.Cursor = Cursors.Arrow;
        }

        private void btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            var templateObj = btn.Tag as EmailTemplate;
            if (templateObj != null)
            {
                var win = new EmailTemplateAdd(templateObj);
                win.Owner = CommonFunctions.GetParentWindow(this);
                win.ShowDialog();
                BindEmailTempaltes();
            }
        }

        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            var templateObj = btn.Tag as EmailTemplate;
            if (templateObj != null)
            {
                MessageBoxResult result = MessageBox.Show("Are you shure to delete this record?", "Delete Template", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    bool res = EmailTemplateManager.DeleteTemplate(templateObj);
                    if (res)
                    {
                        MessageBox.Show("Email Template has been deleted successfully!", "Delete Email Template", MessageBoxButton.OK, MessageBoxImage.Information);
                        BindEmailTempaltes();
                    }
                    else
                    {
                        MessageBox.Show("There is a problem. Please try again!", "Delete Email Template", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            var win = new EmailTemplateAdd();
            win.Owner = CommonFunctions.GetParentWindow(this);
            win.ShowDialog();
            BindEmailTempaltes();
        }
    }
}
