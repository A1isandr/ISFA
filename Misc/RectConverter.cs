using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace YetAnotherMessenger.Misc.Converters
{
    /// <summary>
    /// Changes size of ContentControl in MainWindow according to Window dimensions. 
    /// </summary>
    public class RectConverter : IMultiValueConverter
    {
        /// <summary>
        /// Converts Window dimensions to size of ContentControl. 
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values is [double width, double height] ? new Rect(0, 0, width, height) : Binding.DoNothing;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
