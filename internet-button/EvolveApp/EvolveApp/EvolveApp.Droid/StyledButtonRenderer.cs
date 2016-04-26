using System;
using Android.Widget;
using EvolveApp;
using TextStyles.Android;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(StyledButton), typeof(EvolveApp.Droid.StyledButtonRenderer))]
namespace EvolveApp.Droid
{
	public class StyledButtonRenderer : ButtonRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
		{
			base.OnElementChanged(e);

			var styledElement = Element as StyledButton;
			var cssStyle = styledElement.CssStyle;

			if (Control != null)
			{
				TextStyle.Style<TextView>(Control, cssStyle);
				Control.Gravity = Android.Views.GravityFlags.Center;
			}
		}
	}
}