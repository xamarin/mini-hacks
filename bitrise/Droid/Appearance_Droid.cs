using System;
using Xamarin.Forms;
using XamarinSampleApp.Droid;

[assembly: Dependency (typeof (Appearance_Droid))]
namespace XamarinSampleApp.Droid
{
	public class Appearance_Droid : IAppearance
	{
		public Appearance_Droid ()
		{
		}

		public void UpdateBackground(double r, double g, double b) {
			// Nothing to do
		}
	}
}
