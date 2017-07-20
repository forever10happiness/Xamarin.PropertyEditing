using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Xamarin.PropertyEditing.Drawing
{
	public abstract class CommonGradientBrush
		: CommonBrush
	{
		protected CommonGradientBrush (IEnumerable<CommonGradientStop> stops)
		{
			if (stops == null)
				throw new ArgumentNullException (nameof(stops));

			Stops = stops.ToArray ();
		}

		public IReadOnlyList<CommonGradientStop> Stops
		{
			get;
		}
	}

	public class CommonGradientStop
	{
		public CommonGradientStop (Color color, double offset)
		{
			Color = color;
			Offset = offset;
		}

		public double Offset
		{
			get;
		}

		public Color Color
		{
			get;
		}
	}
}