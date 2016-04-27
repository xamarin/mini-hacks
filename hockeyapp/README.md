Creating a killer mobile app is part science, and part art.  You not only have to create a stable code base that operates across a variety of OS versions and continuously changing connectivity scenarios – you also have to create an app that people love to use.

HockeyApp gives you the information you need to improve your app, evolve with the device platform, and create that great feature set and user interface for everyone to love. In this challenge, you will integrate HockeySDK for Xamarin into your app to collect crash reports and telemetry.

## Walkthrough

1. Sign up for a free [HockeyApp](http://hockeyapp.net/features/) account if you haven't already, and create a new app via the New App button on the dashboard.

2. Copy the App ID from HockeyApp to your clipboard.

3. In Xamarin Studio, create a new Android or iOS project.

4. Add a reference to the [HockeySDK Nuget](https://www.nuget.org/packages/HockeySDK.Xamarin).  You should be using version *4.1.0-beta1*

5. Initialize the SDK: For iOS, look for your FinishedLaunching in your AppDelegate.cs file and add the following code (be sure be sure to replace "HOCKEYAPP_APPID" with your own):

        public override bool FinishedLaunching (UIApplication app, NSDictionary options)
        {
            //Get the shared instance
            var manager = BITHockeyManager.SharedHockeyManager;

            //Configure it to use our APP_ID
            manager.Configure ("HOCKEYAPP_APPID");

            //Start the manager
            manager.StartManager ();

            //Authenticate (there are other authentication options)
            manager.Authenticator.AuthenticateInstallation ();            

			// The rest of your code here
            // ...
        }

    For Android, overwrite OnCreate of your MainActiviy (again, be sure be sure to replace "HOCKEYAPP_APPID" with your own):

        public class MainActivity : Activity
        {      
            public const string HOCKEYAPP_APPID = "YOUR-APP-ID";

            protected override void OnCreate (Bundle bundle)
            {
                base.OnCreate (bundle);

				// ... your own OnCreate implementation
				CrashManager.Register(this, HOCKEYAPP_APPID);

				// The rest of your code here
                // ...
            }
        }  

    Also check that your app's manifest has the permissions for internet access and to write into the external storage.

6. Create a button and a handler for the button's Click (Android) or TouchUpInside (iOS) event which crashes the app, for example a division by zero.

7. Run your app on a real device, press the button, and notice how the crash appears on HockeyApp.

## Additional challenge: Symbolication

HockeyApp can fully symbolicate your crash reports. To enable this, you need to upload your dSYM file. Find the dSYM file for the last build, zip it, then drag & drop it to your HockeyApp app page. After a few minutes, you will see the symbolicated crash report.

## Additional challenge: Custom Events

With the basic integration above, you can already see unique users, new users, sessions, and crash impacted users. To learn even more about your app's usage, add a few TrackEvent calls to your app and inspect the data in the Events tab on HockeyApp.