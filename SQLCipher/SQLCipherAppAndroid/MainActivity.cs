using System;
using System.IO;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Text;

namespace SQLCipherApp
{
	[Activity (Label = "SQLCipher", MainLauncher = true)]
	[IntentFilter (
		new[]{Intent.ActionView},
		Categories=new[]{"android.intent.category.DEFAULT"},
		DataMimeType="*/*")]
	public class MainActivity : Activity
	{
		private String _databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "message.db");
		private MessageDb _messageDb;
		private TextView _textViewMessage;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Main);
			_messageDb = new MessageDb(_databasePath);
			_textViewMessage = FindViewById<TextView> (Resource.Id.editTextMessage);
			var button = FindViewById<Button> (Resource.Id.buttonSave);
			button.Click += delegate {
				Save ();
			};

			button = FindViewById<Button> (Resource.Id.buttonSend);
			button.Click += delegate {
				var intent = new Intent(Android.Content.Intent.ActionSend);
				intent.SetType("text/plain");
				intent.PutExtra(Intent.ExtraEmail, new String[] { });
				intent.PutExtra(Intent.ExtraSubject, "Zetetic Message Database");
				intent.PutExtra(Intent.ExtraText, "Please find a database attached");

				string url = MessageDbProvider.CONTENT_URI.ToString() + "message.db";
				intent.PutExtra(Intent.ExtraStream, Android.Net.Uri.Parse(url));
				StartActivity(intent);
			};

		}

		protected override void OnStart ()
		{
			base.OnStart();
			_textViewMessage.Text = "";
			Android.Net.Uri uri = this.Intent != null ? this.Intent.Data : null;
			if(uri != null) 
			{
				using(var istream = ContentResolver.OpenInputStream(uri)) {
					using(var ostream = new FileStream(_messageDb.FilePath, FileMode.OpenOrCreate)) {
						istream.CopyTo(ostream);
					}
				}
			}

			if(File.Exists(_databasePath)){
				Load ();
			}
		}

		private void Load()
		{

			/*
			_textViewMessage.Text = _messageDb.LoadMessage();
			*/
			var input = new EditText(this);
			input.InputType = (InputTypes.ClassText | InputTypes.TextVariationPassword);
			var builder = new AlertDialog.Builder(this)
				.SetTitle("Enter Password")
					.SetMessage("Password")
					.SetView(input)
					.SetCancelable(false)
					.SetPositiveButton("OK", (sender, args) => {
						_messageDb.Password = input.Text;
						try
						{
							_textViewMessage.Text = _messageDb.LoadMessage();
						} catch (Exception e) 
						{
							_textViewMessage.Text = e.Message;
						}
					});
			builder.Create().Show();

		}

		private void Save()
		{
			/*
			_messageDb.SaveMessage(_textViewMessage.Text);
			*/

			var input = new EditText(this);
			input.InputType = (InputTypes.ClassText | InputTypes.TextVariationPassword);
			var builder = new AlertDialog.Builder(this)
				.SetTitle("Enter Password")
					.SetMessage("Password")
					.SetView(input)
					.SetCancelable(false)
					.SetPositiveButton("OK", (sender, args) => {
						_messageDb.Password = input.Text;
						_messageDb.SaveMessage(_textViewMessage.Text);
					});
			builder.Create().Show();
		}
	}
}


