using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.UI.Classes;
using VSI.EDGEAXConnector.UI.ViewModel;

namespace VSI.EDGEAXConnector.UI.UserControlls
{
    /// <summary>
    /// Interaction logic for Resolvers.xaml
    /// </summary>
    public partial class Resolvers : UserControl
    {
        private bool fileLoaded = false;
        public ResolverViewModel ViewModel { get; set; }
        public Resolvers()
        {
            InitializeComponent();
            this.ViewModel = new ResolverViewModel();
            this.ViewModel.ConfigureResolvers();
            //this.DataContext = ResolverViewModel;
            this.resolverTree.ItemsSource = ResolverViewModel.ExistingResolvers;
        }

        private void resolverTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var resolver = (sender as TreeView).SelectedItem as Resolver;
            if (resolver != null)
            {
                this.ViewModel.SelectedResolver = resolver;

                var paragraph = new Paragraph();
                paragraph.Inlines.Add(new Run { Text = resolver.code });

                FlowDocument document = new FlowDocument(paragraph);
                document.FontFamily = new System.Windows.Media.FontFamily("Arial");
                cCodeReader.Document = document;
            }
        }

        private void btnNewResolver_Click(object sender, RoutedEventArgs e)
        {
            var win = new ResolverAdd();
            win.Owner = CommonFunctions.GetParentWindow(this);
            win.ShowDialog();
            this.resolverTree.ItemsSource = ResolverViewModel.ExistingResolvers;
            this.resolverTree.Items.Refresh();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (this.ViewModel != null && this.ViewModel.SelectedResolver != null)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure to delete selected Resolver?", "Delete Resolver", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    ResolverViewModel.DeleteResolver(this.ViewModel.SelectedResolver);
                    this.resolverTree.ItemsSource = ResolverViewModel.ExistingResolvers;
                    this.resolverTree.Items.Refresh();
                }
            }
            else
            {
                MessageBox.Show("Please select resolver then press Delete button.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }

        private void btnLoadResolver_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML files (*.xml)|*.xml";
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                {
                    fileLoaded = true;
                    Resolver resolver = ResolverViewModel.LoadResolverFromFile(filename);
                    if (resolver != null)
                    {
                        ResolverViewModel.ExistingResolvers.Add(resolver);
                    }
                    else
                    {
                        MessageBox.Show("File has not correct format.", "File Load Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    btnResolver.IsEnabled = true;
                }
            }
        }

        private void btnResolver_Click(object sender, RoutedEventArgs e)
        {
            Resolver resolver = new Resolver();
            resolver = this.ViewModel.SelectedResolver;

            if (resolver != null)
            {
                if (!fileLoaded)
                {
                    ResolverViewModel.GenerateXmlFileResolver(resolver);
                }
                else
                {
                    //Only for map loaded by file 
                    if (!ResolverViewModel.ExistingResolvers.Any(em => em.Name == resolver.Name))
                    {
                        ResolverViewModel.GenerateXmlFileResolver(resolver);
                    }
                    else
                    {
                        MessageBoxResult res = MessageBox.Show("Already exist resolver. Do you want to Replace?", "Exisiting Resolver", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            ResolverViewModel.GenerateXmlFileResolver(resolver);
                        }
                    }
                }
                //Updateing Existing Maps
                //this.ViewModel.LoadFileTransformer(resolver);
                MessageBox.Show("Resolver has been generated successfully!", "Resolver Generation", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Please select Resolver first then press Generate Resolver button.", "Resolver Generation", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnEditResolver_Click(object sender, RoutedEventArgs e)
        {
            if (this.ViewModel != null && this.ViewModel.SelectedResolver != null)
            {
                var win = new ResolverAdd(this.ViewModel.SelectedResolver);
                win.Owner = CommonFunctions.GetParentWindow(this);
                win.ShowDialog();
                this.resolverTree.ItemsSource = ResolverViewModel.ExistingResolvers;
                this.resolverTree.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Please select resolver then press Edit button.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }
    }
}
