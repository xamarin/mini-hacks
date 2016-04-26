using System;
using Android.Widget;
using EvolveApp;
using TextStyles.Android;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer (typeof (StyledLabel), typeof (EvolveApp.Droid.StyledLabelRenderer))]
namespace EvolveApp.Droid
{
	public class StyledLabelRenderer : LabelRenderer
	{
		public StyledLabelRenderer ()
		{
		}

		protected override void OnElementChanged (ElementChangedEventArgs<Label> e)
		{
			base.OnElementChanged (e);

			var styledElement = Element as StyledLabel;
			var cssStyle = styledElement.CssStyle;

			if (Control != null) {
				TextStyle.Style<TextView> (Control, cssStyle);
			}
		}
	}
}

