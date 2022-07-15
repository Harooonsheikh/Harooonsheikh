using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace VSI.EDGEAXConnector.UI.Convertors
{
    public class ValueToImgConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return "";
            var src = (value as ComboBoxItem).Content;

            if (src.ToString() == "ERP (Microsoft Dynamics AX)")
            {
                return "/Contents/ConnectorImages/DynamicsAxLogo.png";
            }
            else if (src.ToString() == "ECommerce (DemandWare)")
            {
                return "/Contents/ConnectorImages/ecom.png";
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
