using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace VSI.EDGEAXConnector.UI.Convertors
{
    public class PropertiesToBrushConvertor : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            //Checking Source Property not null
            if ((value[0] as PropertyInfo) != null)
            {
                if ((value[0] as PropertyInfo).PropertyType != (value[1] as PropertyInfo).PropertyType)
                {
                    //Red Color if types are different
                    return new BrushConverter().ConvertFromString("#FC0000") as SolidColorBrush;
                }
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IsCommentToBrushConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? new BrushConverter().ConvertFromString("#00cc00") as SolidColorBrush : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class EventLevelToBgColorConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString().ToLower().Equals("info") ? new BrushConverter().ConvertFromString("#61EB2F") as SolidColorBrush : new BrushConverter().ConvertFromString("#F71625") as SolidColorBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ActiveJobToBgColorConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? new BrushConverter().ConvertFromString("#ccff66") as SolidColorBrush : new BrushConverter().ConvertFromString("#ff6666") as SolidColorBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StatusJobToBgColorConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value == 1 ? new BrushConverter().ConvertFromString("#ffffff") as SolidColorBrush : new BrushConverter().ConvertFromString("#21ADDB") as SolidColorBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
