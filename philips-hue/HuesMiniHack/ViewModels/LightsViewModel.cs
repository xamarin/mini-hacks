using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Q42.HueApi;
using Q42.HueApi.Interfaces;
using Xamarin.Forms;

namespace HuesMiniHack.ViewModels
{
    public class LightsViewModel : BaseViewModel
    {

        public ObservableCollection<Light> Lights { get; private set; }

        Light selectedLight;
        public Light SelectedLight
        {
            get
            {
                return selectedLight;
            }
            set
            {
                selectedLight = value;
                OnPropertyChanged("SelectedLight");

                var tabbedPage = Application.Current.MainPage as Views.RootPage;
                var navigation = tabbedPage.Children[1].Navigation;
                navigation.PushAsync(new Views.LightPage(selectedLight), true);
            }
        }

        public LightsViewModel()
        {
            Lights = new ObservableCollection<Light>();
        }

        Command lampDiscoveryCommand;
        public Command LampDiscoveryCommand
        {
            get { return lampDiscoveryCommand ?? (lampDiscoveryCommand = new Command(async () => await ExecuteLampDiscoveryCommand())); }
        }

        //Bulbs grow, lamps glow. 
        async Task ExecuteLampDiscoveryCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (string.IsNullOrEmpty(Helpers.Settings.DefaultBridgeIP))
                {
                    Acr.UserDialogs.UserDialogs.Instance.ShowError("No Bridge Selected");
                    return;
                }

                //TODO 4: New up a LocalHueClient and Initialize it.

                //TODO 5: Discover all the lamps connected to the bridge

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

        public Color ButtonBackgroundColor
        {
            get
            {
                return Color.FromHex("#81C134");
            }
        }
    }
}

