using System;
using System.Drawing;
using System.IO;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.MessageUI;

namespace SQLCipherApp
{
	public partial class MainViewController : UIViewController
	{
		private string _databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "message.db");
		private MessageDb _messageDb;

		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public MainViewController ()
			: base (UserInterfaceIdiomIsPhone ? "MainViewController_iPhone" : "MainViewController_iPad", null)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			_messageDb = new MessageDb(_databasePath);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			textViewMessage.Text = "";
			if(File.Exists(_databasePath))
			{
				Load ();
			}
			textViewMessage.BecomeFirstResponder();
		}

		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations ()
		{
			return UIInterfaceOrientationMask.Portrait;
		}

		private void Load() 
		{
			/*
			textViewMessage.Text = _messageDb.LoadMessage();
			*/
			var alert = new UIAlertView("Enter Password", "Password", null, "OK", null) {
				AlertViewStyle = UIAlertViewStyle.SecureTextInput
			};
			alert.Clicked += (s, a) =>  {
				_messageDb.Password = alert.GetTextField(0).Text;
				try
				{
					textViewMessage.Text = _messageDb.LoadMessage();
				} catch (Exception e) 
				{
					textViewMessage.Text = e.Message;
				}
				
			};
			alert.Show();
		}

		private void Save() 
		{
			/*
			_messageDb.SaveMessage(textViewMessage.Text);
			*/
			var alert = new UIAlertView("Enter Password", "Password", null, "OK", null) {
				AlertViewStyle = UIAlertViewStyle.SecureTextInput
			};
			alert.Clicked += (s, a) =>  {
				_messageDb.Password = alert.GetTextField(0).Text;
				_messageDb.SaveMessage(textViewMessage.Text);
			};
			alert.Show();

		}

		partial void SaveButtonClick(MonoTouch.Foundation.NSObject sender) 
		{
			Save ();
		}

		partial void SendButtonClick(MonoTouch.Foundation.NSObject sender) 
		{
			if (MFMailComposeViewController.CanSendMail) 
			{
				var mail = new MFMailComposeViewController ();
				mail.SetSubject( "Zetetic Message Database");
				mail.SetToRecipients(new string[]{});
				mail.SetMessageBody ("Please find a database attached", false);
				mail.AddAttachmentData(NSData.FromFile(_messageDb.FilePath), MessageDb.MIME_TYPE, Path.GetFileName(_messageDb.FilePath));
				mail.Finished += (s, e) => {
					mail.DismissViewController(true, null);
				};
				PresentViewController(mail, true, null);			
			}
		}

		public void HandleOpenUrl(NSUrl url) 
		{
			File.Copy(url.Path, _messageDb.FilePath, true);
			Load ();
		}
	}
}

