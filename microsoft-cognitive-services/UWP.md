#### Bonus Challenge #1 Walkthrough

As we have used a Shared Project, the logic can be easily shared between several apps. We're going to demonstrate this by adding a Universal Windows Platform application to the solution, to target the hundreds of millions Windows 10 devices out there.

##### Create a UWP project in your solution

Create a new project in your solution, with the Templates > Visual C# > Windows > Universal > Blank App (Universal Windows) template. Use a name like "WindowsApp" and choose the appropriate target and minimum versions if prompted.

Add the following Nuget package to your project:
* Microsoft.ProjectOxford.Emotion

You can now add a reference to your SharedProject project from you UWP app, the same way you did with the Android project.

##### Design the UI layer

For this single page application, the UI layer can be built in the ```MainPage.xaml``` file. We're going to use a CaptureElement, an Image, a TextBlock and a Button for our UI. You can copy/paste the following code to save some time:

```xml
<Page x:Class="WindowsApp.MainPage"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:local="using:WindowsApp"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    mc:Ignorable="d">
    <Grid Background="Black">
        <Grid.RowDefinitions>
            <RowDefinition Height="*"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
        </Grid.RowDefinitions>
        <CaptureElement Grid.Row="0" x:Name="captureElement" />
        <Image x:Name="previewImage" Grid.Row="0" Visibility="Collapsed"/>
        <TextBlock Grid.Row="1" x:Name="hapinessRatio"
                    Margin="12"
                    Foreground="BlueViolet"
                    FontSize="24"
                    HorizontalAlignment="Center" />
        <Button Grid.Row="2" x:Name="actionButton" 
                Content="Take Picture" 
                HorizontalAlignment="Stretch" 
                Background="BlueViolet"
                Foreground="White"
                Click="OnActionClick" />
    </Grid>
</Page>
```

##### Get the camera stream, take a picture and send the stream to the shared code

The logic behind the UI is quite simple.  
The MediaCapture is triggered during the page load, the button is bound to an event, which saves the picture as a bitmap, display it on the UI and then send it as a stream to the business logic.

Replace the ```MainPage``` class by the following code:

```csharp
public sealed partial class MainPage : Page
{
    private MediaCapture _captureManager = null;
    private BitmapImage _bmpImage = null;
    private StorageFile _file = null;
    private bool _isCaptureMode = true;

    public MainPage()
    {
        this.InitializeComponent();
        this.Loaded += MainPage_Loaded;
    }

    private async void MainPage_Loaded(object sender, RoutedEventArgs e)
    {
        await InitMediaCapture();
    }

    private async Task<bool> InitMediaCapture()
    {
        _captureManager = new MediaCapture();
        await _captureManager.InitializeAsync();

        captureElement.Source = _captureManager;
        captureElement.FlowDirection = FlowDirection.RightToLeft;

        // start capture preview
        await _captureManager.StartPreviewAsync();

        return true;
    }

    private async Task<bool> ImageCaptureAndDisplay()
    { 
        ImageEncodingProperties imageFormat = ImageEncodingProperties.CreateJpeg(); 

        // create storage file in local app storage 
        _file = await ApplicationData.Current.LocalFolder.CreateFileAsync(
                $"myPhoto_{Guid.NewGuid()}.jpg",
                CreationCollisionOption.GenerateUniqueName); 

        // capture to file 
        await _captureManager.CapturePhotoToStorageFileAsync(imageFormat, _file);

        _bmpImage = new BitmapImage(new Uri(_file.Path));
        previewImage.Source = _bmpImage;

        return true;
    }

    private async void OnActionClick(object sender, RoutedEventArgs e)
    {
        if (_isCaptureMode == true)
        {
            await ImageCaptureAndDisplay();

            try
            {
                float result = await Core.GetAverageHappinessScore(await _file.OpenStreamForReadAsync());

                hapinessRatio.Text = Core.GetHappinessMessage(result);

                previewImage.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                hapinessRatio.Text = ex.Message;
            }
            finally
            {
                actionButton.Content = "Reset";
                _isCaptureMode = false;
            }
        }
        else
        {
            previewImage.Visibility = Visibility.Collapsed;
            actionButton.Content = "Take Picture";
            hapinessRatio.Text = "";
            _isCaptureMode = true;
        }
    }
}
```

Be sure to have the following using statements:

```csharp
using SharedProject;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
```

##### Declare the right capabilities for your app

Open the ```Package.appxmanifest``` file, and add the following Capabilities to your application, thanks to the correct checkboxes in the Capabilities tab:
* Microphone
* Webcam

##### Configure, build and run the project

In the Solution Configuration Manager (by right clicking on the solution node in the Solution Explorer), check the Build and Deploy checkboxes for your UWP project.

Build the solution and run the UWP project. You don't need an Emulator by [using Windows 10 as a development machine](https://msdn.microsoft.com/en-us/windows/uwp/get-started/enable-your-device-for-development#enable-your-windows-10-devices).

By clicking on the "Take a picture" button, you will capture a picture, send it to the API and display the result on the screen.
