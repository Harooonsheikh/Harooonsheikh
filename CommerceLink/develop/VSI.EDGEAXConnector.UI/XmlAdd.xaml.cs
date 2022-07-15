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
using System.Windows.Shapes;
using System.Xml.Linq;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.UI.UserControlls;
using VSI.EDGEAXConnector.UI.ViewModel;

namespace VSI.EDGEAXConnector.UI
{
    /// <summary>
    /// Interaction logic for XmlAdd.xaml
    /// </summary>
    public partial class XmlAdd : Window
    {
        XML objXml = new XML();
        public List<Type> SrcEntities { get; set; }
        public ObservableCollection<PropertyInfo> ObLstSourceProperties = new ObservableCollection<PropertyInfo>();
        
        public XmlAdd(XML XmlObject)
        {
            InitializeComponent();

            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            
            if (XmlObject != null)
            {
                var strName = XmlObject.Name.Split('.');
                if (strName.Length > 2 && strName[0].Equals("READ"))
                {
                    tabCreate.IsSelected = tabCreate.IsEnabled = false;
                    tabRead.IsSelected = true;

                    XmlFileMappingRead readControl = new XmlFileMappingRead(XmlObject);
                    this.ReadLayout.Children.Add(readControl);                    
                }
                else
                {
                    tabCreate.IsSelected = true;
                    tabRead.IsSelected = tabRead.IsEnabled = false;

                    XmlFileMappingCreate createControl = new XmlFileMappingCreate(XmlObject);
                    this.CreateLayout.Children.Add(createControl);
                }
            }
            else
            {
                tabCreate.IsSelected = true;
                tabRead.IsSelected = false;

                XmlFileMappingCreate createControl = new XmlFileMappingCreate(XmlObject);
                this.CreateLayout.Children.Add(createControl);
            }
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
        
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                if (tabCreate.IsSelected)
                {
                    XmlFileMappingCreate createControl = new XmlFileMappingCreate(null);
                    this.CreateLayout.Children.Add(createControl);
                }
                else if (tabRead.IsSelected)
                {
                    XmlFileMappingRead readControl = new XmlFileMappingRead(null);
                    this.ReadLayout.Children.Add(readControl);
                }
            }
        }
    }
}
