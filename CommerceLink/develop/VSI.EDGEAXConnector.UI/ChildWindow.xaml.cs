using System;
using System.Collections.Generic;
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
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.UI.ViewModel;

namespace VSI.EDGEAXConnector.UI
{
    /// <summary>
    /// Interaction logic for ChildWindow.xaml
    /// </summary>
    public partial class ChildWindow : Window
    {
        public EntityViewModel ViewModel { get; set; }

        public ChildWindow()
        {
            InitializeComponent();
        }
        public ChildWindow(Type destProperty, List<Type> srcEntities, MapDirection type)
        {
            InitializeComponent();
            ViewModel = new EntityViewModel(type);
            this.DataContext = ViewModel;
            if (destProperty != null)
            {
                ViewModel.SelectedDestEntity = destProperty;
                ViewModel.SrcEntities = new System.Collections.ObjectModel.ObservableCollection<Type>(srcEntities);
            }
        }
        public event Action<Transformer> IsConfigured;
        private void cboSourceProp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var srcProperty = (sender as ComboBox).SelectedItem as PropertyInfo;
            var transProperty = (sender as ComboBox).DataContext as TransformerProperty;
            transProperty.SourceProperty = srcProperty;
        }


        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            IsConfigured((this.DataContext as EntityViewModel).Transformer);
            this.Close();
        }

    }
}
