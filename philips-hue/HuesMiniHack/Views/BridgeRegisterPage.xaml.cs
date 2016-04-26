using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace HuesMiniHack.Views
{
    public partial class BridgeRegisterPage : ContentPage
    {
        public BridgeRegisterPage(string selectedBridgeIP)
        {
            InitializeComponent();

            var viewModel = new ViewModels.BridgeRegisterViewModel();
            viewModel.SelectedBridgeIP = selectedBridgeIP;

            BindingContext = viewModel;
        }
    }
}

