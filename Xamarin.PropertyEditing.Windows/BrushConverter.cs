using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Xamarin.PropertyEditing.Drawing;
using Xamarin.PropertyEditing.Windows.Drawing;

namespace Xamarin.PropertyEditing.Windows
{
	internal class BrushConverter
		: IValueConverter
	{
		public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is CommonBrush commonBrush)
				return commonBrush.ToWpf ();
			else if (value is Brush wpfBrush)
				return wpfBrush.ToCommon ();

			return null;
		}

		public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Convert (value, targetType, parameter, culture);
		}
	}
}
