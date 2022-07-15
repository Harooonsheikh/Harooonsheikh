using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
using System.Xml;
using System.Xml.Linq;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.UI.Classes;
using VSI.EDGEAXConnector.UI.ViewModel;

namespace VSI.EDGEAXConnector.UI.UserControlls
{
    /// <summary>
    /// Interaction logic for XmlMapping.xaml
    /// </summary>
    public partial class XmlFileMapping : UserControl
    {
        private bool fileLoaded = false;
        public XmlViewModel ViewModel { get; set; }
        public XmlFileMapping()
        {
            InitializeComponent();
            this.ViewModel = new XmlViewModel();
            this.ViewModel.ConfigureXmls();
            //this.DataContext = ViewModel;
            this.xmlMapTree.ItemsSource = XmlViewModel.ExistingXMLs;
        }

        private void xmlMapTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var xml = (sender as TreeView).SelectedItem as XML;
            if (xml != null)
            {
                this.ViewModel.SelectedXML = xml;

                vXMLMapViwer.xmlDocument = ConvertToXmlDocument(xml.Xml);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (this.ViewModel != null && this.ViewModel.SelectedXML != null)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure to delete selected XML Mapping?", "Delete XML Mapping", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    XmlViewModel.DeleteXML(this.ViewModel.SelectedXML);
                    this.xmlMapTree.ItemsSource = XmlViewModel.ExistingXMLs;
                    this.xmlMapTree.Items.Refresh();
                }
            }
            else
            {
                MessageBox.Show("Please select XML mapping then press Delete button.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }

        private void btnXmlMapping_Click(object sender, RoutedEventArgs e)
        {
            XML xml = new XML();
            xml = this.ViewModel.SelectedXML;

            if (xml != null)
            {
                if (!fileLoaded)
                {
                    XmlViewModel.GenerateXmlFileXmlMapping(xml);
                }
                else
                {
                    //Only for map loaded by file 
                    if (!XmlViewModel.ExistingXMLs.Any(em => em.Name == xml.Name))
                    {
                        XmlViewModel.GenerateXmlFileXmlMapping(xml);
                    }
                    else
                    {
                        MessageBoxResult res = MessageBox.Show("Already exist XML mapping. Do you want to Replace?", "Exisiting XML Mapping", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            XmlViewModel.GenerateXmlFileXmlMapping(xml);
                        }
                    }
                }
                MessageBox.Show("XML Map has been generated successfully!", "XML Map Generation", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Please select XML map first then press Generate XML Map button.", "XML Map Generation", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnNewXmlMapping_Click(object sender, RoutedEventArgs e)
        {
            var win = new XmlAdd(null);
            win.Owner = CommonFunctions.GetParentWindow(this);
            win.ShowDialog();
            this.xmlMapTree.ItemsSource = XmlViewModel.ExistingXMLs;
            this.xmlMapTree.Items.Refresh();
        }

        private void btnLoadXmlMapping_Click(object sender, RoutedEventArgs e)
        {
            var loadFromDatabase = ConfigurationManager.AppSettings["IsLoadTemplateFromDatabase"].BoolValue();
           
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "XML files (*.xml)|*.xml";
                openFileDialog.Multiselect = false;
                if (openFileDialog.ShowDialog() == true)
                {
                    foreach (string filename in openFileDialog.FileNames)
                    {
                        fileLoaded = true;
                        XML xml = XmlViewModel.LoadXMLFromFile(filename);
                        if (xml != null)
                        {
                            XmlViewModel.ExistingXMLs.Add(xml);
                        }
                        else
                        {
                            MessageBox.Show("File has not correct format.", "File Load Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        btnXmlMapping.IsEnabled = true;
                    }
                }
            

            
        }

        public static XmlDocument ConvertToXmlDocument(XDocument xDocument)
        {
            var xmlDocument = new XmlDocument();
            using (var reader = xDocument.CreateReader())
            {
                xmlDocument.Load(reader);
            }

            var xDeclaration = xDocument.Declaration;
            if (xDeclaration != null)
            {
                var xmlDeclaration = xmlDocument.CreateXmlDeclaration(
                    xDeclaration.Version,
                    xDeclaration.Encoding,
                    xDeclaration.Standalone);

                xmlDocument.InsertBefore(xmlDeclaration, xmlDocument.FirstChild);
            }

            return xmlDocument;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            XML xml = new XML();
            xml = this.ViewModel.SelectedXML;

            if (xml.Name != null)
            {
                var win = new XmlAdd(xml);
                win.Owner = CommonFunctions.GetParentWindow(this);
                win.ShowDialog();
                this.xmlMapTree.ItemsSource = XmlViewModel.ExistingXMLs;
                this.xmlMapTree.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Please select XML map first then press Edit XML Map button.", "Edit XML Map", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        } 
    }
}
