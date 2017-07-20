using System.Collections.Generic;
using System.Drawing;

namespace Xamarin.PropertyEditing.Drawing
{
	public class CommonLinearGradientBrush
		: CommonGradientBrush
	{
		public CommonLinearGradientBrush (IEnumerable<CommonGradientStop> stops)
			: base (stops)
		{
		}

		public CommonPoint StartPoint
		{
			get;
			set;
		}

		public CommonPoint EndPoint
		{
			get;
			set;
		}
	}
}
