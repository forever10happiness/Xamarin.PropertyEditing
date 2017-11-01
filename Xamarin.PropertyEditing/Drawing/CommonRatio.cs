using System;

namespace Xamarin.PropertyEditing.Drawing
{
	/// <summary>
	/// Struct to hold Ratio values
	/// </summary>
	[Serializable]
	public struct CommonRatio : IEquatable<CommonRatio>
	{
		/// <param name="numerator">The ratio numerator value.</param>
		/// <param name="denominator">The ratio denominator value.</param>
		/// <param name="ratioSeparator">The ratio separator character, '/' or ':'.</param>
		public CommonRatio (double numerator, double denominator, char ratioSeparator)
		{
			Numerator = numerator;
			Denominator = denominator;
			RatioSeparator = ratioSeparator;
		}

		// Normal values like 1, 200, 400 would be stored as:
		// Numerator = 100, Denominator = 1
		//
		// Ratio values such as 1:3 or 5/6 would be stored as
		// Numerator = 1, Denominator = 3 etc.
		public double Numerator { get; set; }
		public double Denominator { get; set; }

		public double Value => Numerator / Denominator;

		// This can be either '/' or ':'
		public char RatioSeparator { get; set; }

		public bool IsRatio => !Denominator.Equals (1.0f);

		public string StringValue => string.Format ("{0}{1}{2}", Numerator, (RatioSeparator == '\0' ? ':' : RatioSeparator), Denominator);

		public static bool operator == (CommonRatio left, CommonRatio right)
		{
			return Equals (left, right);
		}

		public static bool operator != (CommonRatio left, CommonRatio right)
		{
			return !Equals (left, right);
		}

		public override bool Equals (object obj)
		{
			if (obj == null) 
				return false;
			if (!(obj is CommonRatio)) 
				return false;
			return Equals ((CommonRatio)obj);
		}

		public bool Equals (CommonRatio other)
		{
			return Numerator.Equals (other.Numerator)
				&& Denominator.Equals (other.Denominator);
		}

		public override int GetHashCode ()
		{
			var hashCode = 3571; //TODO May need better primes
			unchecked {
				hashCode = hashCode * -7919 + Numerator.GetHashCode ();
				hashCode = hashCode * -7919 + Denominator.GetHashCode ();
			}
			return hashCode;
		}
	}
}
