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
using System.Windows.Navigation;
using System.Windows.Shapes;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.UI.ViewModel;

namespace VSI.EDGEAXConnector.UI.UserControlls
{
    /// <summary>
    /// Interaction logic for NewMap.xaml
    /// </summary>
    public partial class NewMap : UserControl
    {
        public EntityViewModel ViewModel { get; set; }

        MapsViewModel mapsViewModel = new MapsViewModel();

        public NewMap(MapDirection type, MapsViewModel dataContext, bool isChild = false, Type selectedDestEntity = null)
        {
            InitializeComponent();
            EntityViewModel viewModel = new EntityViewModel(type);
            this.DataContext = viewModel;
            ViewModel = viewModel;
            if (isChild) this.ViewModel.SelectedDestEntity = selectedDestEntity;
            mapsViewModel = dataContext;
        }
        private void cboSourceProp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var srcProperty = (sender as ComboBox).SelectedItem as PropertyInfo;
            var transProperty = (sender as ComboBox).DataContext as TransformerProperty;
            transProperty.SourceProperty = srcProperty;
        }

        private void Transform_Click(object sender, RoutedEventArgs e)
        {
            if (cboDest.SelectedIndex != -1 && cboSrc.SelectedIndex != -1)
            {
                OnMapGenerated(ViewModel.Transformer);
            }
            else
            {
                MessageBox.Show("Please select Map first then press Generate Map button.", "Map Generation", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void btnClearMappingButton_OnClick(object sender, RoutedEventArgs e)
        {
            var property = (sender as Button).DataContext as TransformerProperty;
            if (property != null)
            {
                property.SourceProperty = null;
                property.MapType = MapTypes.None;
                property.IsComplex = false;
                property.ConstantValue = new ConstantValue();
                property.IsCustomLogic = false;
                property.BooleanValue.IsBoolean = false;
                property.CustomConditionalValue = new CustomConditionalValue();
                property.Comment = string.Empty;
            }
        }

        void win_IsConfigured(Transformer obj)
        {
            if (obj.ChildMaps == null) this.ViewModel.Transformer.ChildMaps = new List<Transformer>();
            var tproperty = this.ViewModel.Transformer.Properties.Where(p => p.DestinationProperty.PropertyType == obj.DestinationClass).FirstOrDefault();
            
            if (!this.ViewModel.Transformer.ChildMaps.Any(c => c.SourceClass.Name.Equals(obj.SourceClass.Name) && c.DestinationClass.Name.Equals(obj.DestinationClass.Name)))
            {
                this.ViewModel.Transformer.ChildMaps.Add(obj);
            }
        }

        private void btnNullCheck_Click(object sender, RoutedEventArgs e)
        {
            var property = (sender as Button).DataContext as TransformerProperty;
            NullWindow win = new NullWindow(property, ViewModel.SourceProperties.ToList(), this.ViewModel.SelectedDestEntity, this.ViewModel.SelectedSrcEntity, this.ViewModel.DestEntities.ToList(), this.ViewModel.SrcEntities.ToList());
            win.ShowDialog();
        }

        public event Action<Transformer> OnMapGenerated;

        private void complexPropertyCheck_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox.IsChecked == true)
            {
                var prop = checkBox.DataContext as TransformerProperty;
                if (prop.SourceProperty == null) { MessageBox.Show("Please select source property"); return; }
                if (prop.DestinationProperty.PropertyType.IsArray || prop.DestinationProperty.PropertyType.IsGenericType)
                {
                    var collectionProp = prop.DestinationProperty.PropertyType.IsArray
                        ? prop.DestinationProperty.PropertyType.GetElementType()
                        : prop.DestinationProperty.PropertyType.GetGenericArguments().FirstOrDefault();

                    var win = new MapAdd(this.ViewModel.Direction, this.mapsViewModel, true, collectionProp, prop);
                    win.DataContext = this.ViewModel;
                    win.DataContext = this.DataContext;
                    win.ShowDialog();
                }
                else
                {
                    var win = new MapAdd(this.ViewModel.Direction, this.mapsViewModel, true, prop.DestinationProperty.PropertyType, prop);
                    win.DataContext = this.ViewModel;
                    win.DataContext = this.DataContext;
                    win.ShowDialog();
                }
            }
        }

    }
}
