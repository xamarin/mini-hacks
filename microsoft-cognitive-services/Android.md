##### Create an Android blank project

1. With Visual Studio 2015 or Xamarin Studio, create a new project with the Templates > Visual C# > Android > Blank App (Android) project template. Use a name like "AndroidApp" and a new solution with a name like "XamarinCognitive". Click the OK button and the project will be created for you.

2. Add the following Nuget packages to your project (in the correct order):
    * Microsoft.Bcl.Build
    * Microsoft.ProjectOxford.Emotion

##### Design the UI layer

The UI should be composed of an ImageView, a TextView and a Button. You can create them by opening the ```Resources/layout/Main.axml``` file with the Designer or the Source view. You can copy/paste the following code to save some time:

```xml
<?xml version="1.0" encoding="utf-8"?>
<LinearLayout xmlns:android="http://schemas.android.com/apk/res/android"
    android:orientation="vertical"
    android:layout_width="fill_parent"
    android:layout_height="fill_parent">
    <ImageView
    android:src="@android:drawable/ic_menu_gallery"
    android:layout_width="fill_parent"
    android:layout_height="300.0dp"
    android:id="@+id/imageView1"
    android:adjustViewBounds="true" />
    <TextView
    android:textAppearance="?android:attr/textAppearanceLarge"
    android:layout_width="fill_parent"
    android:layout_height="wrap_content"
    android:id="@+id/resultText"
    android:textAlignment="center" />
    <Button
    android:id="@+id/GetPictureButton"
    android:layout_width="fill_parent"
    android:layout_height="wrap_content"
    android:text="Take Picture" />
</LinearLayout>
```

Build the solution which will add the control IDs to the ```Resource.Designer.cs``` file so that you can refer to controls by name in code.

##### Get the camera stream, take a picture and send the stream to the shared code

 The logic behind the UI is quite simple.  
 The button is bound to an event, which creates the logic associated to the ActionImageCapture Android Intent.  
 The image is saved as a bitmap, rotated in portrait mode, displayed on the UI and then sent as a stream to the business logic.
 
 Create a new ```BitmapHelper``` class in the Android project and replace the code with the following:
 
```csharp
using Android.Graphics;
using Android.Media;

namespace AndroidApp
{
    public static class BitmapHelpers
    {
        public static Bitmap GetAndRotateBitmap(string fileName)
        {
            Bitmap bitmap = BitmapFactory.DecodeFile(fileName);

            // Images are being saved in landscape, so rotate them back to portrait if they were taken in portrait
            // See https://forums.xamarin.com/discussion/5409/photo-being-saved-in-landscape-not-portrait
            // See http://developer.android.com/reference/android/media/ExifInterface.html
            using (Matrix mtx = new Matrix())
            {
                ExifInterface exif = new ExifInterface(fileName);
                var orientation = (Orientation)exif.GetAttributeInt(ExifInterface.TagOrientation, (int)Orientation.Undefined);

                //TODO : handle FlipHorizontal, FlipVertical, Transpose and Transverse
                //Undefined might be an emulator issue. Taking the assumption that the picture has been taken in portrait mode
                switch (orientation)
                {
                    case Orientation.Undefined:
                    case Orientation.Rotate90:
                        mtx.PreRotate(90);
                        break;
                    case Orientation.Rotate180:
                        mtx.PreRotate(180);
                        break;
                    case Orientation.Rotate270:
                        mtx.PreRotate(270);
                        break;
                    case Orientation.Normal:
                        // Normal, do nothing
                        break;
                    default:
                        break;
                }

                if (mtx != null)
                    bitmap = Bitmap.CreateBitmap(bitmap, 0, 0, bitmap.Width, bitmap.Height, mtx, false);
            }

            return bitmap;
        }
    }
}
```
 
 Then, replace the ```MainActivity``` class with the following code:

```csharp
[Activity(Label = "AndroidApp", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait)]
public class MainActivity : Activity
{
    public static File _file;
    public static File _dir;
    public static Bitmap _bitmap; 
    private ImageView _imageView;
    private Button _pictureButton;
    private TextView _resultTextView; 
    private bool _isCaptureMode = true;

    private void CreateDirectoryForPictures()
    {
        _dir = new File(
            Android.OS.Environment.GetExternalStoragePublicDirectory(
                Android.OS.Environment.DirectoryPictures), "CameraAppDemo");
        if (!_dir.Exists())
        {
            _dir.Mkdirs();
        }
    }

    private bool IsThereAnAppToTakePictures()
    {
        Intent intent = new Intent(MediaStore.ActionImageCapture);
        IList<ResolveInfo> availableActivities =
            PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
        return availableActivities != null && availableActivities.Count > 0;
    }

    protected override void OnCreate(Bundle bundle)
    {
        base.OnCreate(bundle);

        SetContentView(Resource.Layout.Main);

        if (IsThereAnAppToTakePictures())
        {
            CreateDirectoryForPictures();

            _pictureButton = FindViewById<Button>(Resource.Id.GetPictureButton);
            _pictureButton.Click += OnActionClick;

            _imageView = FindViewById<ImageView>(Resource.Id.imageView1);
            
            _resultTextView = FindViewById<TextView>(Resource.Id.resultText); 
        }
    }

    private void OnActionClick(object sender, EventArgs eventArgs)
    {
        if (_isCaptureMode == true)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            _file = new Java.IO.File(_dir, String.Format("myPhoto_{0}.jpg", Guid.NewGuid()));
            intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(_file));
            StartActivityForResult(intent, 0);
        }
        else
        {
            _imageView.SetImageBitmap(null);
            if (_bitmap != null)
            {
                _bitmap.Recycle();
                _bitmap.Dispose();
                _bitmap = null;
            }
            _pictureButton.Text = "Take Picture";
            _resultTextView.Text = "";
            _isCaptureMode = true;
        }
    }

    protected override async void OnActivityResult(int requestCode, Result resultCode, Intent data)
    {
        base.OnActivityResult(requestCode, resultCode, data);

        try
        {
            //Get the bitmap with the right rotation
            _bitmap = BitmapHelpers.GetAndRotateBitmap(_file.Path);

            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                //Get a stream
                _bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
                stream.Seek(0, System.IO.SeekOrigin.Begin);

                //Get and display the happiness score
                float result = await Core.GetAverageHappinessScore(stream);
                _resultTextView.Text = Core.GetHappinessMessage(result);
            }

            //Display the image
            _imageView.SetImageBitmap(_bitmap);
        }
        catch (Exception ex)
        {
            _resultTextView.Text = ex.Message;
        }
        finally
        {
            _pictureButton.Text = "Reset";
            _isCaptureMode = false;
        }
    }
}
```

Be sure to have the following using statements:

```csharp
using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Widget;
using Java.IO;
```
If you build your project, you should have an issue with the usage of "Core", which does not exist yet. This will be the next step.

##### Create a shared project with the logic to call the Cognitive Service API

Create a new project in your solution, with the Templates > Visual C# > Shared Project template. Use a name like "SharedProject".

Create a new class in the project, with the name "Core".

Copy/Paste the following code, by replacing the placeholder with your Emotion API key. This method is calling the Emotion API, through the SDK you've referenced thanks to the Microsoft.ProjectOxford.Emotion Nuget package.

```csharp
private static async Task<Emotion[]> GetHappiness(Stream stream)
{
    string emotionKey = "YourKeyHere";

    EmotionServiceClient emotionClient = new EmotionServiceClient(emotionKey);

    var emotionResults = await emotionClient.RecognizeAsync(stream);

    if (emotionResults == null || emotionResults.Count() == 0)
    {
        throw new Exception("Can't detect face");
    }

    return emotionResults;
}
```

Copy/Paste the following code in the Core class, which are utilities method to do some calculation and some formating on the API results:

```csharp
//Average happiness calculation in case of multiple people
public static async Task<float> GetAverageHappinessScore(Stream stream)
{
    Emotion[] emotionResults = await GetHappiness(stream);

    float score = 0;
    foreach (var emotionResult in emotionResults)
    {
        score = score + emotionResult.Scores.Happiness;
    }

    return score / emotionResults.Count();
}

public static string GetHappinessMessage(float score)
{
    score = score * 100;
    double result = Math.Round(score, 2);

    if (score >= 50)
        return result + " % :-)";
    else
        return result + "% :-(";
}
```

Be sure to use the following using statements in the Core class:

```csharp
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
```

You can now add a reference to your SharedProject project from you Android app, by using the wizard available after a right clicking on the "References" node in the Solution Explorer. One section of this wizard is a listing of all the Shared Projects available in your solution.

Finally, you have to add the following using statement in your MainActivity.cs class:

```csharp
using SharedProject;
```

##### Declare the right capabilities for your app

Right click on the Android Project and select Properties.  Click on the Android Manifest on the left panel and request the Required Permissions:

* CAMERA
* WRITE_EXTERNAL_STORAGE

Save the project properties.

##### Build and run the project

Build the solution and run the project thanks to the Visual Studio Emulator (pressing F5 is a good option for that).

Before playing with the app, you should tell the emulator to use your laptop camera as the emulator front-facing camera. You can do that by opening the Tools menu (">>" icon), "Camera" tab.

By clicking on the "Take a picture" button, you should launch the webcam capture. Validating the capture will send the picture to the API and display the result on the screen.
