using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstDraft.Support;

public class DateTimeToTimeSpan : IValueConverter
{
    DateTime lastConverted;
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        DateTime v = (DateTime)value;
        lastConverted = v;  
        return new TimeSpan(v.Hour, v.Minute, v.Second);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        TimeSpan v = (TimeSpan)value;
        DateTime ret = new (lastConverted.Year, lastConverted.Month, lastConverted.Day, v.Hours, v.Minutes, v.Seconds);
        return DateTime.SpecifyKind(ret, DateTimeKind.Utc);
        
    }
}
