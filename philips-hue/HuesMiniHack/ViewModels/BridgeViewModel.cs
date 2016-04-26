using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Q42.HueApi.Interfaces;
using Q42.HueApi;
using Xamarin.Forms;

namespace HuesMiniHack.ViewModels
{
    public class BridgeViewModel : BaseViewModel
    {
        public ObservableCollection<string> BridgeIps { get; private set;}

        string selectedBridgeIP;
        public string SelectedBridgeIP
        {
            get
            {
                return selectedBridgeIP;
            }
            set
            {
                selectedBridgeIP = value;
                OnPropertyChanged("SelectedBridge");

                if (!string.IsNullOrEmpty(selectedBridgeIP))
                {
                    Helpers.Settings.DefaultBridgeIP = selectedBridgeIP;

                    var tabbedPage = Application.Current.MainPage as Views.RootPage;
                    var navigation = tabbedPage.Children[0].Navigation;
                    navigation.PushAsync(new Views.BridgeRegisterPage(selectedBridgeIP), true);
                }
            }
        }

        public BridgeViewModel()
        {
            BridgeIps = new ObservableCollection<string>();
        }

        Command bridgeDiscoveryCommand;
        public Command BridgeDiscoveryCommand
        {
            get { return bridgeDiscoveryCommand ?? (bridgeDiscoveryCommand = new Command(async () => await ExecuteBridgeDiscoveryCommand())); }
        }

        async Task ExecuteBridgeDiscoveryCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                //TODO 2: Implement Bridge locator

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


