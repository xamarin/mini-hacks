# Hack: Radial Progress

### Challenge

Create a new project in Xamarin Studio and add the Radial Progress component from the Component Store. Place the Radial Progress control in the center of the application and add a button below that will increment the value of the progress control when clicked.

### Walkthrough

* In Xamarin Studio, Create a new single-view iOS project called `RadialProgressControl`

* Using Xamarin Studio’s built-in Component Store, add the free Radial Progress component to your project

* Double-click the MainStoryboard file in the Xamarin Studio solution pane to open the application's main layout in Xcode's integrated design tool

* Drag a `UIButton` control from the Xcode object library into the application view. Place the button so that it is centered horizontally near the bottom of the form.

* Activate Xcode’s assistant editor view to expose the generated Objective-C code

* Create an outlet for the button control and name it `buttonAdd`

* Save the storyboard file and return focus to Xamarin Studio

* In the view controller source file, add a `using RadialProgress;` line immediately below the other using statements at the top of the file

* Add the following code to the `ViewDidLoad` method to make the application instantiate a radial progress code and add it to the view when launched:

```
var progressView = new RadialProgressView {
     Center = new PointF (View.Center.X, View.Center.Y - 100)
};

View.AddSubview (progressView);
```

* Add running the application to make sure that the widget appears correctly in the center of the form.

* Add the following code to the `ViewDidLoad` method to make the application increment the value of the progress control when the button is pressed:

```
buttonAdd.TouchUpInside += (sender, e) => {
      progressView.Value += 0.05f;
};
```

* Run the app again and click the button to see the progress control fill up!
