# Pixate Theming

### Challenge

Create a new project in Xamarin Studio. Download Pixate.dll[from GitHub](https://github.com/Pixate/MonoTouch-Pixate) and add it to your project. Add a button control to your application and style it with Pixate, applying a gradient background.


### Note

Pixate is a framework that enables users to apply custom styles to iOS applications. It supports a CSS-like syntax, making it easy to produce visual effects like gradients and shadows. In this mini-hack, we will use a free version of Pixate that is intended for non-commercial use. The full, commercial version of Pixate can be purchased through the Xamarin Component Store.

### Walkthrough

* In Xamarin Studio, Create a new single-view iOS project called `PixateTheming`

* Go to Pixate's Xamarin.iOS [GitHub repository](https://github.com/Pixate/MonoTouch-Pixate) in your Web browser. Descend into the `Module` directory and click `Pixate.dll`. Click the Raw button to download the file

* Create a folder called `lib` inside of your project. Copy the `Pixate.dll` file into that folder

* In Xamarin Studio, right-click the References folder in your project and select Edit References. Select the .NET Assembly tab and navigate to the Pixate.dll file

* Select the dll and click the Add button at the bottom of the dialog. The dll will appear in the Selected References list on the right-hand side. Click the OK button to close the dialog

* Double click the MainStoryboard file in the Xamarin Studio solution pane to open the application's main layout in Xcode's integrated design tool

* Drag a `UIButton` control from the Xcode object library into the application view

* Activate Xcode’s assistant editor view to expose the generated Objective-C code

* Create an outlet for the first button and name it `button1`

* Save the storyboard file and return focus to Xamarin Studio

* Open the `Main.cs` file in Xamarin Studio

* Underneath the existing `using` statements, add the following line:

```
using PixateLib;
```

* Add the following line to the top of the `Main` method in the `Application` class:

```
PXEngine.LicenseKeyForUser("", "");
```

* In the view controller source file, add the following code to the `ViewDidLoad` method:

```
button1.SetStyleId("button1");
```

* Right-click the Resources folder in the solution pane. Select New File from the Add submenu. Create a new file called `default.css`

* Add the following code to the `default.css` file:

```
#button1 {
	background-color: linear-gradient(#20364f, rgb(3, 103, 255));
	border-radius: 10px;
	color: white;
}
```

* Run your application! You should see a button with a blue gradient
