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

namespace WindowsApp
{
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
}