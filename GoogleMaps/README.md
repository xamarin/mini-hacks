# Google Maps

### Challenge

Create a Google API key and embed Google Maps to your app!

### Pages

https://developers.google.com/maps/documentation/android/start#the_google_maps_api_key

https://developers.google.com/maps/documentation/android/reference/com/google/android/gms/maps/MapFragment

https://code.google.com/apis/console/

https://raw.github.com/xamarin/monodroid-samples/master/MapsAndLocationDemo_v2/README.md

### Walkthrough (iOS Version - Component Store version) (Difficulty: easy)

* Start a new iOS iPhone Storyboard Single-View Appplication project, name it GMapsiOS
* Add Google Maps component by going to the Component Store
* Goto [Google Console](https://code.google.com/apis/console)
* Create a new Project (if none exist)
* Under Services : Make sure you enable `Google Maps SDK for iOS`
* Under "API Access" > Click "Create new iOS key…"
* Enter the App Identifier : `com.example.minihack.GMapsiOS` and submit
* Copy the generated API key
* In your `AppDelegate.cs` Add the following:

```
using Google.Maps

const string MapsApiKey = "[paste your API key here])"

public override void FinishedLaunching (UIApplication app)
{
	MapServices.ProvideAPIKey (MapsApiKey);
}
```
* Open `GMapsiOSViewController.cs`
* Add the following

```
using Google.Maps;
...
MapView mapView;
...
public override void LoadView ()
{
	base.LoadView ();

	var camera = CameraPosition.FromCamera (latitude: 30.2652898, 
			                                 longitude: -97.7386826, 
			                                 zoom:17,
			                                 bearing:15,
			                                 viewingAngle:15);
			mapView = MapView.FromCamera (View.Bounds, camera);
			mapView.MapType = MapViewType.Normal;
			mapView.MyLocationEnabled = true;
			View = mapView;
}

public override void ViewWillAppear (bool animated)
{
	base.ViewWillAppear (animated);
	mapView.StartRendering ();
}

public override void ViewWillDisappear (bool animated)
{	
	mapView.StopRendering ();
	base.ViewWillDisappear (animated);
}

```

### Additional Challenges: 
* Add a Marker to Evolve 2013 using MapView.AddMarker()
* Add a UIButton to show your current location

### Walkthrough (v2 Android version) (Difficulty: advanced)

**Setup Google API**

* Follow the [instructions here](https://developers.google.com/maps/documentation/android/start#the_google_maps_api_key) to configure your Google Maps API key (instructions summarized below)

		The location of the debug.keystore file that Mono for Android uses depends on your platform:
		
	- **Windows Vista / Windows 7 / Windows 8**: `C:\Users\[USERNAME]\AppData\Local\Xamarin\Mono for Android\debug.keystore`
	- **OSX** : `/Users/[USERNAME]/.local/share/Xamarin/Mono for Android/debug.keystore`

		To obtain the SH-A1 fingerprint of the debug keystore, you can use the `keytool` command that is a part of the JDK. This is an example of using `keytool` at the command-line:

		```
    	$ keytool -V -list -keystore debug.keystore -alias androiddebugkey -storepass android -keypass android
		```
	- Copy the SSHA1 fingerprint (save it for later)

* Add the API Key to your application

	* Goto https://developers.google.com/
	* Click on [API Console](https://code.google.com/apis/console/)
	* Create a new Project if necessary 
	* Click on `API Access` on the left menu.
	* Click on `Create new Android key…`
	* Input your SHA-1 fingerprint with a semicolon and a package name. For example: 
	```
	BB:0D:AC:74:D3:21:E1:43:67:71:9B:62:91:AF:A1:66:6E:44:5D:75;com.example.android.mapexample
	```
	* Copy the API key (for later use in your app)
	
**Android Project**

* In Xamarin Studio, open the starter project titled "GMapsAndroid" included in this repo. 

* Open the main.axml layout file - view in Source mode

* Add the API key to the MapFragment

* Drag a `UIButton` control from the Xcode object library into the application view. In the property editor, change its label to say "Scan"

* Drag a `UILabel` control onto the view and place it near the top. Resize it horizontally so that it stretches across the entire top of the view.

* Activate Xcode’s assistant editor view to expose the generated Objective-C code

* Create an outlet for the button and name it `buttonScan`

* Create an outlet for the label and name it `labelOutput`

* Save the storyboard file and return focus to Xamarin Studio

* Add the following lines to the view controller source file, right under the existing `using` statements at the top of the file:

```
using ZXing;
```

* Add the following code to the `ViewDidLoad` method to make the application launch the ZXing scanner when the Scan button is pressed and display the captured value in the label when a QR code is scanned:

```
buttonScan.TouchUpInside += (sender, e) => {
  var scanner = new ZXing.Mobile.MobileBarcodeScanner();
  scanner.Scan().ContinueWith(t => {
    if (t.Result != null)
      InvokeOnMainThread(() => labelOutput.Text = t.Result.Text);
  });
};
```
* Run your application on a device! You can use [this QR code](https://github.com/xamarin/mini-hacks/blob/master/QRScanner/qrcode.png) to make sure that it works.