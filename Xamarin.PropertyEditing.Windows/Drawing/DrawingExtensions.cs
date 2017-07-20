using System;
using System.CodeDom;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Xamarin.PropertyEditing.Drawing;

using SD=System.Drawing;
using WPF=System.Windows.Media;

namespace Xamarin.PropertyEditing.Windows.Drawing
{
	internal static class DrawingExtensions
	{
		public static Brush ToWpf (this CommonBrush self)
		{
			if (self == null)
				throw new ArgumentNullException (nameof(self));

			if (self is CommonSolidBrush solidBrush)
				return new SolidColorBrush (ToWpf (solidBrush.Color)) { Opacity = solidBrush.Opacity };
			else if (self is CommonLinearGradientBrush linearBrush) {
				return new LinearGradientBrush (new GradientStopCollection (linearBrush.Stops.Select (ToWpf))) {
					Opacity = linearBrush.Opacity,
					StartPoint = ToWpf (linearBrush.StartPoint),
					EndPoint = ToWpf (linearBrush.EndPoint),
				};
			}

			return null;
		}

		public static CommonBrush ToCommon (this Brush self)
		{
			if (self is SolidColorBrush solidBrush)
				return new CommonSolidBrush (FromWpf (solidBrush.Color)) { Opacity = solidBrush.Opacity };
			else if (self is LinearGradientBrush linearBrush) {
				return new CommonLinearGradientBrush (linearBrush.GradientStops.Select (FromWpf)) {
					Opacity = linearBrush.Opacity,
					StartPoint = FromWpf (linearBrush.StartPoint),
					EndPoint = FromWpf (linearBrush.EndPoint)
				};
			}

			return null;
		}

		private static CommonGradientStop FromWpf (GradientStop stop)
		{
			return new CommonGradientStop (FromWpf (stop.Color), stop.Offset);
		}

		private static SD.Color FromWpf (WPF.Color color)
		{
			return SD.Color.FromArgb (color.A, color.R, color.G, color.B);
		}

		private static CommonPoint FromWpf (Point point)
		{
			return new CommonPoint (point.X, point.Y);
		}

		private static Point ToWpf (CommonPoint point)
		{
			return new Point (point.X, point.Y);
		}

		private static GradientStop ToWpf (CommonGradientStop stop)
		{
			return new GradientStop (ToWpf (stop.Color), stop.Offset);
		}

		private static WPF.Color ToWpf (SD.Color color)
		{
			return WPF.Color.FromArgb (color.A, color.R, color.G, color.B);
		}
	}
}
