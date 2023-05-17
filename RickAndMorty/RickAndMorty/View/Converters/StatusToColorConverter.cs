using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace RickAndMorty.View.Converters
{
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is string))
                return null;

            var status = (string)value;
            var color = new SolidColorBrush(Colors.Black);
            if (status.Equals("Alive"))
                color.Color = Colors.Lime;
            else if (status.Equals("Dead"))
                color.Color = Colors.Red;

            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}