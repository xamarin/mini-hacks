using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using Twilio.IPMessaging;

namespace TwilioMiniHack.iOS
{
	partial class MessageCell : UITableViewCell
	{
		public MessageCell (IntPtr handle) : base (handle)
		{
		}

		public Message Message { get; set; }

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			authorTextField.Text = Message?.Author ?? "";
			messageTextField.Text = Message?.Body ?? "";
		}
	}
}
