using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Q42.HueApi;
using Xamarin.Forms;

namespace HuesMiniHack.ViewModels
{
    public class LightViewModel : BaseViewModel
    {
        LocalHueClient client;
        public Light SelectedLight { get; set; }
        public string LampName
        {
            get
            {
                return SelectedLight.Name;
            }
        }

        public string LampStatus
        {
            get
            {
                if (SelectedLight.State.On == true)
                    return "Lamp Status: On";
                return "Lamp Status: Off";
            }
        }

        public string LampModel
        {
            get
            {
                return SelectedLight.ModelId;
            }
        }

        public string LampBrightness
        {
            get
            {
                return $"Brightness: {SelectedLight.State.Brightness}";
            }
        }

        public LightViewModel(Light selectedLight)
        {
            SelectedLight = selectedLight;
        }


        Command lampOnCommand;
        public Command LampOnCommand
        {
            get { return lampOnCommand ?? (lampOnCommand = new Command(async () => await ExecuteLampOnCommand())); }
        }

        Command lampOffCommand;
        public Command LampOffCommand
        {
            get { return lampOffCommand ?? (lampOffCommand = new Command(async () => await ExecuteLampOffCommand())); }
        }

        Command lampAlertCommand;
        public Command LampAlertCommand
        {
            get { return lampAlertCommand ?? (lampAlertCommand = new Command(async () => await ExecuteLampAlertCommand())); }
        }

        Command lampEffectCommand;
        public Command LampEffectCommand
        {
            get { return lampEffectCommand ?? (lampEffectCommand = new Command(async () => await ExecuteLampEffectCommand())); }
        }

        async Task ExecuteLampOnCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (client == null)
                    CreateClient();
                
                //TODO 6: Turn lamp on

                OnPropertyChanged("LampStatus");
            }
            catch (Exception ex)
            {
                Acr.UserDialogs.UserDialogs.Instance.ShowError(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task ExecuteLampOffCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (client == null)
                    CreateClient();

                //TODO 7: Turn lamp off

                OnPropertyChanged("LampStatus");
            }
            catch (Exception ex)
            {
                Acr.UserDialogs.UserDialogs.Instance.ShowError(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task ExecuteLampAlertCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (client == null)
                    CreateClient();

                //TODO 8: Set alert

                OnPropertyChanged("LampStatus");
            }
            catch (Exception ex)
            {
                Acr.UserDialogs.UserDialogs.Instance.ShowError(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task ExecuteLampEffectCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (client == null)
                    CreateClient();

                //TODO 9: Start color effect

                OnPropertyChanged("LampStatus");
                }
            catch (Exception ex)
            {
                Acr.UserDialogs.UserDialogs.Instance.ShowError(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }


        void CreateClient()
        {
            if (!string.IsNullOrEmpty(Helpers.Settings.AppKey))
            {
                client = new LocalHueClient(Helpers.Settings.DefaultBridgeIP, Helpers.Settings.AppKey);
                client.Initialize(Helpers.Settings.AppKey);
            }

            else
                Acr.UserDialogs.UserDialogs.Instance.ShowError("No Hue Client found");
        }
    }
}

