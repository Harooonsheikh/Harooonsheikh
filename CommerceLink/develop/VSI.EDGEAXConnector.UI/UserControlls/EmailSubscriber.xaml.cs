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
    /// Interaction logic for EmailSubscriber.xaml
    /// </summary>
    public partial class EmailSubscriber : UserControl
    {
        private List<VSI.EDGEAXConnector.Data.Subscriber> lstSubscribers = new List<VSI.EDGEAXConnector.Data.Subscriber>();

        public EmailSubscriber()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BindEmailSubscribers();
        }

        private void BindEmailSubscribers()
        {
            this.Cursor = Cursors.Wait;

            lstSubscribers = EmailSubscriberManager.GetAllSubscriber();

            dgEmailSubscribers.ItemsSource = lstSubscribers;

            this.Cursor = Cursors.Arrow;
        }

        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            var win = new EmailSubscriberAdd();
            win.Owner = CommonFunctions.GetParentWindow(this);
            win.ShowDialog();
            BindEmailSubscribers();
        }

        private void btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            var subscriberObj = btn.Tag as Subscriber;
            if (subscriberObj != null)
            {
                var win = new EmailSubscriberAdd(subscriberObj);
                win.Owner = CommonFunctions.GetParentWindow(this);
                win.ShowDialog();
                BindEmailSubscribers();
            }
        }

        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            var subscriberObj = btn.Tag as Subscriber;
            if (subscriberObj != null)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure to delete this record?", "Delete Subscriber", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if(result == MessageBoxResult.Yes)
                {
                    bool res = EmailSubscriberManager.DeleteSubscriber(subscriberObj);
                    if (res)
                    {
                        MessageBox.Show("Email Subscriber has been deleted successfully!", "Delete Email Subscriber", MessageBoxButton.OK, MessageBoxImage.Information);
                        BindEmailSubscribers();
                    }
                    else
                    {
                        MessageBox.Show("There is a problem. Please try again!", "Delete Email Subscriber", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
