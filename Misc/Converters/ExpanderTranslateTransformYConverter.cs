using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace YetAnotherMessenger.Misc.Converters
{
    class ExpanderTranslateTransformYConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object? parameter, CultureInfo culture)
        {
            double result = 1.0;

            foreach (var value in values)
            {
                switch (value)
                {
                    case double num:
                        result *= num;
                        break;
                    case ExpandDirection direction:
                        switch (direction)
                        {
                            case ExpandDirection.Up:
                                result *= -1;
                                break;
                            case ExpandDirection.Left:
                            case ExpandDirection.Right:
                                result = 0;
                                break;
                        }
                        break;
                }
            }

            return parameter is not null ? result * System.Convert.ToDouble(parameter, culture) : result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
