using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstDraft.Support;

public class DateTimeToString : IValueConverter
{
    string lastConverted;
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        lastConverted = ((DateTime)value).ToString();
        return lastConverted;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        DateTime ret;
        try
        {
            ret = DateTime.Parse((string)value);
        }
        catch
        {
            return DateTime.Parse(lastConverted);
        }
        return DateTime.SpecifyKind(ret, DateTimeKind.Utc);

    }
}
