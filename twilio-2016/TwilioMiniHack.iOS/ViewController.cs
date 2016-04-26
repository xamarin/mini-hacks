using System;
using System.Linq;
using Foundation;
using UIKit;
using System.Threading.Tasks;
using System.Net.Http;
using System.Json;
using System.Collections.Generic;
using CoreGraphics;


namespace TwilioMiniHack.iOS
{
	public partial class ViewController : UIViewController, IUITextFieldDelegate
	{
		public ViewController(IntPtr handle) : base(handle)
		{
		}
		public async override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Perform any additional setup after loading the view, typically from a nib.
			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, KeyboardWillShow);
			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, KeyboardDidShow);
			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, KeyboardWillHide);

			this.View.AddGestureRecognizer(new UITapGestureRecognizer(() =>
				{
					this.messageTextField.ResignFirstResponder();
				}));



		}

		#region Keyboard Management
		private void KeyboardWillShow(NSNotification notification)
		{
			var keyboardHeight = ((NSValue)notification.UserInfo.ValueForKey(UIKeyboard.FrameBeginUserInfoKey)).RectangleFValue.Height;
			UIView.Animate(0.1, () =>
				{
					this.messageTextFieldBottomConstraint.Constant = keyboardHeight + 8;
					this.sendButtonBottomConstraint.Constant = keyboardHeight + 8;
					this.View.LayoutIfNeeded();
				});
		}

		private void KeyboardDidShow(NSNotification notification)
		{
			this.ScrollToBottomMessage();
		}

		private void KeyboardWillHide(NSNotification notification)
		{
			UIView.Animate(0.1, () =>
				{
					this.messageTextFieldBottomConstraint.Constant = 8;
					this.sendButtonBottomConstraint.Constant = 8;
				});
		}
		#endregion

		public void ScrollToBottomMessage()
		{
			if (dataSource.Messages.Count == 0)
			{
				return;
			}

			var bottomIndexPath = NSIndexPath.FromRowSection(this.tableView.NumberOfRowsInSection(0) - 1, 0);
			this.tableView.ScrollToRow(bottomIndexPath, UITableViewScrollPosition.Bottom, true);
		}
	}
}
