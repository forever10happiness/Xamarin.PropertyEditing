using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Drawing;

namespace Xamarin.PropertyEditing.Windows
{
	internal class BrushEditorControl
		: Control
	{
		static BrushEditorControl ()
		{
			FocusableProperty.OverrideMetadata (typeof (BrushEditorControl), new FrameworkPropertyMetadata (false));
			DefaultStyleKeyProperty.OverrideMetadata (typeof(BrushEditorControl), new FrameworkPropertyMetadata (typeof(BrushEditorControl)));
		}

		public static readonly DependencyProperty BrushProperty = DependencyProperty.Register (
			"Brush", typeof(Brush), typeof(BrushEditorControl), new PropertyMetadata (default(Brush)));

		public Brush Brush
		{
			get { return (Brush) GetValue (BrushProperty); }
			set { SetValue (BrushProperty, value); }
		}

		public override void OnApplyTemplate ()
		{
			base.OnApplyTemplate ();
		}
	}
}