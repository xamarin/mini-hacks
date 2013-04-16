# Parse

### Challenge

Create a new project in Xamarin Studio and add the Parse famework from the Component Store.  Add your Parse application keys to the app and save your first object to Parse.

### Requirements

<b>Mono 3.0 - with async/await support - Parse is introducing a completely new API taking advantage of the latest Mono framework. Please download the latest MDK, Android and iOS bits from the beta/alpha channel.</b>

### Walkthrough

* Sign up for a free [Parse](http://www.parse.com) account and create a new application named “ParseXamarin”.
* In Xamarin Studio, open the start project for iOS or Android.
* The starter project already includes a copy of the parse dll
* Find your Application ID and Windows Key on your Parse application dashboard (select your application on [Parse](https://parse.com/apps), go to “Settings”, then open the “Application Keys” tab).
	* If you’re building an **iOS** app, add this to your `AppDelegate.cs` file:
	
	```C#
	using Parse;
	```
	
	```C#
	public AppDelegate ()
	{
		// Initialize the Parse client with your Application ID and Windows Key found on
		// your Parse dashboard
		ParseClient.Initialize("YOUR APPLICATION ID", "YOUR WINDOWS KEY");
	}
	```
	* If you’re building an **Android** app, add a class called `App` to your project with the following code:

	
	```C#
	using System;
	using Android.App;
	using Android.Runtime;
	using Parse;

	namespace ParseAndroidStarterProject
	{
		[Application]
		public class App : Application
		{
			public App (IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
			{
			}

			public override void OnCreate ()
			{
				base.OnCreate ();

				// Initialize the Parse client with your Application ID and Windows Key found on
				// your Parse dashboard
				ParseClient.Initialize("YOUR APPLICATION ID", "YOUR WINDOWS KEY");
			}
		}
	}
	```
* Replace `”YOUR APPLICATION ID”` and `”YOUR WINDOWS KEY”` with the keys you found in the previous step.
* Add a button to your main ViewController (iOS) or Activity (Android) and add an async `TouchUpInside` (iOS) or `Click` (Android) event handler that saves a new `ParseObject`:
	* **iOS** (inside of your `ViewDidLoad` override):
	
	```C#
	btn.TouchUpInside += async (sender, e) => {
		var obj = new ParseObject ("Note");
		obj ["text"] = "Hello, world!  This is a Xamarin app using Parse!";
		obj ["tags"] = new List<string> {"welcome", "xamarin", "parse"};
		await obj.SaveAsync ();
	};
	```
	* **Android** (inside of your `OnCreate` override):

	```C#
	var btn = this.FindViewById<Button>(Resource.Id.button);
	btn.Click += async (sender, e) => {
		var obj = new ParseObject("Note");
		obj ["text"] = "Hello, world!  This is a Xamarin app using Parse!";
		obj ["tags"] = new List<string> {"welcome", "xamarin", "parse"};
		await obj.SaveAsync ();
		Toast.MakeText(this, "Parse Sent!", ToastLength.Short).Show();			
	};
	```
* Run the app, touch the button, and then head over to your [Parse](https://parse.com/apps) application’s DataBrowser to see the new “Note” object that you’ve created and stored with Parse!

### Additional Challenges

* Create a user sign-up/login page using ParseUser

* Log in using the Xamarin Facebook component, then use ParseFacebookUtils to create and log in a ParseUser
 
* Save a picture taken with the device's camera as a ParseFile
