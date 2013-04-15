# Hack: Radial Progress

### Challenge

Create a new project in Xamarin Studio and add the Radial Progress component from the Component Store. Place the Radial Progress control in the center of the application and add a button below that will increment the value of the progress control when clicked.

### Walkthrough (iOS)

* In Xamarin Studio, Create a new single-view iOS project called `RadialProgressControl`

* Using Xamarin Studio’s built-in Component Store, add the free Radial Progress component to your project

* Double-click the MainStoryboard file in the Xamarin Studio solution pane to open the application's main layout in Xcode's integrated design tool

* Drag a `UIButton` control from the Xcode object library into the application view. Place the button so that it is centered horizontally near the bottom of the form.

* Activate Xcode’s assistant editor view to expose the generated Objective-C code

* Create an outlet for the button control and name it `buttonAdd`

* Save the storyboard file and return focus to Xamarin Studio

* In the view controller source file, add a `using RadialProgress;` line immediately below the other using statements at the top of the file

* Add the following code to the `ViewDidLoad` method to make the application instantiate a radial progress code and add it to the view when launched:

```C#
var progressView = new RadialProgressView {
     Center = new PointF (View.Center.X, View.Center.Y - 100)
};

View.AddSubview (progressView);
```

* Try running the application to make sure that the widget appears correctly in the center of the form.

* Add the following code to the `ViewDidLoad` method to make the application increment the value of the progress control when the button is pressed:

```C#
buttonAdd.TouchUpInside += (sender, e) => {
      if (progressView.IsDone)
	      progressView.Reset();
      else
	      progressView.Value += 0.1f;
};
```

* Run the app again and click the button to see the progress control fill up!

### Walkthrough (Android)

* In Xamarin Studio, Create a new Android Application project called `RadialProgressControl`

* Using Xamarin Studio’s built-in Component Store, add the free Radial Progress component to your project

* Double-click the main.axml file in the Xamarin Studio solution pane under Resources/layout

* Change the default button text to `Increment by 10`

* Edit the source of the main.axml file

* Add the following code after the Button:

```AXML
<radialprogress.RadialProgressView
	android:id="@+id/progressView"
	android:layout_width="fill_parent"
	android:layout_height="fill_parent"
	min_value="0"
	max_value="100"
	progress_type="big"
	hide_label="false"
	progress_color="#6666CC" />
```

* Save the layout file and return focus MainActivity.cs

* add a `using RadialProgress;` line immediately below the other using statements at the top of the file. (note: you may require to close and reopen the solution for references to update)

```C#
using RadialProgress;
```

* Add a reference to the RadialProgressView with the following code using FindViewById:

```C#
var progressView = FindViewById<RadialProgressView> (Resource.Id.progressView);
```

* Change the contents of `button.Click += delegate` to the following:

```C#
if (progressView.IsDone)
	progressView.Reset();
else
	progressView.Value += 10;
```

* Run the app again and click the button to see the progress control fill up!

### Additional Challenges

* Use a timer to increment the RadialProgressView

* Use the RadialProgressView with a file download