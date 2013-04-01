# Xamarin.Mobile

### Challenge

Create a new project in Xamarin Studio and add the Xamarin.Mobile framework from the Component Store. Use the Xamarin.Mobile `Geolocator` class to display the user's latitude and longitude. Use the Xamarin.Mobile `MediaPicker` class to activate the camera and display a photo captured by the user.

### Walkthrough

* In Xamarin Studio, Create a new single-view iOS project called `XamarinMobileComponent`

* Using Xamarin Studio's built-in Component Store, add the free Xamarin.Mobile component to your project

* Double-click the MainStoryboard file in the Xamarin Studio solution pane to open the application's main layout in Xcode's integrated design tool

* Drag a `UIButton` control from the Xcode object library into the application view. Place the button so that it is near the bottom of the form on the left-hand side. In the property editor, change its label to say "Find Location"

* Drag a second `UIButton` control from the Xcode object library into the application view. Place it alongside the first button and change its label to say "Take a Picture"

* Drag a `UILabel` control onto the view and place it near the top. Resize it horizontally so that it stretches across the entire top of the view.

* Drag a `UIImageView` onto the view. Place it in the center and size it so that it fills the form horizontally and fills the vertical space between the label at the top and the buttons at the bottom. In the property editor, change its "Mode" to "Aspect Fit"

* Activate Xcode’s assistant editor view to expose the generated Objective-C code

* Create an outlet for the first button and name it `buttonLocation`

* Create an outlet for the second button and name it `buttonPicture`

* Create an outlet for the label and name it `labelLocation`

* Create an outlet for the image view and name it `imagePhoto`

* Save the storyboard file and return focus to Xamarin Studio

* Add the following lines to the view controller source file, right under the existing `using` statements at the top of the file:

```
using System.Threading.Tasks;
using Xamarin.Geolocation;

using Xamarin.Media;
```

* Add the following code to the `ViewDidLoad` method to make the application obtain the user's position and display it in the label when the location button is pressed:

```
buttonLocation.TouchUpInside += (sender, e) => {
     var locator = new Geolocator { DesiredAccuracy = 50 };
     locator.GetPositionAsync (timeout: 10000).ContinueWith (t => {
     var text = String.Format("Lat: {0}, Long: {0}", t.Result.Latitude, t.Result.Longitude);
         InvokeOnMainThread(() => LabelLocation.Text = text);
     });

};
```

* Add the following code to the `ViewDidLoad` method to make the application activate the camera and put the captured image in the image control when the button is pressed:

```
buttonPicture.TouchUpInside += (sender, e) => {
    var camera = new MediaPicker ();
                
    if (!camera.IsCameraAvailable) {
        Console.WriteLine("Camera unavailable!");
        return;
    }

    var opts = new StoreCameraMediaOptions {
        Name = "test.jpg",
        Directory = "MiniHackDemo"
    };

    camera.TakePhotoAsync (opts).ContinueWith (t => {
        if (t.IsCanceled)
            return;

        InvokeOnMainThread(() => imagePhoto.Image = UIImage.FromFile(t.Result.Path));
    });

};
```

* Run the app test each button to see the features in action! Remember, the camera functionality will only work if you run the application on an actual device.
