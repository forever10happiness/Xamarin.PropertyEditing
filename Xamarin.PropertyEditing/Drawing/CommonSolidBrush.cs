using System.Drawing;

namespace Xamarin.PropertyEditing.Drawing
{
	public class CommonSolidBrush
		: CommonBrush
	{
		public CommonSolidBrush (Color color)
		{
			Color = color;
		}

		public Color Color
		{
			get;
		}
	}
}