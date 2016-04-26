using System;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using EvolveApp;
using EvolveApp.iOS;

using UIKit;
using CoreAnimation;
using CoreGraphics;
using Foundation;

[assembly: ExportRenderer(typeof(LoginEntry), typeof(LoginEntryRenderer))]

namespace EvolveApp.iOS
{
	public class LoginEntryRenderer : EntryRenderer
	{
		UITextField nativeTextField;
		CALayer bottomBorder;
		bool isInitialized = false;

		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement != null && !isInitialized)
			{
				var formsEntry = e.NewElement as LoginEntry;
				formsEntry.FontFamily = "SegoeUI-Light";
				formsEntry.TextColor = Color.FromHex("#778687");
				formsEntry.FontSize = 18;

				nativeTextField = Control as UITextField;
				nativeTextField.BorderStyle = UITextBorderStyle.None;

				//Figure out how to reference font from Resources as Embedded Resource
				if (!String.IsNullOrEmpty(formsEntry.Placeholder))
					nativeTextField.AttributedPlaceholder = new NSAttributedString(formsEntry.Placeholder, UIFont.FromName("SegoeUI-Light", 18), Color.FromHex("#778687").ToUIColor());

				bottomBorder = new CALayer();
				bottomBorder.BackgroundColor = Color.FromHex("#778687").ToCGColor();
				Control.Layer.AddSublayer(bottomBorder);

				isInitialized = true;
			}
		}

		public override CGSize SizeThatFits(CGSize size)
		{
			bottomBorder.Frame = new CGRect(0, size.Height - 1, size.Width, 1);
			return base.SizeThatFits(size);
		}

		protected override void Dispose(bool disposing)
		{
			nativeTextField = null;
			bottomBorder = null;
			base.Dispose(disposing);
		}
	}
}