using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.UI.Classes;
using VSI.EDGEAXConnector.UI.ClassGenerator;
using VSI.EDGEAXConnector.UI.MageAPI;
using VSI.EDGEAXConnector.UI.ViewModel;

namespace VSI.EDGEAXConnector.UI.UserControlls
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Mapping : UserControl
    {
        private bool fileLoaded = false;
        public Mapping()
        {
            InitializeComponent();
            this.ViewModel = new MapsViewModel();
            this.DataContext = this.ViewModel;

        }

        public MapsViewModel ViewModel { get; set; }

        private void cboSourceProp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var srcProperty = (sender as ComboBox).SelectedItem as PropertyInfo;
            var transProperty = (sender as ComboBox).DataContext as TransformerProperty;
            transProperty.SourceProperty = srcProperty;
            transProperty.MapType = MapTypes.SimpleToSimple;
        }

        private void btnTransform_Click(object sender, RoutedEventArgs e)
        {
            Transformer transformer = new Transformer();
            transformer = this.ViewModel.SelectedTransformer;

            if (transformer != null)
            {
                if (!fileLoaded)
                {
                    this.ViewModel.GenerateFileMap(transformer);
                }
                else
                {
                    //Only for map loaded by file 
                    if (!ViewModel.ExistingMaps.Any(em => em.SourceClass == transformer.SourceClass && em.DestinationClass == transformer.DestinationClass))
                    {
                        this.ViewModel.GenerateFileMap(transformer);
                    }
                    else
                    {
                        MessageBoxResult res = MessageBox.Show("Already exist map. Do you want to Replace?", "Exisiting Map", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            ViewModel.GenerateFileMap(transformer);
                        }
                    }
                }
                //Updateing Existing Maps
                this.ViewModel.LoadFileTransformer(transformer);
                MessageBox.Show("Map has been generated successfully!", "Map Generation", MessageBoxButton.OK, MessageBoxImage.Information);            
            }
            else
            {
                MessageBox.Show("Please select Map first then press Generate Map button.", "Map Generation", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void complexPropertyCheck_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox.IsChecked == true)
            {
                var prop = checkBox.DataContext as TransformerProperty;
                //if (prop.SourceProperty == null)
                //{
                //    MessageBox.Show("Please select source property");
                //    return;
                //}
               // ChildWindow win;
                if (prop.DestinationProperty.PropertyType.IsArray || prop.DestinationProperty.PropertyType.IsGenericType)
                {
                    var collectionProp = prop.DestinationProperty.PropertyType.IsArray
                        ? prop.DestinationProperty.PropertyType.GetElementType()
                        : prop.DestinationProperty.PropertyType.GetGenericArguments().FirstOrDefault();

                    var win = new MapAdd(this.ViewModel.Direction, this.ViewModel, true, collectionProp, prop);
                    win.DataContext = this.ViewModel;
                    win.Owner = CommonFunctions.GetParentWindow(this);
                    win.ShowDialog();

                    //win = new ChildWindow(collectionProp, this.ViewModel.SrcEntities, this.ViewModel.Direction);
                    //win.IsConfigured += win_IsConfigured;
                    //win.Show();
                }
                else
                {
                    var win = new MapAdd(this.ViewModel.Direction, this.ViewModel, true, prop.DestinationProperty.PropertyType,prop);
                    win.DataContext = this.ViewModel;
                    win.ShowDialog();
                    //win = new ChildWindow(prop.DestinationProperty.PropertyType, this.ViewModel.SrcEntities,
                    //    this.ViewModel.Direction);
                    //win.IsConfigured += win_IsConfigured;
                    //win.ShowDialog();
                }
            }
        }

        private void win_IsConfigured(Transformer obj)
        {
            if (obj.ChildMaps == null) this.ViewModel.SelectedTransformer.ChildMaps = new List<Transformer>();
            var tproperty =
                this.ViewModel.SelectedTransformer.Properties.FirstOrDefault(
                    p => p.DestinationProperty.PropertyType == obj.DestinationClass);
            if (
                !this.ViewModel.SelectedTransformer.ChildMaps.Any(
                    c =>
                        c.SourceClass.Name.Equals(obj.SourceClass.Name) &&
                        c.DestinationClass.Name.Equals(obj.DestinationClass.Name)))
            {
                this.ViewModel.SelectedTransformer.ChildMaps.Add(obj);
            }
        }

        private void btnNullCheck_Click(object sender, RoutedEventArgs e)
        {
            var property = (sender as Button).DataContext as TransformerProperty;
            var win = new NullWindow(property, this.ViewModel.SourceProperties.ToList(), this.ViewModel.SelectedTransformer.DestinationClass, this.ViewModel.SelectedTransformer.SourceClass, this.ViewModel.DestEntities, this.ViewModel.SrcEntities);
            win.Owner = CommonFunctions.GetParentWindow(this);
            win.ShowDialog();
        }

        private void btnNewMap_Click(object sender, RoutedEventArgs e)
        {
            var win = new MapAdd(this.ViewModel.Direction, this.ViewModel);            
            win.DataContext = this.ViewModel;
            win.Owner = CommonFunctions.GetParentWindow(this);
            win.ShowDialog();
        }

        private void mapTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            fileLoaded = false;
            var tranformer = (sender as TreeView).SelectedItem as Transformer;
            if (tranformer != null)
            {
                ShowTransformerDetails(tranformer);
                LoadXMLAndCode();
            }
        }

        private void ShowTransformerDetails(Transformer tranformer)
        {
            this.ViewModel.SourceProperties.Clear();
            tranformer.SourceClass.GetProperties().ToList().ForEach(p =>
            {
                //if (p.DeclaringType != typeof (CommerceEntity))
                {
                    this.ViewModel.SourceProperties.Add(p);
                }
            });
            colTarget.Header = "Target (" + tranformer.DestinationClass.Name + ")";
            colSource.Header = "Source (" + tranformer.SourceClass.Name + ")";
            dgMapDetails.DataContext = tranformer;
            this.ViewModel.SelectedTransformer = tranformer;
        }

        private void exstingMapCheck_Checked(object sender, RoutedEventArgs e)
        {

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

        private void mainMappingTabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadXMLAndCode();
        }

        private void LoadXMLAndCode()
        {
            if (this.ViewModel != null && this.ViewModel.SelectedTransformer != null)
            {
                this.Cursor = Cursors.Wait;
                //Loading XML
                if (tabXml.IsSelected)
                {
                    var doc = this.ViewModel.GetXmlOfMapping(this.ViewModel.SelectedTransformer);
                    vXMLViwer.xmlDocument = doc;
                }
                //Loading Code 
                if (tabCode.IsSelected)
                {
                    var map = this.ViewModel.SelectedTransformer;
                    
                    Paragraph paragraph = new Paragraph();

                    paragraph = this.ViewModel.GetCodeOfMapping(map);

                    FlowDocument document = new FlowDocument(paragraph);
                    document.FontFamily = new System.Windows.Media.FontFamily("Arial");
                    cCodeReader.Document = document;
                }
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (this.ViewModel != null && this.ViewModel.SelectedTransformer != null)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure to delete selected Map?", "Delete Map", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    this.ViewModel.DeleteMap(this.ViewModel.SelectedTransformer);
                }
            }
            else
            {
                MessageBox.Show("Please select map then press Delete button.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }

        private void btnShowDirection_Click(object sender, RoutedEventArgs e)
        {
            grdMapping.Visibility = System.Windows.Visibility.Collapsed;
            grdDirection.Visibility = System.Windows.Visibility.Visible;
        }

        private void btnLoadMap_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML files (*.xml)|*.xml";
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() == true)
            {
                foreach(string filename in openFileDialog.FileNames)
                {
                    fileLoaded = true;
                    Transformer transformer = this.ViewModel.LoadFile(filename);
                    if (transformer != null)
                    {
                        ShowTransformerDetails(transformer);
                    }
                    else
                    {
                        MessageBox.Show("Miss match mapping direction or file has not correct format.", "File Load Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }

        }
    }
}