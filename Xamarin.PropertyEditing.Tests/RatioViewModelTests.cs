using System;
using System.Collections.Generic;
using NUnit.Framework;
using Xamarin.PropertyEditing.Drawing;
using Xamarin.PropertyEditing.ViewModels;

namespace Xamarin.PropertyEditing.Tests
{
	internal class RatioViewModelTests
		: PropertyViewModelTests<CommonRatio, RatioViewModel>
	{
		[Test]
		public void Numerator ()
		{
			var property = GetPropertyMock ();
			var editor = GetBasicEditor (property.Object);
			var vm = GetViewModel (property.Object, new[] { editor });
			Assume.That (vm.Value, Is.EqualTo (new CommonRatio (3, 1, ':')));

			bool xChanged = false, valueChanged = false;
			vm.PropertyChanged += (sender, args) => {
				if (args.PropertyName == nameof(RatioViewModel.Numerator))
					xChanged = true;
				if (args.PropertyName == nameof(RatioViewModel.Value))
					valueChanged = true;
			};

			vm.Numerator = 5;
			Assert.That (vm.Value.Numerator, Is.EqualTo (5));
			Assert.That (xChanged, Is.True);
			Assert.That (valueChanged, Is.True);
		}

		[Test]
		public void Denominator ()
		{
			var property = GetPropertyMock ();
			var editor = GetBasicEditor (property.Object);
			var vm = GetViewModel (property.Object, new[] { editor });
			Assume.That (vm.Value, Is.EqualTo (new CommonRatio (3, 1, ':')));

			bool xChanged = false, valueChanged = false;
			vm.PropertyChanged += (sender, args) => {
				if (args.PropertyName == nameof (RatioViewModel.Denominator))
					xChanged = true;
				if (args.PropertyName == nameof (RatioViewModel.Value))
					valueChanged = true;
			};

			vm.Denominator = 5;
			Assert.That (vm.Value.Denominator, Is.EqualTo (5));
			Assert.That (xChanged, Is.True);
			Assert.That (valueChanged, Is.True);
		}

		[Test]
		public void RatioSeparator ()
		{
			var property = GetPropertyMock ();
			var editor = GetBasicEditor (property.Object);
			var vm = GetViewModel (property.Object, new[] { editor });
			Assume.That (vm.Value, Is.EqualTo (new CommonRatio (3, 1, ':')));

			bool xChanged = false, valueChanged = false;
			vm.PropertyChanged += (sender, args) => {
				if (args.PropertyName == nameof (RatioViewModel.RatioSeparator))
					xChanged = true;
				if (args.PropertyName == nameof (RatioViewModel.Value))
					valueChanged = true;
			};

			vm.RatioSeparator = '/';
			Assert.That (vm.Value.RatioSeparator, Is.EqualTo ('/'));
			Assert.That (xChanged, Is.True);
			Assert.That (valueChanged, Is.True);
		}

		[Test]
		public void ValueChangesNumeratorDenominatorRatioSeparator ()
		{
			var property = GetPropertyMock ();
			var editor = GetBasicEditor (property.Object);
			var vm = GetViewModel (property.Object, new[] { editor });
			Assume.That (vm.Numerator, Is.EqualTo (0));
			Assume.That (vm.Denominator, Is.EqualTo (0));

			bool nChanged = false, dChanged = false, sChanged = false, valueChanged = false;
			vm.PropertyChanged += (sender, args) => {
				if (args.PropertyName == nameof(RatioViewModel.Numerator))
					nChanged = true;
				if (args.PropertyName == nameof(RatioViewModel.Denominator))
					dChanged = true;
				if (args.PropertyName == nameof (RatioViewModel.RatioSeparator))
					sChanged = true;
				if (args.PropertyName == nameof(SizePropertyViewModel.Value))
					valueChanged = true;
			};

			vm.Value = new CommonRatio (5, 10, ':');

			Assert.That (vm.Numerator, Is.EqualTo (5));
			Assert.That (vm.Denominator, Is.EqualTo (10));
			Assert.That (nChanged, Is.True);
			Assert.That (dChanged, Is.True);
			Assert.That (sChanged, Is.True);
			Assert.That (valueChanged, Is.True);
		}

		protected override CommonRatio GetRandomTestValue (Random rand)
		{
			return new CommonRatio (rand.Next (), rand.Next (), '/');
		}

		protected override RatioViewModel GetViewModel (IPropertyInfo property, IEnumerable<IObjectEditor> editors)
		{
			return new RatioViewModel (property, editors);
		}
	}
}
