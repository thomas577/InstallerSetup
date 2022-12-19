using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace InstallerSetup.Converters
{
    /// <summary>
    /// This value converter does nothing to the value. It only outputs some diagnostic log
    /// </summary>
    public class DebugConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.WriteLine($"DebugConverter.Convert(value: {value}, targetType: {targetType.Name}, parameter: {parameter})");
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.WriteLine($"DebugConverter.ConvertBack(value: {value}, targetType: {targetType.Name}, parameter: {parameter})");
            return value;
        }
    }
}
