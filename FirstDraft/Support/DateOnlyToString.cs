using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstDraft.Support
{
    internal class DateOnlyToString : IValueConverter
    {
        string lastConverted;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            lastConverted = ((DateOnly)value).ToString();
            return lastConverted;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateOnly ret;
            try
            {
                ret = DateOnly.Parse((string)value);
            }
            catch
            {
                return DateOnly.Parse(lastConverted);
            }
            return ret;
           
        }
    }
}
