using System;
using AppKit;
using Foundation;
using Xamarin.PropertyEditing.ViewModels;

namespace Xamarin.PropertyEditing.Mac
{
	internal class IntegerNumericEditorControl : BaseNumericEditorControl
	{
		public IntegerNumericEditorControl ()
		{
			Formatter = new NSNumberFormatter {
				FormatterBehavior = NSNumberFormatterBehavior.Version_10_4,
				Locale = NSLocale.CurrentLocale,
				MaximumFractionDigits = 0,
				NumberStyle = NSNumberFormatterStyle.None,
				UsesGroupingSeparator = false,
			};

			// update the VM value
			NumericEditor.ValueChanged += (sender, e) => {
				ViewModel.Value = (long)NumericEditor.Value;
			};
		}

		internal new IntegerPropertyViewModel ViewModel {
			get { return (IntegerPropertyViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

		protected override void UpdateValue ()
		{
			NumericEditor.Value = ViewModel.Value;
		}
	}
}
