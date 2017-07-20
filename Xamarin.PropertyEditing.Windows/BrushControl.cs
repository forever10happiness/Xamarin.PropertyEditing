using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;

namespace Xamarin.PropertyEditing.Windows
{
	internal class BrushControl
		: Control
	{
		static BrushControl ()
		{
			FocusableProperty.OverrideMetadata (typeof (BrushControl), new FrameworkPropertyMetadata (false));
			DefaultStyleKeyProperty.OverrideMetadata (typeof(BrushControl), new FrameworkPropertyMetadata (typeof(BrushEditorControl)));
		}

		public static readonly DependencyProperty BrushProperty = DependencyProperty.Register (
			"Brush", typeof(Brush), typeof(BrushControl), new PropertyMetadata (default(Brush)));

		public Brush Brush
		{
			get { return (Brush) GetValue (BrushProperty); }
			set { SetValue (BrushProperty, value); }
		}
	}
}
