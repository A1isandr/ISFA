using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace ISFA.Misc.Converters
{
    class ExpanderTranslateTransformXConverter : IMultiValueConverter
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
                            case ExpandDirection.Right:
                                result *= -1;
                                break;
                            case ExpandDirection.Up:
                            case ExpandDirection.Down:
                                result = 0;
                                break;
                        }
                        break;
                }
            }

            return parameter is not null ? result * System.Convert.ToDouble(parameter, culture) * 1.75 : result * 1.75;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
