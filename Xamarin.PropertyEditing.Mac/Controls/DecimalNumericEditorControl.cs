using System;
using Foundation;
using Xamarin.PropertyEditing.ViewModels;

namespace Xamarin.PropertyEditing.Mac
{
	internal class DecimalNumericEditorControl : BaseNumericEditorControl
	{
		public DecimalNumericEditorControl ()
		{
			Formatter = new NSNumberFormatter {
				FormatterBehavior = NSNumberFormatterBehavior.Version_10_4,
				Locale = NSLocale.CurrentLocale,
				MaximumFractionDigits = 15,
				NumberStyle = NSNumberFormatterStyle.Decimal,
				UsesGroupingSeparator = false,
			};

			// update the VM value
			NumericEditor.ValueChanged += (sender, e) => {
				ViewModel.Value = NumericEditor.Value;
			};
		}

		internal new FloatingPropertyViewModel ViewModel {
			get { return (FloatingPropertyViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

		protected override void UpdateValue ()
		{
			NumericEditor.Value = ViewModel.Value;
		}
	}
}
