using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.UI.Classes;
using VSI.EDGEAXConnector.UI.ViewModel;

namespace VSI.EDGEAXConnector.UI.UserControlls
{
    /// <summary>
    /// Interaction logic for XmlFileMappingRead.xaml
    /// </summary>
    public partial class XmlFileMappingRead : UserControl
    {
        XML objXml = new XML();
        public List<Type> SrcEntities { get; set; }
        public ObservableCollection<PropertyInfo> ObLstSourceProperties = new ObservableCollection<PropertyInfo>();

        public XmlFileMappingRead(XML xmlObject)
        {
            objXml = xmlObject;
            InitializeComponent();
            //BindTemplates();

            IClassInfo connectorInfo = new VSI.EDGEAXConnector.ERPDataModels.ClassInfo();
            this.SrcEntities = connectorInfo.GetClassesInfo();
            this.cboSrc.ItemsSource = this.SrcEntities;
            if (objXml != null)
            {
                if (objXml.Name != null)
                {
                    string[] strName = objXml.Name.Split('.');
                    var selectedSource = this.SrcEntities.Where(s => s.Name == strName[1]).FirstOrDefault();
                    cboSrc.SelectedValue = selectedSource;
                    BindProperties(selectedSource);

                    txtName.Text = strName[0] + "." + strName[1];
                    txtXML.CaretPosition.InsertTextInRun(objXml.Xml.ToString());
                }
            }
            else
            {
                objXml = new XML();
                var selectedSource = this.SrcEntities[0];
                cboSrc.SelectedValue = selectedSource;
                BindProperties(selectedSource);

                BindName();

                string strNewText = "<Targets>" + System.Environment.NewLine + "</Targets>";

                InsertTag(strNewText);
            }
        }

        //private void BindTemplates()
        //{
        //    //this.lstTemplates.ItemsSource = XmlViewModel.ExistingXMLs;
        //}

        private void BindName()
        {
            txtName.Text = "READ.";
            var selectedSource = cboSrc.SelectedValue as Type;
            txtName.Text += selectedSource.Name;
        }

        private void BindProperties(Type selectedSource)
        {
            ObLstSourceProperties.Clear();
            foreach (var p in selectedSource.GetProperties().OrderBy(p => p.Name))
            {
                ObLstSourceProperties.Add(p);
            }
            lstSrcProperties.ItemsSource = ObLstSourceProperties;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                objXml.Name = txtName.Text;
                TextRange txtRange = new TextRange(txtXML.Document.ContentStart, txtXML.Document.ContentEnd);

                if (txtName.Text.Length > 0 && txtRange.Text.Length > 0)
                {
                    //objXml.Name = txtName.Text;                    
                    objXml.Xml = XDocument.Parse(txtRange.Text);

                    var result = XmlViewModel.GenerateXmlFileXmlMapping(objXml);
                    if (result)
                    {
                        MessageBox.Show("XML mapping has been added successfully!", "Read XML Mapping", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("There is a problem to add XML mapping, Please try again or contact with Admin.", "Read XML Mapping", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Please insert XML mapping name and xml.", "Read XML Mapping", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There is a problem to add XML mapping, Please try again or contact with Admin.", "Read XML Mapping", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            CommonFunctions.GetParentWindow(this).Close();
        }

        private void lstSrcProperties_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedSourceProperty = lstSrcProperties.SelectedItem as PropertyInfo;
            if (selectedSourceProperty != null)
            {
                string strNewText = "<Target property=\"" + selectedSourceProperty.ReflectedType.UnderlyingSystemType.Name + "~" + selectedSourceProperty.Name + "\" source-path=\"\"></Target>";
                InsertTag(strNewText);
            }
        }

        private void cboSrc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboSrc.SelectedValue != null)
            {
                var selectedSource = cboSrc.SelectedValue as Type;
                BindName();
                BindProperties(selectedSource);
            }
        }

        private void btnConstantValueTag_Click(object sender, RoutedEventArgs e)
        {
            string strNewText = " constant-value=\"\"";
            InsertTag(strNewText);
        }

        private void btnDefaultValueTag_Click(object sender, RoutedEventArgs e)
        {
            string strNewText = " default-value=\"\"";
            InsertTag(strNewText);
        }

        private void btnAttributeNameTag_Click(object sender, RoutedEventArgs e)
        {
            string strNewText = " attribute-name=\"\"";
            InsertTag(strNewText);
        }

        private void btnRepeat_Click(object sender, RoutedEventArgs e)
        {
            string strNewText = " target-source=\"\" repeat=\"true\"";
            InsertTag(strNewText);
        }

        private void btnIsCustomAttributeTag_Click(object sender, RoutedEventArgs e)
        {
            string strNewText = " is-custom-attribute=\"true\" attribute-id=\"\"";
            InsertTag(strNewText);
        }

        //private void lstTemplates_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{

        //}

        

        private void btnPropertiesTag_Click(object sender, RoutedEventArgs e)
        {
            string strNewText = "<Properties>" + System.Environment.NewLine + "</Properties>";
            InsertTag(strNewText);
        }


        private void btnConcatenate_Click(object sender, RoutedEventArgs e)
        {
            string strNewText = " concatenate=\"true\"";
            InsertTag(strNewText);
        }

        private void InsertTag(string strNewText)
        {
            if (txtXML.Selection.Text != "")
            {
                string _Text = new TextRange(txtXML.Document.ContentStart, txtXML.Document.ContentEnd).Text;
                _Text = _Text.Replace(txtXML.Selection.Text, strNewText);
                if (_Text != new TextRange(txtXML.Document.ContentStart, txtXML.Document.ContentEnd).Text)
                {
                    new TextRange(txtXML.Document.ContentStart, txtXML.Document.ContentEnd).Text = _Text; // Change the current text to _Text
                }
            }
            else
            {
                txtXML.CaretPosition.InsertTextInRun(strNewText);
            }
        }

    }
}
