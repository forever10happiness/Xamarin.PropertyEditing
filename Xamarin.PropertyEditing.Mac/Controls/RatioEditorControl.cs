using System;
using System.Collections;
using System.ComponentModel;
using AppKit;
using Xamarin.PropertyEditing.Mac.Resources;
using Xamarin.PropertyEditing.ViewModels;

namespace Xamarin.PropertyEditing.Mac
{
	internal class RatioEditorControl : PropertyEditorControl
	{
		NumericSpinEditor RatioEditor;

		public RatioEditorControl ()
		{
			base.TranslatesAutoresizingMaskIntoConstraints = false;

			RatioEditor = new NumericSpinEditor {
				AllowNegativeValues = false,
				AllowRatios = true,
				BackgroundColor = NSColor.Clear,
				StringValue = string.Empty,
				TranslatesAutoresizingMaskIntoConstraints = false,
			};

			// update the value on keypress
			RatioEditor.ValueChanged += (sender, e) => {
				ViewModel.ValueString = RatioEditor.StringValue;
			};
			AddSubview (RatioEditor);

			this.DoConstraints (new[] {
				RatioEditor.ConstraintTo (this, (re, c) => re.Top == c.Top - 2),
				RatioEditor.ConstraintTo (this, (re, c) => re.Left == c.Left + 4),
				RatioEditor.ConstraintTo (this, (re, c) => re.Width == c.Width - 33),
				RatioEditor.ConstraintTo (this, (re, c) => re.Height == DefaultControlHeight),
			});

			UpdateTheme ();
		}

		public override NSView FirstKeyView => RatioEditor;
		public override NSView LastKeyView => RatioEditor;

		internal new RatioViewModel ViewModel
		{
			get { return (RatioViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

		protected override void HandleErrorsChanged (object sender, DataErrorsChangedEventArgs e)
		{
            UpdateErrorsDisplayed (ViewModel.GetErrors (ViewModel.Property.Name));
		}

		protected override void SetEnabled ()
		{
			RatioEditor.Editable = ViewModel.Property.CanWrite;
		}

		protected override void UpdateAccessibilityValues ()
		{
			RatioEditor.AccessibilityEnabled = RatioEditor.Enabled;
			RatioEditor.AccessibilityTitle = string.Format (LocalizationResources.AccessibilityNumeric, ViewModel.Property.Name);
		}

		protected override void UpdateErrorsDisplayed (IEnumerable errors)
		{
			if (ViewModel.HasErrors) {
				SetErrors (errors);
			} else {
				SetErrors (null);
				SetEnabled ();
			}
		}

		protected override void UpdateValue ()
		{
			RatioEditor.StringValue = ViewModel.ValueString;
		}
	}
}
