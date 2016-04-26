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
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField messageTextField { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NSLayoutConstraint messageTextFieldBottomConstraint { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton sendButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NSLayoutConstraint sendButtonBottomConstraint { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITableView tableView { get; set; }

		[Action ("SendButton_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void SendButton_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (messageTextField != null) {
				messageTextField.Dispose ();
				messageTextField = null;
			}
			if (messageTextFieldBottomConstraint != null) {
				messageTextFieldBottomConstraint.Dispose ();
				messageTextFieldBottomConstraint = null;
			}
			if (sendButton != null) {
				sendButton.Dispose ();
				sendButton = null;
			}
			if (sendButtonBottomConstraint != null) {
				sendButtonBottomConstraint.Dispose ();
				sendButtonBottomConstraint = null;
			}
			if (tableView != null) {
				tableView.Dispose ();
				tableView = null;
			}
		}
	}
}
