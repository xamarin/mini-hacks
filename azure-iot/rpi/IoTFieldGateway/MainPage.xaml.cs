using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Azure.Devices.Client;

namespace IoTFieldGateway
{
    public sealed partial class MainPage : Page
    {
        const string CONNECTION_STRING = "HostName=PierceHub.azure-devices.net;DeviceId=coffeeMaker;SharedAccessKey=UT39TIzywKkrrycqQeMS2j1DELBs9x3icM5x/Q2jb9I=";

        private ObservableCollection<CloudMessage> Messages = new ObservableCollection<CloudMessage>();
        private DeviceClient deviceClient;

        private const int TIME_UNIT_SECOND = 1000;
        private const int TIME_UNIT_MINUTE = 60 * TIME_UNIT_SECOND;
        private const int TIME_UNIT_HOUR = 60 * TIME_UNIT_MINUTE;

        private const int COFFEE_MAKER_RELAY_PIN = 5;
        private const int COFFEE_MAKER_LED_PIN = 6;

        private bool coffeeRelayStatus = false;
        private bool isListening = false;

        private GpioController gpio;

        private GpioPin coffeeMakerRelay;
        private GpioPin coffeeMakerLED;

        private Timer activeTimer = null;

        private int runningTime = 0;
        private int maxRunningTime = 30 * TIME_UNIT_MINUTE;

        public MainPage()
        {
            this.InitializeComponent();
            App.Current.UnhandledException += Current_UnhandledException;

            DataContext = this;

            InitGpio();
            activeTimer = new Timer(ActiveTimerTick, coffeeRelayStatus, Timeout.Infinite, Timeout.Infinite);
            deviceClient = DeviceClient.CreateFromConnectionString(CONNECTION_STRING);
            MessagesListView.ItemsSource = Messages;
        }

        private void InitGpio()
        {
            gpio = GpioController.GetDefault();

            coffeeMakerRelay = gpio.OpenPin(COFFEE_MAKER_RELAY_PIN);
            coffeeMakerRelay.Write(GpioPinValue.Low);
            coffeeMakerRelay.SetDriveMode(GpioPinDriveMode.Output);

            coffeeMakerLED = gpio.OpenPin(COFFEE_MAKER_LED_PIN);
            coffeeMakerLED.Write(GpioPinValue.Low);
            coffeeMakerLED.SetDriveMode(GpioPinDriveMode.Output);
        }

        private void DisableCoffeeMakerRelay()
        {
            coffeeMakerRelay.Write(GpioPinValue.Low);
            coffeeMakerLED.Write(GpioPinValue.Low);

            activeTimer.Change(Timeout.Infinite, Timeout.Infinite);
            runningTime = 0;
            UpdateTimerUI();

            coffeeRelayStatus = false;
            UpdateUI();
        }

        private void EnableCoffeeMakerRelay()
        {
            coffeeMakerRelay.Write(GpioPinValue.High);
            coffeeMakerLED.Write(GpioPinValue.High);

            activeTimer.Change(0, TIME_UNIT_SECOND);

            coffeeRelayStatus = true;
            UpdateUI();
        }

        private void ToggleCoffeeMakerRelay()
        {
            var coffeeMakerRelayValue = coffeeMakerRelay.Read();
            if(coffeeMakerRelayValue == GpioPinValue.Low)
            {
                EnableCoffeeMakerRelay();
            }
            else
            {
                DisableCoffeeMakerRelay();
            }
        }

        private async Task Listen()
        {
			coffeeMakerRelay.Write(GpioPinValue.Low);
            coffeeMakerLED.Write(GpioPinValue.Low);

            while (isListening)
            {
                Message receivedMessage = await deviceClient.ReceiveAsync();
                if (receivedMessage != null)
                {
                    var message = Encoding.UTF8.GetString(receivedMessage.GetBytes());

                    if (message.ToLower().Contains("turn coffee maker on"))
                    {
                        ToggleCoffeeMakerRelay();
                    }
                    else if (message.ToLower().Contains("turn coffee maker off"))
                    {
                        ToggleCoffeeMakerRelay();
                    }

                    var offset = receivedMessage.EnqueuedTimeUtc.DateTime - DateTime.Now;
                    var cloudMessage = new CloudMessage
                    {
                        Body = message,
                        Timestamp = receivedMessage.EnqueuedTimeUtc.DateTime.Subtract(offset)
                    };
                    Messages.Add(cloudMessage);

                    await deviceClient.CompleteAsync(receivedMessage);
                }

                await Task.Delay(1000);
            }
        }

        private async void UpdateUI()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                BtnOn.IsEnabled = !coffeeRelayStatus;
                BtnOff.IsEnabled = coffeeRelayStatus;
            });
        }

        private async void UpdateTimerUI()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (coffeeRelayStatus)
                {
                    TimeSpan span = TimeSpan.FromMilliseconds(maxRunningTime - runningTime);
                    TextCountdown.Text = span.ToString(@"m\:ss");
                }
                else
                {
                    TextCountdown.Text = "--:--";
                }
            });
        }

        private void BtnOn_Click(object sender, RoutedEventArgs e)
        {
            ToggleCoffeeMakerRelay();
        }

        private void BtnOff_Click(object sender, RoutedEventArgs e)
        {
            ToggleCoffeeMakerRelay();
        }

        private void BtnStartListening_Click(object sender, RoutedEventArgs e)
        {
            isListening = true;
            BtnStartListening.IsEnabled = false;
            BtnStopListening.IsEnabled = true;

            BtnOn.IsEnabled = false;
            BtnOff.IsEnabled = false;

            Listen();
        }

        private void BtnStopListening_Click(object sender, RoutedEventArgs e)
        {
            isListening = false;
            BtnStartListening.IsEnabled = true;
            BtnStopListening.IsEnabled = false;

            BtnOn.IsEnabled = true;
            BtnOff.IsEnabled = true;
        }

        private void ActiveTimerTick(object state)
        {
            runningTime += TIME_UNIT_SECOND;

            if (runningTime >= maxRunningTime)
            {
                activeTimer.Change(Timeout.Infinite, Timeout.Infinite);
                DisableCoffeeMakerRelay();
            }

            UpdateTimerUI();
        }

        private void Current_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (coffeeMakerRelay != null)
            {
                coffeeMakerRelay.Write(GpioPinValue.Low);
                coffeeMakerRelay.SetDriveMode(GpioPinDriveMode.Output);
            }
        }
    }
}