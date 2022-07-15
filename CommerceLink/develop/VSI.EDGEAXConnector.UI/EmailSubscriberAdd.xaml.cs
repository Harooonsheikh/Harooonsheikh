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
    /// Interaction logic for EmailSubscriberAdd.xaml
    /// </summary>
    public partial class EmailSubscriberAdd : Window
    {
        private bool isUpdate = false;
        public EmailSubscriberAdd()
        {
            InitializeComponent();

            List<EmailTemplate> lstEmailTemplate = EmailTemplateManager.GetAllTemplates();
            List<TemplateClass> lstTemplate = new List<TemplateClass>();

            foreach(var temp in lstEmailTemplate)
            {
                TemplateClass tempObj = new TemplateClass();
                tempObj.Id = temp.EmailTemplateId;
                tempObj.Name = temp.Name;
                tempObj.IsSelected = false;
                lstTemplate.Add(tempObj);
            }

            this.lstcheckbox.ItemsSource = lstTemplate;

        }

        public EmailSubscriberAdd(Subscriber subscriberObj)
        {
            InitializeComponent();

            isUpdate = true;
            btnSave.Content = "Update";

            this.lblId.Content = subscriberObj.SubscriberId;
            this.txtName.Text = subscriberObj.Name;
            this.txtEmail.Text = subscriberObj.Email;
            this.chkIsActive.IsChecked = subscriberObj.IsActive;

            List<EmailSubscriber> lstEmailSubscribers = EmailSubscriberManager.GetEmailSubscribtionByID(subscriberObj.SubscriberId);
            
            List<EmailTemplate> lstEmailTemplate = EmailTemplateManager.GetAllTemplates();
            List<TemplateClass> lstTemplate = new List<TemplateClass>();

            foreach (var temp in lstEmailTemplate)
            {
                TemplateClass tempObj = new TemplateClass();
                tempObj.Id = temp.EmailTemplateId;
                tempObj.Name = temp.Name;

                var sub = lstEmailSubscribers.Where(s => s.TemplateId == temp.EmailTemplateId).FirstOrDefault();
                if (sub != null && sub.TemplateId == temp.EmailTemplateId)
                {
                    tempObj.IsSelected = true;
                }
                else
                {
                    tempObj.IsSelected = false;
                }
                lstTemplate.Add(tempObj);
            }
            this.lstcheckbox.ItemsSource = lstTemplate;
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string strMessage = isUpdate ? "Update" : "Add";

            if (txtName.Text != "" && txtEmail.Text != "")
            {
                Subscriber subscriberObj = new Subscriber();
                subscriberObj.SubscriberId = Convert.ToInt32(lblId.Content);
                subscriberObj.Name = this.txtName.Text;
                subscriberObj.Email = this.txtEmail.Text;
                subscriberObj.IsActive = Convert.ToBoolean(this.chkIsActive.IsChecked);

                string selectedTemplates = "";

                foreach (var lstItem in lstcheckbox.Items)
                {
                    if ((lstItem as TemplateClass).IsSelected == true)
                    {
                        selectedTemplates += (lstItem as TemplateClass).Id + ",";
                    }
                }

                bool result = false;

                if (isUpdate)
                {
                    result = EmailSubscriberManager.UpdateSubscriber(subscriberObj, selectedTemplates);
                }
                else
                {
                    result = EmailSubscriberManager.AddSubscriber(subscriberObj, selectedTemplates);
                }
                this.Close();
                

                if (result)
                {
                    MessageBoxResult btnResult = MessageBox.Show("Email Subscriber has been " + (isUpdate ? "updated" : "added") + " successfully!", strMessage + " Email Subscriber", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("There is a problem. Please try again!", strMessage + " Email Subscriber", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please enter Name & Email then press" + (isUpdate ? "updated" : "save") + "!", strMessage + " Email Subscriber", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        public class TemplateClass
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public bool IsSelected { get; set; }
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var temp = (TemplateClass)sender;
        }
    }
}
