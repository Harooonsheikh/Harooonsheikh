using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.UI.Classes;

namespace VSI.EDGEAXConnector.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;

            GlobalUI.MainWindowUI = this;

            //BindConnectorTracking();
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        public void BindConnectorTracking()
        {
            StringBuilder strData = new StringBuilder();
            //while(true)
            //{
            //    strData.Append("EdgeAXCommerceLink : Creating connection with ECommerce FTP" + Environment.NewLine);
            //    txtConnectorTracking.AppendText(strData.ToString());
            //    Thread.Sleep(2000);
            //    strData.Append("EdgeAXCommerceLink : Processing SalesOrder (100090018)" + Environment.NewLine);
            //    txtConnectorTracking.AppendText(strData.ToString());
            //    Thread.Sleep(2000);
            //    strData.Append("EdgeAXCommerceLink : Creating Customer for SalesOrder (100090018)" + Environment.NewLine);
            //    txtConnectorTracking.AppendText(strData.ToString());
            //    Thread.Sleep(2000);
            //    strData.Append("EdgeAXCommerceLink : Updating Status of SalesOrders" + Environment.NewLine);
            //    txtConnectorTracking.AppendText(strData.ToString());
            //    Thread.Sleep(2000);
            //}
        }
        
        private void menuExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void menuAbout_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
