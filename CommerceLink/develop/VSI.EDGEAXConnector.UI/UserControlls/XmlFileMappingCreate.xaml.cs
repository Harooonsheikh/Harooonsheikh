using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
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
    /// Interaction logic for XmlFileMappingCreate.xaml
    /// </summary>
    public partial class XmlFileMappingCreate : UserControl
    {
        XML objXml = new XML();
        public List<Type> SrcEntities { get; set; }
        public ObservableCollection<PropertyInfo> ObLstSourceProperties = new ObservableCollection<PropertyInfo>();

        public XmlFileMappingCreate(XML xmlObject)
        {
            objXml = xmlObject;
            InitializeComponent();
            BindTemplates();

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
            }
        }

        private void BindTemplates()
        {
            this.lstTemplates.ItemsSource = XmlViewModel.ExistingXMLs.Where(t=>t!=null && t.Name.Contains("CREATE."));
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
                        MessageBox.Show("XML mapping has been added successfully!", "Create XML Mapping", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("There is a problem to add XML mapping, Please try again or contact with Admin.", "Create XML Mapping", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Please insert XML mapping name and xml.", "Create XML Mapping", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There is a problem to add XML mapping, Please try again or contact with Admin.", "Create XML Mapping", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            CommonFunctions.GetParentWindow(this).Close();
        }

        private void lstSrcProperties_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedSourceProperty = lstSrcProperties.SelectedItem as PropertyInfo;
            if (selectedSourceProperty != null)
            {
                string strNewText = selectedSourceProperty.ReflectedType.UnderlyingSystemType.Name + "~" + selectedSourceProperty.Name;
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

        private void BindName()
        {
            //if (cboSrcType.SelectedValue != null && cboSrc.SelectedValue != null)
            //{
            //    if (cboSrcType.SelectedValue.Equals("ERP (Microsoft Dynamics AX)"))
            //    {
            //        txtName.Text = "ERP.";
            //    }
            //    else if (cboSrcType.SelectedValue.Equals("ECommerce (DemandWare)"))
            //    {
            //        txtName.Text = "ECom.";
            //    }

            //    var selectedSource = cboSrc.SelectedValue as Type;
            //    txtName.Text += selectedSource.Name;
            //}

            txtName.Text = "CREATE.";
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

        //private void cboSrcType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (cboSrcType.SelectedValue != null)
        //    {
        //        if (cboSrcType.SelectedValue.Equals("ERP (Microsoft Dynamics AX)"))
        //        {
        //            IClassInfo connectorInfo = new VSI.EDGEAXConnector.ERPDataModels.ClassInfo();
        //            this.SrcEntities = connectorInfo.GetClassesInfo().OrderBy(c => c.Name).ToList();
        //        }
        //        else if (cboSrcType.SelectedValue.Equals("ECommerce (DemandWare)"))
        //        {
        //            IClassInfo connectorInfo = new VSI.EDGEAXConnector.ECommerceDataModels.ClassInfo();
        //            this.SrcEntities = connectorInfo.GetClassesInfo().OrderBy(c => c.Name).ToList();
        //        }

        //        this.cboSrc.ItemsSource = this.SrcEntities;
        //    }
        //}

        private void lstTemplates_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedTemplate = lstTemplates.SelectedItem as XML;
            if (selectedTemplate != null)
            {
                string strNewText = " file-source=\"" + selectedTemplate.Name + "\" data-source=\"\"";
                InsertTag(strNewText);
            }
        }

        private void btnDefaultValueTag_Click(object sender, RoutedEventArgs e)
        {
            string strNewText = " default-value=\"\"";
            InsertTag(strNewText);
        }

        private void btnConstantValueTag_Click(object sender, RoutedEventArgs e)
        {
            string strNewText = " constant-value=\"true\"";
            InsertTag(strNewText);
        }

        private void btnExpressionValueTag_Click(object sender, RoutedEventArgs e)
        {
            string strNewText = " expression=\"true\"";
            InsertTag(strNewText);
        }

        private void btnShowNode_Click(object sender, RoutedEventArgs e)
        {
            string strNewText = " show-node=\"true\"";
            InsertTag(strNewText);
        }

        private void btnRepeat_Click(object sender, RoutedEventArgs e)
        {
            string strNewText = " repeat=\"true\" data-source=\"\"";
            InsertTag(strNewText);
        }

        private void btnToUpper_Click(object sender, RoutedEventArgs e)
        {
            string strNewText = " to-upper=\"true\"";
            InsertTag(strNewText);
        }

        private void btnToLower_Click(object sender, RoutedEventArgs e)
        {
            string strNewText = " to-lower=\"true\"";
            InsertTag(strNewText);
        }
        private void btnAttributeExpressionValueTag_Click(object sender, RoutedEventArgs e)
        {
            string strNewText = "{{Expression}}";
            InsertTag(strNewText);
        }

        private void btnDataObject_Click(object sender, RoutedEventArgs e)
        {
            string strNewText = " data-object=\"\"";
            InsertTag(strNewText);
        }


        private void btnConfigurationHelperKey_Click(object sender, RoutedEventArgs e)
        {
            string strNewText = " ConfigurationHelper.KeyName";
            InsertTag(strNewText);
        }



        private void btnCustomAttributeValue_Click(object sender, RoutedEventArgs e)
        {
            string strNewText = " custom-attribute-value=\"true\"";
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

        private void txtReadInstruction_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //string strAppDir = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            //string strFullPathOfStoresFile = System.IO.Path.Combine(strAppDir, "XmlEngineReadMeFile.txt");
            string strFullPathOfStoresFile = "XmlEngineReadMeFile.txt";

            if (File.Exists(strFullPathOfStoresFile))
            {
                string text = System.IO.File.ReadAllText(strFullPathOfStoresFile);
                Instructions winInstruction = new Instructions("XML Template Engine Instructions", text);
                winInstruction.ShowDialog();
                //MessageBox.Show(text, "XML Template Engine Instructions", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("XML Template Engine instructions file not found!", "XML Template Engine Instructions", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
    }
}
