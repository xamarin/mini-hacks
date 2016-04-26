// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace TwilioMiniHack.iOS
{
	[Register ("MessageCell")]
	partial class MessageCell
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel authorTextField { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel messageTextField { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (authorTextField != null) {
				authorTextField.Dispose ();
				authorTextField = null;
			}
			if (messageTextField != null) {
				messageTextField.Dispose ();
				messageTextField = null;
			}
		}
	}
}
