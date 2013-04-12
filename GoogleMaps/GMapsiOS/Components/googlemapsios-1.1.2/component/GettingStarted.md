Showing a Map
=============

### AppDelegate

```csharp
using Google.Maps;
...

const string MapsApiKey = "<Get your ID at https://code.google.com/apis/console/>";

public override bool FinishedLaunching (UIApplication app, NSDictionary options)
{
	MapServices.ProvideAPIKey (MapsApiKey);
	...
}
```

### Your View Controller

```csharp
using Google.Maps;
...

MapView mapView;

public override void LoadView ()
{
	base.LoadView ();
	
	var camera = CameraPosition.FromCamera (latitude: 37.797865, 
			                                longitude: -122.402526, 
			                                zoom: 6);
	mapView = MapView.FromCamera (RectangleF.Empty, camera);
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

Attribution Requirements
========================

If you use the Google Maps SDK for iOS in your application, you must include the attribution text as part of a legal notices section in your application. Including legal notices as an independent menu item, or as part of an "About" menu item, is recommended.

You can get the attribution text by making a call to `Google.Maps.MapServices.OpenSourceLicenseInfo`
 
The Google Maps API Key
=======================

Using an API key enables you to monitor your application's Maps API usage, and ensures that Google can contact you about your application if necessary. The key is free, you can use it with any of your applications that call the Maps API, and it supports an unlimited number of users. You obtain a Maps API key from the Google APIs Console by providing your application's bundle identifier. Once you have the key, you add it to your AppDelegate as described in the next section.

### Obtaining an API Key

You can obtain a key for your app in the [Google APIs Console](https://code.google.com/apis/console/).

1. Create an API project in the [Google APIs Console](https://code.google.com/apis/console/).

2. Select the **Services** pane in your API project, and enable the Google Maps SDK for iOS. This displays the [Google Maps Terms of Service](https://developers.google.com/maps/terms?hl=es).

3. Select the **API Access** pane in the console, and click **Create new iOS key**.

4. Enter one or more bundle identifiers as listed in your application's .plist file, such as `com.example.myapp`.

5. Click **Create**.

6. In the **API Access** page, locate the section **Key for iOS apps (with bundle identifiers)** and note or copy the 40-character **API key**.

You should repeat this process for each new application.