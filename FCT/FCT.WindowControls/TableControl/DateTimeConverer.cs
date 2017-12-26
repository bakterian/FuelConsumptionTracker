using System;
using System.Windows.Data;

namespace FCT.WindowControls.TableControl
{
    public class DateTimeConverer : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var res = value;
            var dateTime = (DateTime)value;
            if (dateTime != null && (DateTime.Compare(dateTime,new DateTime(2000,01,01)) < 0))
            {
                res = DateTime.Now;
            }

            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
