using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace KameraSteuerungDeLuxe
{
    public class RemoveExtensionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string filename)
                return filename.Replace("images/","").Replace(".png","");
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Optional: .png wieder anhängen beim Zurückwandeln
            if (value is string name)
                return "images/" + name + ".png";
            return value;
        }
    }

    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter : IValueConverter
    {
        public bool Invert { get; set; } = false;
        public Visibility FalseVisibility { get; set; } = Visibility.Collapsed;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = false;

            if (value is bool boolean)
                flag = boolean;

            if (Invert)
                flag = !flag;

            return flag ? Visibility.Visible : FalseVisibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                bool result = visibility == Visibility.Visible;
                return Invert ? !result : result;
            }

            return DependencyProperty.UnsetValue;
        }
    }

    [ValueConversion(typeof(bool), typeof(double))]
    public class BoolToHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var flag = false;
            double height = 0;
            if (value is bool boolean)
                flag = boolean;

            if (parameter is string h)
                height = int.Parse(h);


            return flag ? height : 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double height)
            {
                bool result = height > 0;
                return result;
            }

            return DependencyProperty.UnsetValue;
        }
    }
}