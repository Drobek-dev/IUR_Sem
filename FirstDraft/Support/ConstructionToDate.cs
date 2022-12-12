using FirstDraft.Model.DatabaseFramework.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstDraft.Support;

class ConstructionToDate : IValueConverter
{
    string lastConverted =(DateOnly.FromDateTime(DateTime.MinValue)).ToString();
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        
        if (value is not null && 
            value is Construction construction) 
        {
            lastConverted = construction.StartDate.ToString();
            return lastConverted; 
        }

        lastConverted = "Nespecifikováno";
        return lastConverted;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        try
        {
            DateOnly ret = DateOnly.Parse((string)value);
            return ret;
        }
        catch
        {
            return DateOnly.Parse(lastConverted);
        }
    }
}
