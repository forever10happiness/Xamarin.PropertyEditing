using System;
using System.Linq;
using System.Collections.Generic;
using Xamarin.PropertyEditing.Drawing;
using Xamarin.PropertyEditing.Resources;

namespace Xamarin.PropertyEditing.ViewModels
{
	internal class RatioViewModel
		: PropertyViewModel<CommonRatio>
	{
		public double Numerator
		{
			get { return Value.Numerator; }
			set {
				if (Value.Numerator.Equals (value))
					return;

				Value = new CommonRatio (value, Value.Denominator, Value.RatioSeparator);
			}
		}

		public double Denominator
		{
			get { return Value.Denominator; }

			set {
				if (Value.Denominator.Equals (value))
					return;

				Value = new CommonRatio (Value.Numerator, value, Value.RatioSeparator);
			}
		}

		public char RatioSeparator {
			get { return Value.RatioSeparator; }

			set {
				if (Value.RatioSeparator.Equals (value))
					return;

				Value = new CommonRatio (Value.Numerator, Value.Denominator, value);
			}
		}

		public string ValueString
		{
			get { return Value.StringValue; }
			set {
				if (Value.StringValue.Equals (value))
					return;

				SetRatio (value);
			}
		}

		public static char[] Separators = { '/', ':' };

		public RatioViewModel (IPropertyInfo property, IEnumerable<IObjectEditor> editors)
			: base (property, editors)
		{
		}

		protected override void OnValueChanged ()
		{
			base.OnValueChanged ();
			OnPropertyChanged (nameof (Numerator));
			OnPropertyChanged (nameof (Denominator));
			OnPropertyChanged (nameof (RatioSeparator));
		}

		void SetRatio (string value)
		{
			var strippedValue = value.Replace (" ", string.Empty);
			var parsed = true;
			foreach (var separator in Separators) {
				var parts = strippedValue.Split (new[] { separator });
				double numerator = 0f;
				if (parts.Length == 2) { // We have a potential ratio, let's make sure
					if (!IsDouble (parts[0], out numerator))
						parsed = false;
					double denominator = 0f;
					if (!String.IsNullOrEmpty (parts[1]) && !IsDouble (parts[1], out denominator))
						parsed = false;

					if (parsed) {
						Value = new CommonRatio (numerator, denominator, GetSeparator (strippedValue));
						break;
					}
				} else if (parts.Length == 1) { // We have a potential whole number, let's make sure
					if (!IsDouble (parts[0], out numerator)) {
						parsed = false;
					}
					if (parsed) {
						Value = new CommonRatio (numerator, 1, GetSeparator (strippedValue));
						break;
					}
				}
			}

			if (!parsed) {
				SetError (string.Format (LocalizationResources.InvalidRatio, value));
			}
		}

		char GetSeparator (string value)
		{
			foreach (var separator in Separators) {
				if (value.Contains (separator))
					return separator;
			}
			return default (char);
		}

		bool IsDouble (string stringValue, out double value)
		{
			//Checks parsing to number
			if (!double.TryParse (stringValue, out value))
				return false;
					
			//Checks if needs to be positive value
			if (value < 0)
				return false;
					
			//Checks a common validation
			return CommonValidate (stringValue);
		}

		bool CommonValidate (string finalString)
		{
			return finalString != "-" && !string.IsNullOrEmpty (finalString);
		}
	}
}
