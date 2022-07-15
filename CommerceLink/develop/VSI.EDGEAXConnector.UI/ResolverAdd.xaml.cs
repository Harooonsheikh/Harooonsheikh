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
using VSI.EDGEAXConnector.UI.Classes;
using VSI.EDGEAXConnector.UI.ViewModel;

namespace VSI.EDGEAXConnector.UI
{
    /// <summary>
    /// Interaction logic for ResolverAdd.xaml
    /// </summary>
    public partial class ResolverAdd : Window
    {
        public ResolverAdd()
        {
            InitializeComponent();
        }

        public ResolverAdd(Resolver resolver)
        {
            InitializeComponent();
            txtName.Text = resolver.Name;
            txtCode.CaretPosition.InsertTextInRun(resolver.code);
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Resolver objResolver = new Resolver();

                objResolver.Name = txtName.Text;
                TextRange txtRange = new TextRange(txtCode.Document.ContentStart, txtCode.Document.ContentEnd);
                //var paragraph = new Paragraph();
                //paragraph.Inlines.Add(new Run { Text = txtRange.Text });
                objResolver.code = txtRange.Text;

                if (objResolver.Name.Length > 0 && objResolver.code.Length > 0)
                {
                    var result = ResolverViewModel.GenerateXmlFileResolver(objResolver);
                    if (result)
                    {
                        MessageBox.Show("Resolver has been added successfully!", "Add Resolver", MessageBoxButton.OK, MessageBoxImage.Information);                        
                    }
                    else
                    {
                        MessageBox.Show("There is a problem to add Resolver, Please try again or contact with Admin.", "Add Resolver", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Please insert resolver name and code.", "Add Resolver", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("There is a problem to add Resolver, Please try again or contact with Admin.", "Add Resolver", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            this.Close();
        }
    }
}
