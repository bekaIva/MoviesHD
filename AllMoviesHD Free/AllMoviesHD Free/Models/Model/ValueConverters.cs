using MoviesHD.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MoviesHD.Model
{
    public class StringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var format = parameter as string;
            if (!String.IsNullOrEmpty(format))
                return String.Format(format, value);

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class BytesToKMG : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
               
                ulong val = (ulong)value;
                if (val == 0) return "";
                if (val <= 1024) return val.ToString() + "B";
                if (val > 1024 && val <= 1048576) return Math.Round(val / (double)1024, 1).ToString() + "KB";
                if (val > 1048576 && val <= 1073741824) return Math.Round(val / (double)1048576, 1).ToString() + "MB";
                if (val > 1073741824 && val <= 1099511627776) return Math.Round(val / (double)1073741824, 1).ToString() + "GB";
                return "";
            }
            catch (Exception ee)
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
    public class BufferingToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PlayerState st && st == PlayerState.Buffering)
            {
                return true;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
    public class StarsToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                List<string> Stars = (List<string>)value;
                StringBuilder strb = new StringBuilder();
                strb.Append(" | ");
                foreach (string Star in Stars)
                {
                    strb.Append(Star);
                    strb.Append(" | ");
                }
                return strb.ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
    public class IsTvShowToBoolConverter : IValueConverter
    {
       

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b && b)
            {
                return "TV Show";
            }
            else
            {
                return "Movie";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
    public class NegativeBoolConverter : IValueConverter
    {
       



        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b) return !b;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
