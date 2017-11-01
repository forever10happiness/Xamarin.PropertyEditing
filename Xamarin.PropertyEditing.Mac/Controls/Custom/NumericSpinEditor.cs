using System;
using AppKit;
using CoreGraphics;
using Foundation;
using Xamarin.PropertyEditing.Drawing;

namespace Xamarin.PropertyEditing.Mac
{
	public class NumericSpinEditor : NSView
	{
		NumericTextField numericEditor;
		NSStepper stepper;
		bool editing;

		public event EventHandler ValueChanged;
		public event EventHandler EditingEnded;

		public ValidationType NumericMode {
			get { return numericEditor.NumericMode; }
			set {
				numericEditor.NumericMode = value;
				Reset ();
			}
		}

		public string PlaceholderString {
			get { return ((NSTextFieldCell)numericEditor.Cell).PlaceholderString; }
			set { ((NSTextFieldCell)numericEditor.Cell).PlaceholderString = value; }
		}

		NSStepper Stepper {
			get { return stepper; }
		}

		public override CGSize IntrinsicContentSize {
			get {
				var baseSize = stepper.IntrinsicContentSize;
				return new CGSize (baseSize.Width + 20, baseSize.Height);
			}
		}

		public NSColor BackgroundColor {
			get {
				return numericEditor.BackgroundColor;
			}
			set {
				numericEditor.BackgroundColor = value;
			}
		}

		public override nfloat BaselineOffsetFromBottom {
			get { return numericEditor.BaselineOffsetFromBottom; }
		}

		public int Digits {
			get { return (int)formatter.MaximumFractionDigits; }
			set { formatter.MaximumFractionDigits = value; }
		}

		public double Value {
			get { return commonRatio.Numerator; }
			set { SetValue (value); }
		}

		public bool Wrap {
			get { return stepper.ValueWraps; }
			set { stepper.ValueWraps = value; }
		}

		public double MinimumValue {
			get { return stepper.MinValue; }
			set {
				stepper.MinValue = value;
				formatter.Minimum = new NSNumber (value);
			}
		}

		public double MaximumValue {
			get { return stepper.MaxValue; }
			set {
				stepper.MaxValue = value;
				formatter.Maximum = new NSNumber (value);
			}
		}

		double cachedStepperValue = 0.0f;
		public double IncrementValue {
			get { return stepper.Increment; }
			set { stepper.Increment = value; }
		}

		public bool Enabled {
			get {
				return numericEditor.Enabled;
			}
			set {
				numericEditor.Enabled = value;
				stepper.Enabled = value;
			}
		}

		NSNumberFormatter formatter;
		public NSNumberFormatter Formatter {
			get {
				return formatter;
			}
			set {
				formatter = value;
				numericEditor.Formatter = formatter;
			}
		}

		public bool IsIndeterminate {
			get {
				return !string.IsNullOrEmpty (numericEditor.StringValue);
			}
			set {
				if (value)
					numericEditor.StringValue = string.Empty;
				else
					numericEditor.DoubleValue = commonRatio.Value;
			}
		}

		public bool Editable {
			get {
				return numericEditor.Editable;
			}
			set {
				numericEditor.Editable = value;
				stepper.Enabled = value;
			}
		}

		public NSNumberFormatterStyle NumberStyle {
			get { return formatter.NumberStyle; }
			set {
				formatter.NumberStyle = value;
			}
		}

		public bool AllowRatios
		{
			get {
				return numericEditor.AllowRatios;
			}
			set {
				numericEditor.AllowRatios = value;
			}
		}

		public string StringValue
		{
			get { 
				return numericEditor.StringValue; 
			}
			set {
				numericEditor.StringValue = value;
			}
		}

		protected virtual void OnConfigureNumericTextField ()
		{
			numericEditor.Formatter = formatter;
		}

		public bool AllowNegativeValues
		{
			get {
				return numericEditor.AllowNegativeValues;
			}
			set {
				numericEditor.AllowNegativeValues = value;
			}
		}

		public virtual void Reset ()
		{
		}

		CommonRatio commonRatio = new CommonRatio (0, 1, ':');

		public NumericSpinEditor ()
		{
			TranslatesAutoresizingMaskIntoConstraints = false;
			var controlSize = NSControlSize.Small;

			stepper = new NSStepper {
				TranslatesAutoresizingMaskIntoConstraints = false,
				ValueWraps = false,
				ControlSize = controlSize,
			};

			numericEditor = new NumericTextField {
				Alignment = NSTextAlignment.Right,
				TranslatesAutoresizingMaskIntoConstraints = false,
				Font = NSFont.FromFontName (PropertyEditorControl.DefaultFontName, PropertyEditorControl.DefaultFontSize),
				ControlSize = controlSize,
			};

			stepper.Activated += (s, e) => {
				if (!editing) {
					editing = true;
					if (stepper.DoubleValue > cachedStepperValue) {
						IncrementNumericValue ();
					} else {
						DecrementNumericValue ();
					}
					ValueChanged?.Invoke (this, EventArgs.Empty);
					editing = false;
				}
			};

			numericEditor.KeyArrowUp += (sender, e) => { IncrementNumericValue (); };
			numericEditor.KeyArrowDown += (sender, e) => { DecrementNumericValue (); };

			numericEditor.ValidatedEditingEnded += (s, e) => {
				OnEditingEnded (s, e);
			};

			AddSubview (stepper);
			AddSubview (numericEditor);

			this.DoConstraints (new[] {
				numericEditor.ConstraintTo (this, (n, c) => n.Width == c.Width - 16),
				numericEditor.ConstraintTo (this, (n, c) => n.Height == PropertyEditorControl.DefaultControlHeight - 3),
				stepper.ConstraintTo (numericEditor, (s, n) => s.Left == n.Right + 4),
				stepper.ConstraintTo (numericEditor, (s, n) => s.Top == n.Top),
			});
		}

		protected void OnValueChanged (object sender, EventArgs e)
		{
			if (!editing) {
				editing = true;
				SetValue (numericEditor.StringValue);
				ValueChanged?.Invoke (this, EventArgs.Empty);
				editing = false;
			}
		}

		protected void OnEditingEnded (object sender, EventArgs e)
		{
			if (!editing) {
				editing = true;
				SetValue (numericEditor.StringValue);
				EditingEnded?.Invoke (this, EventArgs.Empty);
				ValueChanged?.Invoke (this, EventArgs.Empty);
				editing = false;
			}
		}

		void SetValue (string value)
		{
			numericEditor.StringValue = value;
		}

		public void SetValue (double value)
		{
			SetValue (value.ToString ());
		}

		protected double CoerceValue (double val)
		{
			return FieldValidation.CoerceValue (val, MinimumValue, MaximumValue);
		}

		public void IncrementNumericValue ()
		{
			nint caretLocation = 0;
			nint selectionLength = 0;

			GetEditorCaretLocationAndLength (out caretLocation, out selectionLength);

			if (AllowRatios) {
				// Nothing selected, just workout out which value the cursor is to increment
				if (selectionLength == 0) {
					if (caretLocation <= GetSeparatorLocation ()) {
						SetCommonRatio (commonRatio.Numerator + IncrementValue, commonRatio.Denominator, commonRatio.RatioSeparator);
					} else {
						SetCommonRatio (commonRatio.Numerator, commonRatio.Denominator + IncrementValue, commonRatio.RatioSeparator);
					}
				} else {
					// Increment both values.
					SetCommonRatio (commonRatio.Numerator + IncrementValue, commonRatio.Denominator + IncrementValue, commonRatio.RatioSeparator);
				}
				SetValue (commonRatio.StringValue);
			} else {
				SetCommonRatio (commonRatio.Numerator + IncrementValue, 1, commonRatio.RatioSeparator);
				SetValue (commonRatio.Numerator);
			}

			// Resposition our cursor so it doesn't jump around.
			SetEditorCaretLocationAndLength (caretLocation, selectionLength);
		}

		public void DecrementNumericValue ()
		{
			nint caretLocation = 0;
			nint selectionLength = 0;

			GetEditorCaretLocationAndLength (out caretLocation, out selectionLength);

			if (AllowRatios) {
				// Nothing selected, just workout out which value to increment
				if (selectionLength == 0) {
					if (caretLocation <= GetSeparatorLocation ()) {
						SetCommonRatio (commonRatio.Numerator - IncrementValue, commonRatio.Denominator, commonRatio.RatioSeparator);
					} else {
						SetCommonRatio (commonRatio.Numerator, commonRatio.Denominator - IncrementValue, commonRatio.RatioSeparator);
					}
				} else {
					// Increment both values.
					SetCommonRatio (commonRatio.Numerator - IncrementValue, commonRatio.Denominator - IncrementValue, commonRatio.RatioSeparator);
				}
				SetValue (commonRatio.StringValue);
			} else {
				SetCommonRatio (commonRatio.Numerator - IncrementValue, 1, commonRatio.RatioSeparator);
				SetValue (commonRatio.Numerator);
			}

			// Resposition our cursor so it doesn't jump around.
			SetEditorCaretLocationAndLength (caretLocation, selectionLength);
		}

		void SetCommonRatio (double numerator, double denominator, char separator)
		{
			commonRatio.Numerator = numerator;
			commonRatio.Denominator = denominator;
			commonRatio.RatioSeparator = separator;
			cachedStepperValue = stepper.DoubleValue;
		}

		void SetEditorCaretLocationAndLength (nint caretLocation, nint selectionLength)
		{
			if (numericEditor.CurrentEditor != null) {
				numericEditor.CurrentEditor.SelectedRange = new NSRange (caretLocation, selectionLength);
			}
		}

		void GetEditorCaretLocationAndLength (out nint caretLocation, out nint selectionLength)
		{
			caretLocation = 0;
			selectionLength = 0;
			if (numericEditor.CurrentEditor != null) {
				caretLocation = numericEditor.CurrentEditor.SelectedRange.Location;
				selectionLength = numericEditor.CurrentEditor.SelectedRange.Length;
			}
		}

		int GetSeparatorLocation ()
		{
			return string.Compare (numericEditor.StringValue, commonRatio.RatioSeparator.ToString ());
		}
	}
}
