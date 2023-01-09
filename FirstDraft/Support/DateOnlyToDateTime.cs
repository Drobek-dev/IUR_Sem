using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstDraft.Support;

public class DateOnlyToDateTime : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        DateOnly v = (DateOnly)value;
        return new DateTime(v.Year, v.Month, v.Day);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        DateTime v= (DateTime)value;
        return DateOnly.FromDateTime(v);
    }
}
