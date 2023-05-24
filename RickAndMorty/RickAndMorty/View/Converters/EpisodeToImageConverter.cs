using System.Globalization;
using System.Windows.Data;
using System;
using System.Windows.Media.Imaging;
using System.Text.RegularExpressions;

namespace RickAndMorty.View.Converters
{
    public class EpisodeToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is string))
                return null;

            var episode = (string)value;
            if (episode.Contains("S") == false)
                return null;

            var regex = new Regex(@"S(\d{2})");
            var match = regex.Match(episode);
            if (!match.Success)
                return null;

            var seasonNumber = match.Groups[1].Value;

            var uri = new Uri($"pack://application:,,,/Resources/SeasonThumbnails/Season{seasonNumber}.png");
            var image = new BitmapImage(uri);

            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}