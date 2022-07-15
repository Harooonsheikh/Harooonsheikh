using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
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
using System.Xml;

namespace VSI.EDGEAXConnector.UI
{
    /// <summary>
    /// Interaction logic for StoreSelector.xaml
    /// </summary>
    public partial class StoreSelector : Window
    {
        //private static string strAppDir = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
        //private static string strFullPathOfStoresFile = System.IO.Path.Combine(strAppDir, "EdgeAXCommerceLinkStores.xml");
        private static string strFullPathOfStoresFile = "EdgeAXCommerceLinkStores.xml";

        public StoreSelector()
        {
            InitializeComponent();
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BindStores();
        }

        private void BindStores()
        {
            cmbStore.Items.Clear();

            if (File.Exists(strFullPathOfStoresFile))
            {

                XmlDocument storeDoc = new XmlDocument();
                storeDoc.Load(strFullPathOfStoresFile);

                var xmlStoreNodesList = storeDoc.SelectNodes(@"//stores/store");

                foreach (XmlNode storeNode in xmlStoreNodesList)
                {
                    ComboBoxItem item = new ComboBoxItem();
                    item.Name = storeNode.Attributes["name"].Value.ToString();
                    item.Content = storeNode.Attributes["name"].Value;

                    cmbStore.Items.Add(item);
                }
            }
            else
            {
                MessageBox.Show("EdgeAXCommerceLinkStores.xml file not found!", "EdgeAXCommerceLinkStores File Missing", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void btnProceed_Click(object sender, RoutedEventArgs e)
        {
            if (cmbStore.SelectedValue != "")
            {
                //Updating connection string in app.config
                XmlDocument storeDoc = new XmlDocument();
                storeDoc.Load(strFullPathOfStoresFile);
                var xmlStoreNodesList = storeDoc.SelectNodes(@"//stores/store");

                foreach (XmlNode storeNode in xmlStoreNodesList)
                {
                    if (storeNode.Attributes["name"].Value.Equals(cmbStore.SelectedValue))
                    {
                        try
                        {
                            string strServer = storeNode.SelectSingleNode("dbServer").InnerText;
                            string strCatalog = storeNode.SelectSingleNode("catalog").InnerText;
                            string loginUserName = storeNode.SelectSingleNode("username").InnerText;
                            string loginPassword = storeNode.SelectSingleNode("password").InnerText;

                            string connectionString = "";

                            if (loginUserName == "" && loginPassword == "")
                            {
                                connectionString = @"metadata=res://*/IntegrationModel.csdl|res://*/IntegrationModel.ssdl|res://*/IntegrationModel.msl;provider=System.Data.SqlClient;provider connection string=""data source=" + strServer + ";initial catalog=" + strCatalog + ";integrated security=True;MultipleActiveResultSets=True;App=EntityFramework\";";
                            }
                            else
                            { 
                                connectionString = @"metadata=res://*/IntegrationModel.csdl|res://*/IntegrationModel.ssdl|res://*/IntegrationModel.msl;provider=System.Data.SqlClient;provider connection string=""data Source=" + strServer + ";Initial Catalog=" + strCatalog + ";Integrated Security=False;User Id=" + loginUserName + ";Password=" + loginPassword + ";Encrypt=True;Trusted_Connection=False;App=EntityFramework\";";
                            }
                            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                            var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
                            connectionStringsSection.ConnectionStrings["IntegrationDBEntities"].ConnectionString = connectionString;
                            config.Save();
                            ConfigurationManager.RefreshSection("connectionStrings");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Select Store", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }

                MainWindow objMainWindow = new MainWindow();
                objMainWindow.Show();
                objMainWindow = null;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select EdgeAX CommerceLink Service then proceed", "Select EdgeAX CommerceLink Service", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            grdAddStore.Visibility = Visibility.Collapsed;
            grdSelectStore.Visibility = Visibility.Visible;            
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtStoreName.Text.Length > 0 && txtServerName.Text.Length > 0 && txtCatalogName.Text.Length > 0)
                {
                    XmlDocument storeDoc = new XmlDocument();
                    storeDoc.Load(strFullPathOfStoresFile);

                    XmlNode selectedStore = storeDoc.SelectSingleNode(@"//stores/store[@name='" + cmbStore.SelectedValue + "']");
                    if (selectedStore == null || Convert.ToBoolean(lblIsEdit.Text) == true)
                    {
                        if (Convert.ToBoolean(lblIsEdit.Text))
                        {
                            //storeDoc.RemoveChild(selectedStore);
                            selectedStore.ParentNode.RemoveChild(selectedStore);
                        }

                        XmlElement store = storeDoc.CreateElement("store");
                        XmlElement dbServer = storeDoc.CreateElement("dbServer");
                        dbServer.InnerText = txtServerName.Text;
                        XmlElement catalog = storeDoc.CreateElement("catalog");
                        catalog.InnerText = txtCatalogName.Text;

                        store.SetAttribute("name", txtStoreName.Text);
                        store.AppendChild(dbServer);
                        store.AppendChild(catalog);
                        storeDoc.DocumentElement.AppendChild(store);

                        storeDoc.Save("EdgeAXCommerceLinkStores.xml");

                        BindStores();

                        grdAddStore.Visibility = Visibility.Collapsed;
                        grdSelectStore.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        MessageBox.Show("Store already exist.", "Add/Edit Store", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Please input all fields", "Add/Edit Store", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Add/Edit Store", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAddStore_Click(object sender, RoutedEventArgs e)
        {
            grdSelectStore.Visibility = Visibility.Collapsed;
            grdAddStore.Visibility = Visibility.Visible;
        }

        private void btnEditStore_Click(object sender, RoutedEventArgs e)
        {
            if(cmbStore.SelectedValue != null)
            {
                XmlDocument storeDoc = new XmlDocument();
                storeDoc.Load(strFullPathOfStoresFile);

                XmlNode selectedStore = storeDoc.SelectSingleNode(@"//stores/store[@name='" + cmbStore.SelectedValue + "']");
                if (selectedStore != null)
                {
                    lblIsEdit.Text = "true";
                    txtStoreName.Text = cmbStore.SelectedValue.ToString();
                    txtServerName.Text = selectedStore.SelectSingleNode("dbServer").InnerText;
                    txtCatalogName.Text = selectedStore.SelectSingleNode("catalog").InnerText;
                }
                grdSelectStore.Visibility = Visibility.Collapsed;
                grdAddStore.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Please select store first then press edit button", "Edit Store", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (txtStoreName.Text.Length > 0 && txtServerName.Text.Length > 0 && txtCatalogName.Text.Length > 0)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure to delete this store?", "Delete Store", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {

                        XmlDocument storeDoc = new XmlDocument();
                        storeDoc.Load(strFullPathOfStoresFile);

                        XmlNode selectedStore = storeDoc.SelectSingleNode(@"//stores/store[@name='" + cmbStore.SelectedValue + "']");
                        if (selectedStore != null)
                        {
                            XmlNode pNode = selectedStore.ParentNode;
                            pNode.RemoveChild(selectedStore);

                            storeDoc.Save("EdgeAXCommerceLinkStores.xml");

                            BindStores();

                            grdAddStore.Visibility = Visibility.Collapsed;
                            grdSelectStore.Visibility = Visibility.Visible;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Delete Store", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select store first then press delete button", "Delete Store", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
