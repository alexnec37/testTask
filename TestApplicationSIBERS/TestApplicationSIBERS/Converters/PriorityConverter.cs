using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace TestApplicationSIBERS.Converters
{
    public class PriorityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int priority = (int)value;
            string str_priority =null;
            switch (priority)
            { 
                case 0:
                    str_priority = "Заморожен";
                    break;
                case 1:
                    str_priority = "Низкий";
                    break;
                case 2:
                    str_priority = "Средний";
                    break;
                case 3:
                    str_priority = "Высокий";
                    break;
            }
            return str_priority;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
