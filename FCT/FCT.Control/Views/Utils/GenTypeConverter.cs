using System;
using System.Globalization;
using System.Windows.Data;

namespace FCT.Control.Views.Utils
{
    public class GenTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object convValue = null;
            string toConvType = parameter as string;

            if (value != null && !string.IsNullOrEmpty(toConvType))
            {
                switch (toConvType)
                {
                    case "Int32":
                        if(value is int) convValue = System.Convert.ToInt32(value);
                        break;
                    case "UInt32":
                        if (value is uint) convValue = System.Convert.ToUInt32(value);
                        break;
                    case "Decimal":
                        if (value is decimal) convValue = System.Convert.ToDecimal(value);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            return convValue;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {//one way conversion
            return value;
        }
    }
}
