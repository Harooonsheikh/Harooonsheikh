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
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.UI.UserControlls;
using VSI.EDGEAXConnector.UI.ViewModel;

namespace VSI.EDGEAXConnector.UI
{
    /// <summary>
    /// Interaction logic for MapAdd.xaml
    /// </summary>
    public partial class MapAdd : Window
    {
        TransformerProperty TransformationProperty { get; set; }

        public static MapsViewModel mapsViewModel = new MapsViewModel();
        
        public MapAdd(MapDirection type, MapsViewModel dataContext, bool isChild = false, Type selectedDestEntity = null, TransformerProperty transformationProp = null)
        {
            InitializeComponent();
            NewMap UCNewMap = new NewMap(type, dataContext, isChild, selectedDestEntity);
            UCNewMap.OnMapGenerated += UCNewMap_OnMapGenerated;
            this.Layout.Children.Add(UCNewMap);
            mapsViewModel = dataContext;
            if (isChild)
            {
                TransformationProperty = transformationProp;
            }
        }

        void UCNewMap_OnMapGenerated(Common.Transformer obj)
        {
            if(TransformationProperty !=null)
            {
                TransformationProperty.MapType = MapTypes.ObjectToObject;
               // TransformationProperty.SourceProperty = obj. 
            }
            
            var viewModel = mapsViewModel;

            if (viewModel != null)
            {
                if (!viewModel.ExistingMaps.Any(em => em.SourceClass == obj.SourceClass && em.DestinationClass == obj.DestinationClass))
                {
                    viewModel.ExistingMaps.Add(obj);
                    viewModel.GenerateFileMap(obj);
                    MessageBox.Show("Map has been generated successfully!", "Map Generation", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBoxResult res = MessageBox.Show("Already exist map for Destination & Source Entites. Do you want to Replace?", "Exisiting Map", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if(res == MessageBoxResult.Yes)
                    {
                        var existingMap = viewModel.ExistingMaps.Where(em => em.SourceClass == obj.SourceClass && em.DestinationClass == obj.DestinationClass).FirstOrDefault();
                        viewModel.ExistingMaps.Remove(existingMap);
                        viewModel.GenerateFileMap(obj);
                        MessageBox.Show("Map has been generated successfully!", "Map Generation", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }

            //Old Code
            /*
            List<Transformer> transformers = new List<Transformer>();
            transformers.Add(obj);
            MapsViewModel viewModel = this.DataContext as MapsViewModel;
            if (viewModel != null)
            {
                if (obj.ChildMaps.Any())
                {
                    obj.ChildMaps.ForEach(c => { transformers.Add(c); });
                    obj.ChildMaps.Clear();
                }
                transformers.ForEach(t =>
                {
                    if (!viewModel.ExistingMaps.Any(em => em.SourceClass == t.SourceClass && em.DestinationClass == t.DestinationClass))
                    {
                        viewModel.ExistingMaps.Add(t);
                        viewModel.GenerateMap();
                        MessageBox.Show("Map definition added!");
                    }
                    else
                    {
                        MessageBox.Show("Already exist map for Destination & Source Entites.");
                    }
                });
            }
            */
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
