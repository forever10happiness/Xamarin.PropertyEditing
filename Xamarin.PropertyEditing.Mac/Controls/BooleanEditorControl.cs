using System;
using System.Collections;
using AppKit;
using Xamarin.PropertyEditing.Mac.Resources;
using Xamarin.PropertyEditing.ViewModels;

namespace Xamarin.PropertyEditing.Mac
{
	internal class BooleanEditorControl : PropertyEditorControl
	{
		const string setBezelColorSelector = "setBezelColor:";

		public BooleanEditorControl ()
		{
			BooleanEditor = new NSButton { 
				TranslatesAutoresizingMaskIntoConstraints = false,
				Title = string.Empty,
				ControlSize = NSControlSize.Small,
				Font = NSFont.FromFontName (DefaultFontName, DefaultFontSize),
			};
			BooleanEditor.SetButtonType (NSButtonType.Switch);

			// update the value on 'enter'
			BooleanEditor.Activated += (sender, e) => {
				ViewModel.Value = BooleanEditor.State == NSCellStateValue.On ? true : false;
			};

			AddSubview (BooleanEditor);

            this.DoConstraints (new[] {
				BooleanEditor.ConstraintTo (this, (cb, c) => cb.Width == c.Width),
				BooleanEditor.ConstraintTo (this, (cb, c) => cb.Top == c.Top + 5),
				BooleanEditor.ConstraintTo (this, (cb, c) => cb.Left == c.Left + 4),
			});

			UpdateTheme ();
		}

		internal NSButton BooleanEditor { get; set; }

		public override NSView FirstKeyView => BooleanEditor;
		public override NSView LastKeyView => BooleanEditor;

		public string Title { 
			get { return BooleanEditor.Title; } 
			set { BooleanEditor.Title = value; } 
		}

		internal new PropertyViewModel<bool> ViewModel {
			get { return (PropertyViewModel<bool>)base.ViewModel; }
			set { base.ViewModel = value; }
		}

		protected override void UpdateValue ()
		{
			BooleanEditor.State = ViewModel.Value ? NSCellStateValue.On : NSCellStateValue.Off;
			BooleanEditor.Title = ViewModel.Value.ToString ();
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

		protected override void HandleErrorsChanged (object sender, System.ComponentModel.DataErrorsChangedEventArgs e)
		{
			UpdateErrorsDisplayed (ViewModel.GetErrors (ViewModel.Property.Name));
		}

		protected override void SetEnabled ()
		{
			BooleanEditor.Enabled = ViewModel.Property.CanWrite;
		}

		protected override void UpdateAccessibilityValues ()
		{
			BooleanEditor.AccessibilityEnabled = BooleanEditor.Enabled;
			BooleanEditor.AccessibilityTitle = string.Format (LocalizationResources.AccessibilityBoolean, ViewModel.Property.Name);
		}
	}
}
