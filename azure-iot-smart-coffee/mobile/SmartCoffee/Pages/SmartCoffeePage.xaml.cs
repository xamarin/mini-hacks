using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SmartCoffee
{
	public partial class SmartCoffeePage : ContentPage
	{
		public SmartCoffeePage()
		{
			BindingContext = new SmartCoffeeViewModel();

			InitializeComponent();
		}
	}
}