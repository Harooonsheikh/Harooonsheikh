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

namespace VSI.EDGEAXConnector.UI
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            this.Cursor = Cursors.Wait;
            InitializeComponent();
            this.Cursor = Cursors.Arrow;
        }

        private void Storyboard_Completed(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Wait;
            //LoginWindow objLoginWindow = new LoginWindow();
            //objLoginWindow.ShowDialog();
            //objLoginWindow = null;
            StoreSelector objStoreSelector = new StoreSelector();
            objStoreSelector.Show();
            objStoreSelector = null;

            //MainWindow objMainWindow = new MainWindow();
            //objMainWindow.Show();
            //objMainWindow = null;
            
            this.Close();
            //this.Dispatcher.InvokeShutdown();
            this.Cursor = Cursors.Arrow;
        }
    }
}
